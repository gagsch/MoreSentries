using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Armor.Mahi
{
	[AutoloadEquip(EquipType.Head)]
	public class MahiMask : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Lime;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
        {
			player.maxTurrets += 1;
			player.GetDamage<SentryDamageClass>() += 0.14f;

        }
	}
}