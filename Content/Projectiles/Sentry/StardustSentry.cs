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
	public class StardustSentry : ModProjectile
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
			Projectile.width = 62;
			Projectile.height = 54;
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
			const int ShootFrequency = 90;
			const int TargetingRange = 50 * 16;

			Lighting.AddLight(Projectile.Center, 0.4f, 0.6f, 1f);

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

			NPC targetNPC = MoreSentries.NearestTarget(Projectile, TargetingRange, true);
			if (targetNPC != null && ShootTimer <= 0)
			{
				ShootTimer = ShootFrequency;
				SoundEngine.PlaySound(SoundID.DD2_OgreSpit with { Volume = 0.4f }, Projectile.Center);

				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 shootVelocity = (targetNPC.Center - Projectile.Center) / 100f;
					shootVelocity.Y -= 14;

					Projectile.NewProjectile(
						Projectile.GetSource_FromThis(),
						Projectile.Center,
						shootVelocity,
						ModContent.ProjectileType<StardustSpider>(),
						Projectile.damage,
						Projectile.knockBack,
						Projectile.owner,
						targetNPC.whoAmI
					);
				}
			}

			ShootTimer -= Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
			Projectile.frame = (int)Math.Clamp(ShootTimer / 30, 0, 2);
		}

		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
			int textureWidth = texture.Width;
			int textureFrameHeight = texture.Height / Main.projFrames[Type];

            Rectangle frameRect = new Rectangle(0, textureFrameHeight * Projectile.frame, textureWidth, textureFrameHeight);

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                frameRect,
                Color.White * Projectile.Opacity,
                Projectile.rotation,
                Projectile.Size / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
	}
}