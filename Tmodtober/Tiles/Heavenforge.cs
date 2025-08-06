using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Tmodtober.Tiles
{
    public class Heavenforge:ModTile
    {
		public override void SetStaticDefaults()
		{
			// Properties
			Main.tileSolidTop[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			
			DustType = DustID.Torch;
			AdjTiles = new int[] { TileID.Hellforge,TileID.Furnaces };

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.addTile(Type);

			// Etc
			AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Furnance"));
			AnimationFrameHeight = 456/12;
		}

		public override void NumDust(int x, int y, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
			if (++frameCounter>= 4)
			{
				frameCounter = 0;
				frame = (frame+1) % 12;
			}
        }

    }
}
