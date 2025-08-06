using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Tmodtober.NPCs
{
    public class Bunnygirl_Default:ModNPC
    {

        Player player;
        private bool choiceShown;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 23;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 1000000;

            NPC.defense = 500;

            NPC.knockBackResist = 0;

            NPC.width = 42;
            NPC.height = 1334/23;

            NPC.value = 0;
            NPC.npcSlots = 5;

            NPC.friendly = false;
            NPC.lavaImmune = true;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.behindTiles = true;


            NPC.immortal = true;

            Music = MusicLoader.GetMusicSlot("Tmodtober/Sounds/Music/friendlyBunny_music");

        }

        public override void AI()
        {

            NPC.TargetClosest(true);

            if (Main.player[NPC.target] != null)
            {
                player = Main.player[NPC.target];
            }else{
                return;
            }

            ArianellePlayer _ArPlayer = player.GetModPlayer<ArianellePlayer>();

            if (NPC.ai[1] < 60 * 4 && !choiceShown){

                _ArPlayer.SetMidCutscene();
                if (NPC.ai[0] == 0) {
                    NPC.alpha = 255;
                    Point _playerPos = player.Center.ToTileCoordinates();
                    for (int i = 0; i < 20; i++)
                    {
                        _playerPos += new Point(0, 1);
                        if (WorldGen.SolidTile(_playerPos.X, _playerPos.Y, true))
                        {
                            break;
                        }
                    }
                    NPC.Center = _playerPos.ToWorldCoordinates() + new Vector2(5, NPC.height);
                } else if (NPC.ai[0] < 60)
                {
                    int _dustNum = Main.rand.Next(1, 4);
                    for (int i = 0; i < _dustNum; i++) {
                        Dust.NewDust(NPC.position + new Vector2(NPC.width / 2, -NPC.height), 64, 16, DustID.Dirt, Main.rand.Next(-15, 15) / 4, Main.rand.Next(-10, -5) / 4, Scale: 2);
                    }
                } else if (NPC.ai[0] == 60) {
                    NPC.noGravity = false;
                    NPC.noTileCollide = false;
                    NPC.alpha = 0;
                    NPC.velocity = new Vector2(NPC.direction * -5, -10);
                    NPC.Center += new Vector2(0, -NPC.height*1.5f);
                } else if (NPC.ai[0] > 60+30) {

                    if (ArianelleDeffeatedSystem.bunniesKilled > 13)
                    {
                        if (NPC.ai[1] == 60)
                        {
                            ArianelleDeffeatedSystem.DoGeneralTextMessage("You should have listened.", NPC.Center - new Vector2(0, NPC.height), Color.Red);
                        }
                        if (NPC.ai[1]==60*2.5f)
                        {
                            Projectile.NewProjectile(NPC.GetSource_None(), NPC.Center, Vector2.Zero, ProjectileID.Explosives, 0, 0);
                            NPC.immortal = false;
                            NPC.life = 0;
                            NPC.checkDead();
                        }
                        NPC.ai[1]++;
                    }
                    else{
                        if (NPC.ai[1] == 60)
                        {
                            ArianelleDeffeatedSystem.DoGeneralTextMessage("Hewo there! OwO", NPC.Center - new Vector2(0, NPC.height), Color.Pink);
                        }
                        if (NPC.ai[1] == 60 * 2)
                        {
                            ArianelleDeffeatedSystem.DoGeneralTextMessage("Could you pwease stop killing bunnies?", NPC.Center - new Vector2(0, NPC.height), Color.Pink);
                        }
                        if (NPC.ai[1] == 60 * 3)
                        {
                            ArianelleDeffeatedSystem.DoGeneralTextMessage("Killing bunnies is cruel. Period. UnU", NPC.Center - new Vector2(0, NPC.height), Color.Pink);
                        }
                        NPC.ai[1]++;

                        if (Math.Abs(NPC.velocity.Y) < 3 && NPC.oldVelocity.Y > 5)
                        {
                            NPC.rotation = 0;
                            NPC.velocity = Vector2.Zero;
                        }
                        else if (Math.Abs(NPC.velocity.Y) > 1)
                        {
                            NPC.rotation += MathHelper.PiOver4;
                        }
                    }
                }


                NPC.ai[0]++;
                ArianelleDeffeatedSystem.ResetChoise();
            }
            else
            {
                NPC.TargetClosest(true);
                NPC.velocity = Vector2.Zero;
                choiceShown = true;
                _ArPlayer.SetShowingChoices(true);
                if (NPC.ai[0] >0)
                {
                    NPC.ai[1]=0;
                    NPC.ai[0]=0;
                }
                if (ArianelleDeffeatedSystem.GetSelection() != ArianelChoiseSelection.none)
                {
                    _ArPlayer.SetShowingChoices(false);
                    switch (ArianelleDeffeatedSystem.GetSelection())
                    {
                        case ArianelChoiseSelection.murderer:
                            if (NPC.ai[1] == 0)
                            {
                                ArianelleDeffeatedSystem.DoGeneralTextMessage("I gave you a peaceful option...", NPC.Center + new Vector2(0, -NPC.height), Color.Red);
                            }
                            else if (NPC.ai[1] == 60 * 2)
                            {
                                ArianelleDeffeatedSystem.DoGeneralTextMessage("But I'm glad you didn't take it", NPC.Center + new Vector2(0, -NPC.height), Color.Red);
                            }
                            else if (NPC.ai[1] > 60 * 2.5f)
                            {
                                Projectile.NewProjectile(NPC.GetSource_None(),NPC.Center,Vector2.Zero,ProjectileID.Explosives,0,0);
                                NPC.immortal = false;
                                NPC.life = 0;
                                NPC.checkDead();
                                return;
                            }
                            NPC.ai[1]++;
                            break;
                        case ArianelChoiseSelection.noMoreKilling:
                            if (NPC.ai[1] == 0)
                            {
                                ArianelleDeffeatedSystem.DoGeneralTextMessage("<3 Twank you! You tha bestie <3", NPC.Center + new Vector2(0, -NPC.height), Color.Pink);
                            }else if (NPC.ai[1] == 60*2)
                            {
                                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Tink_",0,2), NPC.Center);
                                ArianelleDeffeatedSystem.DoGeneralTextMessage("Just don't hurt any more or you'll regret it! Love ya, bye! <3", NPC.Center + new Vector2(0, -NPC.height), Color.Pink);
                            }else if (NPC.ai[1] > 60*2)
                            {
                                NPC.velocity = new Vector2(0, 13);
                                NPC.noTileCollide = true;
                                NPC.noGravity = false;
                            }
                            NPC.ai[1]++;
                            break;
                    }
                    return;
                }
                _ArPlayer.SetMidCutscene();
            }

        }

        public override void OnKill()
        {
            NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<NPCs.Boss.BunnyBoss_FightForm>());
        }

    }
}
