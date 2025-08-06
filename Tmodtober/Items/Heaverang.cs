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
    public class Heaverang: ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Flamarang);

            Item.shoot = ModContent.ProjectileType<Projectiles.Heaverang>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Heavenstone_Bar>(10)
                .AddIngredient(ItemID.EnchantedBoomerang)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool CanUseItem(Player player)
        {
            int projAmmount = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                projAmmount += (Main.projectile[i].active && Main.projectile[i].type == Item.shoot) ? 1 : 0;
            }
            return projAmmount <= 0;
        }

    }
}
