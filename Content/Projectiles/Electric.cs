using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Projectiles
{
	public class Electric : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 10;
            Projectile.netImportant = true; // Sentries need this so they are synced to newly joining players
        }

        public override void OnSpawn(IEntitySource source)
        {
            Dust.NewDust(Projectile.position, Projectile.width, 8, DustID.Electric, 0, -2);

            foreach (int i in Projectile.ai)
            {
                if (i == -1) break;
                
                NPC targetNPC = Main.npc[i];
                Dust.NewDust(targetNPC.position, targetNPC.width, targetNPC.height, DustID.Electric, 0, -2);
                if (Main.myPlayer == Projectile.owner)
                {
                    NPC.HitInfo hit = new NPC.HitInfo()
                    {
                        Damage = (int)(Projectile.damage * Main.rand.NextFloat(0.9f, 1.1f)),
                        Knockback = 0,
                        HitDirection = 0,
                        Crit = false,
                        DamageType = DamageClass.Summon
                    };

                    if (targetNPC.HasBuff(BuffID.Wet))
                    {
                        hit.Damage = (int)(hit.Damage * 1.5f);
                    }

                    Main.LocalPlayer.StrikeNPCDirect(targetNPC, hit);
                }

                Projectile.damage = (int)(Projectile.damage * 0.8f) - 15;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            float scaleY = 0.65f;
            Vector2 position = Projectile.Center;

            for (int npc = 0; npc < 3; npc++)
            {
                int id = (int)Projectile.ai[npc];
                if (id == -1) return false;

                Vector2 enemyPos = Main.npc[id].Center;

                for (int i = 0; i < 25 && position.Distance(enemyPos) > 32; i++)
                {
                    float rotation = (enemyPos - position).ToRotation() + Main.rand.NextFloat(-0.4f, 0.4f);
                    float length = Main.rand.Next(16, 32);

                    Main.spriteBatch.Draw(
                        texture,
                        position - Main.screenPosition,
                        null,
                        Color.White,
                        rotation,
                        Vector2.UnitY * 8,
                        new Vector2(length / 8, scaleY), // width 2px
                        SpriteEffects.None,
                        0f
                    );

                    float cos = (float)Math.Cos(rotation);
                    float sin = (float)Math.Sin(rotation);

                    Vector2 rotatedOffset = new Vector2(
                        length * cos * 1.9f,
                        length * sin * 1.9f
                    );

                    scaleY -= 0.022f;
                    position += rotatedOffset;
                }
            }
            
            return false;
        }
	}
}