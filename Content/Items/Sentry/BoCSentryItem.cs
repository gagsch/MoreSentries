using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MoreSentries.Content.Projectiles.Sentry;

namespace MoreSentries.Content.Items.Sentry
{
	public class BoCSentryItem : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
		}

		public override void SetDefaults() {
			Item.damage = 38;
			Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
			Item.mana = 10;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 6.6f;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item83;
			Item.shoot = ModContent.ProjectileType<BoCSentry>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			position = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref position);
			int halfProjectileHeight = (int)Math.Ceiling(ContentSamples.ProjectilesByType[type].height / 2f);
			position.Y += halfProjectileHeight;

			Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, Main.myPlayer);

			player.UpdateMaxTurrets();

			return false;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 12);
			recipe.AddIngredient(ItemID.Vertebrae, 6);
			recipe.Register();
        }
	}
}
