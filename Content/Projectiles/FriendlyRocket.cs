using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles;

public class FriendlyRocket : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.CritChance = 0;
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Summon;
		Projectile.timeLeft = 600;
	}

    public override bool? CanHitNPC(NPC target)
    {
        if (!target.friendly && Projectile.Hitbox.Intersects(target.Hitbox))
        {
            Projectile grenade = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.JackOLantern, Projectile.damage, 4, Projectile.owner);
            grenade.timeLeft = 1;  
            Projectile.Kill();
        }
        return false;
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Projectile.AngleTo(Projectile.Center + Projectile.velocity);
    }

    public override void AI()
    {
        Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0, 0, 0);
        dust.noGravity = true;
        dust.noLight = true;
    }
}