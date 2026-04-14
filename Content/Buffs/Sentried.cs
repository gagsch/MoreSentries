using Terraria;
using Terraria.ModLoader;
using MoreSentries.Common;

namespace MoreSentries.Content.Buffs
{
	public class Sentried : ModBuff
	{
		public override void Update(Player player, ref int buffIndex) {
			player.GetDamage<SentryDamageClass>() += 0.05f;
			player.GetModPlayer<MoreSentriesPlayer>().SentryFireRate += 0.05f;
			player.GetAttackSpeed<SentryDamageClass>() += 0.1f;
		}
	}
}