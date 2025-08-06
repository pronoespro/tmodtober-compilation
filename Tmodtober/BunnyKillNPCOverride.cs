using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace Tmodtober
{
    public class BunnyKillNPCOverride:GlobalNPC
    {

        public override void OnKill(NPC npc)
        {
            if (!ArianelleDeffeatedSystem.downedArianel && IsBunny(npc) && !Terraria.NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.BunnyBoss_FightForm>()))
            {
                Player _targ;
                npc.TargetClosest();
                _targ = (npc.target>=0)?Main.player[npc.target]:Main.player[Main.myPlayer];
                KillBunny(_targ, npc);
            }
        } 

        public static bool IsBunny(NPC npc)
        {
            return npc.type == NPCID.Bunny || npc.type == NPCID.BunnySlimed || npc.type == NPCID.BunnyXmas || npc.type == NPCID.TownBunny
                || npc.type == NPCID.GemBunnyAmber || npc.type == NPCID.GemBunnyAmethyst || npc.type == NPCID.GemBunnyDiamond || npc.type == NPCID.GemBunnyEmerald
                || npc.type == NPCID.GemBunnyRuby || npc.type == NPCID.GemBunnySapphire || npc.type == NPCID.GemBunnyTopaz || npc.type == NPCID.GoldBunny || npc.type == NPCID.PartyBunny 
                || npc.type==NPCID.ExplosiveBunny;
        }

        public static void KillBunny(Player _targ,NPC npc)
        {
            if (ArianelleDeffeatedSystem.downedArianel){
                return;
            }

            ArianelleDeffeatedSystem.bunniesKilled++;
            if (ArianelleDeffeatedSystem.bunniesKilled == 10)
            {
                EntitySource_BossSpawn _s = new EntitySource_BossSpawn(_targ);
                Terraria.NPC.NewNPC(_s, (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NPCs.Bunnygirl_Default>(), Target: npc.target);
            }
            else if (ArianelleDeffeatedSystem.bunniesKilled > 15 && ArianelleDeffeatedSystem.bunniesKilled % 5 == 0)
            {
                EntitySource_BossSpawn _s = new EntitySource_BossSpawn(_targ);
                if (ArianelleDeffeatedSystem.arianelSummoned)
                {
                    Terraria.NPC.NewNPC(_s, (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NPCs.Boss.BunnyBoss_FightForm>(), Target: npc.target);
                }
                else
                {
                    Terraria.NPC.NewNPC(_s, (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NPCs.Bunnygirl_Default>(), Target: npc.target);
                }
            }
        }

    }
}
