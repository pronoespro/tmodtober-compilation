using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
using Tmodtober.Items;

namespace Tmodtober
{
    class PartyGirlHasCakeForYouNow:GlobalNPC
    {

        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            PartyPlayer _p = Main.player[Main.myPlayer].GetModPlayer<PartyPlayer>();

            if (npc.type == NPCID.PartyGirl && _p.IsPlayerBirthday)
            {
                //Main.NewText("Here's some cake");
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] == null)
                    {
                        items[i] = new Item(ItemID.SliceOfCake);
                        break;
                    }
                    else
                    {
                        if (items[i].IsAir)
                        {
                            items[i] = new Item(ItemID.SliceOfCake);
                            break;
                        }

                    }
                }
            }
            base.ModifyActiveShop(npc, shopName, items);
        }
    }
}
