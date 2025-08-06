using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace Tmodtober.NPCs.Boss
{

    class RollCloneEffectPositions
    {
        public Vector2 pos;
        public float rotation;
        public int framesLasted;
        public int frame;
        public float startingAlpha;

        public RollCloneEffectPositions(Vector2 _pos, float _rot, int _frame, float _startAlpha)
        {
            pos = _pos;
            rotation = _rot;
            framesLasted = 0;
            frame = _frame;
            startingAlpha = _startAlpha;
        }
    }
    class SuckedInBunny {
        public Vector2 direction;
        public float distance;
        public float alpha;
        public float rotation;
        public int spriteDirection;

        public SuckedInBunny(Vector2 _dir,float _distance,float _rot, int spriteDir)
        {
            direction = _dir;
            distance = _distance;
            alpha = 0;
            rotation = _rot;
            spriteDirection = spriteDir;
        }
    }

    [AutoloadBossHead]
    public class BunnyBoss_FightForm:ModNPC
    {

        public override string Texture => "Tmodtober/NPCs/Boss/bunnygirl_FightForm";
        public override string BossHeadTexture => "Tmodtober/NPCs/Boss/bunnygirl_FightForm_Head";


        public const float DeathAnimWindup =180,DeathFloatAnim=75;
        public const int NormalAttackMax = 6,EnragedAttackMax=8;

        int frame;
        int curAttack;
        bool recheckDirection;
        Vector2 _desVel;

        List<RollCloneEffectPositions> _rollClones;
        List<SuckedInBunny> _bunniesSuckedIn;

        private bool _isFirstSpawn;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 23;
        }

        public override void SetDefaults()
        {

            NPC.aiStyle = -1;
            NPC.lifeMax = 10000;

            NPC.defense = 5;

            NPC.knockBackResist = 0;

            NPC.width = 250;
            NPC.height = 1260 / 6;

            NPC.value = 15000;
            NPC.npcSlots = 5;

            NPC.boss = true;
            NPC.friendly = false;
            NPC.lavaImmune = false;

            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.behindTiles = true;

            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.immortal = true;

            NPC.BossBar = ModContent.GetInstance<BossBars.BunnygirlBossBar>();
            Music= MusicLoader.GetMusicSlot("Tmodtober/Sounds/Music/bunny_music");
            recheckDirection = true;

            _rollClones = new List<RollCloneEffectPositions>();
            _bunniesSuckedIn = new List<SuckedInBunny>();


        }

        public override List<string> SetNPCNameList()
        {
            List<string> _possibleNames = new List<string>();

            if (ArianelleDeffeatedSystem.downedArianel)
            {
                _possibleNames.Add("Buff Bunny");
                _possibleNames.Add("Bunny the Buff");
                _possibleNames.Add("Terrarian's Buff Bunny");
                _possibleNames.Add("The Second Coming");
                _possibleNames.Add("Killing Bunnies is evil, period");
            }
            else
            {
                _possibleNames.Add("Arianesadsadsl the Terrarian Killer");
            }

            return _possibleNames;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("A bunny that made a human-sized mecha to fight you" +
                "\nWatch out for her ''Girl's Usurper Nanite-launcher'' (or G.U.N. for short)")
            });
        }

        public override void AI()
        {
            NPC.TargetClosest(false);

            int _selectedPlayer=-1;
            Player _player=null;
            if (NPC.target >= 0){
                _player = Main.player[NPC.target];
            }else{
                _selectedPlayer=FindTargetPlayer();
                _player = Main.player[_selectedPlayer];
            }
            if (recheckDirection)
            {
                _desVel = _player.Center - NPC.Center;
            }
            if (_player.dead)
            {
                NPC.velocity = new Vector2(0, 20);
                frame = Main.time%10<5? 13:14;
                NPC.alpha += 10;
                NPC.timeLeft = Math.Min(10,NPC.timeLeft);
                NPC.despawnEncouraged = true;
                NPC.noTileCollide = true;
                return;
            }

            if (NPC.ai[0] == 0 && curAttack == 0)
            {
                //DoTextMessage("This is the first time I've spawned, huh?","I've spawned before, but not beaten","I've been beaten before... wait, what am I even saying?",_player.Center + new Vector2(_player.direction * 50, -NPC.height / 2));
                //ArianelleDeffeatedSystem.DoGeneralTextMessage(ArianelleDeffeatedSystem.arianelSummoned?"Summoned before":"Not summoned before", NPC.Center, Color.DarkGreen);
                _isFirstSpawn = !(ArianelleDeffeatedSystem.downedArianel || ArianelleDeffeatedSystem.arianelSummoned);
                ArianelleDeffeatedSystem.arianelSummoned = true;


            }

            switch (curAttack)
            {
                default:
                    curAttack = 1;
                    break;
                case 0:
                    //Spawn

                    if (NPC.ai[0] == 0)
                    {
                        DoTextMessage("Well then, F**K YOU B***H!!!",
                            "DIE ALREADY, YOU PIECE OF S**T!!!",
                            "I'M STRONGER THAN EVER!!!",
                            _player.Center + new Vector2(_player.direction * 50, -NPC.height / 2));
                    }

                    NPC.velocity = new Vector2(0,NPC.velocity.Y);
                    NPC.ai[0]++;
                    NPC.noTileCollide = true;
                    if (NPC.ai[0] < 3 * 60)
                    {
                    NPC.noGravity = true;
                        if (_selectedPlayer >= -1)
                        {


                            NPC.Center = _player.Center + new Vector2(MathF.Sin((float)Main.time / 20f * MathHelper.PiOver2) * 150, _player.height * 2);
                            Point _npcPos = NPC.Center.ToTileCoordinates();

                            for (int i = 0; i < 100; i++)
                            {
                                if (WorldGen.SolidOrSlopedTile(_npcPos.X, _npcPos.Y))
                                {
                                    NPC.Center = _npcPos.ToWorldCoordinates() + new Vector2(0, 150);
                                    break;
                                }
                                _npcPos += new Point(0, 1);
                            }

                            int _dustNum = Main.rand.Next(1, 4);
                            for(int i = 0; i < _dustNum; i++) { 
                                Dust.NewDust(NPC.position + new Vector2(NPC.width / 2,-NPC.height/2), 64, 16, DustID.Dirt, Main.rand.Next(-15, 15)/4, Main.rand.Next(-10, -5)/4, Scale: 2);
                            }
                            if (NPC.ai[0] % 10 == 0)
                            {
                                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Roar_1"), NPC.Center);
                            }
                        }
                        else
                        {
                            Main.NewText("No target");
                        }

                        frame = 0;

                        NPC.immortal = true;
                        NPC.behindTiles = true;
                        NPC.noTileCollide = false;
                        NPC.velocity = Vector2.Zero;
                    }else if (NPC.ai[0] < 4 * 60)
                    {
                        NPC.velocity = Vector2.Zero;
                    }
                    else if (NPC.ai[0] == 4 * 60)
                    {
                        DoTextMessage("Time to face my mecha version",
                            "I can do this forever if needed",
                            "Let me show you my new strength",
                            NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                        NPC.noGravity = false;
                        NPC.noTileCollide = true;
                        NPC.velocity = new Vector2(0, -30);
                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_167"), NPC.Center);
                        return;
                    }else if(NPC.ai[0] < 5 * 60 + 5 && (!(NPC.velocity.Y < 1 && NPC.oldVelocity.Y > 5) || NPC.position != NPC.oldPosition)){
                        if (NPC.velocity.Y < 0 && NPC.ai[0]<5*60)
                        {
                            frame = Main.time % 10 < 5 ? 13 : 14;
                            NPC.direction = (Main.time % 20 < 10) ? -1 : 1;
                            NPC.spriteDirection = -NPC.direction;
                            NPC.ai[0]--;
                            NPC.behindTiles = true;
                            frame = 6;
                            NPC.noTileCollide = true;
                            NPC.velocity.Y += 0.5f;
                        }
                        else
                        {
                            if (frame==6){
                                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_162"), NPC.Center);
                            }
                            frame = Main.time % 10 < 5 ? 13 : 14;
                            NPC.direction = (Main.time % 20 < 10) ? -1 : 1;
                            NPC.spriteDirection = -NPC.direction;
                            NPC.behindTiles = false;
                            NPC.noTileCollide = false;
                        }
                        NPC.immortal = true;
                        NPC.damage = 30;
                    }else
                    {

                        if (frame>5)
                        {
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Roar_2"), _player.Center);
                        }
                        NPC.noGravity = false;
                        NPC.noTileCollide = false;

                        frame = NPC.ai[1] % 10 < 5 ? 1 : 2;
                        NPC.ai[1]++;
                    }

                    if (NPC.ai[0] >= 10 * 60 || NPC.ai[1]>=60*2)
                    {
                        curAttack = 1;
                        NPC.ai[0] = 0;
                        frame = 0;
                        break;
                    }

                    break;
                case 1:
                    //Idle

                    recheckDirection = true;

                    NPC.knockBackResist = 0;
                    NPC.noGravity = false;
                    NPC.rotation = 0;
                    NPC.noTileCollide = false;
                    NPC.damage = 0;
                    NPC.immortal = false;
                    NPC.behindTiles = false;
                    frame = 0;

                    NPC.ai[0]++;
                    if (NPC.ai[0] >=60* (NPC.life<NPC.lifeMax/2?(NPC.life<NPC.lifeMax/4?0:1):2)){

                        if (NPC.life < NPC.lifeMax / 4)
                        {
                            if (Main.rand.Next(3) == 0)
                            {
                                curAttack = 9;
                            }
                            else
                            {
                                if (Vector2.DistanceSquared(NPC.Center, _player.Center) > 1200 * 1200)
                                {
                                    curAttack = 2;
                                }else{
                                    curAttack = 4 + Main.rand.Next(EnragedAttackMax - 2);
                                }
                            }
                        }
                        else if (NPC.life < NPC.lifeMax / 2)
                        {
                            if (Vector2.DistanceSquared(NPC.Center, _player.Center) > 900 * 900)
                            {
                                curAttack = 2;
                            }else{
                                curAttack = 2 + Main.rand.Next(EnragedAttackMax);
                            }
                        }
                        else
                        {
                            if (Vector2.DistanceSquared(NPC.Center, _player.Center) > 900 * 900)
                            {
                                curAttack = 2;
                            }else{
                                curAttack = 2 + Main.rand.Next(NormalAttackMax);
                            }
                        }
                        NPC.ai[0] = 0;
                        NPC.ai[1] = 0;
                        break;
                    }

                    break;
                case 2:
                    //Punch
                    {
                        NPC.direction = MathF.Sign(_desVel.X);
                        NPC.spriteDirection = NPC.direction;
                        if (NPC.ai[0] < 30)
                        {
                            if (frame != 6)
                            {
                                DoTextMessage("Escape is futile, I'LL ALWAYS FIND YOU!!!",
                                    "You can never escape me!",
                                    "Try to dodge this!",
                                    _player.Center + new Vector2(Math.Sign(_desVel.X)* 50, -NPC.height / 2));
                                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_15"), _player.Center);
                            }
                            NPC.direction = -NPC.direction;
                            NPC.spriteDirection = NPC.direction;

                            frame = 6;

                            NPC.alpha = (int)((NPC.ai[0] / 30f) * 255);
                            NPC.velocity = new Vector2(NPC.direction * 20, 0);
                            NPC.rotation = MathHelper.PiOver4 / 2*-NPC.direction;
                            NPC.immortal = true;
                        }else if (NPC.ai[0] < 60 * 2)
                        {
                            NPC.rotation = 0;
                            NPC.alpha = (int)(((60-NPC.ai[0]) / 30f) * 255);
                            NPC.damage = 0;
                            frame = 3;
                            NPC.velocity = Vector2.Zero;

                            if (NPC.ai[0]<60) {
                                NPC.Center = _player.Center + new Vector2(-NPC.width/2+(NPC.ai[0]-30f)/30f*25f,0) * NPC.direction-new Vector2(0,NPC.height/2-50);
                            }
                            
                            NPC.immortal = NPC.ai[0] > 60;
                        }
                        else if (NPC.ai[0] < 60 * 2 + 10)
                        {
                            if (frame == 3){
                                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_167"), NPC.Center);
                            }
                            NPC.immortal = false;
                            frame = 4;
                            NPC.damage = 100;
                            NPC.velocity = new Vector2(NPC.direction * ((60f * 2f + 10f - NPC.ai[0]) / 10f)*10,0);
                        }
                        else if (NPC.ai[0] <= 60 * 3)
                        {
                            NPC.velocity = Vector2.Zero;
                            frame = 5;
                            NPC.damage = 0;
                        }
                        else if (NPC.ai[0] < 60 * 4.5f)
                        {
                            if (NPC.velocity.X == 0)
                            {
                                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Double_Jump"), NPC.Center);
                                NPC.velocity = new Vector2(NPC.direction * -5, -10);
                            }
                            frame = 8;
                            NPC.rotation -= NPC.direction * MathHelper.PiOver4;
                        }else{
                            frame = 8;
                            NPC.rotation -= NPC.direction * MathHelper.PiOver4 / 3f;

                            Point _npcGroundPoint = (NPC.Center + new Vector2(0, NPC.height / 2 + 5)).ToTileCoordinates();
                            if (WorldGen.SolidOrSlopedTile(_npcGroundPoint.X, _npcGroundPoint.Y) || Math.Abs(NPC.velocity.Y) < 3)
                            {
                                curAttack = 1;
                                NPC.rotation = 0;
                                NPC.ai[0] = 0;
                                NPC.velocity = Vector2.Zero;
                                break;
                            }
                        }
                    }
                    NPC.ai[0]++;

                    break;
                case 3:
                    //Jump kick
                    NPC.damage = 0;
                    NPC.direction = Math.Sign(_desVel.X);
                    NPC.spriteDirection = NPC.direction;
                    if (NPC.ai[0] == 0){
                        DoTextMessage("I'll kick your a** up to your throat",
                            "KISS MY BOOT AND GET YOUR HEAD SMASHED TO BITS",
                            "Bunny style, kick technique!",
                            NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                    }
                    if (NPC.ai[0] < 60)
                    {
                        recheckDirection = NPC.ai[0] < 30;
                        frame =9;
                        NPC.velocity = Vector2.Zero;
                    }
                    else
                    {
                        if (frame == 9)
                        {
                            NPC.velocity = Vector2.Normalize(_desVel) * 20+new Vector2(0,-10);
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_7"), NPC.Center);
                            frame = 6;
                        }

                        if (NPC.velocity.Y < 0)
                        {
                            Vector2 _vel=Vector2.Normalize(_desVel) * 20 + new Vector2(0, -10);
                            NPC.velocity = new Vector2(_vel.X, NPC.velocity.Y);
                        }
                        recheckDirection = true;

                        if ((MathF.Abs(_desVel.X) + MathF.Abs(_desVel.Y) < NPC.width/2 || NPC.ai[1]>0) && NPC.ai[1]<30)
                        {
                            if (frame == 6)
                            {
                                DoTextMessage("WHAT A F***ING GREAT FEELING!!!",
                                    "I'LL KICK YOU TO THE MOON AND BACK!!!",
                                    "Sorry, not sorry!",
                                    NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_167"), NPC.Center);
                            }
                            NPC.damage = 75;
                            frame = 7;
                            NPC.ai[1]++;
                            NPC.velocity = new Vector2(0,-0.3f);
                        }

                        if (NPC.ai[1] >= 30)
                        {
                            NPC.ai[1]++;
                            if (frame == 7) {
                                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Double_Jump"), NPC.Center);
                                frame = 8;
                                NPC.velocity = -Vector2.Normalize(_desVel) * 10 + new Vector2(0, -7);
                            }
                            NPC.rotation -=NPC.direction*MathHelper.PiOver4;
                        }

                    }

                    NPC.ai[0]++;
                    if (NPC.ai[0] > 60 * 2f + 1 && NPC.ai[1]==0 || NPC.ai[1]>=40)
                    {
                        Point _botPos = (NPC.Center + new Vector2(0, NPC.height / 2 + 3)).ToTileCoordinates();

                        if ((Math.Abs(NPC.velocity.Y)<2 && Math.Abs(NPC.oldVelocity.Y)>10) || (NPC.position==NPC.oldPosition) || (NPC.velocity.Y==0 && NPC.oldVelocity.Y==0) ||(Vector2.DistanceSquared(_player.Center, NPC.Center) > 100*100 && MathF.Sign(_desVel.X)!=MathF.Sign(NPC.velocity.X)) || Vector2.DistanceSquared(_player.Center, NPC.Center) > 500 * 500 || WorldGen.SolidOrSlopedTile(_botPos.X,_botPos.Y)){
                            recheckDirection = true;
                            curAttack = 1;
                            NPC.ai[0] = 0;
                            NPC.ai[1] = 0;
                            break;
                        }
                    }

                    break;
                case 4:
                    //Pull out giant carrot
                    recheckDirection = true;
                    NPC.velocity = Vector2.Zero;
                    if (NPC.ai[0] < 30)
                    {
                        frame = 10;
                    }
                    else if (NPC.ai[0] < 60)
                    {
                        if (NPC.ai[0] == 30)
                        {
                            DoTextMessage("I'LL SHOVE THIS UP YOUR A**!!!",
                                "I'LL LEAVE YOU IN A VEGETATIVE STATE WITH THIS!",
                                "Let me help your sight!",
                                NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Tink_0"), NPC.Center);
                        }
                        if (Main.rand.Next(2) == 0 && NPC.ai[0]<40)
                        {
                            Dust.NewDust(NPC.position + new Vector2(NPC.width / 2, NPC.height / 2), 64, 16, DustID.Dirt, Main.rand.Next(-15, 15), Main.rand.Next(-10, -5), Scale: 2);
                        }
                        frame = 11;
                    }else if (NPC.ai[0] < 70)
                    {
                        frame = 18;
                    }else if (NPC.ai[0] < 100)
                    {
                        NPC.direction = Math.Sign(_desVel.X);
                        NPC.spriteDirection = NPC.direction;
                        frame = 12;
                    }
                    else
                    {
                        NPC.direction = Math.Sign(_desVel.X);
                        NPC.spriteDirection = NPC.direction;
                        frame = 5;
                    }

                    if (NPC.ai[0] == 100)
                    {
                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_73"), NPC.Center);
                        EntitySource_Parent _s = new EntitySource_Parent(NPC);
                        Projectile.NewProjectile(_s, NPC.Center, Vector2.Normalize(_desVel) * 17, ModContent.ProjectileType<Projectiles.Bunnygirl_Carrot>(), 60, 1);
                    }

                    NPC.ai[0]++;
                    if (NPC.ai[0] > 60*2)
                    {
                        recheckDirection = true;
                        curAttack = 1;
                        NPC.ai[0] = 0;
                        NPC.ai[1] = 0;
                        break;
                    }

                    break;
                case 5:
                    //Pull out tree

                    if (NPC.ai[0] == 0)
                    {
                        DoTextMessage("NATURE FIGHTS BACK THE A***OLES THAT HURT IT!!!",
                            "DO LIKE THIS TREE AND LEAF MY FAMILY ALONE!!!",
                            "Nature aids both of us",
                            NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_73"), NPC.Center);
                    }
                    if (NPC.ai[0] == 150)
                    {
                        DoTextMessage("EAT S**T AND EAT THIS!!!",
                            "THIS IS FOR MY BROTHER!!!",
                            "Catch!",
                            NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                        EntitySource_Parent _s = new EntitySource_Parent(NPC);
                        Projectile.NewProjectile(_s, NPC.Center, Vector2.Normalize(_desVel) * 30 + new Vector2(0, -0.5f * Math.Abs(_player.Center.X - NPC.Center.X) / 10 / 2), ModContent.ProjectileType<Projectiles.Bunnygirl_tree>(), 50, 1);
                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_70"), NPC.Center);
                    }

                    if (NPC.ai[0] < 60)
                    {
                        frame = 12;
                        NPC.direction = Math.Sign(_desVel.X);
                        NPC.spriteDirection = NPC.direction;
                    }
                    else if(NPC.ai[0]<120)
                    {
                        NPC.damage = 0;
                        frame = 5;
                        if (NPC.ai[0] == 60)
                        {
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_45"), _player.Center);
                        }
                        if (NPC.ai[0] < 90)
                        {
                            NPC.damage = 30;
                            NPC.velocity = new Vector2(NPC.spriteDirection * 15*(1-(NPC.ai[0]-60f)/30f), 0);
                        }
                        if (NPC.ai[0] == 90)
                        {
                            NPC.velocity = Vector2.Zero;
                            NPC.direction = -NPC.direction;
                            NPC.spriteDirection = -NPC.spriteDirection;
                        }
                    }else if (NPC.ai[0] < 60 * 2.5f)
                    {
                        recheckDirection = false;
                        NPC.direction = Math.Sign(_desVel.X);
                        NPC.spriteDirection = NPC.direction;
                        frame = 12;
                    }else{
                        frame = 5;
                    }
                    

                    NPC.ai[0]++;
                    if (NPC.ai[0] >= 60*3)
                    {
                        recheckDirection = true;
                        NPC.ai[0] = 0;
                        curAttack = 1;
                        break;
                    }

                    break;
                case 6:
                    //Tornado
                    if (NPC.ai[0] == 0)
                    {
                        DoTextMessage("MY FURY KNOWS NO BOUNDS!",
                            "I'LL KILL YOU, JUST YOU WAIT!",
                            "You spin me right round, baby right round",
                            NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                    }
                    if (NPC.ai[0] <= 30)
                    {
                        frame = 3;

                        NPC.direction = (Main.time % 5 < 2.5f) ? -1 : 1;
                        NPC.spriteDirection = -NPC.direction;
                    }
                    else if (NPC.ai[0]<60*3.5f)
                    {
                        if (NPC.ai[0] % 60 == 0){
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_162"),NPC.Center);
                        }
                        NPC.knockBackResist = 1f;
                        NPC.immortal = true;
                        NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Normalize(_desVel) * 20, 0.03f);
                        NPC.noTileCollide = true;
                        NPC.noGravity = true;
                        NPC.damage = 20;

                        frame = Main.time % 10 < 5 ? 13 : 14;
                        NPC.direction = (Main.time % 20 < 10) ? -1 : 1;
                        NPC.spriteDirection = -NPC.direction;
                    }else
                    {
                        if (NPC.ai[0] == 60 * 3 + 30)
                        {
                            DoTextMessage("YOU SHALL LOSE, YOU CAN'T ESCAPE",
                                "I'LL SPIN MY FIST UP YOUR THROAT AND MAKE YOU SWALLOW YOUR EYES",
                                "Good job staying alive!",
                                NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                        }
                        Point _desPos;
                        int i = -20;
                        for (; i < 0; i++)
                        {
                            _desPos = (NPC.Center + new Vector2(0, NPC.height / 2)).ToTileCoordinates() + new Point(0, i);
                            if (WorldGen.SolidTile(_desPos.X, _desPos.Y, true))
                            {
                                break;
                            }
                        }
                        NPC.Center += new Vector2(0, 16 * i);

                        NPC.immortal = true;
                        NPC.noTileCollide = false;
                        NPC.noGravity = false;
                        NPC.knockBackResist = 0f;
                        frame = 9;
                        NPC.damage = 0;
                        NPC.velocity = Vector2.Zero;
                    }


                    NPC.ai[0]++;
                    if (NPC.ai[0] > 60 * 4)
                    {
                        curAttack = 1;
                        NPC.ai[0] = 0;
                        NPC.velocity = Vector2.Zero;
                        return;
                    }

                    break;
                case 7:
                    //Dive to floor and punch

                    NPC.noGravity = true;
                    if (NPC.ai[0] == 0){
                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_7"), NPC.Center);
                        DoTextMessage("I CAN KEEP UP WITH YOU FOREVER!!!",
                            "BUNNY POWER, BUNNY STRENGTH!!",
                            "Digging is fun, LOOK AT THIS!!!",
                            NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                    }

                    if (NPC.ai[0] < 30)
                    {
                        float _verticalVel = MathF.Cos(NPC.ai[0] / 29f * MathF.PI);
                        NPC.Center +=new Vector2(0, -_verticalVel*15);
                        NPC.velocity = Vector2.Zero;
                        frame = _verticalVel > 0 ? 6 : 8;

                    }else if (NPC.ai[0] < 60)
                    {
                        if (frame==6 || frame==8)
                        {
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_51"), NPC.Center);
                        }
                        if (NPC.ai[0] == 30)
                        {
                            Point _desPos;
                            int i = 0;
                            for (; i < 20; i++)
                            {
                                _desPos = (NPC.Center + new Vector2(0, NPC.height / 2)).ToTileCoordinates()+new Point(0,i);
                                if (WorldGen.SolidTile(_desPos.X, _desPos.Y, true))
                                {
                                    break;
                                }
                            }
                            NPC.Center += new Vector2(0, 16 * i);
                        }

                        Dust.NewDust(NPC.position + new Vector2(NPC.width / 2, NPC.height/2), 64, 16, DustID.Dirt, Main.rand.Next(-15, 15), Main.rand.Next(-10, -5), Scale: 2);
                        frame = Main.time % 10 < 5 ?15:16;

                    }else if (NPC.ai[0] < 60 * 3)
                    {
                        frame = 14;
                        NPC.alpha = 255;
                        NPC.immortal = true;
                        if (NPC.ai[0] < 60 * 2.5f)
                        {
                            if (NPC.ai[0] % 10 == 0)
                            {
                                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Roar_1"), NPC.Center);
                            }
                            NPC.Center =new Vector2(MathHelper.Lerp(NPC.Center.X,_player.Center.X,0.1f),NPC.Center.Y);

                            Point _desPos;
                            int i = -10;
                            for(; i < 20; i++)
                            {
                                _desPos = (NPC.Center + new Vector2(0, NPC.height / 2)).ToTileCoordinates()+new Point(0,i);
                                if (WorldGen.SolidTile(_desPos.X,_desPos.Y,true))
                                {
                                    Dust.NewDust(_desPos.ToWorldCoordinates() + new Vector2(0, -32), 64, 16, DustID.Dirt, Main.rand.Next(-15, 15)/4, Main.rand.Next(-10, -5)/4, Scale: 2);
                                    break;
                                }
                            }
                            NPC.Center += new Vector2(0, 16*i);
                        }
                    }else if(NPC.ai[0]<60*3.5f)
                    {
                        if (NPC.ai[0] == 0)
                        {
                            DoTextMessage("NUT BREAKER!!!",
                                "YOU'LL NEVER HAVE CHILDREN AFTER THIS!",
                                "This is my family's secret technique, A hundred seconds of pain!",
                                NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));

                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_167"), NPC.Center);
                        }
                        if (NPC.ai[1]==0)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                Dust.NewDust(NPC.position + new Vector2(NPC.width / 2, -32), 64, 16, DustID.Dirt, Main.rand.Next(-15, 15), Main.rand.Next(-10, -5), Scale: 2);
                            }
                            NPC.ai[1] = 1;
                        }
                        frame = 17;
                        NPC.alpha = 0;
                        NPC.damage = (NPC.ai[0] < 60 * 3 + 10) ? 30 : 0;
                        NPC.scale = 1 + Math.Max(0, 60 * 3.5f - NPC.ai[0])/60*3.5f*0.5f;
                    }else
                    {
                        if (frame == 17)
                        {
                            NPC.Center += new Vector2(0, NPC.height/2);
                            frame = 6;
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_7"), NPC.Center);
                        }
                        NPC.immortal = false;
                        NPC.noGravity = false;
                        if (NPC.ai[0]<= 60 * 3+32 && NPC.velocity.Y>=0)
                        {
                            NPC.noTileCollide = true;
                            NPC.velocity = new Vector2(0, -15);
                        }
                        NPC.noTileCollide = NPC.velocity.Y>=0;

                        Point _npcPos = (NPC.Center +new Vector2(0, NPC.height / 2 + 5)).ToTileCoordinates();
                        if (NPC.oldVelocity.Y>0 && WorldGen.SolidTile(_npcPos.X, _npcPos.Y, true))
                        {
                            frame = 9;
                            NPC.rotation = 0;
                        }
                        if (frame != 9) {
                            frame = NPC.velocity.Y < 0 ? 6 : 8;
                            NPC.rotation += (NPC.velocity.Y > 0) ? NPC.spriteDirection * MathHelper.PiOver2:0;
                        }
                    }

                    NPC.ai[0]++;

                    if (NPC.ai[0] >= 60 * 5)
                    {
                        NPC.noGravity = false;
                        curAttack = 1;
                        NPC.ai[0] = 0;
                        NPC.ai[1] = 0;
                    }
                    break;
                case 8:
                    //Pull out gun and shoot
                    if (NPC.ai[0] < 30)
                    {
                        frame = 20;
                    }
                    else if (NPC.ai[0] < 60)
                    {
                        if (frame == 20)
                        {
                            DoTextMessage("EAT LEAD, MOTHERF****R",
                                "KILLED A B***H, MADE A GUN, will do the same with your corpse",
                                "Where did this gun come from? I'll just use it anyways",
                                NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_149"), NPC.Center);
                        }
                        frame = 21;
                    }
                    else
                    {
                        NPC.direction = Math.Sign(_desVel.X);
                        NPC.spriteDirection = NPC.direction;
                        NPC.ai[1] = _desVel.ToRotation();
                        frame = 22;

                        if(NPC.ai[0]>60+40 && NPC.ai[0] % 10 == 0)
                        {
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_11"), NPC.Center);
                            EntitySource_Parent _s = new EntitySource_Parent(NPC);
                            int _proj= Projectile.NewProjectile(_s, NPC.Center + Vector2.Normalize(_desVel) * 30, Vector2.Normalize(_desVel).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4/4f,MathHelper.PiOver4/4f)) * 10, ProjectileID.Bullet, 10, 0.2f);
                            Main.projectile[_proj].friendly = false;
                            Main.projectile[_proj].hostile= true;
                        }
                    }

                    NPC.ai[0]++;
                    if (NPC.ai[0] >= 60 * 5)
                    {
                        DoTextMessage("I have more where that came from",
                            "I'll shove this up your d**k and shoot it later",
                            "Welp, that was fun!",
                            NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_7"), NPC.Center);

                        EntitySource_Parent _s = new EntitySource_Parent(NPC);
                        Gore.NewGore(_s, NPC.Center,new Vector2(-NPC.spriteDirection*5,-7),ModContent.GoreType<Gores.GUN_Gore>());
                        curAttack = 1;
                    }

                    break;
                case 9:
                    //Giant fist prepare

                    if (NPC.ai[0] < 10)
                    {
                        frame = 18;
                    }
                    else
                    {
                        if (frame == 18)
                        {
                            DoTextMessage("RAAAAAAAGHHHH!!!!",
                                "DIIIIIIIIIIIIEEEEEEE!!!!",
                                "NOW I'M MOTIVATED!!!",
                                NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Roar_2"), NPC.Center);
                        }
                        NPC.immortal = true;
                        NPC.damage = 40;
                        frame = (NPC.ai[0] % 10 < 5) ? 1 : 2;
                        NPC.spriteDirection = (NPC.ai[0] % 20 < 10) ? 1 : -1;

                        if(NPC.ai[0]<60+30 && NPC.ai[0] % 3 == 0)
                        {
                            _bunniesSuckedIn.Add(new SuckedInBunny(new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, 1) * MathHelper.TwoPi), 300,Main.rand.NextFloat(0,MathHelper.TwoPi),(Main.rand.Next(2)==0?-1:1)));
                        }

                        List<int> _bunniesToRemove = new List<int>();
                        for(int i = 0; i < _bunniesSuckedIn.Count; i++)
                        {
                            _bunniesSuckedIn[i].alpha += 255 / 10;
                            _bunniesSuckedIn[i].distance = MathHelper.Lerp(_bunniesSuckedIn[i].distance, 0,Math.Clamp(0.1f+Math.Clamp(250 - _bunniesSuckedIn[i].distance,0,250)/300*0.5f,0,1));
                            _bunniesSuckedIn[i].rotation += _bunniesSuckedIn[i].spriteDirection * MathHelper.PiOver4 / 4;
                            if (_bunniesSuckedIn[i].distance <= 75)
                            {
                                _bunniesToRemove.Add(i);
                                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/NPC_Killed_9"), _player.Center);
                            }
                        }
                        for(int i = 0; i < _bunniesToRemove.Count; i++)
                        {
                            _bunniesSuckedIn.RemoveAt(_bunniesToRemove[i]);
                        }
                    }

                    NPC.ai[0]++;
                    if (NPC.ai[0] >= 60 * 2)
                    {
                        NPC.immortal = false;
                        NPC.ai[0] = 0;
                        curAttack = 10;
                        NPC.damage = 0;
                    }

                    break;
                case 10:
                    //Giant fist punch

                    if (NPC.ai[0] == 0)
                    {
                        DoTextMessage("FOR MY "+(Main.rand.Next(0,5)==0?"MOTHER!!!":"FATHER!!!"),
                            "FOR MY LOST BROTHERS!!!",
                            "Punch!",
                            NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                        Vector2 oldPos = NPC.Center;
                        NPC.Center = new Vector2(MathHelper.Clamp(NPC.Center.X,_player.Center.X-550,_player.Center.X+550),MathHelper.Lerp(NPC.Center.Y, _player.Center.Y-NPC.height/4,0.1f));
                        int _iterations = (int)(Vector2.DistanceSquared(NPC.Center, oldPos) / (80*80));
                        if (_iterations > 2)
                        {
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_15"), _player.Center);
                        }
                        Vector2 _dir = Vector2.Normalize(NPC.Center - oldPos) * 20;
                        for(int i=0; i < _iterations; i++)
                        {
                            _rollClones.Add(new RollCloneEffectPositions(oldPos + _dir * i, 0, frame, NPC.alpha*((float)i/_iterations)));
                        }
                    }
                    if (NPC.ai[0] < 20+(NPC.life<NPC.lifeMax/4?10:0))
                    {
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;

                        NPC.direction = Math.Sign(_desVel.X);
                        NPC.spriteDirection = NPC.direction;
                        frame = 3;
                        NPC.Center = new Vector2(MathHelper.Clamp(NPC.Center.X,_player.Center.X-550,_player.Center.X+550),MathHelper.Lerp(NPC.Center.Y, _player.Center.Y-NPC.height/4,0.1f));
                    }else if (NPC.ai[0] < 45)
                    {
                        frame = 3;
                    }
                    else if(NPC.ai[0]<90)
                    {
                        if (frame == 3){
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_45"), _player.Center);
                        }
                        frame = 5;
                        NPC.damage = 40;
                        NPC.velocity = new Vector2(35 * NPC.direction*(1f-(NPC.ai[0]-45f)/80f), 0);
                        NPC.immortal = true;
                    }else
                    {
                        NPC.immortal = false;
                        NPC.velocity = new Vector2(0, NPC.velocity.Y);
                        NPC.noTileCollide = false;
                        NPC.noGravity = false;
                        frame = 5;
                        NPC.damage = 0;
                    }

                    NPC.ai[0]++;
                    if (NPC.ai[0] >= 60*2-(NPC.life<NPC.lifeMax/4?20:0))
                    {
                        NPC.ai[0] = 0;
                        NPC.ai[1]++;
                        if (NPC.ai[1] <  (NPC.life < NPC.lifeMax / 4 ? 5 : 2))
                        {
                            curAttack = 10;
                        }
                        else
                        {
                            curAttack = 11;
                            NPC.ai[1] = 0;
                        }
                    }

                    break;
                case 11:
                    //Giant fist shoot

                    if (NPC.ai[0] == 0)
                    {
                        DoTextMessage("KILL HIM NOW SISTERS!!!",
                            "TRY TO KILL US NOW!!!",
                            "BUNNY BARRAGE!",
                            NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));

                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_7"), _player.Center);
                        Point _desPos;
                        int i = -10;
                        for (; i < 20; i++)
                        {
                            _desPos = (NPC.Center + new Vector2(0, NPC.height / 2)).ToTileCoordinates() + new Point(0, i);
                            if (WorldGen.SolidTile(_desPos.X, _desPos.Y, true))
                            {
                                Dust.NewDust(_desPos.ToWorldCoordinates() + new Vector2(0, -32), 64, 16, DustID.Dirt, Main.rand.Next(-15, 15) / 4, Main.rand.Next(-10, -5) / 4, Scale: 2);
                                break;
                            }
                        }
                        NPC.Center += new Vector2(0, 16 * i);
                    }

                    if (NPC.ai[0] < 60)
                    {
                        frame = 18;
                    }else{
                        frame = NPC.ai[0]%10<5? 1:2;
                        NPC.damage = 100;

                        if (NPC.ai[0] % 2 == 0 && NPC.ai[0]<60*2-15)
                        {
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_14"), NPC.Center);
                            EntitySource_Parent _s = new EntitySource_Parent(NPC);
                            float _power = Main.rand.NextFloat(5, 10);
                            Projectile.NewProjectile(_s, NPC.Center, new Vector2(_power*2,0).RotatedBy(Main.rand.NextFloat(0,MathHelper.TwoPi)),ModContent.ProjectileType<Projectiles.BunnyProjectile>(),(int)_power,0.1f);
                        }
                    }

                    NPC.ai[0]++;
                    if (NPC.ai[0] >= 60 * 2)
                    {
                        NPC.ai[0] = 0;
                        curAttack = 1;
                    }

                    break;

                case 69:
                    //Death
                    NPC.velocity = Vector2.Zero;
                    NPC.alpha = 0;
                    NPC.damage = 0;
                    frame = (NPC.ai[0] < DeathAnimWindup)?18:19;
                    if (NPC.ai[0] >= DeathAnimWindup)
                    {
                        NPC.velocity = new Vector2(0, -1 * Math.Max(0.275f,3f-((NPC.ai[0]-DeathAnimWindup)/DeathFloatAnim*2.5f)));
                        NPC.noTileCollide = true;
                        NPC.rotation += MathHelper.PiOver4 / 50f*(-NPC.spriteDirection);

                        if (NPC.ai[0] == DeathAnimWindup){
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Female_Hit_2"), NPC.Center);
                            DoTextMessage("NOOOOOOOOO!!!",
                                "NOOOOOOOOO!!!",
                                "I'm sorry for not being... strong... enough",
                                NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                        }
                    }else{
                        NPC.noTileCollide = false;
                        NPC.rotation = 0;
                    }
                    if (NPC.ai[0] >= DeathAnimWindup+DeathFloatAnim)
                    {
                        NPC.life = 0;
                        NPC.checkDead();
                    }
                    NPC.ai[0]++;
                    break;
            }

            if ((frame == 8 || NPC.alpha>10) && Main.time%5==0)
            {
                _rollClones.Add(new RollCloneEffectPositions(NPC.Center, NPC.rotation,frame,NPC.alpha));
            }

            List<int> _toRemove = new List<int>();
            for(int i = 0; i < _rollClones.Count; i++)
            {
                _rollClones[i].framesLasted++;
                if (_rollClones[i].framesLasted > 20)
                {
                    _toRemove.Add(i);
                }
            }

            for(int i = _toRemove.Count-1; i >0; i--)
            {
                _rollClones.RemoveAt(i);
            }
            _toRemove.Clear();

        }

        public void DoTextMessage(string _normalText, string _firstSpawnText, string _alreadyDeffeatedText,Vector2 _pos)
        {


            string _desiredText;
            if (ArianelleDeffeatedSystem.downedArianel)
            {
                _desiredText = _alreadyDeffeatedText;
            }
            else if (_isFirstSpawn){
                _desiredText = _firstSpawnText;
            }else{
                _desiredText = _normalText;
            }

            ArianelleDeffeatedSystem.DoGeneralTextMessage(_desiredText, _pos,Color.Red);
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (!CanBeHit()) { return false; }
            return null;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (!CanBeHit()) { return false; }
            return null;
        }

        private bool CanBeHit()
        {
            return !NPC.immortal || curAttack == 6;
        }

        public override bool CanHitNPC(NPC target)
        {
            if (BunnyKillNPCOverride.IsBunny(target)){
                return false;
            }

            return true;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (curAttack == 10 && MathF.Abs(target.Center.Y-NPC.Center.Y)>NPC.height/4)
            {
                return false;
            }
            return base.CanHitPlayer(target, ref cooldownSlot);
        }

        public int FindTargetPlayer()
        {
            int _selectedPlayer = -1;
            for(int i = 0; i < Main.maxPlayers; i++)
            {
                if( Vector2.DistanceSquared(NPC.Center,Main.player[i].Center)<5000*5000)
                {
                    if (_selectedPlayer<0 || Vector2.DistanceSquared(NPC.Center, Main.player[i].Center) < Vector2.DistanceSquared(NPC.Center, Main.player[_selectedPlayer].Center))
                    {
                        _selectedPlayer = i;
                    }
                }
            }
            NPC.target = _selectedPlayer;
            return _selectedPlayer;
        }


        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.BunnyEars));
            npcLoot.Add(ItemDropRule.Common(ItemID.BunnyTail));
            npcLoot.Add(ItemDropRule.Common(ItemID.FuzzyCarrot,100));
            npcLoot.Add(ItemDropRule.Common(ItemID.Carrot,10000));
            npcLoot.Add(ItemDropRule.Common(ItemID.Sunglasses));
            npcLoot.Add(ItemDropRule.Common(ItemID.Minishark,100));
            npcLoot.Add(ItemDropRule.Common(ItemID.Acorn,1,100,300));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weights>(),1,1,3));
        }

        public override void FindFrame(int frameHeight)
        {

            NPC.frame.Y = frame * frameHeight;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {

            if (curAttack == 0 && NPC.ai[0] <= 3 * 60)
            {
                return false;
            }

            if (NPC.alpha > 10 || NPC.immortal)
            {
                return false;
            }

            scale = 4f;
            return null;
        }

        public override bool CheckDead()
        {

            if (NPC.ai[0] > 60 * 6 && curAttack == 69)
            {
                ArianelleDeffeatedSystem.downedArianel = true;
                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/NPC_Killed_14"), NPC.Center);

                Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(-10, 0), new Vector2(Main.rand.NextFloat(-10,-3), -7), ModContent.GoreType<Gores.bunnygirl_Fist_Gore>(),1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(10, 0), new Vector2(Main.rand.NextFloat(10, 3), -7), ModContent.GoreType<Gores.bunnygirl_Fist_Gore>(),1f);

                Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(0, -10), new Vector2(0, -7), ModContent.GoreType<Gores.bunnygirl_Head_Gore>(),1f);

                Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(-10, 20), new Vector2(Main.rand.NextFloat(-10, -3), -7), ModContent.GoreType<Gores.bunnygirl_Leg_Gore>(),1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(10, 20), new Vector2(Main.rand.NextFloat(10, 3), -7), ModContent.GoreType<Gores.bunnygirl_Leg_Gore>(),1f);

                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, -7), ModContent.GoreType<Gores.bunnygirl_Torso_Gore>(),1f);

                int _proj= Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileID.Explosives, 10, 2f);
                Main.projectile[_proj].friendly = true;
                Main.projectile[_proj].hostile = true;

                int _explosives=NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<NPCs.ArianelExplosiveBunny>());
                Main.npc[_explosives].velocity = new Vector2(0, 5);

                return true;
            }
            if (curAttack != 69)
            {
                DoTextMessage("You piece... of... sh**...",
                    "I'll... be... back...",
                    "Well... fought...",
                    NPC.Center + new Vector2(-NPC.direction * 50, -NPC.height / 2));
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center - new Vector2(0, 20), new Vector2(-NPC.spriteDirection * 15, -20), ModContent.GoreType<Gores.SunglassesGore>(), 2f);
                NPC.ai[0] = 0;
                curAttack = 69;
            }
            NPC.life = 1;
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Blood, 2 * hit.HitDirection, -2f);
            base.HitEffect(hit);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            for (int i = 0; i < _rollClones.Count; i++)
            {
                Texture2D _texture = ModContent.Request<Texture2D>(Texture).Value;
                Rectangle _rect = new Rectangle(0, _texture.Height / Main.npcFrameCount[Type] *_rollClones[i].frame, _texture.Width, _texture.Height / Main.npcFrameCount[Type]);

                spriteBatch.Draw(_texture, _rollClones[i].pos - Main.screenPosition, _rect, drawColor*0.5f *((255f-_rollClones[i].startingAlpha)/255)* ((20f - _rollClones[i].framesLasted) / 20f), _rollClones[i].rotation, new Vector2(_texture.Width / 2, _texture.Height / Main.npcFrameCount[Type] / 2), NPC.scale, SpriteEffects.None, 0);
            }

            if (curAttack==0 && NPC.ai[0]<=3*60)
            {
                return false;
            }

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            if (curAttack == 4)
            {
                //Carrot
                Texture2D _texture = ModContent.Request<Texture2D>("Tmodtober/NPCs/Boss/bunnygirl_Effects").Value;
                Rectangle _rect = new Rectangle(0, _texture.Height / 5 * 2, _texture.Width, _texture.Height / 5);

                switch (frame)
                {
                    case 18:
                        spriteBatch.Draw(_texture, NPC.Center + new Vector2(0, _texture.Height / 5) - Main.screenPosition, _rect, drawColor, 0, new Vector2(_texture.Width / 2, _texture.Width / 2), NPC.scale * 2f, SpriteEffects.None, 0);
                        break;
                    case 12:
                        spriteBatch.Draw(_texture, NPC.Center + new Vector2(100 * -NPC.spriteDirection, 50) - Main.screenPosition, _rect, drawColor, 0, new Vector2(_texture.Width / 2, _texture.Width / 2), NPC.scale * 2f, SpriteEffects.None, 0);
                        break;
                }
            } else if (curAttack == 5)
            {
                //Tree
                float _curFallTime = Math.Max(0, 30f - NPC.ai[0]) / 30f;
                Texture2D _texture = ModContent.Request<Texture2D>("Tmodtober/NPCs/Boss/bunnygirl_Effects").Value;
                Rectangle _rect = new Rectangle(0, _texture.Height / 5 * 3, _texture.Width, _texture.Height / 5);

                if (frame == 12)
                {
                    spriteBatch.Draw(_texture, NPC.Center + new Vector2(100 * -NPC.spriteDirection + 50, 50 - 1500 * _curFallTime) - Main.screenPosition, _rect, drawColor * (1 - _curFallTime), MathHelper.Pi * 5 * _curFallTime - MathHelper.PiOver2, new Vector2(_texture.Width / 2, _texture.Width / 2), NPC.scale * 2f, SpriteEffects.None, 0);
                }
                if (frame == 5 && NPC.ai[0] < 60 * 2.5f)
                {
                    spriteBatch.Draw(_texture, NPC.Center + new Vector2(200 * NPC.spriteDirection, NPC.height / 2) - Main.screenPosition, _rect, drawColor, 0, new Vector2(_texture.Width / 2, _texture.Width / 2), NPC.scale * 2f, NPC.direction >= 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            } else if (curAttack == 8 && frame == 22)
            {
                //Gun
                Texture2D _texture = ModContent.Request<Texture2D>("Tmodtober/NPCs/Boss/bunnygirl_Effects").Value;
                Rectangle _rect = new Rectangle(0, _texture.Height / 5 * 4, _texture.Width, _texture.Height / 5);

                bool unflipped = (MathHelper.TwoPi + MathHelper.PiOver2 - MathF.Abs(NPC.ai[1] - MathHelper.Pi)) % MathHelper.TwoPi > MathHelper.Pi;
                spriteBatch.Draw(_texture, NPC.Center + new Vector2(-NPC.direction * 2, -5) - Main.screenPosition, _rect, drawColor, NPC.ai[1], new Vector2(_texture.Width / 6, _texture.Height / 5 / 2), 2, (!unflipped) ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);

            } else if (curAttack == 9 && frame < 3)
            {
                //Bunnies suck in
                Texture2D _texture = ModContent.Request<Texture2D>("Terraria/Images/NPC_46").Value;
                Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height / 6);

                foreach (SuckedInBunny _bunny in _bunniesSuckedIn)
                {
                    spriteBatch.Draw(_texture, NPC.Center + _bunny.direction * _bunny.distance - Main.screenPosition, _rect, Color.White * Math.Min(1, _bunny.alpha / 255f), _bunny.rotation, new Vector2(_texture.Width / 2, _texture.Height / 7 / 2), 1f, _bunny.spriteDirection >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                }

                // Bunny arms

                _texture = ModContent.Request<Texture2D>("Tmodtober/NPCs/Boss/bunnygirl_Effects").Value;
                _rect = new Rectangle(0, _texture.Height / 5, _texture.Width, _texture.Height / 5);

                spriteBatch.Draw(_texture, NPC.Center + new Vector2(40, 30 - (NPC.ai[0]) / (60 * 2) * 30) - Main.screenPosition, _rect, drawColor, MathHelper.PiOver4 / 2, new Vector2(_texture.Width / 2, _texture.Height / 5 / 2), 1 + (NPC.ai[0]) / (60 * 2), SpriteEffects.None, 0);
                spriteBatch.Draw(_texture, NPC.Center + new Vector2(-45, 30 - (NPC.ai[0]) / (60 * 2) * 30) - Main.screenPosition, _rect, drawColor, -MathHelper.PiOver4 / 2, new Vector2(_texture.Width / 2, _texture.Height / 5 / 2), 1 + (NPC.ai[0]) / (60 * 2), SpriteEffects.None, 0);
            } else if (curAttack == 10)
            {
                //Bunny arms punch
                Texture2D _texture = ModContent.Request<Texture2D>("Tmodtober/NPCs/Boss/bunnygirl_Effects").Value;
                Rectangle _rect = new Rectangle(0, _texture.Height / 5, _texture.Width, _texture.Height / 5);

                switch (frame)
                {
                    default:
                        break;
                    case 3:
                        spriteBatch.Draw(_texture, NPC.Center + new Vector2(-25 * -NPC.direction,25) - Main.screenPosition, _rect, drawColor, -MathHelper.PiOver2*NPC.spriteDirection, new Vector2(_texture.Width / 2, _texture.Height / 5 / 2 + (_texture.Height / 5 - _texture.Height / 5 / 4)),2, SpriteEffects.None, 0);
                        break;
                    case 5:
                        {
                            float _speed = Vector2.DistanceSquared(NPC.velocity, Vector2.Zero)/(35*35);
                            spriteBatch.Draw(_texture, NPC.Center + new Vector2(-25 * NPC.direction, 25) - Main.screenPosition, _rect, drawColor, MathHelper.PiOver2 * NPC.spriteDirection, new Vector2(_texture.Width / 2, _texture.Height / 5 / 2 + (_texture.Height / 5 - _texture.Height / 5 / 4)), 2 +_speed*2, SpriteEffects.None, 0);
                        }
                        break;
                }
            } else if (curAttack == 11)
            {
                //Bunny arms disappear
                Texture2D _texture = ModContent.Request<Texture2D>("Tmodtober/NPCs/Boss/bunnygirl_Effects").Value;
                Rectangle _rect = new Rectangle(0, _texture.Height / 5, _texture.Width, _texture.Height / 5);

                switch (frame) {
                    case 1:
                    case 2:
                        spriteBatch.Draw(_texture, NPC.Center + new Vector2(45, 30 - (1-NPC.ai[0]/(60f * 2)) * 30) - Main.screenPosition, _rect, drawColor, MathHelper.PiOver4 / 2, new Vector2(_texture.Width / 2, _texture.Height / 5 / 2), 1 + (1-NPC.ai[0] / (60f * 2)), SpriteEffects.None, 0);
                        spriteBatch.Draw(_texture, NPC.Center + new Vector2(-50, 30 - (1-NPC.ai[0]/ (60f * 2)) * 30) - Main.screenPosition, _rect, drawColor, -MathHelper.PiOver4 / 2, new Vector2(_texture.Width / 2, _texture.Height / 5 / 2), 1 + (1-NPC.ai[0] / (60f * 2)), SpriteEffects.None, 0);
                        break;
                    case 18:
                        spriteBatch.Draw(_texture, NPC.Center + new Vector2(-0,15) - Main.screenPosition, _rect, drawColor,MathHelper.PiOver2, new Vector2(_texture.Width / 2, _texture.Height / 5 / 2), 2, SpriteEffects.None, 0);
                        spriteBatch.Draw(_texture, NPC.Center + new Vector2(10, 15) - Main.screenPosition, _rect, drawColor, -MathHelper.PiOver2, new Vector2(_texture.Width / 2, _texture.Height / 5 / 2),2, SpriteEffects.None, 0);
                        break;
                }
            }
            else if (NPC.ai[0] > DeathAnimWindup && curAttack == 69)
            {
                //Death
                float _alpha = (NPC.ai[0] - DeathAnimWindup) / DeathFloatAnim;

                Texture2D _texture = ModContent.Request<Texture2D>("Tmodtober/NPCs/Boss/bunnygirl_Effects").Value;
                Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height / 5);

                spriteBatch.Draw(_texture, NPC.Center - Main.screenPosition, _rect, Color.White * Math.Min(1, _alpha * 1.25f), _alpha * MathHelper.TwoPi + MathHelper.PiOver2, new Vector2(_texture.Width / 2, _texture.Height / 5 / 2), Math.Max(0, _alpha - 0.1f) * 65, SpriteEffects.None, 0);
                spriteBatch.Draw(_texture, NPC.Center - Main.screenPosition, _rect, Color.White * Math.Min(1, _alpha * 1.25f)*0.5f, _alpha * MathHelper.TwoPi + MathHelper.PiOver2, new Vector2(_texture.Width / 2, _texture.Height / 5 / 2), Math.Max(0, _alpha - 0.1f) * 65*2, SpriteEffects.None, 0);
                spriteBatch.Draw(_texture, NPC.Center - Main.screenPosition, _rect, Color.White * Math.Min(1, _alpha * 1.25f)*0.25f, _alpha * MathHelper.TwoPi + MathHelper.PiOver2, new Vector2(_texture.Width / 2, _texture.Height / 5 / 2), Math.Max(0, _alpha - 0.1f) * 65*4, SpriteEffects.None, 0);
            }

        }

    }
}
