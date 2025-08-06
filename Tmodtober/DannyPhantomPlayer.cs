using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Tmodtober
{
    public class DannyPhantomPlayer:ModPlayer
    {

        public const int MaxPhantomPower = 100, ShieldConsumeCooldown=60;

        public bool HasPhantomPowers;
        public bool IsPhantom;
        public Vector2 _desMovement;
        public int shootTimer;
        public static bool showPhantomPower;

        private int phantomPower;
        private bool shielding;
        private int shieldConsumeCurCooldown;

        public void AddPhantomPower(int _power)
        {
            phantomPower += _power;
            phantomPower = Math.Min(phantomPower, MaxPhantomPower);
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            if (HasPhantomPowers){
                tag.Add("PhantomPower", true);
            }
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            if (tag.ContainsKey("PhantomPower"))
            {
                HasPhantomPowers = true;
            }
        }

        public override void PreUpdate()
        {
            if (!HasPhantomPowers)
            {
                phantomPower = 0;
                IsPhantom = false;
                Player.fullRotation = 0;
            }

            if (IsPhantom)
            {
                Player.lifeRegen += 20;
                if (Main.time % 25 == 0)
                {
                    phantomPower--;
                }

                if (phantomPower <= 0)
                {
                    GoGhost();
                }

            }
            else
            {
                if (Main.time % 50 == 0)
                {
                    phantomPower = Math.Min(phantomPower + 1, MaxPhantomPower);
                }
            }
            shieldConsumeCurCooldown--;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            shielding = false;
            if (TmodtoberMod.TriggerPowersKeybind.JustPressed && HasPhantomPowers)
            {
                if (!IsPhantom && phantomPower == MaxPhantomPower)
                {
                    GoGhost();
                }
                else if (IsPhantom)
                {
                    GoGhost();
                }
            }

            _desMovement =Vector2.Lerp(_desMovement, new Vector2(triggersSet.Left ? -1 : (triggersSet.Right ? 1 : 0), triggersSet.Up ? -1 : (triggersSet.Down ? 1 : 0)),0.2f);

            if (IsPhantom)
            {

                shielding = triggersSet.MouseRight;

                if (triggersSet.MouseLeft && shootTimer<=0 && !shielding)
                {
                    phantomPower -= 5;
                    shootTimer = 10;

                    EntitySource_Parent _s = new EntitySource_Parent(Player);
                    Vector2 _vel = Vector2.Normalize(Main.MouseWorld - Player.Center) * 20;
                    Vector2 _pos= _vel+Player.Center;
                    int _proj=Projectile.NewProjectile(_s, _pos, _vel, ProjectileID.PhantasmalBolt, 300, 1, Player.whoAmI);
                    Main.projectile[_proj].friendly = true;
                    Main.projectile[_proj].hostile= false;
                }

            }
            base.ProcessTriggers(triggersSet);
        }

        public void GoGhost()
        {
            IsPhantom = !IsPhantom;
            Player.fullRotation = 0;
        }

        public override void PreUpdateMovement()
        {

            base.PreUpdateMovement();

            if (IsPhantom)
            {
                shootTimer--;

                Player.velocity = new Vector2(0, -0.01f);
                Player.Center += _desMovement*15;
                Player.fullRotation = MathHelper.Lerp(0, _desMovement.ToRotation()+MathHelper.PiOver2,MathHelper.Clamp(Vector2.DistanceSquared(_desMovement,Vector2.Zero), 0,1));
                Player.fullRotationOrigin = new Vector2(Player.width / 2, Player.height / 2);
            }

        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {

            if (shielding)
            {
                if (shieldConsumeCurCooldown <= 0)
                {
                    phantomPower -= 10;
                    shieldConsumeCurCooldown = ShieldConsumeCooldown;
                }

                return false;
            }

            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {

            if (shielding)
            {
                if (shieldConsumeCurCooldown <= 0)
                {
                    phantomPower -= 10;
                    shieldConsumeCurCooldown = ShieldConsumeCooldown;
                }
                return false;
            }

            return base.CanBeHitByProjectile(proj);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {

            if (IsPhantom)
            {
                Player.statLife = Player.statLifeMax2;
                IsPhantom = false;
                phantomPower = 0;
                return false;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }

        public override void FrameEffects()
        {
            base.FrameEffects();

            if (!Main.dedServ && IsPhantom && Vector2.DistanceSquared(_desMovement,Vector2.Zero)<=0.1f)
            {
                Player.body = TmodtoberMod.PlayerInvisibilityTorsoTexture;
                Player.legs = TmodtoberMod.PlayerInvisibilityLegsTexture;
                Player.head = TmodtoberMod.PlayerInvisibilityHeadTexture;
                Player.face = TmodtoberMod.PlayerInvisibilityFaceTexture;
                Player.faceHead = TmodtoberMod.PlayerInvisibilityFaceTexture;
            }
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            base.ModifyDrawInfo(ref drawInfo);

            DannyPhantomPlayer _danny = drawInfo.drawPlayer.GetModPlayer<DannyPhantomPlayer>();
            if (_danny.IsPhantom)
            {
                drawInfo.hideEntirePlayer = true;

                drawInfo.hideCompositeShoulders = true;
                drawInfo.hideHair = true;
                drawInfo.hidesBottomSkin = true;
                drawInfo.hidesTopSkin = true;
                drawInfo.armorHidesArms = true;
                drawInfo.armorHidesHands = true;
                drawInfo.drawFrontAccInNeckAccLayer = false;
                drawInfo.drawFrontAccInNeckAccLayer = false;
                drawInfo.hidesBottomSkin = true;
                drawInfo.hidesTopSkin = true;

            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {

            if (phantomPower > 0 && HasPhantomPowers && showPhantomPower)
            {
                float scale = 0.75f;
                float rotation = Player.fullRotation;

                Texture2D _texture = ModContent.Request<Texture2D>("Tmodtober/PowerSprites/powerMeter_bg").Value;
                Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
                Vector2 _pivot = new Vector2(_texture.Width / 2, _texture.Height / 2);
                Vector2 _offset = new Vector2(0, Player.height * 4).RotatedBy(rotation);
                Color _clr = IsPhantom ? Color.White : (Color.Gray * (phantomPower == MaxPhantomPower ? 1 : 0.5f));

                Main.spriteBatch.Draw(_texture, Player.Center + _offset - Main.screenPosition, _rect, _clr, rotation, _pivot, scale, SpriteEffects.None, 0);

                Rectangle _fillRect = new Rectangle(0, 0, (int)(_texture.Width * ((float)phantomPower / MaxPhantomPower)), _texture.Height);
                _texture = ModContent.Request<Texture2D>("Tmodtober/PowerSprites/powerMeter_fill").Value;
                Main.spriteBatch.Draw(_texture, Player.Center + _offset - Main.screenPosition, _fillRect, _clr, rotation, _pivot, scale, SpriteEffects.None, 0);

                _texture = ModContent.Request<Texture2D>("Tmodtober/PowerSprites/powerMeter_outline").Value;
                Main.spriteBatch.Draw(_texture, Player.Center + _offset - Main.screenPosition, _rect, _clr, rotation, _pivot, scale, SpriteEffects.None, 0);

                if(phantomPower==MaxPhantomPower || IsPhantom)
                {
                    _pivot = new Vector2(_texture.Width / 4.2f, _texture.Height/2);
                    _texture = ModContent.Request<Texture2D>("Tmodtober/PowerSprites/powerMeter_chargedIcon").Value;
                    Main.spriteBatch.Draw(_texture, Player.Center + _offset - Main.screenPosition, _rect, _clr, rotation, _pivot, scale*3, SpriteEffects.None, 0);
                }
            }

            if(shielding)
            {
                Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>("Tmodtober/PowerSprites/shield").Value;
                Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
                Vector2 _pivot = new Vector2(_texture.Width / 2, _texture.Height / 2);

                Main.spriteBatch.Draw(_texture, Player.Center-Main.screenPosition, _rect, Color.White *0.5f, Player.fullRotation, _pivot, 1f, SpriteEffects.None, 0);
            }



            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }

    }
}
