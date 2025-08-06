using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace Tmodtober
{
    public class BreakTileOverride:GlobalTile
    {

        public const int TileBreakMaxDistance = 150;

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {

            base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);

            if (!Main.dedServ)
            {

                Vector2 _tilePos = new Point(i, j).ToWorldCoordinates();

                if (!fail && !effectOnly)
                {
                    Player _player = null;
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        if (Main.player[p].active && Vector2.DistanceSquared(Main.player[p].Center, _tilePos) < TileBreakMaxDistance * TileBreakMaxDistance)
                        {
                            if (_player == null || Vector2.DistanceSquared(Main.player[p].Center, _tilePos) < Vector2.DistanceSquared(_player.Center, _tilePos))
                            {
                                _player = Main.player[p];
                            }
                        }
                    }
                    if (_player != null)
                    {
                        BlockBreakerPlayer _breaker = _player.GetModPlayer<BlockBreakerPlayer>();
                        _breaker.BreakBlock();
                    }
                }
            }

        }

    }
}
