using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class HallowedCasque : ModItem
	{
		public override void SetDefaults()
		{
			Item.defense = 3;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(gold: 5);
			Item.width = 24;
			Item.height = 22;
		}

		public override void UpdateEquip(Player player)
        {
            player.maxTurrets += 1;
			player.GetDamage<SentryDamageClass>() += 0.1f;
        }

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("ArmorSetBonus.Hallowed") + "\nIncreases your max sentries by 1";
			player.maxTurrets += 1;
			player.onHitDodge = true;
		}

        public override void ArmorSetShadows(Player player)
        {
			player.armorEffectDrawShadow = true;
			player.armorEffectDrawOutlines = true;
        }

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.HallowedPlateMail && legs.type == ItemID.HallowedGreaves;
		}

        public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.HallowedBar, 12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}