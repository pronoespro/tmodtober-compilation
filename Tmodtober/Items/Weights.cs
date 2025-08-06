using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace Tmodtober.Items
{
    public class Weights:ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 9999;

            Item.rare= ItemRarityID.LightRed;

            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.consumable = true;

            Item.useStyle = ItemUseStyleID.Thrust;
        }

        public override bool CanUseItem(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.BunnyBoss_FightForm>()))
            {
                return false;
            }

            NPC _npc;
            for(int i = 0; i <Main.maxNPCs;i++)
            {
                _npc = Main.npc[i];
                if(_npc.active && BunnyKillNPCOverride.IsBunny(_npc))
                {
                    EntitySource_Parent _s = new EntitySource_Parent(player);
                    _npc.life = 0;

                    NPC.NewNPC(_s, (int)_npc.Center.X, (int)_npc.Center.Y, ModContent.NPCType<NPCs.Boss.BunnyBoss_FightForm>());

                    return true;
                }
            }

            return false;
        }

        public override bool ConsumeItem(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            Recipe _recipe= Recipe.Create(Type);
            _recipe.AddIngredient(ItemID.IronBar, 3).AddTile(TileID.Anvils);
            _recipe.Register();
        }

    }
}
