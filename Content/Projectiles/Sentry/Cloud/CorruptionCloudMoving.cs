using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles.Sentry.Cloud
{
    public class CorruptionCloudMoving : ModProjectile
    {
        public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
		}

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BloodCloudMoving);
        }

        public override void AI()
        {
            float posX = Projectile.ai[0];
            float posY = Projectile.ai[1];

            bool matchedX = false;
            bool matchedY = false;
            if (Projectile.velocity.X == 0f || (Projectile.velocity.X < 0f && Projectile.Center.X < posX) || (Projectile.velocity.X > 0f && Projectile.Center.X > posX))
            {
                Projectile.velocity.X = 0f;
                matchedX = true;
            }
            if (Projectile.velocity.Y == 0f || (Projectile.velocity.Y < 0f && Projectile.Center.Y < posY) || (Projectile.velocity.Y > 0f && Projectile.Center.Y > posY))
            {
                Projectile.velocity.Y = 0f;
                matchedY = true;
            }
            if (Projectile.owner == Main.myPlayer && matchedX && matchedY)
            {
                Projectile.Kill();
            }

            Projectile.rotation += Projectile.velocity.X * 0.02f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<CorruptionCloudRaining>(),
                    Projectile.damage,
                    Projectile.knockBack
                );
            }
        }
    }
}