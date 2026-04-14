using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MoreSentries.Content.Projectiles.Sentry;

namespace MoreSentries.Content.Items.Sentry
{
	public class DartSentryItem : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
		}

		public override void SetDefaults() {
			Item.damage = 15;
			Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
			Item.mana = 5;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(silver: 25);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item83;
			Item.shoot = ModContent.ProjectileType<DartSentry>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref position);
			int halfProjectileHeight = (int)Math.Ceiling(ContentSamples.ProjectilesByType[type].height / 2f);
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out _);
			position = new Vector2(worldX, worldY - halfProjectileHeight);

			Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, Main.myPlayer);

			player.UpdateMaxTurrets();

			return false;
		}
		
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.DartTrap)
				.AddRecipeGroup(RecipeGroupID.IronBar, 6)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
