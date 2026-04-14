using Terraria;
using Terraria.ModLoader;

namespace MoreSentries.Common
{
    public class DoTNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int CactusNeedles;
        public uint TicksAlive;

        public override void AI(NPC npc)
        {
            CactusNeedles = CactusNeedles > 20 ? 20 : CactusNeedles;
            if (TicksAlive++ % 20 == 0 && CactusNeedles > 0) {
                MoreSentries.DamageOverTimeHit(npc, CactusNeedles);
                CactusNeedles = 0;
            }
        }
    }
}