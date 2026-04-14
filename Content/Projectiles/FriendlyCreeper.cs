using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles
{
	public class FriendlyCreeper : ModProjectile
	{
		private const int ExplosionWidthHeight = 180;

		public override void SetStaticDefaults() {
			ProjectileID.Sets.SentryShot[Type] = true;
			Main.projFrames[Type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.CritChance = 0;
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = Projectile.SentryLifeTime;
		}

		public override void AI()
		{
			Projectile brain = Main.projectile[(int)Projectile.ai[0]];
			if (brain == null || !brain.active)
			{
				Projectile.Kill();
				return;
			}

			float brainTimeLeft = brain.timeLeft / 24f;
			float degrees = brainTimeLeft * Projectile.ai[1];
			int distance = (int) (Math.Sin(Projectile.timeLeft / 8f) * 16f) + 64;

			Projectile.position = brain.position;
			Projectile.position.X += distance * (float)Math.Cos(degrees) + 4;
			Projectile.position.Y += distance * (float)Math.Sin(degrees);
		}

		public override void OnKill(int timeLeft)
		{
			Projectile brain = Main.projectile[(int)Projectile.ai[0]];
			if (brain != null && brain.active)
            {
				brain.ai[1]--;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Confused, 240);
        }
	}
}