using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class FlinxEarmuffs : ModItem
	{
		public static LocalizedText SetBonusText { get; private set; }

		public override void SetStaticDefaults() {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
			SetBonusText = this.GetLocalization("SetBonus");
		}

		public override void SetDefaults() {
			Item.width = 36;
			Item.height = 36;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.rare = ItemRarityID.Green;
			Item.defense = 1;
		}

        public override void UpdateEquip(Player player) {
			player.maxTurrets += 1;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.FlinxFurCoat;
        }

		public override void UpdateArmorSet(Player player) {
			player.setBonus = SetBonusText.Value;
			player.GetDamage(DamageClass.Summon) += 0.05f;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.FlinxFur, 6)
				.AddIngredient(ItemID.GoldBar, 6)
				.AddTile(TileID.Loom)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.FlinxFur, 6)
				.AddIngredient(ItemID.PlatinumBar, 6)
				.AddTile(TileID.Loom)
				.Register();
		}
	}
}