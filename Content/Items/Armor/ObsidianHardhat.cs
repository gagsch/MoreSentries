using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ObsidianHardhat : ModItem
	{
		public static LocalizedText SetBonusText { get; private set; }

        public override void SetStaticDefaults()
        {
            SetBonusText = this.GetLocalization("SetBonus");
        }

		public override void SetDefaults()
		{
			Item.defense = 4;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 9);
			Item.width = 30;
			Item.height = 24;
		}

		public override void UpdateEquip(Player player)
        {
            player.maxTurrets += 1;
        }

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = SetBonusText.Value;
			player.GetDamage(DamageClass.Summon) += 0.15f;
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
			player.whipRangeMultiplier += 0.3f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.ObsidianShirt && legs.type == ItemID.ObsidianPants;
		}

        public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.Obsidian, 20)
				.AddIngredient(ItemID.ShadowScale, 5)
				.AddTile(TileID.Hellforge)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.Obsidian, 20)
				.AddIngredient(ItemID.TissueSample, 5)
				.AddTile(TileID.Hellforge)
				.Register();
		}
	}
}