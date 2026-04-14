using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Accessories
{
	public class TheBlueprint : ModItem
	{
		public override void SetDefaults() {
			Item.width = 44;
			Item.height = 56;
			Item.value = Item.buyPrice(gold: 2);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual) {
			player.maxTurrets += 1;
		}
	}
}