using System;
using System.Linq;
using MoreSentries.Content.Items;
using MoreSentries.Content.Items.Sentry;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Common
{
    public class BossBagLoot : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.QueenBeeBossBag)
            {
                itemLoot.Add(ItemDropRule.ByCondition(new HasSentryDropCondition(), ModContent.ItemType<HiveSentryItem>(), chanceDenominator: 2));
            }
            else if (item.type == ItemID.WallOfFleshBossBag)
            {
                itemLoot.Add(ItemDropRule.ByCondition(new HasSentryDropCondition(), ModContent.ItemType<PDAUpgradeChip>()));
            }
            else if (item.type == ItemID.PlanteraBossBag)
            {
                itemLoot.Add(ItemDropRule.ByCondition(new HasSentryDropCondition(), ModContent.ItemType<VenusSentryItem>(), chanceDenominator: 2));
                itemLoot.Add(ItemDropRule.ByCondition(new HasSentryDropCondition(), ModContent.ItemType<PDAExpansionModule>()));
            }
            else if (item.type == ItemID.GolemBossBag)
            {
                itemLoot.Add(ItemDropRule.ByCondition(new HasSentryDropCondition(), ModContent.ItemType<GolemSentryItem>(), chanceDenominator: 2));
            }
            else if (item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(ItemDropRule.ByCondition(new HasSentryDropCondition(), ModContent.ItemType<BubbleSentryItem>(), chanceDenominator: 2));
            }
        }
    }
    
    public class HasSentryDropCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return info.player.maxTurrets > 1;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return "Dropped when max sentries is greater than 1";
        }
    }
}