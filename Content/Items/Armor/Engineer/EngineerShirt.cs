using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Armor.Engineer
{
	[AutoloadEquip(EquipType.Body)]
	public class EngineerShirt : ModItem
	{
		public static LocalizedText SetBonusText { get; private set; }

		public override void SetStaticDefaults() {
			SetBonusText = this.GetLocalization("SetBonus");
		}

		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.buyPrice(gold: 8, silver: 50);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ItemID.EngineeringHelmet && legs.type == ModContent.ItemType<EngineerPants>();
        }

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = SetBonusText.Value;
			player.maxTurrets += 1;
		}
		
		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed<SentryDamageClass>() += 0.35f;
			player.GetModPlayer<MoreSentriesPlayer>().SentryFireRate += 0.15f;
        }
	}
}