using System;
using Microsoft.Xna.Framework;
using MoreSentries.Common;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles
{
	public class CactusNeedle : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.SentryShot[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CritChance = 0;
			Projectile.width = 20;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 600;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == -1) return;

			NPC npc = Main.npc[(int)Projectile.ai[0]];
			if (!npc.active || npc.GetGlobalNPC<DoTNPC>().CactusNeedles > 20) {
				Projectile.active = false;
				return;
			}

			Vector2 pos = npc.position + new Vector2(Projectile.ai[1], Projectile.ai[2]);
			Projectile.position = pos;
			if (Projectile.timeLeft % 20 == 0) {
				if (Projectile.owner == Main.myPlayer) Main.LocalPlayer.dpsDamage++;
				npc.GetGlobalNPC<DoTNPC>().CactusNeedles++;
			}
		}

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.position.AngleTo(Projectile.position + Projectile.velocity);
        }

        public override void OnKill(int timeLeft)
        {
			if (Projectile.ai[0] == -1) return;
            SoundEngine.PlaySound(SoundID.Item64 with { Volume = 0.5f, Pitch = 0.5f }, Projectile.Center);
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.ai[0] = target.whoAmI;
			Projectile.ai[1] = Projectile.position.X - target.position.X;
			Projectile.ai[2] = Projectile.position.Y - target.position.Y;
			Projectile.timeLeft = 599;
			Projectile.netUpdate = true;
		}

        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.ai[0] == -1 && !target.friendly;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return Projectile.ai[0] == -1;
        }
	}
}