using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Tmodtober
{
    public class TownNPCOverride:GlobalNPC
    {

        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {

            if (!Main.dedServ && npc.townNPC && !NPC.AnyNPCs(ModContent.NPCType<NPCs.TownDefender>()))
            {
                EntitySource_Parent _s = new EntitySource_Parent(npc);
                NPC.NewNPC(_s,(int) npc.Center.X,(int) npc.Center.Y, ModContent.NPCType<NPCs.TownDefender>(),ai0:npc.whoAmI);
            }

            base.HitEffect(npc, hit);
        }

    }
}
