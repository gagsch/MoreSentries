using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles
{
	public class GolemSentryFist : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.SentryShot[Type] = true;
			Main.projFrames[Type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.CritChance = 0;
			Projectile.width = 30;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 60;

			Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 30)
			{
				Projectile.velocity = -Projectile.velocity;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;

			Rectangle rect = new Rectangle(0, (int)(28 * Projectile.ai[1]), 30, 28);
			Rectangle chainRect = new Rectangle(0, 56, 20, 12);

			Vector2 vectorian = Projectile.velocity;
			if (Projectile.timeLeft >= 30)
			{
				vectorian *= -1;
			}

			int loops;
			if (Projectile.timeLeft >= 30)
			{
				loops = 60 - Projectile.timeLeft;
			}
			else
			{
				loops = Projectile.timeLeft;
			}

			for (int i = 0; i < loops + 1; i++)
			{
				Vector2 theEvilVectorianDemon = Projectile.Center + vectorian * i;
				Color color = Lighting.GetColor((int)theEvilVectorianDemon.X / 16, (int)theEvilVectorianDemon.Y / 16);
				Main.EntitySpriteDraw(
					texture,
					drawPosition + vectorian * i,
					chainRect,
					color,
					Projectile.ai[0],
					new Vector2(8, 4),
					Projectile.scale,
					SpriteEffects.None,
					0
				);
			}

			Main.EntitySpriteDraw(
				texture,
				drawPosition,
				rect,
				lightColor,
				Projectile.ai[0],
				new Vector2(0, 14),
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			return false;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.5f }, Projectile.Center);
			for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.Torch, 0, 0, 0, default, 2f);
            }
        }

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
	}
}