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
    public class PhantomConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool showPhantomPower;

        public override void OnChanged()
        {
            DannyPhantomPlayer.showPhantomPower= showPhantomPower;
        }

    }
}
