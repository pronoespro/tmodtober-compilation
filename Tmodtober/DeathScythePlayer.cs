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
    public class DeathScythePlayer:ModPlayer
    {

        int soulDashing=-1;
        bool[] _originalShowAccesories;


        public override void PostUpdate()
        {
            if (soulDashing>=0)
            {
                Player.velocity = Vector2.Normalize(Main.MouseWorld - Player.Center) * 15;
                float _desRot = Player.velocity.ToRotation() - MathHelper.PiOver2;
                Player.fullRotation = _desRot;
                Player.fullRotationOrigin = new Vector2(Player.width / 2, Player.height / 2);

                if (soulDashing % 5 == 0)
                {
                    for(int i = 0; i < Main.maxNPCs; i++)
                    {
                        if(Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].CountsAsACritter && !Main.npc[i].townNPC && !Main.npc[i].immortal && !Main.npc[i].dontTakeDamage && Player.CanHit(Main.npc[i]) && Vector2.DistanceSquared(Main.npc[i].Center,Player.Center)<50*50)
                        {
                            NPC.HitInfo _info= Main.npc[i].CalculateHitInfo(100, 1, true, 0.1f);
                            Main.npc[i].life -= _info.Damage;
                            Main.npc[i].HitEffect(_info);
                            Main.npc[i].checkDead();
                        }
                    }
                }

            }else if (soulDashing == -1)
            {
                Player.fullRotation = 0;
            }
            soulDashing--;
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            base.ModifyDrawInfo(ref drawInfo);

            if (soulDashing >= 0)
            {
                drawInfo.hideEntirePlayer = true;

                drawInfo.hideCompositeShoulders = true;
                drawInfo.hideHair = true;
                drawInfo.hidesBottomSkin = true;
                drawInfo.hidesTopSkin = true;
                drawInfo.armorHidesArms = true;
                drawInfo.armorHidesHands = true;
                drawInfo.drawFrontAccInNeckAccLayer = false;
                drawInfo.drawFrontAccInNeckAccLayer= false;
                drawInfo.hidesBottomSkin= true;
                drawInfo.hidesTopSkin= true;

                if (_originalShowAccesories == null)
                {
                    _originalShowAccesories = Player.hideVisibleAccessory;

                    for (int i = 0; i < Player.hideVisibleAccessory.Length; i++)
                    {
                        Player.hideVisibleAccessory[i] = true;
                    }
                }

            }
            else if(_originalShowAccesories!=null)
            {
                for (int i = 0; i < Player.hideVisibleAccessory.Length; i++)
                {
                    Player.hideVisibleAccessory[i] = _originalShowAccesories[i];
                }
                _originalShowAccesories = null;
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);

            if (drawInfo.shadow == 0 && GetIsSoulDashing())
            {
                if (Main.rand.Next(1) == 0)
                {
                    Dust.NewDust(Player.Center + new Vector2(-Player.width / 2, -Player.width / 2), Player.width, Player.width, DustID.Frost, -Player.velocity.X /4, -Player.velocity.Y / 4, Alpha: 150, Color.Blue);
                }
            }
        }

        public override void FrameEffects()
        {
            base.FrameEffects();

            if (GetIsSoulDashing())
            {
                if (!Main.dedServ)
                {
                    for (int i = 0; i < Player.hideVisibleAccessory.Length; i++) { 
                        Player.hideVisibleAccessory[i] = true;
                    }

                    Player.body = TmodtoberMod.PlayerFlameTexture;
                    Player.legs = TmodtoberMod.PlayerInvisibilityLegsTexture;
                    Player.head = TmodtoberMod.PlayerInvisibilityHeadTexture;
                    Player.face = TmodtoberMod.PlayerInvisibilityFaceTexture;
                    Player.faceHead = TmodtoberMod.PlayerInvisibilityFaceTexture;
                }
            }
        }

        public void SetSoulDashing(int dashTime)
        {
            soulDashing = dashTime;
        }

        public bool GetIsSoulDashing()
        {
            return soulDashing >= 0;
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (GetIsSoulDashing())
            {
                return true;
            }
            return base.FreeDodge(info);
        }

    }
}
