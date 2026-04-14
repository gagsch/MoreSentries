using System;
using MoreSentries.Common.GlobalProjectiles;
using MoreSentries.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MoreSentries.Common
{
    public class MoreSentriesPlayer : ModPlayer
    {
        public bool VoodooSetbonus;
        public bool MahiSetbonus;
        public bool TavernkeepRadar;
        public float MahiArmorPenetration;
        public float SentryFireRate;
        public int PDAUpgrade;

        public override void ResetEffects()
        {
            if (!MahiSetbonus)
            {
                MahiArmorPenetration = 0;
            }
            VoodooSetbonus = false;
            MahiSetbonus = false;
            TavernkeepRadar = false;
            SentryFireRate = 1f;
        }

        public override void PostUpdateEquips()
        {
            Player.GetDamage<SentryDamageClass>().Flat -= Player.slotsMinions;
            Player.GetDamage<SentryDamageClass>() /= 1 + Player.slotsMinions * Player.maxMinions / 50f;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (proj.GetGlobalProjectile<SentryHook>().IsOrFromSentry)
            {
                if (VoodooSetbonus)
                {
                    target.AddBuff(BuffID.ShadowFlame, 240);
                }
                else if (MahiSetbonus && MahiArmorPenetration < 15)
                {
                    MahiArmorPenetration += 0.1f;
                }
            }
        }

        public override void SaveData(TagCompound tag) {
			tag["PDAUpgrade"] = PDAUpgrade;
		}

		public override void LoadData(TagCompound tag) {
			PDAUpgrade = tag.GetInt("PDAUpgrade");
		}
    }
}