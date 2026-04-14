using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Accessories
{
	public class TavernkeepRadar : ModItem
	{
		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 32;
			Item.value = Item.buyPrice(gold: 2);
			Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 10;
			Item.useTime = 10;
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

        public override void UpdateInfoAccessory(Player player)
        {
            PointToTavernkeep(player);
        }

		public void PointToTavernkeep(Player player)
		{
			player.GetModPlayer<MoreSentriesPlayer>().TavernkeepRadar = true;
		}

        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.MeteoriteBar, 10)
				.AddIngredient(ItemID.ShadowScale, 4)
				.AddIngredient(ItemID.FallenStar)
				.AddTile(TileID.Anvils)
				.Register();

            CreateRecipe()
				.AddIngredient(ItemID.MeteoriteBar, 10)
				.AddIngredient(ItemID.TissueSample, 4)
				.AddIngredient(ItemID.FallenStar)
				.AddTile(TileID.Anvils)
				.Register();
        }
	}
}