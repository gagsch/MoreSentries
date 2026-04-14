using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MoreSentries.Common;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry
{
	public class TeslaSentry : ModProjectile
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
			Projectile.width = 30;
			Projectile.height = 46;
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
			float TargetingRange = 40 * 16;
			int ShootFrequency = 24;

			if (JustSpawned)
			{
				JustSpawned = false;
				ShootTimer = ShootFrequency;

				SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
			}

			Projectile.velocity.X = 0f;
			Projectile.velocity.Y += 0.2f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}

			if (ShootTimer > 0)
			{
				ShootTimer -= Main.player[Projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate;
				Projectile.frame = ShootTimer > ShootFrequency / 2 ? 1 : 0;
				return;
			}

			List<NPC> validList = BuildValidList(TargetingRange);
			NPC targetNPC = null;
			float closestTargetDistance = TargetingRange;
			int[] chain = { -1, -1, -1 };

			foreach (var npc in validList)
			{
				TryTargeting(npc, ref closestTargetDistance, ref targetNPC, Projectile.position);
			}

			if (targetNPC != null)
			{
				validList.Remove(targetNPC);
				chain[0] = targetNPC.whoAmI;
				float elapsedDistance = closestTargetDistance;
				for (int i = 1; i <= 2; i++)
				{
					Vector2 position = targetNPC.Center;
					closestTargetDistance = TargetingRange;
					foreach (var npc in validList)
					{
						TryTargeting(npc, ref closestTargetDistance, ref targetNPC, position);
					}
					elapsedDistance += closestTargetDistance;
					if (chain[i - 1] == targetNPC.whoAmI || elapsedDistance > TargetingRange) break;
					validList.Remove(targetNPC);
					chain[i] = targetNPC.whoAmI;
				}

				Lighting.AddLight(Projectile.Center, 0.4f, 0.4f, 0.8f);

				SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap with { Volume = 0.6f }, Projectile.Center);
				ShootTimer = ShootFrequency;

				if (Main.myPlayer != Projectile.owner) return;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 16), Vector2.Zero, ModContent.ProjectileType<Electric>(), Projectile.damage, 0, Projectile.owner, chain[0], chain[1], chain[2]);
			}
		}
		
		private List<NPC> BuildValidList(float targetingRange)
		{
			List<NPC> list = new List<NPC>(Main.maxNPCs);
			int sqrd = (int)(targetingRange * targetingRange);
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.CanBeChasedBy(this) && Vector2.DistanceSquared(Projectile.Center, npc.Center) < sqrd)
                {
                    list.Add(npc);
                }
            }
			return list;
        }

		private void TryTargeting(NPC npc, ref float closestTargetDistance, ref NPC targetNPC, Vector2 position)
		{
			float dist = Vector2.DistanceSquared(position, npc.Center);
			if (dist < closestTargetDistance * closestTargetDistance)
			{
				closestTargetDistance = (float)Math.Sqrt(dist);
				targetNPC = npc;
			}
		}
	}
}