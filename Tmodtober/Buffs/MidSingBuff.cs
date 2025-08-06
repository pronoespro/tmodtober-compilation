using Microsoft.Xna.Framework;
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
    public class MidSingBuff : ModBuff
    {

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
            tip = "Your cautios singing defends you";
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
                player.statDefense += 10;
                player.runAcceleration =0.1f;
                player.maxRunSpeed =1f;
                player.stepSpeed = 1f;

                if (Math.Abs(player.velocity.X) >1f) {
                    player.velocity = new Vector2(Math.Sign(player.velocity.X)*1f, player.velocity.Y);
                }
            }
        }

    }
}
