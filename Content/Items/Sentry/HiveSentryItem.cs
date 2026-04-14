using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MoreSentries.Content.Projectiles.Sentry;

namespace MoreSentries.Content.Items.Sentry
{
	public class HiveSentryItem : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
		}

		public override void SetDefaults() {
			Item.damage = 11;
			Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
			Item.mana = 10;
			Item.width = 18;
			Item.height = 18;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item83;
			Item.shoot = ModContent.ProjectileType<HiveSentry>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref position);

			int frame = 1;

			int halfProjectileHeight = (int)Math.Ceiling(ContentSamples.ProjectilesByType[type].height / 2f);

			int x = (int)position.X / 16;
			int y = (int)position.Y / 16;
			for (int i = 0; i < 6; i++)
			{
				Tile tile = Main.tile[x, y - i];
				if (tile.HasTile && Main.tileSolid[tile.TileType])
				{
					frame = 0;
					position.Y = (y - i) * 16 + 34;
					break;
				}
			}
			
			if (frame == 1)
            {
                player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
				position = new Vector2(worldX, worldY - halfProjectileHeight);
            }

			Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, Main.myPlayer, 0, frame);
			player.UpdateMaxTurrets();

			return false;
		}
	}
}
