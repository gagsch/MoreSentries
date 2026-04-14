using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreSentries.Content.Items
{
	public class BigRedButton : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.value = Item.sellPrice(silver: 50);
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 15;
			Item.useTime = 15;
			Item.rare = ItemRarityID.Green;
		}

        public override bool? UseItem(Player player)
		{
			if (player.itemTime != 0) return true;
			
			SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
			if (player.altFunctionUse == 2) SecondaryUse();
			else PrimaryUse();

			return true;
        }

		public void PrimaryUse()
		{
			int dmg = 70 * (Main.hardMode ? 3 : 1);
			foreach (Projectile proj in Main.ActiveProjectiles)
			{
				if (proj.sentry && proj.owner == Main.myPlayer)
				{
					Projectile grenade = Projectile.NewProjectileDirect(proj.GetSource_FromThis(), proj.Center, Vector2.Zero, ProjectileID.JackOLantern, dmg, 4, proj.owner);
					grenade.timeLeft = 1;
					proj.Kill();
				}
			}
		}

		public void SecondaryUse()
		{
			int dmg = 60 * (Main.hardMode ? 2 : 1);
			Projectile closestSentry = null;
			float distance = 100000;
			foreach (Projectile proj in Main.ActiveProjectiles)
			{
				if (proj.sentry && proj.owner == Main.myPlayer)
				{
					float newDistance = proj.Distance(Main.MouseWorld);
					if (newDistance < distance)
					{
						distance = newDistance;
						closestSentry = proj;
					}
				}
			}

			if (closestSentry != null)
			{
				Projectile grenade = Projectile.NewProjectileDirect(closestSentry.GetSource_FromThis(), closestSentry.Center, Vector2.Zero, ProjectileID.JackOLantern, dmg, 4, closestSentry.owner);
				grenade.timeLeft = 1;
				closestSentry.Kill();
			}
		}

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }

		public override void AddRecipes() {
			CreateRecipe()
				.AddRecipeGroup(RecipeGroupID.IronBar, 12)
				.AddIngredient(ItemID.Dynamite, 3)
				.AddIngredient(ItemID.Wire)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}