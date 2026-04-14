using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace MoreSentries.Common
{
	public class MoreSentriesConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[DefaultValue(true)]
		[ReloadRequired]
		public bool UseSentryDamageType;
	}

	public class MoreSentriesClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[DefaultValue(true)]
		public bool HideSentryTooltipLine;
	}
}