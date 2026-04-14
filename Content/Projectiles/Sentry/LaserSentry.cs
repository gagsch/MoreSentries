using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoreSentries.Common;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry
{
	public class LaserSentry : ModProjectile
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
			Projectile.width = 46;
			Projectile.height = 50;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.sentry = true; // Sets the weapon as a sentry for sentry accessories to properly work.
			Projectile.timeLeft = Projectile.SentryLifeTime; // Sentries last 10 minutes
			Projectile.ignoreWater = true;
			Projectile.netImportant = true; // Sentries need this so they are synced to newly joining players
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			return false;
		}

		public override void AI()
		{
			const int ShootFrequency = 60;
			const int TargetingRange = 60 * 16;
			const float FireVelocity = 10;

			if (Main.rand.NextBool(3)) Dust.NewDustPerfect(Projectile.position + new Vector2(Main.rand.Next(12, 36), 48), DustID.Torch, new Vector2(0, Main.rand.NextFloat(4,6))).noGravity = true;
			Lighting.AddLight(Projectile.Center, 0.7f, 0.6f, 0.6f);

			if (JustSpawned)
			{
				JustSpawned = false;
				ShootTimer = ShootFrequency;

				SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
			}

			NPC targetNPC = MoreSentries.NearestTarget(Projectile, TargetingRange);
			int frame = 0;

			if (targetNPC != null)
			{
				ShootTimer -= Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
				
				if (ShootTimer < 7)
				{
					frame = 2;
				}
				else if (ShootTimer < 40)
				{
					frame = 1;
				}
			}
			else
            {
				ShootTimer = ShootFrequency;
            }

			Projectile.frame = frame;

			if (targetNPC != null && ShootTimer <= 0)
			{
				ShootTimer = ShootFrequency;

				SoundEngine.PlaySound(SoundID.Item33 with { Volume = 0.5f }, Projectile.Center);

				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 shootDirection = (targetNPC.Center - Projectile.Center + Vector2.One).SafeNormalize(Vector2.UnitX);
					Vector2 shootVelocity = shootDirection * FireVelocity;

					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + shootVelocity * 8, shootVelocity, ModContent.ProjectileType<PrimeLaser>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				}
			}
		}

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			Vector2 origin = Projectile.Size / 2f;
			const int frameHeight = 50;
			int reaaaa = Projectile.timeLeft / 2 % 3 * 30;

			Rectangle sentryRectangle = new Rectangle(0, frameHeight * Projectile.frame, 46, frameHeight);
			Rectangle standRectangle = new Rectangle(46, reaaaa, 22, 30);

			Main.EntitySpriteDraw(
				texture,
				drawPosition,
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
				drawPosition + new Vector2(12, 50),
				standRectangle,
				lightColor,
				0,
				origin,
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			return false;
        }
	}
}