using System;
using Microsoft.Xna.Framework;
using MoreSentries.Common;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry
{
	public class BoCSentry : ModProjectile
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
			Projectile.width = 36;
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
		
		public override void AI() {
			const int ShootFrequency = 120;

			if (JustSpawned) {
				JustSpawned = false;
				ShootTimer = ShootFrequency;

				SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
			}

			Projectile.velocity.X = 0f;
			float t = Projectile.frameCounter / 96f;
			Projectile.velocity.Y = 1f - 4f * MathF.Abs(t - 0.5f);

			if (ShootTimer <= 0 && Projectile.ai[1] < 3)
			{
				ShootTimer = ShootFrequency;
				SoundEngine.PlaySound(SoundID.NPCDeath19 with { Volume = 0.5f }, Projectile.Center);
				Projectile.ai[1]++;

				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FriendlyCreeper>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.identity, Main.rand.NextFloat() * 2 - 1);
				}
			}
			
			ShootTimer -= Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
			
			Projectile.frameCounter += 1;
			if (Projectile.frameCounter % 24 == 0)
			{
				Projectile.frame = (Projectile.frame + 1) % 4;
				if (Projectile.frameCounter >= 96)
				{
					Projectile.frameCounter = 0;
				}
			}
		}
	}
}