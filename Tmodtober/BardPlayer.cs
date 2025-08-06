using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober
{
    public class BardPlayer:ModPlayer
    {

        public static int SoundPlayDelay;
        public static string BasePath = "Tmodtober/Sounds/";

        private float _singTimer;
        private int _lastVoiceUsed;

        public bool damageUp;
        public bool damageDown;

        public static bool doSingingSounds;

        public override void ResetEffects()
        {
            base.ResetEffects();
            damageUp = false;
            damageDown = false;
        }

        public override void Load()
        {
            base.Load();
            SoundPlayDelay = (int)(63 * 1.65f);
            _singTimer = SoundPlayDelay;
            _lastVoiceUsed = -1;
        }

        public override void Unload()
        {
            base.Unload();
            SoundPlayDelay = 0;
            _lastVoiceUsed = 0;
            _singTimer = 0;
            _lastVoiceUsed = 0;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {

            _singTimer++;
            if (_singTimer >= SoundPlayDelay)
            {

                if (Player.whoAmI == Main.myPlayer && TmodtoberMod.SingKeybind.Current)
                {
                    int _curVoice = Main.rand.Next(1, 4);
                    while (_curVoice == _lastVoiceUsed)
                    {
                        _curVoice = Main.rand.Next(1, 4);
                    }
                    _lastVoiceUsed = _curVoice;

                    Vector2 _mousePos = new Vector2(Main.mouseX, Main.mouseY) - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                    float _distance = (Math.Abs(_mousePos.X) + Math.Abs(_mousePos.Y)) / 2;

                    string _genderStart = Player.Male ? "m_" : "f_";

                    if (_distance < 100)
                    {
                        if (doSingingSounds)
                        {
                            SoundStyle _newStyle = new SoundStyle(BasePath + _genderStart + "terrarianCloseVoice_" + _curVoice.ToString());
                            SoundEngine.PlaySound(_newStyle);
                        }

                        Player.AddBuff(ModContent.BuffType<Buffs.CloseSingBuff>(), (int)SoundPlayDelay * 2);

                        CreateNotes(Color.Lime);
                    }
                    else if (_distance < 200)
                    {
                        if (doSingingSounds)
                        {
                            SoundStyle _newStyle = new SoundStyle(BasePath + _genderStart + "terrarianMidVoice_" + _curVoice.ToString());
                            SoundEngine.PlaySound(_newStyle);
                        }

                        Player.AddBuff(ModContent.BuffType<Buffs.MidSingBuff>(), (int)SoundPlayDelay * 3);

                        CreateNotes(Color.Blue);
                    }
                    else
                    {
                        if (doSingingSounds)
                        {
                            SoundStyle _newStyle = new SoundStyle(BasePath + _genderStart + "terrarianFarVoice_" + _curVoice.ToString());
                            SoundEngine.PlaySound(_newStyle);
                        }

                        Player.AddBuff(ModContent.BuffType<Buffs.FarSingBuff>(), (int)SoundPlayDelay * 6);
                        CreateNotes(Color.Purple);
                    }
                    
                   _singTimer = 0;
                }
            }


            base.ProcessTriggers(triggersSet);
        }

        public void CreateNotes(Color _clr)
        {
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Player.position + new Vector2(-Player.width +i*20, Main.rand.Next(-25,25)), 5, 5, ModContent.DustType<Dusts.NoteDust>(), newColor: _clr);
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            base.ModifyWeaponDamage(item, ref damage);

            if (damageDown)
            {
                damage *= 0.01f;
            }else if (damageUp)
            {
                damage *= 2;
            }
        }

    }
}
