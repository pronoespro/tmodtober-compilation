using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober.Items
{
    public class LivingFireTable_item : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LivingFireTable>());
			Item.width = 20;
			Item.height = 38;
			Item.value = 150;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.LivingFireBlock, 8)
				.AddTile(TileID.Hellforge)
				.Register();
		}
    }
}
