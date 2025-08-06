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

namespace Tmodtober.NPCs
{

    public enum TownDefenderAttackType
    {
        dash,
        projectile,
        heal
    }

    public class TownDefender:ModNPC
    {

        TownDefenderAttackType curAttack;
        NPC summoner;

        int curTarget=-1;
        bool hitTarget;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 16;
            NPC.height = 16;

            NPC.lifeMax = 100;
            NPC.friendly = true;

            if (NPC.ai[0] >= 0 && Main.npc[(int)NPC.ai[0]].active && Main.npc[(int)NPC.ai[0]].townNPC)
            {
                summoner = Main.npc[(int)NPC.ai[0]];

            }

            NPC.damage = 100;
            NPC.defense = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.Center += new Vector2(0,-20);

            NPC.ai[0] = 0;
        }

        public override void AI()
        {


            if (curTarget < 0 || !Main.npc[curTarget].active){
                curTarget = FindClosestEnemy();
            }


            int curFlyFrame = (int)(NPC.ai[0] % 18);

            if (NPC.ai[1] < 60) {
                hitTarget = false;
                NPC.rotation = 0;
                if (curFlyFrame <8) {
                    NPC.frameCounter= 0;
                }else if (curFlyFrame < 10){
                    NPC.frameCounter = 1;
                }else if (curFlyFrame < 16){
                    NPC.frameCounter = 2;
                }else{
                    NPC.frameCounter = 3;
                }

                if (curFlyFrame < 10){
                    NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(0, -3),0.35f);
                }else{
                    NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(0, 3),0.35f);
                }

                if (summoner!=null && summoner.active && Vector2.DistanceSquared(summoner.Center, NPC.Center) < 100){
                    Vector2 _dir = Vector2.Normalize(summoner.Center - NPC.Center);
                    NPC.velocity = _dir * 35f;
                }

            }
            else{

                if (curTarget < 0)
                {
                    NPC.ai[1] = 0;
                }else{

                    if (NPC.ai[1] == 60)
                    {
                        curAttack = (TownDefenderAttackType)Main.rand.Next(0, 3);
                    }
                    int curAttackFrame = (int)NPC.ai[1] - 60;
                    int maxAttackTime = 15;

                    switch (curAttack)
                    {
                        case TownDefenderAttackType.dash:
                            maxAttackTime = 30;

                            if (curAttackFrame == 0)
                            {
                                SoundEngine.PlaySound(SoundID.Bird, NPC.Center);
                            }

                            if (Vector2.DistanceSquared(NPC.Center, Main.npc[curTarget].Center) < 32*32)
                            {
                                if (!hitTarget)
                                {
                                    SoundEngine.PlaySound(SoundID.NPCHit1,NPC.Center);
                                }
                                hitTarget = true;

                                Main.npc[curTarget].life -= 15;
                                Main.npc[curTarget].HitEffect(dmg: 15);
                                Main.npc[curTarget].checkDead();
                            }
                            else if (curAttackFrame > 5)
                            {
                                Vector2 _dir = Vector2.Normalize(Main.npc[curTarget].Center - NPC.Center);
                                NPC.velocity = _dir * Math.Min(32f, Vector2.Distance(Main.npc[curTarget].Center, NPC.Center));
                                NPC.rotation = _dir.ToRotation();
                            }

                            if (curAttackFrame < 5){
                                NPC.frameCounter = 4;
                            }else{
                                NPC.frameCounter = 5;
                            }
                            break;
                        case TownDefenderAttackType.heal:

                            if (NPC.ai[1] % 5 == 0 && summoner != null && summoner.active) {
                                summoner.life += 10;
                                summoner.HealEffect(10);
                            }

                            if (NPC.ai[1] % 5 <= 1){
                                NPC.frameCounter = 4;
                            }else{
                                NPC.frameCounter = 2;
                            }
                            break;
                        case TownDefenderAttackType.projectile:
                            if (curAttackFrame == 5) {
                                EntitySource_Parent _s = new EntitySource_Parent(NPC);
                                Vector2 _dir = Vector2.Normalize(Main.npc[curTarget].Center - NPC.Center);

                                Projectile.NewProjectile(_s, NPC.Center, _dir * 15, ProjectileID.MagicMissile, NPC.damage, 1f, Main.myPlayer);
                            }

                            if (curAttackFrame < 5){
                                NPC.frameCounter = 4;
                            }else{
                                NPC.frameCounter = 5;
                            }

                            break;
                    }

                    if (curAttackFrame>= maxAttackTime)
                    {
                        NPC.ai[1] = 0;
                    }
                }
            }

            NPC.ai[0]++;
            NPC.ai[1]++;

            if (summoner == null){
                NPC.timeLeft = 0;
            }

            base.AI();
        }

        public int FindClosestEnemy()
        {

            int curEnemy = -1;

            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if(Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].CountsAsACritter)
                {
                    if(curEnemy==-1 || (Vector2.DistanceSquared(NPC.Center, Main.npc[i].Center) < Vector2.DistanceSquared(NPC.Center, Main.npc[curEnemy].Center))){
                        curEnemy = i;
                    }
                }
            }

            return curEnemy;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = (int)(NPC.frameCounter * frameHeight);
        }

    }
}
