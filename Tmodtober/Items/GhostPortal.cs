using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;

namespace Tmodtober.Items
{
    public class GhostPortal:ModItem
    {

        public override void SetDefaults()
        {

            Item.width = 12;
            Item.height = 12;

            Item.useTime = 100;
            Item.useAnimation = 100;
            
            Item.useStyle = ItemUseStyleID.HoldUp;

            Item.UseSound = SoundID.Item104;

        }

        public override bool? UseItem(Player player)
        {

            DannyPhantomPlayer _danny = player.GetModPlayer<DannyPhantomPlayer>();
            if (!_danny.HasPhantomPowers)
            {
                _danny.HasPhantomPowers = true;
                Main.NewText("Your ghostly powers awaken");
            }
            else
            {
                Main.NewText("You already have ghost powers");
            }

            return true;
        }

        public override void AddRecipes()
        {
            Recipe _recipe=Recipe.Create(Type);
            _recipe.AddIngredient(ItemID.Ectoplasm,10);
            _recipe.AddIngredient(ItemID.IronBar,10);
            _recipe.AddIngredient(ItemID.Wire,25);
            _recipe.AddIngredient(ItemID.Cog,10);
            _recipe.AddTile(TileID.MythrilAnvil);
            _recipe.Register();
        }

    }
}
