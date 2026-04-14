using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles
{
	public class Twinkle : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.SentryShot[Type] = true;
			Main.projFrames[Type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 56;
			Projectile.height = 56;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 100;
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesIDStaticNPCImmunity = false;
		}

        public override void AI()
        {
            if (Main.rand.NextBool(4))
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted, Vector2.Zero, 100, Color.Aqua, 0.5f);
            }

            Lighting.AddLight(Projectile.Center, 0, 0.9f, 0.9f);

            if (Projectile.timeLeft > 60)
            {
                Projectile.velocity.Y += 0.7f;
            }
            else if (Projectile.timeLeft == 60)
            {
                NPC npc = MoreSentries.NearestTarget(Projectile, 4096, true);
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[0] = -1;

                if (npc != null)
                {
                    Projectile.ai[0] = npc.whoAmI;
                }
            }
            else
            {
                if (Projectile.ai[0] != -1)
                {
                    float dividend = Projectile.timeLeft * Projectile.timeLeft / 10;
                    Projectile.velocity = (Main.npc[(int)Projectile.ai[0]].Center - Projectile.Center) / dividend;
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			return false;
		}

        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.timeLeft <= 60 && !target.friendly;
        }
	}
}