using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Common
{
    public class HelmetOverride : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.type == ItemID.EngineeringHelmet;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            tooltips.Insert(3, new TooltipLine(Mod, "Tooltip0", "Increases your max number of sentries by 1"));
        }

        public override void SetDefaults(Item item)
        {
            item.vanity = false;
            item.rare = ItemRarityID.Green;
            item.defense = 5;
            item.value = Item.buyPrice(gold: 8, silver: 50);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            player.maxTurrets += 1;
        }
    }
}