using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Tmodtober.Buffs
{
    public class HeartbreakDebuff : ModBuff
    {


        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
            tip = "You can't believe they are gone...for now.";
        }

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

            player.statDefense /= 2;
            player.statLifeMax2 -= 20;

            HeartbreakPlayer _hbPlayer = player.GetModPlayer<HeartbreakPlayer>();
            _hbPlayer.SetHeartbroken();

        }

    }
}
