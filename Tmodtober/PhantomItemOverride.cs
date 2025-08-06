using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;

namespace Tmodtober
{
    public class PhantomItemOverride:GlobalItem
    {

        public override bool CanUseItem(Item item, Player player)
        {

            DannyPhantomPlayer _danny = player.GetModPlayer<DannyPhantomPlayer>();
            if (_danny.IsPhantom)
            {
                return false;
            }


            return base.CanUseItem(item, player);
        }

    }
}
