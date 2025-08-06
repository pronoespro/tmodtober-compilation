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
    public class DarkNPCOverride:GlobalNPC
    {

        public static List<int> _darkNPCs = new List<int>()
        {
            NPCID.SkeletronPrime,
            NPCID.PrimeCannon,
            NPCID.PrimeLaser,
            NPCID.PrimeSaw,
            NPCID.PrimeVice,
            NPCID.PossessedArmor,
            NPCID.Wraith
        };

        public static List<int> _hardmodeDarkNPCs = new List<int>()
        {
            NPCID.EyeofCthulhu,
            NPCID.SkeletronHead,
            NPCID.SkeletronHand
        };

        public static List<int> _nighttimeDarkNPCs = new List<int>()
        {
            NPCID.HallowBoss
        };

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            base.OnHitPlayer(npc, target, hurtInfo);

            if (_darkNPCs.Contains(npc.type) ||
                (Main.hardMode && _hardmodeDarkNPCs.Contains(npc.type))||
                (!Main.dayTime && _nighttimeDarkNPCs.Contains(npc.type)))
            {
                target.AddBuff(ModContent.BuffType<Buffs.DarkCorruptionDebuff>(), 300);
                return;
            }

            if (npc.type == ModContent.NPCType<NPCs.Boss.BunnyBoss_FightForm>())
            {
                target.AddBuff(ModContent.BuffType<Buffs.DarkCorruptionDebuff>(), 100);
                return;
            }

        }

    }
}
