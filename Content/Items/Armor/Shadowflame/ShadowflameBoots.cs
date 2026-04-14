using MoreSentries.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items.Armor.Shadowflame
{

	[AutoloadEquip(EquipType.Legs)]
	public class ShadowflameBoots : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.ShimmerTransformToItem[ItemID.ShadowFlameBow] = Item.type;
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 14;
			Item.value = Item.sellPrice(gold: 4);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 5;
		}

        public override void UpdateEquip(Player player)
        {
			player.GetDamage<SentryDamageClass>() += 0.08f;
			player.moveSpeed += 0.25f;
			player.runAcceleration *= 1.25f;
        }
	}
}