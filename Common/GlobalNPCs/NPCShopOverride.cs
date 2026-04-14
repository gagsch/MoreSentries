using MoreSentries.Content.Items;
using MoreSentries.Content.Items.Accessories;
using MoreSentries.Content.Items.Armor.Engineer;
using MoreSentries.Content.Items.Armor.Mahi;
using MoreSentries.Content.Items.Sentry;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Common
{
    public class MechanicShop : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Mechanic;
        }
        public override void ModifyShop(NPCShop shop)
        {
            shop.InsertAfter(ItemID.EngineeringHelmet, ModContent.ItemType<EngineerShirt>());
            shop.InsertAfter(ItemID.EngineeringHelmet, ModContent.ItemType<EngineerPants>());
            shop.InsertAfter(ItemID.EngineeringHelmet, ModContent.ItemType<MinigunSentryItem>());
        }
    }

    public class WitchDoctorShop : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.WitchDoctor;
        }
        public override void ModifyShop(NPCShop shop)
        {
            shop.InsertAfter(ItemID.TikiPants, ModContent.ItemType<MahiMask>(), Condition.DownedPlantera);
            shop.InsertAfter(ItemID.TikiPants, ModContent.ItemType<MahiShirt>(), Condition.DownedPlantera);
            shop.InsertAfter(ItemID.TikiPants, ModContent.ItemType<MahiPants>(), Condition.DownedPlantera);
        }
    }

    public class GoblinTinkererShop : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.GoblinTinkerer;
        }
        public override void ModifyShop(NPCShop shop)
        {
            shop.Add(ModContent.ItemType<TheBlueprint>());
        }
    }
}