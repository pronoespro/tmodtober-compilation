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
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;

namespace Tmodtober.NPCs
{
    public class Lagann:ModNPC
    {

        int curFrame = 0;
        bool sawPlayer;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height= 64;

            NPC.damage= 75;
            NPC.defense = 100;

            NPC.lifeMax = 10000;
            NPC.behindTiles = false;
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath56;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0.1f;
            NPC.lavaImmune = true;

            NPC.value = 100000;

        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.hardMode ||spawnInfo.Player.Center.ToTileCoordinates().Y<Main.maxTilesY-200)
            {
               return 0;
            }
            return NPC.downedPlantBoss?0.5f:0.025f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.AdamantiteDrill));
            npcLoot.Add(ItemDropRule.Common(ItemID.PanicNecklace,10));
            npcLoot.Add(ItemDropRule.Common(ItemID.AdamantiteBar,3,10,20));
            npcLoot.Add(ItemDropRule.Common(ItemID.IronBar,1,5,10));
        }

        public override void AI()
        {
            Point _npcPos = NPC.Center.ToTileCoordinates();
            NPC.TargetClosest(false);

            if (NPC.target >= 0)
            {
                Player _targ = Main.player[NPC.target];

                if (WorldGen.SolidOrSlopedTile(_npcPos.X, _npcPos.Y))
                {
                    NPC.ai[0] = 0;


                    if (Vector2.DistanceSquared(NPC.velocity, Vector2.Zero) <= 5 * 5)
                    {
                        NPC.velocity *= 1.2f;
                    }
                    else
                    {
                        Vector2 _dir = Vector2.Normalize(_targ.Center - NPC.Center);

                        NPC.velocity = Vector2.Lerp(NPC.velocity, _dir * 30, 0.03f);
                    }

                    NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;

                    if (Main.time % 45 == 0)
                    {
                        SoundStyle _style = new SoundStyle("Terraria/Sounds/Item_22");
                        SoundEngine.PlaySound(_style, NPC.Center);
                    }
                } else {

                    if (sawPlayer)
                    {
                        NPC.ai[0] = 0;
                        NPC.velocity += new Vector2(0, 0.1f);
                    }
                    else if (Vector2.Dot(Vector2.Normalize(_targ.Center-NPC.Center),Vector2.Normalize(NPC.velocity))>0.5f && Vector2.DistanceSquared(NPC.velocity,Vector2.Zero)>10*10)
                    {
                        NPC.velocity += new Vector2(0, 0.1f);
                        NPC.ai[0] = 0;
                        if (NPC.ai[0] < 40)
                        {
                            sawPlayer = true;
                        }
                    }
                    else {
                        if (NPC.ai[0] < 100)
                        {
                            NPC.rotation = 0;
                            NPC.ai[0]++;
                            NPC.velocity *= 0.9f;

                            if (NPC.ai[0] == 40)
                            {
                                SoundStyle _style = new SoundStyle("Terraria/Sounds/Zombie_5");
                                SoundEngine.PlaySound(_style, NPC.Center);
                            }
                        }
                        else
                        {
                            NPC.ai[0]++;
                            NPC.velocity += new Vector2(0, 1f);
                            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                        }
                    }
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            sawPlayer = false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[0] == 0 || NPC.ai[0]>=100)
            {
                if (Main.time % 10 == 0)
                {
                    curFrame = Main.rand.Next(0, 5);
                }
            }else{
                curFrame = 4;
            }

            NPC.frame.Y = frameHeight * curFrame;
        }

    }
}
