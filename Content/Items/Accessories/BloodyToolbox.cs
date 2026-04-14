using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Accessories
{
	public class BloodyToolbox : ModItem
	{
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.SharkToothNecklace;
			ItemID.Sets.ShimmerTransformToItem[ItemID.SharkToothNecklace] = Type;
        }

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 26;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MoreSentriesPlayer>().SentryFireRate += 0.08f;
			player.GetAttackSpeed<SentryDamageClass>() += 0.2f;
		}
	}
}