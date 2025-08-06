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
using Terraria.Audio;

namespace Tmodtober.Projectiles
{
    class MedusaSight : ModProjectile
    {

        List<Vector2> targetPos;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {

            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 3;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 10000;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.damage = 10;

            Projectile.minionSlots = 10;

            Projectile.ignoreWater = true;
            Projectile.tileCollide=false;
            Projectile.penetrate=100;

            targetPos = new List<Vector2>();
        }

        public override void AI()
        {
            Player _p =Main.player[Main.myPlayer];
            Projectile.velocity = Vector2.Zero;
            if (Projectile.timeLeft <= 3)
            {
                Projectile.Center = _p.Center + new Vector2(0, -25f * Math.Min(Projectile.ai[0] / 3f, 1));
                Projectile.ai[0]--;
                return;
            }

            if (Projectile.friendly && Projectile.owner == Main.myPlayer)
            {
                Projectile.ai[0]++;
                Projectile.timeLeft = 10;

                Projectile.Center = _p.Center + new Vector2(0, -25f * Math.Min(Projectile.ai[0] / 30f, 1));


                if (targetPos.Count <= 0)
                {
                    Projectile.scale = 3f;
                }else{
                    Projectile.scale = 5-((Projectile.ai[0] - 30f)/15f);
                    if (Projectile.ai[0] >= 60)
                    {
                        targetPos.Clear();
                    }
                }

                if (Projectile.ai[0] >= 200)
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        if (Main.npc[i].active && !Main.npc[i].CountsAsACritter && !Main.npc[i].townNPC && !Main.npc[i].boss && Vector2.Distance(_p.Center,Main.npc[i].Center)<400 && !Main.npc[i].immortal)
                        {
                            SoundEngine.PlaySound(SoundID.Item167);
                            targetPos.Add(Vector2.Normalize(Projectile.Center - Main.npc[i].Center));

                            GetStatue(Main.npc[i]);

                            Main.npc[i].life = 0;
                            Main.npc[i].checkDead();
                        }
                    }

                    Projectile.ai[0] = 30;
                }

                bool _stillUsing = (_p.HasBuff<Buffs.MedusaHeadMinionBuff>());

