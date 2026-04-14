using MoreSentries.Content.Projectiles.Sentry.Cloud;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Common.GlobalProjectiles
{
    public class MageCloudItems : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.type == ItemID.CrimsonRod || item.type == ItemID.NimbusRod;
        }

        public override void SetDefaults(Item item)
        {
            item.sentry = true;
            item.DamageType = DamageClass.Summon;
            item.mana = 10;
            item.useTime = 30;
            item.useAnimation = 30;
            item.crit = 0;
        }
    }

    public class MageCloudProjectiles : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile proj, bool lateInstantiation)
        {
            return proj.type == ProjectileID.BloodCloudRaining || proj.type == ProjectileID.RainCloudRaining;
        }

        public override void SetDefaults(Projectile proj)
        {
            proj.DamageType = DamageClass.Default;
            proj.sentry = true;
            proj.timeLeft = Projectile.SentryLifeTime;
            proj.netImportant = true;
        }

        public override bool PreAI(Projectile projectile)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 8)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 5)
                {
                    projectile.frame = 0;
                }
            }
            
            projectile.ai[0] += Main.player[projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;

            int type;
            if (projectile.type == ProjectileID.BloodCloudRaining && projectile.ai[0] > 10f)
            {
                projectile.ai[0] = 0f;
                type = ProjectileID.BloodRain;

            }
            else if (projectile.type == ProjectileID.RainCloudRaining && projectile.ai[0] > 8f)
            {
                projectile.ai[0] = 0f;
                type = ProjectileID.RainFriendly;
            }
            else
            {
                return false;
            }

            if (projectile.owner == Main.myPlayer)
            {
                int posX = (int)projectile.Center.X;
                int posY = (int)projectile.position.Y + projectile.height;
                posX += Main.rand.Next(-14, 15);
                Projectile.NewProjectile(projectile.GetSource_FromThis(), posX, posY, 0f, 5f, type, projectile.damage, 0f);
            }

            return false;
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.owner == Main.myPlayer)
            {
                Main.LocalPlayer.UpdateMaxTurrets();
            }
        }
    }

    public class RainProjectiles : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstantiation)
        {
            return projectile.type == ProjectileID.RainFriendly || projectile.type == ProjectileID.BloodRain || projectile.type == ModContent.ProjectileType<AcidRain>();
        }

        public override void SetDefaults(Projectile projectile)
        {
            projectile.CritChance = 0;
            projectile.DamageType = DamageClass.Default;
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.type == NPCID.EaterofWorldsBody || target.type == NPCID.TheDestroyerBody) modifiers.FinalDamage /= 2.5f;
        }
    }
}