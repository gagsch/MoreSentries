using System;
using Microsoft.Xna.Framework;
using MoreSentries.Common;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry
{
	public class HiveSentry : ModProjectile
	{
		public ref float ShootTimer => ref Projectile.ai[0];

		public bool JustSpawned {
			get => Projectile.localAI[0] == 0;
			set => Projectile.localAI[0] = value ? 0 : 1;
		}

		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 2;
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 46;
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
			Projectile.ai[1] = oldVelocity.Y > 0 ? 1 : 0;
			Projectile.velocity.Y = 0;
			Projectile.rotation = 0;
			return false; // Prevent tile collision from killing the projectile
		}

		public override void AI()
		{
			Projectile.frame = (int)Projectile.ai[1];
			const int ShootFrequency = 100;
			const int TargetingRange = 30 * 16;

			float tileX = Projectile.position.X / 16f;
			float tileY = Projectile.position.Y / 16f;
			if (IsTileAbove(tileX, tileY))
			{
				Projectile.velocity.Y = 0;
			}
			else
			{
				Projectile.velocity.Y += 0.3f;
				Projectile.rotation += Projectile.velocity.Y / 50;
			}

			if (JustSpawned)
			{
				JustSpawned = false;
				ShootTimer = ShootFrequency;

				SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
			}

			if (MoreSentries.NearestTarget(Projectile, TargetingRange) != null && ShootTimer <= 0)
			{
				ShootTimer = ShootFrequency;

				SoundEngine.PlaySound(SoundID.Item97 with { Volume = 0.4f }, Projectile.Center);

				if (Main.myPlayer == Projectile.owner)
				{
					for (int i = 0; i < 4; i++)
					{
						Vector2 shootDirection = new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootDirection, Main.LocalPlayer.beeType(), Projectile.damage, Projectile.knockBack, Projectile.owner);
					}
				}
			}

			ShootTimer -= Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
		}

		private bool IsTileAbove(float tileX, float tileY)
		{
			tileY -= 1;
			int itileX = (int)Math.Round(tileX);
			int itileY = (int)Math.Round(tileY);

			bool isTileAbove = false;

			for (int x = itileX; x < itileX + 3; x++)
			{
				Tile tile = Main.tile[x, itileY];
				if (tile.HasUnactuatedTile && Main.tileSolid[tile.TileType])
				{
					isTileAbove = true;
					break;
				}
			}
			
			return isTileAbove;
		}
	}
}