                if (!_stillUsing)
                {
                    Projectile.timeLeft = 3;
                    Projectile.ai[0] = 3;
                }

            }
        }

        public void GetStatue(NPC _npc)
        {
            EntitySource_Parent _s = new EntitySource_Parent(Projectile);
            switch (_npc.type)
            {
                case NPCID.Medusa:
                    Item.NewItem(_s, _npc.targetRect, ItemID.MedusaStatue);
                    break;
                case NPCID.CaveBat:
                case NPCID.GiantBat:
                case NPCID.IceBat:
                case NPCID.IlluminantBat:
                case NPCID.JungleBat:
                case NPCID.SporeBat:
                    Item.NewItem(_s, _npc.targetRect, ItemID.BatStatue);
                    break;
                case NPCID.Mimic:
                    Item.NewItem(_s, _npc.targetRect, ItemID.ChestStatue);
                    break;
                case NPCID.BloodZombie:
                    Item.NewItem(_s, _npc.targetRect, ItemID.BloodZombieStatue);
                    break;
                case NPCID.Crab:
                    Item.NewItem(_s, _npc.targetRect, ItemID.CrabStatue);
                    break;
                case NPCID.EaterofSouls:
                case NPCID.Corruptor:
                    Item.NewItem(_s, _npc.targetRect, ItemID.CorruptStatue);
                    break;
                case NPCID.Drippler:
                    Item.NewItem(_s, _npc.targetRect, ItemID.DripplerStatue);
                    break;
                case NPCID.DemonEye:
                case NPCID.DemonEye2:
                case NPCID.DemonEyeOwl:
                case NPCID.DemonEyeSpaceship:
                    Item.NewItem(_s, _npc.targetRect, ItemID.EyeballStatue);
                    break;
                case NPCID.GoblinScout:
                    Item.NewItem(_s, _npc.targetRect, ItemID.GoblinStatue);
                    break;
                case NPCID.GraniteGolem:
                case NPCID.GraniteFlyer:
                    Item.NewItem(_s, _npc.targetRect, ItemID.GraniteGolemStatue);
                    break;
                case NPCID.Harpy:
                    Item.NewItem(_s, _npc.targetRect, ItemID.HarpyStatue);
                    break;
                case NPCID.GreekSkeleton:
                    Item.NewItem(_s, _npc.targetRect, ItemID.HopliteStatue);
                    break;
                case NPCID.Hornet:
                case NPCID.HornetFatty:
                case NPCID.HornetHoney:
                case NPCID.HornetLeafy:
                case NPCID.HornetSpikey:
                case NPCID.HornetStingy:
                case NPCID.BigHornetFatty:
                case NPCID.BigHornetHoney:
                case NPCID.BigHornetLeafy:
                case NPCID.BigHornetSpikey:
                case NPCID.BigHornetStingy:
                case NPCID.BigMossHornet:
                case NPCID.GiantMossHornet:
                case NPCID.LittleHornetFatty:
                case NPCID.LittleHornetHoney:
                case NPCID.LittleHornetLeafy:
                case NPCID.LittleHornetSpikey:
                case NPCID.LittleHornetStingy:
                case NPCID.LittleMossHornet:
                case NPCID.MossHornet:
                case NPCID.TinyMossHornet:
                    Item.NewItem(_s, _npc.targetRect, ItemID.HornetStatue);
                    break;
                case NPCID.FireImp:
                    Item.NewItem(_s, _npc.targetRect, ItemID.ImpStatue);
                    break;
                case NPCID.BloodJelly:
                case NPCID.BlueJellyfish:
                case NPCID.GreenJellyfish:
                case NPCID.PinkJellyfish:
                    Item.NewItem(_s, _npc.targetRect, ItemID.JellyfishStatue);
                    break;
                case NPCID.Lihzahrd:
                case NPCID.LihzahrdCrawler:
                    Item.NewItem(_s, _npc.targetRect, ItemID.LihzahrdStatue);
                    break;
                case NPCID.PigronCorruption:
                case NPCID.PigronCrimson:
                case NPCID.PigronHallow:
                    Item.NewItem(_s, _npc.targetRect, ItemID.PigronStatue);
                    break;
                case NPCID.Piranha:
                    Item.NewItem(_s, _npc.targetRect, ItemID.PiranhaStatue);
                    break;
                case NPCID.Reaper:
                    Item.NewItem(_s, _npc.targetRect, ItemID.ReaperStatue);
                    break;
                case NPCID.Shark:
                    Item.NewItem(_s, _npc.targetRect, ItemID.SharkStatue);
                    break;
                case NPCID.Skeleton:
                    Item.NewItem(_s, _npc.targetRect, ItemID.SkeletonStatue);
                    break;
                case NPCID.GreenSlime:
                case NPCID.BlueSlime:
                case NPCID.RedSlime:
                case NPCID.PurpleSlime:
                case NPCID.YellowSlime:
                case NPCID.BlackSlime:
                case NPCID.MotherSlime:
                case NPCID.IceSlime:
                case NPCID.SandSlime:
                case NPCID.JungleSlime:
                case NPCID.SpikedIceSlime:
                case NPCID.SpikedJungleSlime:
                case NPCID.SlimeSpiked:
                case NPCID.BabySlime:
                case NPCID.LavaSlime:
                case NPCID.DungeonSlime:
                case NPCID.Pinky:
                case NPCID.GoldenSlime:
                case NPCID.UmbrellaSlime:
                case NPCID.WindyBalloon:
                case NPCID.ShimmerSlime:
                case NPCID.SlimeRibbonGreen:
                case NPCID.SlimeRibbonRed:
                case NPCID.SlimeRibbonWhite:
                case NPCID.SlimeRibbonYellow:
                case NPCID.ToxicSludge:
                case NPCID.CorruptSlime:
                case NPCID.Slimeling:
                case NPCID.Slimer:
                case NPCID.Slimer2:
                case NPCID.Crimslime:
                case NPCID.Gastropod:
                case NPCID.IlluminantSlime:
                case NPCID.RainbowSlime:
                case NPCID.QueenSlimeMinionBlue:
                case NPCID.QueenSlimeMinionPink:
                case NPCID.QueenSlimeMinionPurple:
                    Item.NewItem(_s, _npc.targetRect, ItemID.SlimeStatue);
                    break;
                case NPCID.UndeadViking:
                case NPCID.UndeadMiner:
                    Item.NewItem(_s, _npc.targetRect, ItemID.UndeadVikingStatue);
                    break;
                case NPCID.Unicorn:
                    Item.NewItem(_s, _npc.targetRect, ItemID.UnicornStatue);
                    break;
                case NPCID.WallCreeper:
                case NPCID.WallCreeperWall:
                case NPCID.BlackRecluse:
                case NPCID.BlackRecluseWall:
                case NPCID.BloodCrawler:
                case NPCID.BloodCrawlerWall:
                case NPCID.JungleCreeper:
                case NPCID.JungleCreeperWall:
                    Item.NewItem(_s, _npc.targetRect, ItemID.WallCreeperStatue);
                    break;
                case NPCID.Wraith:
                    Item.NewItem(_s, _npc.targetRect, ItemID.WraithStatue);
                    break;
                case NPCID.ArmedTorchZombie:
                case NPCID.ArmedZombie:
                case NPCID.ArmedZombieCenx:
                case NPCID.ArmedZombieEskimo:
                case NPCID.ArmedZombiePincussion:
                case NPCID.ArmedZombieSlimed:
                case NPCID.ArmedZombieSwamp:
                case NPCID.ArmedZombieTwiggy:
                    Item.NewItem(_s, _npc.targetRect, ItemID.ZombieArmStatue);
                    break;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            float _scale = 20;

            if (targetPos.Count > 0)
            {
                foreach (Vector2 _dir in targetPos)
                {
                    float _ang = (_dir).ToRotation();
                    Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>("Tmodtober/Projectiles/medusa_eye_shine");
                    Rectangle _sourceRect = new Rectangle(0, 0, _texture.Width, _texture.Height);
                    Color _beamColor = new Color(1, 1, 1, 255f);

                    _beamColor *= 0.05f;
                    Main.spriteBatch.Draw(((Texture2D)_texture), Projectile.Center + new Vector2(_scale * (float)Math.Cos(_ang), _scale *(float) Math.Sin(_ang))/4f-Main.screenPosition, _sourceRect, _beamColor, _ang, new Vector2(_texture.Width, _texture.Height / 2f),new Vector2(20f,5f)/4f, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(((Texture2D)_texture), Projectile.Center + new Vector2(_scale * (float)Math.Cos(_ang), _scale *(float) Math.Sin(_ang))/2f-Main.screenPosition, _sourceRect, _beamColor, _ang, new Vector2(_texture.Width, _texture.Height / 2f),new Vector2(20f,5f)/2f, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(((Texture2D)_texture), Projectile.Center + new Vector2(_scale * (float)Math.Cos(_ang), _scale *(float) Math.Sin(_ang))/4f*3f-Main.screenPosition, _sourceRect, _beamColor, _ang, new Vector2(_texture.Width, _texture.Height / 2f),new Vector2(20f,5f)/4f*3f, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(((Texture2D)_texture), Projectile.Center + new Vector2(_scale * (float)Math.Cos(_ang), _scale *(float) Math.Sin(_ang))-Main.screenPosition, _sourceRect, _beamColor, _ang, new Vector2(_texture.Width, _texture.Height / 2f),new Vector2(20f,5f), SpriteEffects.None, 0);
                }

                base.PostDraw(lightColor);
            }

        }
    }
}