using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Armor.Shadowflame
{
	[AutoloadEquip(EquipType.Body)]
	public class ShadowflameRobe : ModItem
	{
		public static LocalizedText SetBonusText { get; private set; }

		public override void SetStaticDefaults()
		{
			SetBonusText = this.GetLocalization("SetBonus");
			ItemID.Sets.ShimmerTransformToItem[ItemID.ShadowFlameHexDoll] = Item.type;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 4);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 10;
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return legs.type == ModContent.ItemType<ShadowflameBoots>() && head.type == ModContent.ItemType<ShadowflameMask>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = SetBonusText.Value;
			player.GetModPlayer<MoreSentriesPlayer>().VoodooSetbonus = true;
		}

		public override void ArmorSetShadows(Player player)
        {
			player.armorEffectDrawOutlinesForbidden = true;
        }
		
		public override void UpdateEquip(Player player)
		{
			player.maxTurrets += 1;
			player.GetDamage<SentryDamageClass>() += 0.08f;
        }
	}
}