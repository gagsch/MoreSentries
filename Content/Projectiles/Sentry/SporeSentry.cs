using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoreSentries.Common;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry
{
	public class SporeSentry : ModProjectile
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
			Projectile.width = 42;
			Projectile.height = 40;
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

			if (JustSpawned) {
				JustSpawned = false;
				ShootTimer = ShootFrequency * 1.5f;

				SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
			}

			Projectile.velocity.X = 0f;
			Projectile.velocity.Y += 0.2f;
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}
			
			if (ShootTimer <= 0 && Main.myPlayer == Projectile.owner)
			{
                ShootTimer = ShootFrequency;
                Vector2 pos = new Vector2(Projectile.Center.X, Projectile.Center.Y);
                pos.X += Main.rand.Next(-150,150);
                pos.Y += Main.rand.Next(-150,150);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<Spore>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
            Projectile.frame = 2 - (int)(ShootTimer / (ShootFrequency / 3));
            Projectile.frame = Math.Clamp(Projectile.frame, 0, 3 - 1);
			
			ShootTimer -= Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D baseTex = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D glowTex = ModContent.Request<Texture2D>(
				"MoreSentries/Content/Projectiles/Sentry/SporeSentry_Glow"
			).Value;

			Rectangle frameRect = new Rectangle(0, 40 * Projectile.frame, 42, 40);
			Vector2 origin = new Vector2(Projectile.width / 2, Projectile.height / 2);
			Vector2 pos = Projectile.Center - Main.screenPosition;

			// Draw base sprite (vanilla lighting)
			Main.EntitySpriteDraw(
				baseTex,
				pos,
				frameRect,
				lightColor,
				Projectile.rotation,
				origin,
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			// Draw glow overlay (full brightness)
			Main.EntitySpriteDraw(
				glowTex,
				pos,
				frameRect,
				Color.White,
				Projectile.rotation,
				origin,
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			return false; // We handled drawing ourselves
		}

	}
}