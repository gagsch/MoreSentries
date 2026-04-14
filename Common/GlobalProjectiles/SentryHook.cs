using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MoreSentries.Content;
using System.Collections.Generic;

namespace MoreSentries.Common.GlobalProjectiles;

public class SentryHook : GlobalProjectile
{
    public override bool InstancePerEntity => true;
    public bool IsOrFromSentry;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (projectile.sentry || source is EntitySource_Parent parent && parent.Entity is Projectile proj && proj.GetGlobalProjectile<SentryHook>().IsOrFromSentry)
        {
            IsOrFromSentry = true;
            if (projectile.DamageType != DamageClass.Default && ModContent.GetInstance<MoreSentriesConfig>().UseSentryDamageType) projectile.DamageType = ModContent.GetInstance<SentryDamageClass>();
        }
    }
}

public class SentryItemOverride : GlobalItem
{
    public override bool AppliesToEntity(Item item, bool lateInstantiation)
    {
        return item.sentry;
    }

    public override void SetDefaults(Item item)
    {
        if (ModContent.GetInstance<MoreSentriesConfig>().UseSentryDamageType)
        {
            item.DamageType = ModContent.GetInstance<SentryDamageClass>();
        }
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (!ModContent.GetInstance<MoreSentriesClientConfig>().HideSentryTooltipLine) return;

        foreach(TooltipLine tooltip in tooltips)
        {
            if (tooltip.Text == "Summons a sentry") tooltip.Hide();
        }
    }
}