using System.Collections.Generic;
using MoreSentries.Common;
using MoreSentries.Content.Items.Sentry;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry.Cloud
{
    public class CorruptionCloudRaining : ModProjectile
    {
        public override void SetStaticDefaults() {
			Main.projFrames[Type] = 6;
		}

        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.BloodCloudRaining);
            Projectile.DamageType = DamageClass.Default;
            Projectile.sentry = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.netImportant = true;
		}

        public override bool PreAI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 5)
                {
                    Projectile.frame = 0;
                }
            }

            Projectile.ai[0] += Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
            if (Projectile.ai[0] <= 10)
            {
                return false;
            }

            Projectile.ai[0] = 0f;
            if (Projectile.owner == Main.myPlayer)
            {
                int posX = (int)Projectile.Center.X;
            int posY = (int)Projectile.position.Y + Projectile.height;
                posX += Main.rand.Next(-14, 15);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), posX, posY, 0f, 5f, ModContent.ProjectileType<AcidRain>(), Projectile.damage, 0f);
            }

            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Main.LocalPlayer.UpdateMaxTurrets();
            }

            Projectile.damage = ContentSamples.ItemsByType[ModContent.ItemType<CorruptionCloudItem>()].damage;
        }
    }
}