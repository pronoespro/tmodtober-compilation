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
    public class CloseSingBuff : ModBuff
    {

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
            tip = "Your carefree singing heals you";
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.lifeRegen += 20;

                BardPlayer _b = player.GetModPlayer<BardPlayer>();
                _b.damageDown = true;
            }
        }


    }
}
