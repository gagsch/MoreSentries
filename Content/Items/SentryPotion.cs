using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items
{
	public class SentryPotion : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 20;

			ItemID.Sets.DrinkParticleColors[Type] = [
				new Color(240, 240, 240),
				new Color(200, 200, 200),
				new Color(140, 140, 140)
			];
		}

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 34;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 2);
            Item.buffType = ModContent.BuffType<Buffs.Sentried>();
            Item.buffTime = 28800;
        }
        
        public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient(ItemID.Blinkroot, 1)
                .AddIngredient(ItemID.Moonglow, 1)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}
}