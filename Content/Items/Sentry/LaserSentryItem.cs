using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MoreSentries.Content.Projectiles.Sentry;

namespace MoreSentries.Content.Items.Sentry
{
	public class LaserSentryItem : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
		}

		public override void SetDefaults() {
			Item.damage = 47;
			Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
			Item.mana = 10;
			Item.width = 16;
			Item.height = 16;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.value = Item.sellPrice(gold: 3, silver: 50);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item83;
			Item.shoot = ModContent.ProjectileType<LaserSentry>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref position);
			position.Y -= (int)Math.Ceiling(ContentSamples.ProjectilesByType[type].height / 2f);

			Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, Main.myPlayer);

			player.UpdateMaxTurrets();

			return false;
		}

		public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 20)
				.AddIngredient(ItemID.SoulofFright, 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
