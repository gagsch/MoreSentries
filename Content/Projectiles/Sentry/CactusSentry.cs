using System;
using Microsoft.Xna.Framework;
using MoreSentries.Common;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry
{
	public class CactusSentry : ModProjectile
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
			Projectile.width = 50;
			Projectile.height = 52;
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
		
		public override void AI() {
			const int ShootFrequency = 60;
			const int TargetingRange = 40 * 16;
			const float FireVelocity = 14f;

			if (JustSpawned) {
				JustSpawned = false;
				ShootTimer = ShootFrequency;

				SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
			}

			Projectile.velocity.X = 0f;
			Projectile.velocity.Y += 0.2f;
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}

			NPC targetNPC = MoreSentries.NearestTarget(Projectile, TargetingRange);

			if (targetNPC != null && ShootTimer <= 0)
			{
				Projectile.spriteDirection = targetNPC.position.X > Projectile.position.X ? 1 : -1;
				Projectile.frame = 2;
				ShootTimer = ShootFrequency;

				SoundEngine.PlaySound(SoundID.Item17 with { Volume = 0.5f }, Projectile.Center);

				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 shootDirection = (targetNPC.Center - Projectile.Center + Vector2.One).SafeNormalize(Vector2.UnitX);
					Vector2 shootVelocity = shootDirection * FireVelocity;

					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVelocity, ModContent.ProjectileType<CactusNeedle>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -1);
				}
			}

			if (Projectile.frame > 0)
            {
                Projectile.frameCounter++;
				if (Projectile.frameCounter >= 16)
				{
					Projectile.frameCounter = 0;
					Projectile.frame--;
				}
            }
			
			ShootTimer -= Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
		}
	}
}