using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;

namespace MoreSentries.Common;

public class DDR2Accessories : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation)
    {
        return item.type == ItemID.ApprenticeScarf || item.type == ItemID.SquireShield || item.type == ItemID.MonkBelt || item.type == ItemID.HuntressBuckler;
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "Tooltip1" && x.Mod == "Terraria");
        if (line != null)
        {
            line.Text += "\nDoes not stack with other Old One's Army accessories";
        }
    }
}