using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Accessories
{
	public class BessBeetle : ModItem
	{
		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 32;
			Item.value = Item.sellPrice(gold: 12);
			Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.GetDamage<SentryDamageClass>() += 0.25f;
			player.GetKnockback<SentryDamageClass>() += 0.25f;
            player.maxTurrets += 1;
		}
		
		public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient(ItemID.HerculesBeetle)
				.AddIngredient(ModContent.ItemType<TheBlueprint>())
                .AddIngredient(ItemID.BeetleHusk, 8)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}