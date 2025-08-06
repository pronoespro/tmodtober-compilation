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
using Microsoft.Xna.Framework.Graphics;

namespace Tmodtober.Projectiles
{
    public class PaintProjectile:ModProjectile
    {

        public static int TILE_PAINT_SIZE = 6;
        public static int Wall_PAINT_SIZE = 10;
        byte curPaint;

        public void SetCurrentPaint(byte _paint)
        {
            curPaint = _paint;
        }

        public override void SetDefaults()
        {
            curPaint = PaintID.None;

            Projectile.width = 8;
            Projectile.height= 8;

            Projectile.friendly= true;
            Projectile.hostile= false;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.knockBack = 1f;
            Projectile.ignoreWater = false;

        }

        public override void AI()
        {

            if (Projectile.wet)
            {
                Projectile.Kill();
            }

            Projectile.velocity +=new Vector2(0, 0.2f);

            Point _tilePos = Projectile.Center.ToTileCoordinates();

            WorldGen.paintTile(_tilePos.X, _tilePos.Y, curPaint);

            for (int i = -Wall_PAINT_SIZE; i < Wall_PAINT_SIZE; i++)
            {
                for (int j = -Wall_PAINT_SIZE; j < Wall_PAINT_SIZE; j++)
                {
                    if (Math.Abs(i) + Math.Abs(j) < Wall_PAINT_SIZE)
                    {
                        WorldGen.paintWall(_tilePos.X + i, _tilePos.Y + j, curPaint);
                    }
                }
            }

            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {


            if (Projectile.wet)
            {
                return base.OnTileCollide(oldVelocity);
            }

            Point _tilePos = Projectile.Center.ToTileCoordinates();

            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if(Main.npc[i].active && IsNPCWhitInDistance(Main.npc[i]))
                {
                    Main.npc[i].color = ColourHelper.ConvertCurPaintToColor(curPaint);
                }
            }

            for(int i = -TILE_PAINT_SIZE; i < TILE_PAINT_SIZE; i++)
            {
                for(int j = -TILE_PAINT_SIZE; j < TILE_PAINT_SIZE; j++)
                {
                    if (Math.Abs(i) + Math.Abs(j) < TILE_PAINT_SIZE)
                    {
                        WorldGen.paintTile(_tilePos.X + i, _tilePos.Y + j, curPaint);
                    }
                }
            }
            for (int i = -Wall_PAINT_SIZE; i < Wall_PAINT_SIZE; i++)
            {
                for (int j = -Wall_PAINT_SIZE; j < Wall_PAINT_SIZE; j++)
                {
                    if (Math.Abs(i) + Math.Abs(j) < Wall_PAINT_SIZE)
                    {
                        WorldGen.paintWall(_tilePos.X + i, _tilePos.Y + j, curPaint);
                    }
                }
            }

            return base.OnTileCollide(oldVelocity);
        }

        public bool IsNPCWhitInDistance(NPC _target)
        {
            Point _targetPos = _target.Center.ToTileCoordinates(), _curPos = Projectile.Center.ToTileCoordinates();
            for(int i = -5;i<=5;i++)
            {
                for(int j = -5; j <= 5; j++)
                {
                    if(new Point(_targetPos.X + i, _targetPos.Y + j) == _curPos)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            if (curPaint != PaintID.None)
            {
                target.color = ConvertCurPaintToColor();
            }
            else
            {
                target.color = Color.White;
            }

            base.OnHitNPC(target, hit, damageDone);
        }

        public Color ConvertCurPaintToColor()
        {
            return ColourHelper.ConvertCurPaintToColor(curPaint);
        }

        public override bool PreDraw(ref Color lightColor)
        {

            if (curPaint != PaintID.None)
            {
                Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>(Texture).Value;

                Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);

                Main.spriteBatch.Draw(_texture,Projectile.Center-Main.screenPosition, _rect, ConvertCurPaintToColor(),Projectile.rotation,new Vector2(_texture.Width/2,_texture.Height/2),Projectile.scale,SpriteEffects.None,0);
                return false;
            }

            return base.PreDraw(ref lightColor);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            if (curPaint != PaintID.None)
            {
                modifiers.SourceDamage += 20;
            }
        }

    }
}
