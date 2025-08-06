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
    public class ItemsFightBackConfig: ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(false)]
        public bool itemsFightBack;
        [DefaultValue(250)]
        public int playerMinDistance;

        public override void OnChanged()
        {
            ItemFightBackItemOverride.ItemsFightBackEnabled = itemsFightBack;
            ItemFightBackItemOverride.minPlayerDistance = playerMinDistance;
        }

    }
}
