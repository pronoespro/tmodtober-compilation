using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.IO;
using Microsoft.Xna.Framework;

namespace Tmodtober
{
    public class IceOutpostPlacingSystem:ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int PiramidIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));

            if (PiramidIndex != -1)
            {
                tasks.Insert(PiramidIndex, new SnowOutpostGenPass("Snow Outpost", 100f));
            }

            base.ModifyWorldGenTasks(tasks, ref totalWeight);
        }

    }

    public class SnowOutpostGenPass : GenPass
    {
        public SnowOutpostGenPass(String name, float loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Adding a snow outpost";

            int _outpostAmmount = WorldGen.genRand.Next(1, 4);
            for (int k = 0; k < _outpostAmmount; k++)
            {
                bool success = false;
                int attempts = 0;

                while (!success)
                {
                    attempts++;
                    if (attempts >= 1000)
                    {
                        break;
                    }
                    int x = WorldGen.genRand.Next(40, Main.maxTilesX - 40);
                    int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, (int)GenVars.worldSurfaceHigh);

                    Tile _tile = Main.tile[x, y];
                    if (_tile != null && _tile.HasTile && (_tile.TileType == TileID.SnowBlock))
                    {
                        int _height = 0;
                        Point _pos = new Point(x, y - 1);
                        while (Main.tile[_pos.X,_pos.Y].HasTile) {
                            _height++;
                            if (_height > 50) { break; }
                            _pos.Y -= 1;
                        }
                        if (PlaceSnowOutpost(_pos+new Point(0,-1)))
                        {
                            success = true;
                        }
                    }
                }
                if (!success)
                {
                    TmodtoberMod.Instance.Logger.Error("Couldn't find good place for the outpost");
                }
            }

        }

        public bool PlaceSnowOutpost(Point _position)
        {


            //legs
            if (!WorldUtils.Gen(_position + new Point(-5, 0), new Shapes.Rectangle(1, 4), new Actions.SetTile(TileID.BorealBeam)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place outpost");
                return false;
            }
            if (!WorldUtils.Gen(_position + new Point(4, 0), new Shapes.Rectangle(1, 4), new Actions.SetTile(TileID.BorealBeam)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place outpost");
                return false;
            }

            //Box shape
            if (!WorldUtils.Gen(_position+new Point(-5,-6), new Shapes.Rectangle(10, 8), new Actions.ClearTile()))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't Destroy blocks for outpost");
                return false;
            }
            if (!WorldUtils.Gen(_position + new Point(-5, -6), new Shapes.Rectangle(10, 8), new Actions.SetTile(TileID.BorealWood)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place outpost");
                return false;
            }
            if (!WorldUtils.Gen(_position + new Point(-4, -5), new Shapes.Rectangle(8, 6), new Actions.ClearTile()))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't Destroy blocks for outpost");
                return false;
            }

            //Doors
            if (!WorldUtils.Gen(_position + new Point(-6, -2), new Shapes.Rectangle(12, 3), new Actions.ClearTile()))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't Destroy blocks for door");
                return false;
            }

            if (!WorldGen.PlaceDoor(_position.X+4, _position.Y-1, TileID.ClosedDoor, 13))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't make left door");
                return false;
            }
            if (!WorldGen.PlaceDoor(_position.X -5, _position.Y-1, TileID.ClosedDoor, 13))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't make right door");
                return false;
            }

            //Walls
            if (!WorldUtils.Gen(_position + new Point(-4, -5), new Shapes.Rectangle(8, 6), new Actions.PlaceWall(WallID.BorealWood)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place general wall");
                return false;
            }
            if (!WorldUtils.Gen(_position + new Point(-6, 2), new Shapes.Rectangle(12, 2), new Actions.PlaceWall(WallID.BorealWoodFence)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place fence");
                return false;
            }
            if (!WorldUtils.Gen(_position + new Point(-2, -2), new Shapes.Rectangle(4, 2), new Actions.PlaceWall(WallID.BorealWoodFence)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place fence");
                return false;
            }

            //Roof
            if (!WorldUtils.Gen(_position + new Point(-6, -7), new Shapes.Rectangle(12, 1), new Actions.SetTile(TileID.BorealWood)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place outpost");
                return false;
            }
            if (!WorldUtils.Gen(_position + new Point(-5, -8), new Shapes.Rectangle(10, 1), new Actions.SetTile(TileID.BorealWood)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place outpost");
                return false;
            }
            if (!WorldUtils.Gen(_position + new Point(-4, -9), new Shapes.Rectangle(8, 1), new Actions.SetTile(TileID.BorealWood)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place outpost");
                return false;
            }
            if (!WorldUtils.Gen(_position + new Point(-3, -10), new Shapes.Rectangle(6, 1), new Actions.SetTile(TileID.BorealWood)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place outpost");
                return false;
            }
            if (!WorldUtils.Gen(_position + new Point(-2, -11), new Shapes.Rectangle(4, 1), new Actions.SetTile(TileID.BorealWood)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place outpost");
                return false;
            }
            if (!WorldUtils.Gen(_position + new Point(-1, -12), new Shapes.Rectangle(2, 1), new Actions.SetTile(TileID.BorealWood)))
            {
                TmodtoberMod.Instance.Logger.Error("Couldn't place outpost");
                return false;
            }

            Point _tilePoint =  _position + new Point(-1, 0);

            int _chestIndex = WorldGen.PlaceChest(_tilePoint.X, _tilePoint.Y,style:3);
            if (_chestIndex !=-1)
            {
                Chest _chest = Main.chest[_chestIndex];

                List<(int type, int stack)> _itemsInChest=new List<(int type, int stack)>()
                {
                    new (ItemID.FlurryBoots, 1),
                    new (ItemID.BlizzardinaBottle, 1),
                    new (ItemID.FlareGun, 1),
                    new (ItemID.BlueFlare,Main.rand.Next(5, 50)),
                    new (ItemID.BlueTorch,WorldGen.genRand.Next(5, 50)),
                    new (ItemID.LesserHealingPotion,  WorldGen.genRand.Next(1, 5))
                };

                int _chestItemIndex = 0;
                
                foreach(var _itemToAdd in _itemsInChest)
                {
                    Item item = new Item();
                    item.SetDefaults(_itemToAdd.type);
                    item.stack = _itemToAdd.stack;
                    _chest.item[_chestItemIndex] = item;

                    _chestItemIndex++;
                    if (_chestItemIndex >= 40)
                    {
                        break;
                    }
                }
            }else{
                TmodtoberMod.Instance.Logger.Error("Couldn't place chest");
                return false;
            }

            TmodtoberMod.Instance.Logger.Info("Finished placing outpost!");

            return true;
        }

    }

}
