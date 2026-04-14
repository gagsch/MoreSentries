using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries
{
	public class MoreSentries : Mod
	{
		public static bool targetCheck(Projectile projectile, NPC npc, ref float maxDistance, bool ignoreLineOfSight = false)
		{
			if (npc.CanBeChasedBy(projectile)) {
				float distanceToTargetNPC = Vector2.Distance(projectile.Center, npc.Center);
				if (distanceToTargetNPC < maxDistance && (ignoreLineOfSight || Collision.CanHit(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))) {
					maxDistance = distanceToTargetNPC;
					return true;
				}
			}
			return false;
		}

		public static NPC NearestTarget(Projectile projectile, float maxDistance, bool ignoreLineOfSight = false, bool checkMinionAttackTarget = true)
		{
			if (checkMinionAttackTarget && projectile.OwnerMinionAttackTargetNPC != null && targetCheck(projectile, projectile.OwnerMinionAttackTargetNPC, ref maxDistance, ignoreLineOfSight)) {
				return projectile.OwnerMinionAttackTargetNPC;
			}

			NPC targetNPC = null;
			foreach (var npc in Main.ActiveNPCs)
			{
				if (targetCheck(projectile, npc, ref maxDistance, ignoreLineOfSight)) targetNPC = npc;
			}

			return targetNPC;
		}

		public static void DamageOverTimeHit(NPC npc, int damage)
		{
			if (npc.life <= 0)
				return;

			npc.life -= damage;

			if (Main.netMode != NetmodeID.Server)
			{
				CombatText.NewText(
					npc.Hitbox,
					new Color(230, 140, 50),
					damage,
					false,
					true
				);
			}

			if (npc.life <= 0)
			{
				npc.life = 0;
				npc.checkDead();
			}
		}
	}
}
