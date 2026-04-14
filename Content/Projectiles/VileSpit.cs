using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles
{
	public class VileSpit : ModProjectile
	{
		private const int ExplosionWidthHeight = 140;

		public override void SetStaticDefaults() {
			ProjectileID.Sets.SentryShot[Type] = true;
			Main.projFrames[Type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.CritChance = 0;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 2;
			Projectile.scale = 0.8f;
		}

        public override void AI()
		{
			if (Main.rand.NextBool(4))
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone);
			}
			
			Projectile.velocity.Y += 0.3f;
			Projectile.rotation += 0.2f;
        }
	}
}