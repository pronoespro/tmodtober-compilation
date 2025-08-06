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
    public class MoonsterEnergyDrink:ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height= 64;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                MoonsterPlayer _mp = player.GetModPlayer<MoonsterPlayer>();
                if (_mp.drankMoonsterDrink)
                {
                    PlayerDeathReason _reson = new PlayerDeathReason();
                    _reson.SourceItem = Item;
                    player.Hurt(_reson, player.statLifeMax* 400, 0, false, false, 1, false, int.MaxValue/4);
                }
                else
                {
                    _mp.drankMoonsterDrink = true;
                }
            }
            return true;
        }

        public override bool ConsumeItem(Player player)
        {
            return true;
        }

    }
}
