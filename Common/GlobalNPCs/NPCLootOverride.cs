using MoreSentries.Content.Items;
using MoreSentries.Content.Items.Accessories;
using MoreSentries.Content.Items.Armor.Shadowflame;
using MoreSentries.Content.Items.Sentry;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Common
{
    public class NPCLootOverride : GlobalNPC
    {
        private static SimpleItemDropRuleCondition always = new SimpleItemDropRuleCondition(null, () => true, ShowItemDropInUI.Never);

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.EaterofSouls || npc.type == NPCID.Crimera)
            {
                npcLoot.Add(ItemDropRule.ByCondition(always, npc.type == NPCID.EaterofSouls ? ModContent.ItemType<EoWSentryItem>() : ModContent.ItemType<BoCSentryItem>(), chanceDenominator: 75));
            }
            else if (npc.type == NPCID.BloodZombie || npc.type == NPCID.Drippler)
            {
                npcLoot.Add(ItemDropRule.ByCondition(always, ModContent.ItemType<BloodyToolbox>(), chanceDenominator: 75));
            }
            else if (npc.type == NPCID.QueenBee)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new ClassicSentryDropCondition(), ModContent.ItemType<HiveSentryItem>(), chanceDenominator: 2));
            }
            else if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new ClassicSentryDropCondition(), ModContent.ItemType<PDAUpgradeChip>(), chanceDenominator: 2));
            }
            else if (npc.type == NPCID.GoblinSummoner)
            {
                OneFromOptionsNotScaledWithLuckDropRule dropWithConditionRule = new OneFromOptionsNotScaledWithLuckDropRule(2, 1,
                    ModContent.ItemType<ShadowflameMask>(),
                    ModContent.ItemType<ShadowflameRobe>(),
                    ModContent.ItemType<ShadowflameBoots>()
                );
                npcLoot.Add(dropWithConditionRule);
            }
            else if (npc.type == NPCID.Plantera)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new ClassicSentryDropCondition(), ModContent.ItemType<VenusSentryItem>(), chanceDenominator: 2));
                npcLoot.Add(ItemDropRule.ByCondition(new ClassicSentryDropCondition(), ModContent.ItemType<PDAExpansionModule>(), chanceDenominator: 2));
            }
            else if (npc.type == NPCID.Golem)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new ClassicSentryDropCondition(), ModContent.ItemType<GolemSentryItem>(), chanceDenominator: 2));
            }
            else if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new ClassicSentryDropCondition(), ModContent.ItemType<BubbleSentryItem>(), chanceDenominator: 2));
            }
            else if (npc.type == NPCID.MartianTurret)
            {
                ItemDropWithConditionRule dropWithConditionRule = new ItemDropWithConditionRule(
                    ModContent.ItemType<TeslaSentryItem>(), 20, 1, 1, always
                );
                npcLoot.Add(dropWithConditionRule);
            }
        }
    }

    public class ClassicSentryDropCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return info.player.maxTurrets > 1 && !info.IsExpertMode && !info.IsMasterMode;
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