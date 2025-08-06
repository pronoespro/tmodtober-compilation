using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober.Buffs
{
    public class FarSingBuff:ModBuff
    {

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
            tip = "Your agresive singing strengthens you";
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            base.Update(player, ref buffIndex);

            if (player.whoAmI == Main.myPlayer)
            {
                BardPlayer _bard = player.GetModPlayer<BardPlayer>();
                _bard.damageUp = true;
                player.runAcceleration += 0.25f;
                player.maxRunSpeed += 2;

                player.lifeRegen = 0;
                player.statDefense-=10;
            }
        }

    }
}
