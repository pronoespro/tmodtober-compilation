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
    class Heavenforge_Item:ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;

		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Heavenforge>());
			Item.width = 30;
			Item.height = 26;
			Item.value = 3000;
		}

        public override void AddRecipes()
        {
			Recipe.Create(Type)
				.AddIngredient<Heavenstone>(20).
				AddIngredient(ItemID.Furnace).
				AddTile(TileID.Anvils).Register();

			Recipe.Create(ItemID.AdamantiteForge)
				.AddIngredient(ItemID.AdamantiteOre,30).
				AddIngredient(Type).
				AddTile(TileID.MythrilAnvil).Register();

			Recipe.Create(ItemID.TitaniumForge)
				.AddIngredient(ItemID.TitaniumOre, 30).
				AddIngredient(Type).
				AddTile(TileID.MythrilAnvil).Register();
		}
    }
}
