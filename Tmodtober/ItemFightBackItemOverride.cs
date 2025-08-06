using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Tmodtober
{
    public class ItemFightBackItemOverride : GlobalItem
    {

        public static bool ItemsFightBackEnabled;
        public static int minPlayerDistance = 250;

        public int actionTimer;
        public override bool InstancePerEntity => true;

        public Player _closestTarget;
        public int _dir;

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {

            if (!ItemsFightBackEnabled)
            {
                base.Update(item, ref gravity, ref maxFallSpeed);
                return;
            }

            if (_closestTarget == null || !_closestTarget.active || _closestTarget.dead)
            {
                FindClosestPlayer(item);
            }

            Vector2 _dirFromTraget = (_closestTarget == null) ? new Vector2(0, 25) : _closestTarget.Center - item.Center;
            _dirFromTraget = Vector2.Normalize(_dirFromTraget);

            int _types = 0;

            if (Vector2.DistanceSquared(_closestTarget.Center, item.Center) > 25 * 25)
            {
                if (item.accessory)
                {
                    gravity = 0;
                    maxFallSpeed = 0;
                    if (Vector2.DistanceSquared(_closestTarget.Center, item.Center) < 500 * 500)
                    {
                        Vector2 _desDir = -_dirFromTraget * Math.Clamp((int)item.value / 10000, 1, 5);
                        item.velocity = Vector2.Lerp(item.velocity, _desDir, 0.025f);
                    }
                    else
                    {
                        item.velocity = Vector2.Lerp(item.velocity, Vector2.Zero, 0.2f);
                    }

                    _types++;

                }
                if (item.damage >= 0)
                {

                    if (actionTimer % 30 == 0 && actionTimer > 80)
                    {
                        EntitySource_Parent _s = new EntitySource_Parent(item);
                        int _proj;
                        if (item.shoot > ProjectileID.None)
                        {
                            _proj = Projectile.NewProjectile(_s, item.Center, _dirFromTraget * 25, item.shoot, item.damage, item.knockBack);
                        }
                        else
                        {
                            _proj = Projectile.NewProjectile(_s, item.Center, _dirFromTraget * 25, ProjectileID.SwordBeam, item.damage, item.knockBack);
                        }
                        Main.projectile[_proj].friendly = false;
                        Main.projectile[_proj].hostile = true;

                        item.velocity = -_dirFromTraget * 5f;
                    }

                    _types++;
                }

                if (item.pick > 0)
                {
                    _types++;
                    if (actionTimer % 60 == 0)
                    {
                        Point _itemPos = item.Center.ToTileCoordinates();
                        Point _minePoint;
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                _minePoint = new Point(x, y) + _itemPos;

                                WorldGen.digTunnel(_minePoint.X, _minePoint.Y, 0, 0, 2, 2);
                            }
                        }
                    }
                }
                if (item.axe > 0)
                {
                    _types++;
                    if (actionTimer % 60 == 0 && actionTimer >= 80)
                    {
                        Point _itemPos = item.Center.ToTileCoordinates();
                        Point _minePoint;
                        for (int x = -5; x <= 5; x++)
                        {
                            for (int y = -5; y <= 5; y++)
                            {
                                if (y == 0 && x == 0) { continue; }

                                _minePoint = new Point(x, y) + _itemPos;
                                if (WorldGen.TileEmpty(_minePoint.X, _minePoint.Y))
                                {
                                    Main.tile[_minePoint].ResetToType(TileID.WoodBlock);
                                }
                            }
                        }
                    }
                }
                if (item.hammer > 0)
                {
                    if (actionTimer % 100 == 0 && actionTimer >= 60 * 4)
                    {
                        EntitySource_Parent _s = new EntitySource_Parent(item);

                        NPC.SpawnOnPlayer(_closestTarget.whoAmI, NPCID.CursedHammer);

                    }
                }
                if (item.healLife > 0 || item.healMana > 0)
                {
                    gravity = 0;
                    maxFallSpeed = 0;
                    _types++;
                    if (actionTimer % 100 == 0 && actionTimer > 80)
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            if (Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].townNPC && !Main.npc[i].CountsAsACritter)
                            {
                                Main.npc[i].life = Math.Clamp(Main.npc[i].life + (item.healLife + item.healMana), 1, Main.npc[i].lifeMax);
                            }
                        }
                    }
                }

                if (item.makeNPC > 0)
                {
                    if (actionTimer % 50 == 0 && actionTimer >= 60 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        EntitySource_Parent _s = new EntitySource_Parent(item);
                        NPC.NewNPC(_s, (int)item.Center.X, (int)item.Center.Y, item.makeNPC);
                    }
                }

                if (_types == 0)
                {

                    Point _itemPos = item.Center.ToTileCoordinates();

                    if (WorldGen.SolidOrSlopedTile(_itemPos.X, _itemPos.Y))
                    {
                        _dir = Math.Sign(item.Center.X - _closestTarget.Center.X);
                        item.Center += new Vector2(0, -3);
                    }
                    else if (WorldGen.SolidOrSlopedTile(_itemPos.X, _itemPos.Y + 1))
                    {
                        _dir = Math.Sign(item.Center.X - _closestTarget.Center.X);
                        item.velocity = new Vector2(0, -15);
                    }
                    item.velocity = new Vector2(_dir * Math.Clamp((int)item.value / 10000, 1, 6), item.velocity.Y);
                    gravity = 1.2f;
                }



                actionTimer++;
            }

            base.Update(item, ref gravity, ref maxFallSpeed);
        }

        public void FindClosestPlayer(Item item)
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && Vector2.DistanceSquared(Main.player[i].Center, item.Center) < minPlayerDistance * minPlayerDistance && (_closestTarget == null || Vector2.DistanceSquared(Main.player[i].Center, item.Center) < Vector2.DistanceSquared(_closestTarget.Center, item.Center)))
                {
                    _closestTarget = Main.player[i];
                }
            }
        }

    }
}
