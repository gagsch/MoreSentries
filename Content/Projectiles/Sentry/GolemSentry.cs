using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry
{
	public class GolemSentry : ModProjectile
	{
		public static Rectangle[] stages = {
			new Rectangle(0, 0, 70, 68),
			new Rectangle(0, 68, 82, 78),
			new Rectangle(0, 146, 90, 92),
		};

		public ref float ShootTimer => ref Projectile.ai[0];

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.DontAttachHideToAlpha[Type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 70;
			Projectile.height = 68;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.sentry = true;
			Projectile.timeLeft = Projectile.SentryLifeTime; // Sentries last 10 minutes
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
			Projectile.hide = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false; // Allow this projectile to collide with platforms
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false; // Prevent tile collision from killing the projectile
		}

		public override void AI()
		{
			const int ShootFrequency = 30;
			const int TargetingRange = 40 * 16;

			Projectile.velocity.X = 0f;
			Projectile.velocity.Y += 0.2f;

			float closestTargetDistance = TargetingRange;
			NPC targetNPC = null;

			if (Projectile.OwnerMinionAttackTargetNPC != null)
			{
				TryTargeting(Projectile.OwnerMinionAttackTargetNPC, ref closestTargetDistance, ref targetNPC);
			}
			else
			{
				foreach (var npc in Main.ActiveNPCs)
				{
					TryTargeting(npc, ref closestTargetDistance, ref targetNPC);
				}
			}

			if (ShootTimer-- <= 0 && targetNPC != null)
			{
				Projectile.ai[2]++;
				ShootTimer = ShootFrequency;

				SoundEngine.PlaySound(SoundID.Item10 with { Volume = 0.7f }, Projectile.Center);

				if (Main.myPlayer == Projectile.owner)
				{
					int damage = (int)(Projectile.damage * (Projectile.ai[1] / 1.5f + 1));
					Vector2 fistPos = Projectile.Center;
					fistPos.X += Projectile.width * (Projectile.ai[2] % 2 == 0 ? -1 : 1) / 2;
					fistPos.Y += 8;

					Vector2 shootVelocity = (targetNPC.Center - fistPos).SafeNormalize(Vector2.UnitX) * 20;
					Projectile.NewProjectile(
						Projectile.GetSource_FromThis(),
						fistPos,
						shootVelocity,
						ModContent.ProjectileType<GolemSentryFist>(),
						damage,
						Projectile.knockBack,
						Projectile.owner,
						Projectile.AngleTo(Projectile.Center + shootVelocity),
						Projectile.frame == 2 ? 1 : 0
					);
				}
			}

			int newFrame = Math.Min((int)(Projectile.ai[1] / 2.5f), 2);

			if (newFrame != Projectile.frame)
            {
                Projectile.frame = Math.Min((int)Projectile.ai[1] / 3, 2);
				int newWidth = stages[Projectile.frame].Width;
				int newHeight = stages[Projectile.frame].Height;

				Projectile.position.X += Projectile.width - newWidth;
				Projectile.position.Y += Projectile.height - newHeight;

				Projectile.width = newWidth;
				Projectile.height = newHeight;
            }
			
			Projectile.ai[1] = 0;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawPosition = Projectile.position - Main.screenPosition;

			Rectangle fist;
			Vector2 leftFistOffset;
			Vector2 rightFistOffset;
			switch (Projectile.frame)
            {
                case 0:
					{
						fist = new Rectangle(70, 0, 22, 20);
						leftFistOffset = new Vector2(-12, 28);
						rightFistOffset = new Vector2(60, 28);
						break;
					}
				case 1:
					{
						fist = new Rectangle(70, 0, 22, 20);
						leftFistOffset = new Vector2(-10, 34);
						rightFistOffset = new Vector2(66, 34);
						break;
					}
				case 2:
					{
						fist = new Rectangle(70, 20, 30, 28);
						leftFistOffset = new Vector2(-16, 38);
						rightFistOffset = new Vector2(76, 38);
						break;
					}
				default:
                    {
						return false;
                    }
            }

			Main.EntitySpriteDraw(
				texture,
				drawPosition,
				stages[Projectile.frame],
				lightColor,
				Projectile.rotation,
				Vector2.Zero,
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			if (Projectile.ai[2] % 2 == 0 || ShootTimer <= 0)
			{
				Main.EntitySpriteDraw(
					texture,
					drawPosition + rightFistOffset,
					fist,
					lightColor,
					Projectile.rotation,
					Vector2.Zero,
					Projectile.scale,
					SpriteEffects.None,
					0
				);
			}

			if (Projectile.ai[2] % 2 == 1 || ShootTimer <= 0)
			{
				Main.EntitySpriteDraw(
					texture,
					drawPosition + leftFistOffset,
					fist,
					lightColor,
					Projectile.rotation,
					Vector2.Zero,
					Projectile.scale,
					SpriteEffects.FlipHorizontally,
					0
				);
            }

			return false;
		}

		private void TryTargeting(NPC npc, ref float closestTargetDistance, ref NPC targetNPC)
		{
			if (npc.CanBeChasedBy(this))
			{
				Vector2 fistCenter = Projectile.Center;
				fistCenter.X += Projectile.width * (Projectile.ai[2] % 2 == 0 ? 1 : -1) / 2;
				float distanceToTargetNPC = Vector2.Distance(fistCenter, npc.Center);
				if (distanceToTargetNPC < closestTargetDistance && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
				{
					closestTargetDistance = distanceToTargetNPC;
					targetNPC = npc;
				}
			}
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindProjectiles.Add(index);
        }
	}
	
	public class HiddenGolemSentry : ModProjectile
	{
		public override string Texture => "MoreSentries/Content/Projectiles/Sentry/GolemSentry";

		public override void SetDefaults()
		{
			Projectile.width = 0;
			Projectile.height = 68;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.sentry = true; // Sets the weapon as a sentry for sentry accessories to properly work.
			Projectile.timeLeft = Projectile.SentryLifeTime; // Sentries last 10 minutes
			Projectile.ignoreWater = true;
			Projectile.netImportant = true; // Sentries need this so they are synced to newly joining players
		}

        public override void OnSpawn(IEntitySource source)
		{
			foreach (Projectile proj in Main.ActiveProjectiles)
			{
				if (proj.owner == Projectile.owner && proj.type == ModContent.ProjectileType<GolemSentry>())
				{
					Projectile.ai[0] = proj.identity;
					return;
				}
			}

			Projectile.NewProjectile(source, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GolemSentry>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
			Projectile.Kill();
        }

		public override bool PreAI()
		{
			Projectile mainPiece = Main.projectile.FirstOrDefault(x => x.identity == (int)Projectile.ai[0]);

			if (!mainPiece.active || mainPiece.owner != Projectile.owner || mainPiece.type != ModContent.ProjectileType<GolemSentry>())
			{
				Projectile.Kill();
				return false;
			}

			mainPiece.ai[1]++;
			Projectile.position = mainPiece.position;
			Projectile.timeLeft = mainPiece.timeLeft;

			return false;
		}

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false; // Allow this projectile to collide with platforms
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false; // Prevent tile collision from killing the projectile
		}

        public override bool PreDraw(ref Color lightColor)
        {
			return false;
        }
    }
}