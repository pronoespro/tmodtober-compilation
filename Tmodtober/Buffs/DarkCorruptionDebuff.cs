using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Tmodtober.Buffs
{
    public class DarkCorruptionDebuff : ModBuff
    {


        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
            tip = "Your shadow betrays you";
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Enlightened");
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                DarkCorruptionPlayer _dcp = player.GetModPlayer<DarkCorruptionPlayer>();

                if (player.buffTime[buffIndex] <= 1)
                {
                    _dcp.corruptionLevel = 0;
                }
                else
                {
                    _dcp.corruptionLevel = Math.Max(1, _dcp.corruptionLevel);
                }
            }
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                DarkCorruptionPlayer _dcp = player.GetModPlayer<DarkCorruptionPlayer>();

                _dcp.corruptionLevel++;

            }
            return base.ReApply(player, time, buffIndex);
        }

    }
}
