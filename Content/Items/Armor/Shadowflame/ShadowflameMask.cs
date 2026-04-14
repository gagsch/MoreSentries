using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Armor.Shadowflame
{
	[AutoloadEquip(EquipType.Head)]
	public class ShadowflameMask : ModItem
	{
		public override void SetStaticDefaults() {
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
			ItemID.Sets.ShimmerTransformToItem[ItemID.ShadowFlameKnife] = Item.type;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 24;
			Item.value = Item.sellPrice(gold: 4);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
        {
			player.maxTurrets += 1;
			player.GetDamage<SentryDamageClass>() += 0.08f;
        }
	}
}