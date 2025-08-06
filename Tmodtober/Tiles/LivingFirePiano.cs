using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ObjectData;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Tmodtober.Tiles
{
    public class LivingFirePiano : ModTile
	{

		public const int NextStyleHeight = 40;

		public override void SetStaticDefaults()
		{
			// Properties
			Main.tileTable[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.

			DustType = DustID.Torch;
			AdjTiles = new int[] { TileID.Tables };

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.addTile(Type);

			// Etc
			AddMapEntry(new Color(255, 0, 0), Language.GetText("MapObject.Piano"));

			AnimationFrameHeight = 37;
		}


		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter % 7 == 0)
			{
				int curFrame = frame;
				while (frame == curFrame)
				{
					frame = Main.rand.Next(0, 3);
				}
			}
		}

	}
}
