using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Common.GlobalProjectiles
{
    public class DDR2Sentry : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile proj, bool lateInstantiation)
        {
            return proj.type == ProjectileID.DD2BallistraTowerT1 || proj.type == ProjectileID.DD2BallistraTowerT2 || proj.type == ProjectileID.DD2BallistraTowerT3 || proj.type == ProjectileID.DD2FlameBurstTowerT1 || proj.type == ProjectileID.DD2FlameBurstTowerT2 || proj.type == ProjectileID.DD2FlameBurstTowerT3;
        }

        public override bool PreAI(Projectile projectile)
        {
            if (projectile.ai[0] == 1f && projectile.ai[1] > 0f)
            {
                projectile.ai[1] -= Main.player[projectile.owner].GetModPlayer<MoreSentriesPlayer>().SentryFireRate - 1;
                if (projectile.ai[1] < 0f)
                    projectile.ai[1] = 0f;
            }
            return true;
        }
    }
}