using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Armor.Mahi
{
	[AutoloadEquip(EquipType.Body)]
	public class MahiShirt : ModItem
	{
		public static LocalizedText SetBonusText { get; private set; }

		public override void SetStaticDefaults() {
			SetBonusText = this.GetLocalization("SetBonus");
		}

		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Lime;
			Item.defense = 18;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return legs.type == ModContent.ItemType<MahiPants>() && head.type == ModContent.ItemType<MahiMask>();
        }

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = SetBonusText.Value;

			MoreSentriesPlayer moddedPlayer = player.GetModPlayer<MoreSentriesPlayer>();
			moddedPlayer.MahiSetbonus = true;
			player.GetArmorPenetration<SentryDamageClass>() += moddedPlayer.MahiArmorPenetration;
			moddedPlayer.MahiArmorPenetration -= 0.05f;
			if (moddedPlayer.MahiArmorPenetration < 0)
			{
				moddedPlayer.MahiArmorPenetration = 0;
			}
		}
		
		public override void UpdateEquip(Player player)
		{
			player.maxTurrets += 1;
			player.GetDamage<SentryDamageClass>() += 0.1f;
			player.GetModPlayer<MoreSentriesPlayer>().SentryFireRate += 0.14f;
        }
	}
}