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
    public class PartyConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;


        [DefaultValue(10), Range(1, 365)]
        public int partyChance;
        public override void OnChanged()
        {
            TmodtoberMod.PartyChance = partyChance;
        }
    }
}
