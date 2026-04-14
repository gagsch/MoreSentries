using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Armor.Mahi
{
	[AutoloadEquip(EquipType.Legs)]
	public class MahiPants : ModItem
	{
		public override void SetStaticDefaults() {
			ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Lime;
			Item.defense = 12;
		}

        public override void UpdateEquip(Player player)
		{
			player.maxTurrets += 1;
			player.runAcceleration *= 1.4f;
        }
	}
}