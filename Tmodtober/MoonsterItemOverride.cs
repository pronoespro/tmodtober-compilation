using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;

namespace Tmodtober
{
    public class MoonsterItemOverride:GlobalItem
    {



        public override void UpdateInventory(Item item, Player player)
        {
            if (item.type == ItemID.JojaCola && NPC.downedMoonlord)
            {
                ItemID.Sets.ShimmerTransformToItem[ItemID.JojaCola] = ModContent.ItemType<Items.MoonsterEnergyDrink>();
            }
            base.UpdateInventory(item, player);
        }

        public override void AddRecipes()
        {
            Recipe.Create(ItemID.JojaCola).AddIngredient(ItemID.IronBar, 2).AddIngredient(ItemID.SlimeBanner).AddTile(TileID.Anvils).Register();
        }

    }
}

