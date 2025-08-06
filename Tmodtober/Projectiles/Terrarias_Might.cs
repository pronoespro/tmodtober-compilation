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
    public class Terrarias_Might:ModProjectile
    {

        public Vector2[] _swordPositions;
        public Vector2 _targetPos;
        public float _lerpToTarget;
        public const float _distance=100;

        int attackCooldown;
        NPC _target;

        public int swordAmmount;

        TerrariaMightPlayer _mightPlayer;

        public override void SetDefaults()
        {
            _swordPositions = new Vector2[0];

            Projectile.friendly = true;
            Projectile.damage = 30;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.width = 32*4;
            Projectile.height = 32*4;
            Projectile.tileCollide = false;
            Projectile.penetrate =- 1;

        }

        public override void AI()
        {
            base.AI();


            if (Projectile.owner >= 0 && _mightPlayer == null)
            {
                _mightPlayer = Main.player[Projectile.owner].GetModPlayer<TerrariaMightPlayer>();
            }

            if (!_mightPlayer.usingBlessing && Projectile.timeLeft > 10)
            {

                Projectile.timeLeft = 1;

                return;
            }

            if (_target==null || !_target.active  ||_target.life<=0){
                _target = Projectile.FindTargetWithinRange(400);
            }
            
            if(_target!=null && _target.active)
            {
                _targetPos = _target.Center;
                Projectile.ai[1] += 0.1f;
                attackCooldown -= 1;
            }else{

                attackCooldown = 0;
                Projectile.ai[1] = 0;
                _targetPos = Vector2.Zero;
            }

            Projectile.ai[0]+=0.1f;

            swordAmmount = (int)(((float)_mightPlayer.Player.statLife) / _mightPlayer.Player.statLifeMax*10f);

            CheckCurrentBladeCount(swordAmmount);

            float _curRot;
            int _curBlade = (int)(Projectile.ai[0] * 10f/25f )%_swordPositions.Length;

            for (int i = 0; i < _swordPositions.Length; i++)
            {
                _curRot = ((float)i + Projectile.ai[0])/ swordAmmount * MathF.PI*2;
                _swordPositions[i] = new Vector2(MathF.Cos(_curRot), MathF.Sin(_curRot)) * _distance;

                if (_target!=null && _curBlade == i)
                {
                    _swordPositions[i] = Vector2.Lerp(_swordPositions[i], _targetPos-Projectile.Center, Math.Clamp(Math.Min(1,(Math.Min(Projectile.ai[1]%1*2,(1 - (Projectile.ai[1]%1))*2))),0,1));

                    if (Vector2.DistanceSquared(_target.Center, Projectile.Center + _swordPositions[i]) < 150 * 150 && attackCooldown<=0)
                    {
                        attackCooldown = 15;
                        int _damage = (int)(Projectile.damage * _mightPlayer.Player.GetTotalDamage(DamageClass.Summon).Multiplicative+ _mightPlayer.Player.GetTotalDamage(DamageClass.Summon).Additive);
                        _mightPlayer.Player.ApplyDamageToNPC(_target, _damage, Projectile.knockBack, Math.Sign((_target.Center - (Projectile.Center + _swordPositions[i])).X));
                    }

                }

            }

            Projectile.Center = _mightPlayer.Player.Center;

            Projectile.timeLeft = 100;

        }

        public override bool? CanHitNPC(NPC target)
        {

            Player _p = Main.player[Projectile.owner];

            if (target.friendly ||target.CountsAsACritter)
            {
                return false;
            }

            for (int i = 0; i < _swordPositions.Length;i++) {
                if (Vector2.Distance(target.Center,Projectile.Center+_swordPositions[i])<=300)
                {
                    return true;
                }
            }

            return false;
        }

        public void CheckCurrentBladeCount(int _count)
        {
            if (_swordPositions.Length != _count)
            {
                _swordPositions = new Vector2[_count];
                float _curRot;
                for(int i = 0; i < _swordPositions.Length; i++)
                {
                    _curRot = ((float)i) / _count*MathF.PI;
                    _swordPositions[i] = new Vector2(MathF.Cos(_curRot),MathF.Sin(_curRot))*_distance;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D _texture = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
            for (int i = 0; i < _swordPositions.Length; i++) {
                Main.spriteBatch.Draw(_texture, Projectile.Center+_swordPositions[i]-Main.screenPosition, _rect, lightColor, -MathHelper.PiOver4+Math.Clamp((Projectile.ai[0]-i) % _swordPositions.Length * MathHelper.PiOver2,0,MathHelper.Pi*2), new Vector2(_texture.Width / 2, _texture.Height / 2),3f, SpriteEffects.None, 0);
            }
            return false;
        }

    }
}
