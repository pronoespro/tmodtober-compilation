using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace Tmodtober
{
    public class EaseOfCraftSystem:ModSystem
    {

        public override void AddRecipes()
        {
            Recipe.Create(ItemID.LivingFireBlock, 100)
                .AddIngredient(ItemID.Torch, 100)
                .AddTile(TileID.Hellforge)
                .Register();
        }

    }
}
