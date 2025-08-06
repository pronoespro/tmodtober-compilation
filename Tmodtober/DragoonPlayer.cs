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
using Microsoft.Xna.Framework.Graphics;

namespace Tmodtober
{

    public class DragoonCloneTrail
    {
        public Vector2 position;
        public float timeSinceMade;
        public float maxAlpha;

        public DragoonCloneTrail(Vector2 _pos,float _maxAlpha)
        {
            position = _pos;
            maxAlpha = _maxAlpha;
        }

    }

    public class DragoonPlayer:ModPlayer
    {

        public List<DragoonCloneTrail> clones;

        public int jumpTime,startTime,dodgeTime;
        public Vector2 jumpStartPosition, targetPosition;

        public override void Initialize()
        {
            clones = new List<DragoonCloneTrail>();
        }

        public override void Unload()
        {
            clones = null;
        }

        public void Jump(int _jumpTime,int _dodgeTime,Vector2 _targetPos)
        {
            dodgeTime = _dodgeTime;
            jumpTime = _jumpTime;
            startTime= _jumpTime;
            jumpStartPosition = Player.Center;
            targetPosition = _targetPos;
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            return dodgeTime>0;
        }

        public override void PostUpdate()
        {
            if (jumpTime > startTime / 4*3)
            {
                Player.fullRotation = MathHelper.Pi;
                Player.direction = MathF.Sign(targetPosition.X - jumpStartPosition.X);
            }
            else if (jumpTime > 0)
            {
                Player.fullRotation = 0;
                Player.direction = MathF.Sign(targetPosition.X - jumpStartPosition.X);
            }
            else if (jumpTime == 0)
            {
                Player.direction = MathF.Sign(targetPosition.X - jumpStartPosition.X);
                Player.fullRotation = 0;
            }

            jumpTime--;
            dodgeTime--;

            List<int> _clonesToRemove = new List<int>();
            for(int i = 0; i < clones.Count; i++)
            {
                clones[i].timeSinceMade++;
                if (clones[i].timeSinceMade > 20)
                {
                    _clonesToRemove.Add(i);
                }
            }
            for(int i = _clonesToRemove.Count - 1; i >= 0; i--)
            {
                clones.RemoveAt(i);
            }

        }

        public override void ModifyScreenPosition()
        {
            if (jumpTime > 0) {
                Main.screenPosition = Vector2.Lerp(targetPosition,jumpStartPosition, (float)jumpTime / startTime)+new Vector2(-Main.screenWidth/2,-Main.screenHeight/2);
            } 
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);

            Texture2D _cloneSprite = (Texture2D)ModContent.Request<Texture2D>("Tmodtober/PowerSprites/Aura");
            Rectangle _rect = new Rectangle(0, 0, _cloneSprite.Width, _cloneSprite.Height);
            for (int i = 0; i < clones.Count; i++) {
                Main.spriteBatch.Draw(_cloneSprite, clones[i].position- Main.screenPosition, _rect, Color.Blue * ((20 - clones[i].timeSinceMade)/20f*clones[i].maxAlpha), 0, new Vector2(_cloneSprite.Width / 2, _cloneSprite.Height / 2), 2f, SpriteEffects.None, 0) ;
            } 
        }

        public void AddClone(Vector2 _pos,float alpha)
        {
            clones.Add(new DragoonCloneTrail(_pos,alpha));
        }

    }
}
