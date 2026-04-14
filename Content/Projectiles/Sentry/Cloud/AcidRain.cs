using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry.Cloud
{
    public class AcidRain : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.RainFriendly);
            Projectile.height = 36;
            Projectile.width = 2;
        }

        public override void AI()
        {
            Vector2 dustPos = Projectile.Center;
            dustPos.Y += 24;
            Lighting.AddLight(dustPos, 0.3f, 0.4f, 0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 40);
        }

        public override void OnKill(int timeLeft)
        {
            Vector2 dustPos = Projectile.Center;
            dustPos.Y += 24;
            Dust.NewDustPerfect(dustPos, DustID.PureSpray, -Vector2.UnitY, 50, Color.Gray, 0.5f);
        }
    }
}