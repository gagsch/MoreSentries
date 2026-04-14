using Microsoft.Xna.Framework;
using MoreSentries.Common;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items
{
	public class PDAUpgradeChip : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 26;
			Item.value = Item.sellPrice(gold: 2);
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 15;
			Item.useTime = 15;
			Item.consumable = true;
			Item.rare = ItemRarityID.Green;
		}

        public override bool? UseItem(Player player)
		{
			if (player.GetModPlayer<MoreSentriesPlayer>().PDAUpgrade > 0)
				return false;

			SoundEngine.PlaySound(SoundID.Item4, player.Center);
			player.GetModPlayer<MoreSentriesPlayer>().PDAUpgrade = 1;
			return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
	}
}