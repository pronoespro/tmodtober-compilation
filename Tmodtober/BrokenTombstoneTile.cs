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
    class BrokenTombstoneTile:GlobalTile
    {

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (type == TileID.Tombstones)
            {
                GhostBustedWorld.Instance.AddToGhostLevel();
            }
            base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);
        }

    }
}
