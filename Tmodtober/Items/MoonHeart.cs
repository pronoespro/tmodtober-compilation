using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace Tmodtober.Items
{
    public class MoonHeart:ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.healLife = 1;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.value = 100;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = ItemRarityID.Orange;
            Item.maxStack = 9999999;

            Item.UseSound = SoundID.GuitarEm;
            Item.autoReuse = false;

            Item.consumable = true;
            Item.autoReuse = true;
        }

        public override bool ConsumeItem(Player player)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                MoonHeartPlayer _mhp = player.GetModPlayer<MoonHeartPlayer>();

                _mhp.IncreaseMoonHeartAmmount();
                player.statLife = Math.Max(player.statLife, player.statLifeMax);
            }
            base.OnConsumeItem(player);
        }


        public override void AddRecipes()
        {
            Recipe _r= Recipe.Create(Type);
            _r.AddIngredient(ItemID.LifeCrystal);
            _r.AddIngredient(ItemID.LunarBar,9);
            _r.AddTile(TileID.LunarCraftingStation);
            _r.Register();
        }

    }
}
