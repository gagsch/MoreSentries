using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoreSentries.Common;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry.TF2
{
	public class SentryLevel1 : ModProjectile
	{
		public ref float ShootTimer => ref Projectile.ai[0];

		public bool JustSpawned {
			get => Projectile.localAI[0] == 0;
			set => Projectile.localAI[0] = value ? 0 : 1;
		}

		public override void SetStaticDefaults() {
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 56;
			Projectile.height = 56;
			Projectile.ArmorPenetration = 12;
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
			const int ShootFrequency = 13;
			const int TargetingRange = 40 * 16;

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

				SoundEngine.PlaySound(SoundID.Item41 with { Volume = 0.3f }, Projectile.Center);

				Vector2 bulletOrigin = new Vector2(Projectile.Center.X - 10, Projectile.Center.Y - 4);
				Vector2 shootVelocity = targetNPC.Center - bulletOrigin;

				shootVelocity.Normalize();
				shootVelocity *= 16;

				float spread = MathHelper.ToRadians(3f);
				shootVelocity = shootVelocity.RotatedBy(Main.rand.NextFloat(-spread, spread));
				Projectile.rotation = shootVelocity.ToRotation();

				bulletOrigin += shootVelocity * 2;
				Dust.NewDust(bulletOrigin, 16, 16, DustID.Torch);
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), bulletOrigin, shootVelocity, ProjectileID.Bullet, Projectile.damage, Projectile.knockBack, Projectile.owner);
				}
			}
			
			ShootTimer -= Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition - new Vector2(0, 8);
			Vector2 gunOrigin = new Vector2(32, 14);

			Rectangle turretRectangle = new Rectangle(0, 0, 52, 30); // turret rect in texture
			Rectangle standRectangle = new Rectangle(0, 30, 40, 36); // stand rect in texture

			SpriteEffects effect = SpriteEffects.None;
			float rotation = Projectile.rotation;

			if (rotation < -MathHelper.PiOver2 || rotation > MathHelper.PiOver2)
			{
				rotation += MathHelper.Pi;
				effect = SpriteEffects.FlipHorizontally;

				gunOrigin.X = turretRectangle.Width - 32;
			}

			Main.EntitySpriteDraw( // stand part
				texture,
				drawPosition + new Vector2(4, 0),
				standRectangle,
				lightColor,
				0,
				new Vector2(24, 0),
				Projectile.scale,
				effect,
				0
			);

			Main.EntitySpriteDraw( // gun part
				texture,
				drawPosition,
				turretRectangle,
				lightColor,
				rotation,
				gunOrigin,
				Projectile.scale,
				effect,
				0
			);

			return false;
        }
	}
}