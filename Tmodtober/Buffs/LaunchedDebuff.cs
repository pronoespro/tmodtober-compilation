using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace Tmodtober.Buffs
{
    public class LaunchedDebuff:ModBuff
    {

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
            tip = "Fore!!!";
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.velocity.LengthSquared() <= 15 * 15)
            {
                npc.velocity =new Vector2(npc.velocity.X* 1.5f,npc.velocity.Y);
            }

            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if(!Main.npc[i].CountsAsACritter && !Main.npc[i].townNPC && !Main.npc[i].friendly
                    && Vector2.DistanceSquared(npc.Center,Main.npc[i].Center)<50*50 && npc.whoAmI!=i)
                {
                    Main.npc[i].velocity = npc.velocity;
                    Main.npc[i].AddBuff(ModContent.BuffType<LaunchedDebuff>(),15);
                    Main.npc[i].life -= 20;
                    Main.npc[i].checkDead();
                    npc.velocity = new Vector2(-npc.velocity.X / 2, npc.velocity.Y);
                    break;
                }
            }
        }

    }
}
