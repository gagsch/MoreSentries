using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MoreSentries.Common;

public class Lunar : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation)
    {
        return item.type == ItemID.MoonlordTurretStaff;
    }

    public override void SetDefaults(Item item)
    {
        item.damage = 115;
    }
}