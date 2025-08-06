using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace Tmodtober
{
    public class BlockBreakConfig: ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(10), Range(1, 1000)]
        public int breakTimer;

        [DefaultValue(10), Range(1, 1000)]
        public int breakTimeLowerer;

        [DefaultValue(false)]
        public bool blockBreakActive;

        public override void OnChanged()
        {
            BlockBreakerPlayer.InitTimer= breakTimer*30;
            BlockBreakerPlayer.TileTimeLower = breakTimeLowerer;
            BlockBreakerPlayer.needsBreakBlocks = blockBreakActive;
        }

    }
}
