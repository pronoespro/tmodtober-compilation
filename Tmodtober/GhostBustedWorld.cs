using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;
using System.IO;
using Terraria.Localization;

namespace Tmodtober
{
    class GhostBustedWorld:ModSystem
    {

        public static GhostBustedWorld Instance;

        public override void OnModLoad()
        {
            base.OnModLoad();
            Instance = this;
        }

        public override void OnModUnload()
        {
            base.OnModUnload();
            Instance = null;
        }

        public int curGhostLevel;
        public bool ghostRaidIncoming,twicePowerfullRaidIncoming,thricePowerRaidIncoming;

        public static int TileBreakRaidCount = 48;

        public void AddToGhostLevel()
        {
            curGhostLevel++;
            if (curGhostLevel >= TileBreakRaidCount && !ghostRaidIncoming)
            {
                Main.NewText("The ghosts get agitated");
                ghostRaidIncoming = true;
            }
            if (curGhostLevel >= TileBreakRaidCount*2 && !twicePowerfullRaidIncoming)
            {
                Main.NewText("The ghosts anger is rising");
                twicePowerfullRaidIncoming = true;
            }
            if (curGhostLevel >= TileBreakRaidCount*4 && !thricePowerRaidIncoming)
            {
                Main.NewText("What have you done?!!");
                thricePowerRaidIncoming = true;
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = ghostRaidIncoming;
            flags[1] = twicePowerfullRaidIncoming;
            flags[2] = thricePowerRaidIncoming;
            writer.Write(flags);

            base.NetSend(writer);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            ghostRaidIncoming= flags[0];
            twicePowerfullRaidIncoming = flags[1];
            thricePowerRaidIncoming = flags[2];
            base.NetReceive(reader);
        }

        public override void SaveWorldData(TagCompound tag)
        {
            base.SaveWorldData(tag);
            if (ghostRaidIncoming)
                tag["ghostRaidIncoming"] = true;
            if (twicePowerfullRaidIncoming)
                tag["secondGhostRaid"] = true;
            if (thricePowerRaidIncoming)
                tag["thirdGhostRaid"] = true;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            ghostRaidIncoming = tag.ContainsKey("ghostRaidIncoming");
            twicePowerfullRaidIncoming = tag.ContainsKey("secondGhostRaid");
            thricePowerRaidIncoming = tag.ContainsKey("thirdGhostRaid");
            base.LoadWorldData(tag);
        }

        bool wasNight;

        public override void PreUpdateWorld()
        {

            if (ghostRaidIncoming)
            {
                if (!Main.dayTime && !wasNight)
                {
                    Main.NewText("The ghosts... are coming");
                }

                if (Main.dayTime && wasNight)
                {
                    int[] _npcToKill = thricePowerRaidIncoming ? GhostRaidEvent.ThriceRaidSpawns : (twicePowerfullRaidIncoming ? GhostRaidEvent.TwiceRaidSpawns : GhostRaidEvent.NormalRaidSpawns);
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        if (Main.npc[i] != null && Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].CountsAsACritter)
                        {
                            for (int j = 0; j < _npcToKill.Length; j++)
                            {
                                if (Main.npc[i].type == _npcToKill[j])
                                {
                                    Main.npc[i].life = -1;
                                    Main.npc[i].checkDead();
                                }
                            }
                        }
                    }

                    curGhostLevel = 0;

                    ghostRaidIncoming = false;
                    twicePowerfullRaidIncoming = false;
                    thricePowerRaidIncoming = false;
                    Main.NewText("The ghosts vaporize with the sun, alongside their anger");

                }
            }


            wasNight = !Main.dayTime;

            base.PreUpdateWorld();
        }

    }
}
