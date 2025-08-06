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
    class LivingFirePiano_item:ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LivingFirePiano>());
			Item.width = 42;
			Item.height = 32;
			Item.value = 150;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.LivingFireBlock, 15)
				.AddIngredient(ItemID.Book)
				.AddIngredient(ItemID.Bone,4)
				.AddTile(TileID.Hellforge)
				.Register();
		}
	}
}
