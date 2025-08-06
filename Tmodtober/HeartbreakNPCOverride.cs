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

namespace Tmodtober
{
    public class HeartbreakNPCOverride:GlobalNPC
    {

        public override void OnKill(NPC npc)
        {

            if (npc.townNPC){
                AddDebuffToNearbyPlayers(npc.Center, 60*60);
            }
            if (npc.CountsAsACritter){
                AddDebuffToNearbyPlayers(npc.Center, 60*5);
            }

            base.OnKill(npc);
        }

        public void AddDebuffToNearbyPlayers(Vector2 _pos, int time){
            for(int i = 0; i < Main.maxPlayers; i++)
            {
                if(Main.player[i].active && !Main.player[i].dead && Vector2.DistanceSquared(Main.player[i].Center, _pos) < 1500 * 1500)
                {
                    Main.player[i].AddBuff(ModContent.BuffType<Buffs.HeartbreakDebuff>(), time);
                }
            }
        }

    }
}
