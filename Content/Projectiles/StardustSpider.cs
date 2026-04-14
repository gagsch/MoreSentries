using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles
{
	public class StardustSpider : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.SentryShot[Type] = true;
			Main.projFrames[Type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.CritChance = 0;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 300;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			NPC target = Main.npc[(int)Projectile.ai[0]];
			fallThrough = target.position.Y > Projectile.position.Y; // Allow this projectile to collide with platforms
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			return false; // Prevent tile collision from killing the projectile
		}

		public override void AI()
		{
			NPC target = Main.npc[(int)Projectile.ai[0]];
			Lighting.AddLight(Projectile.Center, 0.4f, 0.6f, 1f);

			float distance = Math.Abs(target.Center.X - Projectile.Center.X);
			if (distance < 8 && Projectile.timeLeft <= 200)
			{
				Projectile.Kill();
			}

			if (Projectile.velocity.X == 0)
			{
				Projectile.rotation = -1.570f * Projectile.ai[1];
				Projectile.velocity.Y = -6f;
			}
			else
			{
				Projectile.rotation = 0;
			}

			if (target.position.X > Projectile.position.X && Projectile.velocity.X <= 7)
			{
				Projectile.ai[1] = 1;
				Projectile.velocity.X += 0.2f;
			}
			else if (Projectile.velocity.X >= -7)
			{
				Projectile.ai[1] = -1;
				Projectile.velocity.X -= 0.2f;
			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter > 8)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame > 4)
					Projectile.frame = 0;
			}

			Projectile.velocity.Y += 0.6f;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath7 with { Volume = 0.3f }, Projectile.Center);

			if (Main.myPlayer != Projectile.owner) return;

			for (int i = Main.rand.Next(0,2); i < 3; i++)
			{
				Vector2 shootVelocity = new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-24, -20));

				Projectile.NewProjectile(
					Projectile.GetSource_FromThis(),
					new Vector2(Projectile.Center.X, Projectile.Center.Y),
					shootVelocity,
					ModContent.ProjectileType<Twinkle>(),
					(int)(Projectile.damage / 1.345f),
					3,
					Projectile.owner
				);
			}
        }
	}
}