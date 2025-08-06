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

namespace Tmodtober
{

    public struct ShadowSword
    {
        public Vector2 position;
        public float rotation;
        public float scale;

        public ShadowSword(Vector2 _pos,float _rot=0,float _scale=1f)
        {
            position = _pos;
            rotation = _rot;
            scale = _scale;
        }
    }

    public class DarkCorruptionPlayer:ModPlayer
    {

        public int corruptionLevel;
        public static int SwordsPerCorruptionLevel = 4;

        public ShadowSword[] _swordPositions;
        private int _curSwordAttacking;
        private float _curAttackTime;
        private bool _shadowAttacked;

        public override void PreUpdate()
        {

            if (_swordPositions == null)
            {
                _swordPositions = new ShadowSword[corruptionLevel* SwordsPerCorruptionLevel];
            }

            if (corruptionLevel>0 && !Player.HasBuff<Buffs.DarkCorruptionDebuff>())
            {
                corruptionLevel = 0;
                _swordPositions = new ShadowSword[corruptionLevel* SwordsPerCorruptionLevel];
            }

            if (corruptionLevel == 0 && _swordPositions!=null)
            {
                _swordPositions = new ShadowSword[corruptionLevel* SwordsPerCorruptionLevel];
            }

            if (corruptionLevel > 0)
            {
                CheckSwordPositions();

                for (int i = 0; i < _swordPositions.Length; i++)
                {
                    if (i == _curSwordAttacking)
                    {
                        _swordPositions[i].scale = 1f;

                        float _curDistance = Math.Max(0, (_curAttackTime - 0.7f)) * 10;
                        _swordPositions[i].position = new Vector2(MathF.Cos(_swordPositions[i].rotation) * _curDistance, MathF.Sin(_swordPositions[i].rotation) * _curDistance) *50;

                        float _curScale = Math.Max(0, Math.Min(1,2f*_curAttackTime));
                        _swordPositions[i].scale = _curScale;

                    }else { 
                        _swordPositions[i].scale -= 0.01f;
                    }
                }

                if (_curSwordAttacking >= _swordPositions.Length)
                {
                    _curSwordAttacking = 0;
                }

                if (!_shadowAttacked && _curAttackTime <= 0.7f)
                {
                    int _shadowBuff = -1;
                    for (int i = 0; i < Player.buffType.Length; i++)
                    {
                        if (Player.buffType[i] != 0 && Player.buffType[i] == ModContent.BuffType<Buffs.DarkCorruptionDebuff>())
                        {
                            _shadowBuff = i;
                        }
                    }
                    if (_shadowBuff >= 0) {
                        PlayerDeathReason _dr=PlayerDeathReason.ByCustomReason(Player.name+"'s shadow stabbed them in the heart");
                        Player.Hurt(_dr,Player.statLifeMax/100*5,Math.Sign(-_swordPositions[_curSwordAttacking].position.X),dodgeable:false,armorPenetration:int.MaxValue,knockback:0f);
                    }
                }

                _curAttackTime -= 0.04f;
                if (_curAttackTime < 0)
                {
                    _swordPositions[_curSwordAttacking].rotation = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
                    _curAttackTime = 1;
                    _curSwordAttacking = (_curSwordAttacking + 1) % _swordPositions.Length;
                    _shadowAttacked = false;
                }

            }

            base.PreUpdate();
        }

        private void CheckSwordPositions()
        {
            if (_swordPositions.Length != corruptionLevel* SwordsPerCorruptionLevel)
            {
                ShadowSword[] _oldSwords = _swordPositions;

                _swordPositions = new ShadowSword[corruptionLevel* SwordsPerCorruptionLevel];
                for(int i = 0; i < _swordPositions.Length; i++)
                {
                    if (i < _oldSwords.Length){
                        _swordPositions[i] = _oldSwords[i];
                    }else{
                        _swordPositions[i] = new ShadowSword(Vector2.Zero, Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi), 0f);
                    }
                }

            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (_swordPositions!=null && _swordPositions.Length > 0 && drawInfo.shadow == 0f)
            {
                Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>("Tmodtober/impaleSword");
                Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
                for (int i = 0; i < _swordPositions.Length; i++)
                {
                    Main.spriteBatch.Draw(_texture, Player.Center+_swordPositions[i].position - Main.screenPosition, _rect, Color.White, _swordPositions[i].rotation+MathHelper.PiOver4, new Vector2(_texture.Width / 2, _texture.Height / 2), Math.Max(0,_swordPositions[i].scale*4f), SpriteEffects.None, 0);
                }
            }

            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }

    }
}
