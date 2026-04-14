using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Accessories
{
	public class DefenderEmblem : ModItem
	{
		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(gold: 6);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage<SentryDamageClass>() += 0.12f;
			player.GetModPlayer<MoreSentriesPlayer>().SentryFireRate += 0.12f;
			player.GetAttackSpeed<SentryDamageClass>() += 0.12f;
			player.maxTurrets += 1;
		}
		
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.AvengerEmblem)
				.AddIngredient(ItemID.MonkBelt)
				.AddIngredient(ModContent.ItemType<BloodyToolbox>())
				.AddTile(TileID.MythrilAnvil)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.AvengerEmblem)
				.AddIngredient(ItemID.HuntressBuckler)
				.AddIngredient(ModContent.ItemType<BloodyToolbox>())
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}