using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace Tmodtober.Gores
{
    public class bunnygirl_Leg_Gore : ModGore
    {
        public override void OnSpawn(Gore gore, IEntitySource source)
        {
            gore.numFrames = 0;
            gore.behindTiles = false;
            //gore.scale = 1.5f;
        }
    }
}
