using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Tmodtober.Items
{
    public class Heavenly_Pickaxe:ModItem
    {

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MoltenPickaxe);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Heavenstone_Bar>(20)
                .AddTile(TileID.Anvils)
                .Register();
        }

    }
}
