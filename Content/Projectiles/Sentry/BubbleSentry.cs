using System;
using Microsoft.Xna.Framework;
using MoreSentries.Common;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry
{
	public class BubbleSentry : ModProjectile
	{
		public ref float ShootTimer => ref Projectile.ai[0];

		public bool JustSpawned {
			get => Projectile.localAI[0] == 0;
			set => Projectile.localAI[0] = value ? 0 : 1;
		}

		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 56;
			Projectile.height = 88;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.sentry = true;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			return false;
		}

		public override void AI()
		{
			const int ShootFrequency = 75;
			const int TargetingRange = 40 * 16;

			float shootTimerRatio = ShootTimer / ShootFrequency;

			if (shootTimerRatio > 0.96f) Projectile.frame = 3;
			else if (Projectile.timeLeft % 7 >= 4) Projectile.frame = 1;
			else Projectile.frame = 0;

			if (JustSpawned)
			{
				JustSpawned = false;
				ShootTimer = 0;

				SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
			}

			float t = Projectile.timeLeft % 512 / 512f;
			Projectile.velocity.Y = 0.25f - MathF.Abs(t - 0.5f);
			
			if (MoreSentries.NearestTarget(Projectile, TargetingRange) == null) return;
			
			if (shootTimerRatio < 0.10f) Projectile.frame = 3;
			else if (shootTimerRatio < 0.18f) Projectile.frame = 2;

			ShootTimer -= Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
			if (ShootTimer > 0) return;

			ShootTimer = ShootFrequency;
			SoundEngine.PlaySound(SoundID.Item54 with { Volume = 0.6f }, Projectile.Center);
			SoundEngine.PlaySound(SoundID.Item85 with { Volume = 0.4f }, Projectile.Center);

			if (Main.myPlayer == Projectile.owner)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 shootDirection = new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootDirection, ModContent.ProjectileType<Bubble>(), Projectile.damage, Projectile.knockBack, Projectile.owner);	
				}
			}
		}
	}
}