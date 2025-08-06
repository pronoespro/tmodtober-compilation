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

namespace Tmodtober.NPCs
{
    public class Hooker:ModNPC
    {

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 7;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.LostGirl);
        }

        public override bool PreAI()
        {
            NPC.TargetClosest(true);
            NPC.velocity = Vector2.Zero;

            if (NPC.target >= 0 && Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center) < 150 * 150)
            {
                NPC.ai[0] = 1;
            }
            if (NPC.ai[0] == 1)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 7 * 10)
                {
                    EntitySource_Parent _s = new EntitySource_Parent(NPC);
                    Terraria.NPC.NewNPC(_s, (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.IcyMerman, Target: NPC.target);

                    NPC.life = 0;
                }
            }
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = (int)(NPC.frameCounter / 10)*frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (spawnInfo.Water && !spawnInfo.PlayerSafe && MathF.Abs(spawnInfo.PlayerFloorX- Main.maxTilesX/2) > Main.maxTilesX / 2- WorldGen.beachDistance)
            {
                return 20f;
            }

            return 0;
        }

    }
}
