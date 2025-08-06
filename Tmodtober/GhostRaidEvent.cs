using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace Tmodtober
{
    public class GhostRaidEvent:GlobalNPC
    {

        public static int[] NormalRaidSpawns = new int[]{
            NPCID.Ghost
            };

        public static int[] TwiceRaidSpawns = new int[]
        {
            NPCID.Ghost,
            NPCID.Wraith
        };

        public static int[] ThriceRaidSpawns = new int[]
        {
            NPCID.Ghost,
            NPCID.Wraith,
            NPCID.PirateGhost
        };

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (!Main.dayTime && Main.player[Main.myPlayer].townNPCs >= 2)
            {
                if (GhostBustedWorld.Instance.thricePowerRaidIncoming)
                {
                    spawnRate /= 10;
                    maxSpawns *= 10;
                }else if (GhostBustedWorld.Instance.twicePowerfullRaidIncoming)
                {
                    spawnRate /= 3;
                    maxSpawns *= 2;
                }
                else if (GhostBustedWorld.Instance.ghostRaidIncoming)
                {
                    spawnRate /= 2;
                }
            }
            base.EditSpawnRate(player, ref spawnRate, ref maxSpawns);
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {

            if(!Main.dayTime && (Main.player[Main.myPlayer].townNPCs>=2 || Main.bloodMoon ||Main.player[Main.myPlayer].ZoneCorrupt|| Main.player[Main.myPlayer].ZoneCrimson|| Main.player[Main.myPlayer].ZoneDungeon|| Main.player[Main.myPlayer].ZoneGraveyard|| Main.player[Main.myPlayer].ZoneRain|| Main.player[Main.myPlayer].ZoneShadowCandle|| Main.player[Main.myPlayer].ZoneSnow|| Main.player[Main.myPlayer].ZoneUndergroundDesert|| Main.player[Main.myPlayer].ZoneUnderworldHeight|| Main.player[Main.myPlayer].ZoneWaterCandle))
            {
                if (GhostBustedWorld.Instance.thricePowerRaidIncoming)
                {
                    pool.Clear();
                    for(int i = 0; i < ThriceRaidSpawns.Length;i++)
                    {
                        pool.Add(ThriceRaidSpawns[i],50f);
                    }
                }else if (GhostBustedWorld.Instance.twicePowerfullRaidIncoming)
                {
                    pool.Clear();
                    for (int i = 0; i < TwiceRaidSpawns.Length; i++)
                    {
                        pool.Add(TwiceRaidSpawns[i], 15f);
                    }

                }
                else if (GhostBustedWorld.Instance.ghostRaidIncoming)
                {
                    pool.Clear();
                    for (int i = 0; i < NormalRaidSpawns.Length; i++)
                    {
                        pool.Add(NormalRaidSpawns[i], 3f);
                    }

                }
            }

            base.EditSpawnPool(pool, spawnInfo);
        }

    }
}
