using System.Collections.Generic;
using MoreSentries.Content.Items.Armor;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Common
{
    public class ObsidianHelmetOverride : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.type == ItemID.ObsidianHelm;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (tooltips.Count > 3) tooltips[3] = new TooltipLine(Mod, "Tooltip1", "Increases your max number of minions by 1");
        }

        public override void UpdateEquip(Item item, Player player)
        {
            player.GetDamage(DamageClass.Summon) -= 0.08f; // revert original bonus
            player.maxMinions += 1;
        }
    }

    public class ObsidianLongcoatOverride : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.type == ItemID.ObsidianShirt;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (tooltips.Count > 3) tooltips[3] = new TooltipLine(Mod, "Tooltip1", "Increases summon damage by 8%");
        }

        public override void UpdateEquip(Item item, Player player)
        {
            player.maxMinions -= 1; // revert original bonus
			player.GetDamage(DamageClass.Summon) += 0.08f;
        }
    }
}