using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Armor.Engineer
{
	[AutoloadEquip(EquipType.Legs)]
	public class EngineerPants : ModItem
	{
		public static LocalizedText SetBonusText { get; private set; }

		public override void SetStaticDefaults() {
			ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.buyPrice(gold: 8, silver: 50);
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}

        public override void UpdateEquip(Player player)
        {
			player.GetDamage<SentryDamageClass>() += 0.2f;
        }
	}
}