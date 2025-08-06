using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Tmodtober
{
    public class MightyPlayer:ModPlayer
    {

        public const int MightToPower=100;

        public int storedMight;
        public bool usingMightPower;

        public const int mightNormalReduction=10;
        public const int mightUsingReduction=20;

        public override void OnHurt(Player.HurtInfo info)
        {
            if (!usingMightPower && storedMight != MightToPower)
            {
                storedMight = Math.Max(storedMight - (usingMightPower ? mightUsingReduction : mightNormalReduction), 0);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (!usingMightPower)
            {
                float damageMultiplier = target.boss ? 1f / 10f : 1f / 100f;
                storedMight = (int)Math.Min(storedMight + Math.Max( damageDone*damageMultiplier,2),MightToPower);
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            base.ProcessTriggers(triggersSet);
            if (TmodtoberMod.mightTriggerKeybing.JustPressed)
            {
                if (storedMight >= MightToPower)
                {
                    SoundStyle _s = new SoundStyle("Tmodtober/Sounds/TriggerSFX");
                    SoundEngine.PlaySound(_s,Player.Center);
                    usingMightPower = true;
                }
                else if (usingMightPower)
                {
                    SoundStyle _s = new SoundStyle("Tmodtober/Sounds/DispellSFX");
                    SoundEngine.PlaySound(_s,Player.Center);
                    usingMightPower = false;
                }
            }
        }

        public override void PreUpdate()
        {
            base.PreUpdate();
            if (usingMightPower)
            {
                Player.lifeRegen += 400;

                Player.statDefense+= 20;

                Player.manaCost = 0.5f;
                Player.manaRegen += 20;

                if (Main.time % 15 == 0)
                {
                    storedMight -= 2;
                }
                if (storedMight <= 0)
                {
                    storedMight = 0;
                    usingMightPower = false;

                    SoundStyle _s = new SoundStyle("Tmodtober/Sounds/DispellSFX");
                    SoundEngine.PlaySound(_s, Player.Center);
                }
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            base.ModifyWeaponDamage(item, ref damage);
            if (usingMightPower)
            {
                damage *= 1.5f;
            }
        }

        public override void ModifyItemScale(Item item, ref float scale)
        {
            base.ModifyItemScale(item, ref scale);
            if (usingMightPower)
            {
                scale *= 3f;
            }
        }

        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            base.ModifyWeaponCrit(item, ref crit);
            if (usingMightPower)
            {
                crit += 10;
            }
        }

        public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback)
        {
            base.ModifyWeaponKnockback(item, ref knockback);
            if (usingMightPower)
            {
                knockback *= 3f;
            }
        }

        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            base.ModifyManaCost(item, ref reduce, ref mult);
            if (usingMightPower)
            {
                mult = 0.5f;
            }
        }
        public override float UseSpeedMultiplier(Item item)
        {
            if (usingMightPower)
            {
                return 1.25f*base.UseSpeedMultiplier(item);
            }
            return base.UseSpeedMultiplier(item);
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {

            if (usingMightPower && Main.rand.Next(0,2)==0)
            {
                return true;
            }
            return base.FreeDodge(info);
        }

        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (usingMightPower && item.damage > 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(source, position + new Vector2(25, 0).RotatedBy(MathHelper.PiOver2 * i), velocity*2, ModContent.ProjectileType<Projectiles.MightProjectile>(), damage, knockback, Player.whoAmI); ;
                }
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);

            float barSize = 0.75f;
            Vector2 _barPos = Player.Center + new Vector2(0,Player.height +50);

            if (storedMight > MightToPower/10 ||usingMightPower)
            {
                float _alpha =(usingMightPower || storedMight>=MightToPower)?1:0.5f;
                string _meterTextureName = Mod.Name + "/PowerSprites/mightMeter_";

                Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>(_meterTextureName + "bg").Value;

                if (_texture != null)
                {
                    Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
                    Vector2 _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

                    Main.spriteBatch.Draw(_texture, _barPos - Main.screenPosition, _rect, Color.White*_alpha, 0, _origin, barSize, SpriteEffects.None, 0);

                    _texture = (Texture2D)ModContent.Request<Texture2D>(_meterTextureName + "fill").Value;
                    if (_texture != null)
                    {
                        Rectangle _newRect = new Rectangle(0, 0, (int)(_texture.Width * ((storedMight * 1f) / MightToPower)), _texture.Height);
                        Main.spriteBatch.Draw(_texture, _barPos - Main.screenPosition, _newRect, Color.White * _alpha, 0, _origin, barSize, SpriteEffects.None, 0);
                    }
                    _texture = (Texture2D)ModContent.Request<Texture2D>(_meterTextureName + "outline").Value;
                    if (_texture != null)
                    {
                        Main.spriteBatch.Draw(_texture, _barPos - Main.screenPosition, _rect, Color.White * _alpha, 0, _origin, barSize, SpriteEffects.None, 0);
                    }
                }
            }

            if (usingMightPower)
            {
                Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>(Mod.Name + "/PowerSprites/Aura").Value;

                if (_texture != null)
                {
                    Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
                    Vector2 _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

                    Vector2 _pos;
                    for (int i = 0; i < 4; i++) {
                        _pos = new Vector2(35, 0).RotatedBy(i * MathHelper.PiOver2);
                        Main.spriteBatch.Draw(_texture, Player.Center + (_pos*0.25f).RotatedBy(Main.time * MathHelper.PiOver4 / 40) - Main.screenPosition, _rect, Color.White * 0.35f, Player.fullRotation, _origin, 1, Player.direction < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                        Main.spriteBatch.Draw(_texture, Player.Center + (_pos*0.5f).RotatedBy(-Main.time * MathHelper.PiOver4 / 20) - Main.screenPosition, _rect, Color.White * 0.25f, Player.fullRotation, _origin, 1, Player.direction < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                        Main.spriteBatch.Draw(_texture, Player.Center + _pos.RotatedBy(Main.time * MathHelper.PiOver4 / 10) - Main.screenPosition, _rect, Color.White* 0.1f, Player.fullRotation, _origin, 1, Player.direction < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    }
                }

            }
        }

        public override void UpdateDead()
        {
            base.UpdateDead();
            storedMight = 0;
            usingMightPower = false;
        }

    }
}
