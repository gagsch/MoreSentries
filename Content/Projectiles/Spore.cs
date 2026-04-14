using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles
{
	public class Spore : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.SentryShot[Type] = true;
			Main.projFrames[Type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 255;
            Projectile.penetrate = 2;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesIDStaticNPCImmunity = false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			return false; // Prevent tile collision from killing the projectile
		}

		public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.0f, 0.4f, 0.08f);
            Projectile.scale = 1 + (float)Math.Sin(Projectile.timeLeft / 20f) / 6f;
            Projectile.rotation += (Projectile.frame * 2 - 1) / 150f;
            Projectile.alpha = 255 - Projectile.timeLeft;

            if (Projectile.penetrate > 1)
            {
                NPC npc = MoreSentries.NearestTarget(Projectile, 512, true);
                
                if (npc != null && npc.active) {
                    Vector2 direction = npc.Center - Projectile.Center;
                    direction.Normalize();
                    Projectile.velocity += direction / 4;
                }
                Projectile.velocity *= 0.95f;
            }
            else {
                Projectile.timeLeft -= 3;
                Projectile.velocity *= 0.9f;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity = new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() - 0.5f);
            Projectile.velocity.Normalize();
            Projectile.velocity /= 10;
            Projectile.frame = Main.rand.Next(0,2);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity /= 4;
            if (Main.myPlayer != Projectile.owner) return;

            Projectile sporeGas = Projectile.NewProjectileDirect(
                Projectile.GetSource_FromThis(),
                new Vector2(Projectile.Center.X, Projectile.Center.Y),
                Projectile.velocity,
                ProjectileID.SporeGas + Main.rand.Next(0, 3),
                Projectile.damage / 2,
                0,
                Projectile.owner
            );

            sporeGas.usesIDStaticNPCImmunity = true;
            sporeGas.idStaticNPCHitCooldown = 10;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle frameRect = new Rectangle(0, 16 * Projectile.frame, 16, 16);

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                frameRect,
                Color.White * Projectile.Opacity,
                Projectile.rotation,
                texture.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
	}
}