using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MoreSentries.Content.Projectiles.Sentry.TF2;
using MoreSentries.Common;

namespace MoreSentries.Content.Items.Sentry
{
	public class MinigunSentryItem : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
		}

		public override void SetDefaults() {
			Item.damage = 6;
			Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
			Item.mana = 10;
			Item.width = 20;
			Item.height = 20;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.DD2_DefenseTowerSpawn;
			Item.scale = 0.8f;
			Item.shoot = ModContent.ProjectileType<SentryLevel1>();
		}

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
			MoreSentriesPlayer modPlayer = player.GetModPlayer<MoreSentriesPlayer>();
			if (modPlayer.PDAUpgrade > 0)
				damage.Base = 5;
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref position);
			int halfProjectileHeight = (int)Math.Ceiling(ContentSamples.ProjectilesByType[ModContent.ProjectileType<SentryLevel3>()].height / 2f);

			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			position = new Vector2(worldX, worldY - halfProjectileHeight);

			MoreSentriesPlayer modPlayer = player.GetModPlayer<MoreSentriesPlayer>();
			if (modPlayer.PDAUpgrade == 2)
				type = ModContent.ProjectileType<SentryLevel3>();
			else if (modPlayer.PDAUpgrade == 1)
				type = ModContent.ProjectileType<SentryLevel2>();
				
			Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, Main.myPlayer);

			player.UpdateMaxTurrets();

			return false;
		}
	}
}
