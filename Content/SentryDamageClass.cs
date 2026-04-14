using Terraria;
using Terraria.ModLoader;

namespace MoreSentries.Content
{
	public class SentryDamageClass : DamageClass
	{
		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) {
            if (damageClass == Generic || damageClass == Summon) return StatInheritanceData.Full;

			return new StatInheritanceData(
				damageInheritance: 0f,
				critChanceInheritance: 0f,
				attackSpeedInheritance: 0f,
				armorPenInheritance: 0f,
				knockbackInheritance: 0f
			);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass == Magic;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass == Summon;
		public override bool UseStandardCritCalcs => false;
		public override bool ShowStatTooltipLine(Player player, string lineName) => lineName != "CritChance";
		
	}
}