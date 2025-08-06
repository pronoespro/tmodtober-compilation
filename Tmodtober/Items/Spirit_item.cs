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
    class Spirit_item : ModItem
	{
		public override void SetDefaults()
		{
			// useStyle = 1;
			// autoReuse = true;
			// useTurn = true;
			// useAnimation = 15;
			// useTime = 10;
			// maxStack = CommonMaxStack;
			// consumable = true;
			// width = 12;
			// height = 12;
			// makeNPC = 361;
			// noUseGraphic = true;

			// Cloning ItemID.Frog sets the preceding values
			Item.CloneDefaults(ItemID.Frog);
			Item.makeNPC = ModContent.NPCType<NPCs.Spirit>();
			Item.value += Item.buyPrice(0, 0, 0, 1); // Make this critter worth slightly more than the frog
			Item.rare = ItemRarityID.Blue;
			Item.width = 16;
			Item.height = 16;
		}
	}
}
