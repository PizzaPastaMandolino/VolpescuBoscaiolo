using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VolpescuBoscaiolo.Potions
{
	public class DeathpointPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sabakuer Potion"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("Ti tippa dove sei morto... PEFFORZA");
		}
		public override void SetDefaults()
		{
			Item.potion = true;

			Item.width = 26;
			Item.height = 32;

			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 45;
			Item.rare = ItemRarityID.Blue;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.maxStack = 30;

		}

		public override bool CanUseItem(Player player)
		{
			return player.showLastDeath;
		}

		public override bool? UseItem(Player player)
		{
			var deathPoint = new Vector2(player.lastDeathPostion.X - 16, player.lastDeathPostion.Y - 24);
			player.Teleport(deathPoint);
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BottledWater)
				.AddIngredient(ItemID.Tombstone)
				.AddIngredient(ItemID.Mushroom)
				.Register();
		}
	}
}
