using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles
{
	public class PoisonDart : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.SentryShot[Type] = true;
			Main.projFrames[Type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.CritChance = 0;
			Projectile.width = 10;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 4;
		}

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.X > 0 ? 1.570796f : 4.712389f; // 90 and 270 degrees to radians
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.Poisoned, 360);
        }
	}
}