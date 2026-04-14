using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoreSentries.Common;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry
{
	public class EoWSentry : ModProjectile
	{
		public ref float ShootTimer => ref Projectile.ai[0];

		public bool JustSpawned {
			get => Projectile.localAI[0] == 0;
			set => Projectile.localAI[0] = value ? 0 : 1;
		}

		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 3;
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 38;
			Projectile.height = 42;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.sentry = true; // Sets the weapon as a sentry for sentry accessories to properly work.
			Projectile.timeLeft = Projectile.SentryLifeTime; // Sentries last 10 minutes
			Projectile.ignoreWater = true;
			Projectile.netImportant = true; // Sentries need this so they are synced to newly joining players
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			fallThrough = false; // Allow this projectile to collide with platforms
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			return false; // Prevent tile collision from killing the projectile
		}

		public override void AI()
		{
			const int ShootFrequency = 60;
			const int TargetingRange = 50 * 16;

			if (JustSpawned)
			{
				JustSpawned = false;
				ShootTimer = ShootFrequency * 1.5f;

				SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
			}

			Projectile.velocity.X = 0f;
			Projectile.velocity.Y += 0.2f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}

			NPC targetNPC = MoreSentries.NearestTarget(Projectile, TargetingRange);
			if (targetNPC != null && ShootTimer <= 0)
			{
				ShootTimer = ShootFrequency;

				SoundEngine.PlaySound(SoundID.Item17 with { Volume = 0.4f }, Projectile.Center);

				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 shootVelocity = (targetNPC.Center - Projectile.Center) / 100f;
					float tickSteps = (30 - shootVelocity.Y) / 100f;
					tickSteps = Math.Max(tickSteps, 0.3f);
					
					shootVelocity += targetNPC.velocity * tickSteps;
					shootVelocity.Y -= 15;

					Projectile.rotation = (float)(shootVelocity.ToRotation() + Math.PI / 2f);
					Projectile.NewProjectile(
						Projectile.GetSource_FromThis(),
						Projectile.Center,
						shootVelocity,
						ModContent.ProjectileType<VileSpit>(),
						Projectile.damage,
						Projectile.knockBack,
						Projectile.owner
					);
				}
			}

			ShootTimer -= Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
			Projectile.frame = (int)Math.Clamp(ShootTimer / 20, 0, 2);
		}

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			Vector2 origin = Projectile.Size / 2f + new Vector2(0, 16);
			const int frameHeight = 42;

			Rectangle sentryRectangle = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
			Rectangle standRectangle = new Rectangle(0, 126, 26, 12); // 126 = 42 * 4, 26 = width of stand, 12 = height of stand

			Main.EntitySpriteDraw(
				texture,
				drawPosition + new Vector2(0, 12),
				sentryRectangle,
				lightColor,
				Projectile.rotation,
				origin,
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			Main.EntitySpriteDraw(
				texture,
				drawPosition,
				standRectangle,
				lightColor,
				0,
				new Vector2(13, -11),
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			return false;
        }
	}
}