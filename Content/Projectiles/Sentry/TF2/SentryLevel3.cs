using System;
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
	public class SentryLevel3 : ModProjectile
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
			Projectile.width = 48;
			Projectile.height = 72;
			Projectile.ArmorPenetration = 20;
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
			const int ShootFrequency = 8;
			const int TargetingRange = 60 * 16;

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

				Vector2 bulletOrigin = new Vector2(Projectile.Center.X - 10, Projectile.Center.Y - 16);
				Vector2 shootVelocity = targetNPC.Center - bulletOrigin;

				shootVelocity.Normalize();
				shootVelocity *= 16;

				float spread = MathHelper.ToRadians(4.5f);
				shootVelocity = shootVelocity.RotatedBy(Main.rand.NextFloat(-spread, spread));
				Projectile.rotation = shootVelocity.ToRotation();

				bulletOrigin += shootVelocity * 2;
				Dust.NewDust(bulletOrigin, 16, 16, DustID.Torch);
				Projectile.frame = 1;
				if (Main.myPlayer == Projectile.owner)
				{
					int adddamage = (int)Main.LocalPlayer.GetTotalDamage<SentryDamageClass>().ApplyTo(5) + 1;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), bulletOrigin, shootVelocity, ProjectileID.Bullet, Projectile.damage + adddamage, Projectile.knockBack, Projectile.owner);
				}
			}

			if (targetNPC != null && Projectile.timeLeft % 180 == 0 && Projectile.timeLeft < Projectile.SentryLifeTime - 180)
			{
				SoundEngine.PlaySound(SoundID.Item41 with { Volume = 0.3f }, Projectile.Center);

				Vector2 bulletOrigin = new Vector2(Projectile.Center.X - 10, Projectile.Center.Y - 16);
				Vector2 shootVelocity = targetNPC.Center - bulletOrigin;

				shootVelocity.Normalize();
				shootVelocity *= 12;

				bulletOrigin += shootVelocity * 4;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), bulletOrigin, shootVelocity, ModContent.ProjectileType<FriendlyRocket>(), 100, Projectile.knockBack, Projectile.owner);
				}
			}

			if (ShootTimer < 4)
            {
                Projectile.frame = 0;
            }

			ShootTimer -= Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition - new Vector2(0, 14);
			Vector2 gunOrigin = new Vector2(46, 32);
			Vector2 standOrigin = new Vector2(44, 0);

			Rectangle turretRectangle = new Rectangle(0, 56 * Projectile.frame, 88, 56); // turret rect in texture
			Rectangle standRectangle = new Rectangle(0, 112, 60, 50); // stand rect in texture

			SpriteEffects effect = SpriteEffects.None;
			float rotation = Projectile.rotation;

			if (rotation < -MathHelper.PiOver2 || rotation > MathHelper.PiOver2)
			{
				rotation += MathHelper.Pi;
				effect = SpriteEffects.FlipHorizontally;

				gunOrigin.X = turretRectangle.Width - 46;
				standOrigin.X = standRectangle.Width - 44;
			}

			Main.EntitySpriteDraw( // stand part
				texture,
				drawPosition,
				standRectangle,
				lightColor,
				0,
				standOrigin,
				Projectile.scale,
				effect,
				0
			);

			Main.EntitySpriteDraw( // gun part
				texture,
				drawPosition + new Vector2(0, 8),
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

		private void TryTargeting(NPC npc, ref float closestTargetDistance, ref NPC targetNPC) {
			if (npc.CanBeChasedBy(this)) {
				float distanceToTargetNPC = Vector2.Distance(Projectile.Center, npc.Center);
				if (distanceToTargetNPC < closestTargetDistance && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height)) {
					closestTargetDistance = distanceToTargetNPC;
					targetNPC = npc;
				}
			}
		}
	}
}