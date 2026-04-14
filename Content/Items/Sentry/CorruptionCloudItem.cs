using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MoreSentries.Content.Projectiles.Sentry.Cloud;

namespace MoreSentries.Content.Items.Sentry
{
	public class CorruptionCloudItem : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.CrimsonRod);
			Item.damage = 10;
			Item.shoot = ModContent.ProjectileType<CorruptionCloudMoving>();
			Item.crit = 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 mousePos = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref mousePos);

			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, mousePos.X, mousePos.Y);

			return false;
		}
		
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.DemoniteBar, 16)
				.AddIngredient(ItemID.ShadowScale, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
