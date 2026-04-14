using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry
{
	public class VenusSentry : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 3;
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 50;
			Projectile.height = 26;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.sentry = true;
			Projectile.ArmorPenetration = 15;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;

			Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 48;
            Projectile.usesIDStaticNPCImmunity = false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			return false;
		}

        public override void OnSpawn(IEntitySource source)
        {
			Projectile.ai[0] = Projectile.Center.X;
			Projectile.ai[1] = Projectile.Center.Y;
        }

		public override void AI()
		{
			const int TargetingRange = 32 * 16;

			Vector2 targetPosition = Vector2.Zero;
			Vector2 originalPos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			Projectile.rotation = Projectile.AngleTo(originalPos) - MathHelper.PiOver2;

			float closestTargetDistance = TargetingRange;
			NPC targetNPC = null;

			if (Projectile.OwnerMinionAttackTargetNPC != null) {
				TryTargeting(Projectile.OwnerMinionAttackTargetNPC, ref closestTargetDistance, ref targetNPC);
			}

			if (targetNPC == null)
			{
				foreach (var npc in Main.ActiveNPCs)
				{
					TryTargeting(npc, ref closestTargetDistance, ref targetNPC);
				}
			}

			Projectile.frame = 0;

			if (targetNPC != null)
			{
				if (Projectile.Hitbox.Intersects(targetNPC.Hitbox))
				{
					Projectile.Center = targetNPC.Center;
					Projectile.frame = 1;
				}
				else
				{
					targetPosition = targetNPC.Center;
				}
			}
			else
			{
				float randX = (float)Math.Cos(Projectile.timeLeft / 60 + Projectile.position.Y) * 1024;
				float randY = (float)Math.Sin(Projectile.timeLeft / 60 + Projectile.position.X) * 1024;
				targetPosition = originalPos + new Vector2(randX, randY);
			}

			if (targetPosition != Vector2.Zero)
            {
                Projectile.velocity += (targetPosition - Projectile.Center).SafeNormalize(Vector2.UnitX);
            }
			
			Projectile.velocity /= 1.1f;
		}


		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Rectangle vineOriginRect = new Rectangle(32, 56, 18, 18);
			Vector2 originalPos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			float rotation = (Projectile.Center - new Vector2(0, 9)).AngleTo(originalPos);
			int dist = (int)Projectile.Distance(originalPos);
			int totalPixelLen = 0;

			while (totalPixelLen < dist)
			{
				int drawWidth = Math.Min(32, dist - totalPixelLen);
				totalPixelLen += drawWidth;
				Vector2 offset = new Vector2(originalPos.X - (float)(totalPixelLen * Math.Cos(rotation)), originalPos.Y - (float)(totalPixelLen * Math.Sin(rotation)));
				Rectangle vineRect = new Rectangle(32 - drawWidth, 56, drawWidth, 18);
				Color color = Lighting.GetColor((int)offset.X / 16, (int)offset.Y / 16);
				Main.spriteBatch.Draw(
					texture,
					offset - Main.screenPosition + new Vector2(0, 9),
					vineRect,
					color,
					rotation,
					new Vector2(0, 9),
					Projectile.scale,
					SpriteEffects.None,
					0
				);
			}

			Main.EntitySpriteDraw(
				texture,
				originalPos - Main.screenPosition - new Vector2(9, 0),
				vineOriginRect,
				Lighting.GetColor((int)(originalPos.X / 16f), (int)(originalPos.Y / 16f)),
				0,
				Vector2.Zero,
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			return true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.owner == Main.myPlayer && Main.rand.NextBool(5))
			{
				Main.LocalPlayer.Heal(Main.rand.Next(1, 5));
			}
		}

		private void TryTargeting(NPC npc, ref float closestTargetDistance, ref NPC targetNPC) {
			if (npc.CanBeChasedBy(this)) {
				float distanceToTargetNPC = Vector2.Distance(new Vector2(Projectile.ai[0], Projectile.ai[1]), npc.Center);
				if (distanceToTargetNPC < closestTargetDistance) {
					closestTargetDistance = distanceToTargetNPC;
					targetNPC = npc;
				}
			}
		}
	}
}