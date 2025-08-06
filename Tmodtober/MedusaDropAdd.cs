using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace Tmodtober
{
    public class MedusaDropAdd:GlobalNPC
    {

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Medusa)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new PostMoonLordDrop(), ModContent.ItemType<Items.MedusaHelmet>()));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }

    }

    public class PostMoonLordDrop : IItemDropRuleCondition
    {
        private static LocalizedText Description;

        public PostMoonLordDrop()
        {
            Description ??= Language.GetOrRegister("Mods.Tmodtober.DropConditions.MoonLord");
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            return NPC.downedMoonlord;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return Description.Value;
        }
    }
}
