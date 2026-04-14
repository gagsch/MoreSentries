using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles
{
	public class PrimeLaser : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.SentryShot[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CritChance = 0;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.scale = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 3;
			Projectile.extraUpdates = 2;
			Projectile.alpha = 255;

			Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesIDStaticNPCImmunity = false;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 0.75f);
        }

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.9f, 0.1f, 0.3f);
		}

        public override void OnSpawn(IEntitySource source)
		{
			Projectile.rotation = Projectile.position.AngleTo(Projectile.position + Projectile.velocity) + MathHelper.PiOver2;
			SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
			Projectile.alpha = 0;
        }
	}
}