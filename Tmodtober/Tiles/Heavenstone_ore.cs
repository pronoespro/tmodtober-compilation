using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Tmodtober.Tiles
{
    class Heavenstone_ore : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileOreFinderPriority[Type] = 410; // Metal Detector value, see https://terraria.wiki.gg/wiki/Metal_Detector
			Main.tileShine2[Type] = true; // Modifies the draw color slightly.
			Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
			Main.tileMerge[Type][TileID.Cloud] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(175, 175, 255), name);

			DustType = 84;
			HitSound = SoundID.Tink;
			MineResist = 1f;
			MinPick = 65/5;
		}
	}

	public class ExampleOreSystem : ModSystem
	{


		// World generation is explained more in https://github.com/tModLoader/tModLoader/wiki/World-Generation
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			// Because world generation is like layering several images on top of each other, we need to do some steps between the original world generation steps.

			// Most vanilla ores are generated in a step called "Shinies", so for maximum compatibility, we will also do this.
			// First, we find out which step "Shinies" is.
			int LarvaIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Larva"));

			if (LarvaIndex != -1)
			{
				// Next, we insert our pass directly after the original "Shinies" pass.
				// ExampleOrePass is a class seen bellow
				tasks.Insert(LarvaIndex + 1, new HeavenstoneOrePass("Heavenstone Ores", 200));
			}
		}
	}

	public class HeavenstoneOrePass : GenPass
	{
		public HeavenstoneOrePass(string name, float loadWeight) : base(name, loadWeight)
		{

		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			// progress.Message is the message shown to the user while the following code is running.
			// Try to make your message clear. You can be a little bit clever, but make sure it is descriptive enough for troubleshooting purposes.
			progress.Message = "Generating Heavenstone";

			// Ores are quite simple, we simply use a for loop and the WorldGen.TileRunner to place splotches of the specified Tile in the world.
			// "6E-05" is "scientific notation". It simply means 0.00006 but in some ways is easier to read.
			for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05/4); k++)
			{
				// The inside of this for loop corresponds to one single splotch of our Ore.
				// First, we randomly choose any coordinate in the world by choosing a random x and y value.
				int cloudX = WorldGen.genRand.Next(0, Main.maxTilesX);

				// WorldGen.worldSurfaceLow is actually the highest surface tile. In practice you might want to use WorldGen.rockLayer or other WorldGen values.
				int cloudY = WorldGen.genRand.Next(0, (int)GenVars.worldSurface/2);

				// Then, we call WorldGen.TileRunner with random "strength" and random "steps", as well as the Tile we wish to place.
				// Feel free to experiment with strength and step to see the shape they generate.

				int _width = WorldGen.genRand.Next(4, 10);
				int _height= WorldGen.genRand.Next(4, 10);

				WorldGen.TileRunner(cloudX, cloudY,WorldGen.genRand.Next(12,15), 5, TileID.Cloud,true,0,0,false,true);


				for (int _x = -_width / 2; _x < _width / 2+1; _x++)
				{
					Point _pos = new Point(cloudX + _x, cloudY);
					if (WorldGen.InWorld(_pos.X, _pos.Y))
					{
						WorldGen.KillTile(_pos.X, _pos.Y);
						WorldGen.PlaceTile(_pos.X,_pos.Y, ModContent.TileType<Heavenstone_ore>());
					}
				}
				for (int _y = -_height / 2; _y < _height / 2+1; _y++)
				{
					Point _pos = new Point(cloudX, cloudY + _y);
					if (WorldGen.InWorld(_pos.X, _pos.Y))
					{
						WorldGen.KillTile(_pos.X, _pos.Y);
						WorldGen.PlaceTile(_pos.X, _pos.Y, ModContent.TileType<Heavenstone_ore>());
					}
				}

			}
		}
	}
}
