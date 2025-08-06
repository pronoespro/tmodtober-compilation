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
using Terraria.GameContent.ItemDropRules;

namespace Tmodtober
{

    public enum WeakSpotType
    {
        none,
        weak,
        strong,
        bulletImmune
    }

    public class WeakSpot
    {
        public string name;
        public Vector2 position;
        public float range;
        public WeakSpotType type;
        public bool scaleWithNPCSize;

        public WeakSpot(string _name,Vector2 _pos,float _range,WeakSpotType _type,bool _scaleWithSize=false)
        {
            name = _name;
            position = _pos;
            range = _range;
            type = _type;
            scaleWithNPCSize = _scaleWithSize;
        }
    }

    public class BossWeaknessOverride:GlobalNPC
    {

        public override bool InstancePerEntity => true;

        public List<WeakSpot> weakSpots;

        public override void SetDefaults(NPC entity)
        {
            base.SetDefaults(entity);
            weakSpots = new List<WeakSpot>();

            switch (entity.type)
            {
                case NPCID.EyeofCthulhu:
                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                    weakSpots.Add(new WeakSpot("Iris", new Vector2(0, 50), 50,WeakSpotType.weak));
                    break;
                case NPCID.KingSlime:
                    weakSpots.Add(new WeakSpot("Crown", new Vector2(0, -75), 100, WeakSpotType.strong));
                    weakSpots.Add(new WeakSpot("Bottom", new Vector2(0, 175), 300, WeakSpotType.bulletImmune,true));
                    break;
                case NPCID.Plantera:
                    weakSpots.Add(new WeakSpot("Side",new Vector2(-50,0),75,WeakSpotType.weak));
                    weakSpots.Add(new WeakSpot("Side",new Vector2(50,0),75,WeakSpotType.weak));
                    break;
                case NPCID.QueenBee:
                    weakSpots.Add(new WeakSpot("Head", new Vector2(200, 0), 200, WeakSpotType.strong));
                    weakSpots.Add(new WeakSpot("Head", new Vector2(-200, 0), 200, WeakSpotType.strong));
                    weakSpots.Add(new WeakSpot("Head", new Vector2(0, -200), 200, WeakSpotType.weak));
                    weakSpots.Add(new WeakSpot("Head", new Vector2(0, 200), 200, WeakSpotType.strong));
                    break;
                case NPCID.QueenSlimeBoss:
                    weakSpots.Add(new WeakSpot("Wing_l", new Vector2(200, 0), 100, WeakSpotType.weak, true));
                    weakSpots.Add(new WeakSpot("Wing_r", new Vector2(-200, 0), 100, WeakSpotType.weak, true));
                    weakSpots.Add(new WeakSpot("Crown", new Vector2(0, -75), 100, WeakSpotType.strong,true));
                    weakSpots.Add(new WeakSpot("Bottom", new Vector2(0, 175), 300, WeakSpotType.bulletImmune, true));
                    break;
                case NPCID.EaterofWorldsBody:
                    weakSpots.Add(new WeakSpot("General",  Vector2.Zero, 300, WeakSpotType.bulletImmune, true));
                    break;
                case NPCID.EaterofWorldsHead:
                    break;
                case NPCID.EaterofWorldsTail:
                    weakSpots.Add(new WeakSpot("General",  Vector2.Zero, 300, WeakSpotType.weak, true));
                    break;
                case NPCID.TheDestroyerBody:
                    weakSpots.Add(new WeakSpot("General", Vector2.Zero, 100, WeakSpotType.bulletImmune, true));
                    break;
                case NPCID.TheDestroyerTail:
                case NPCID.TheDestroyer:
                    weakSpots.Add(new WeakSpot("General", Vector2.Zero, 100, WeakSpotType.weak, true));
                    break;
                case NPCID.GolemHead:
                case NPCID.Golem:
                    weakSpots.Add(new WeakSpot("General", Vector2.Zero, 500, WeakSpotType.weak, true));
                    break;
                case NPCID.GolemFistRight:
                case NPCID.GolemFistLeft:
                    weakSpots.Add(new WeakSpot("General", Vector2.Zero, 500, WeakSpotType.bulletImmune, true));
                    break;
                case NPCID.HallowBoss:
                    weakSpots.Add(new WeakSpot("Head", new Vector2(0,-50), 100, WeakSpotType.weak, true));
                    weakSpots.Add(new WeakSpot("Wings", new Vector2(-50,0), 100, WeakSpotType.strong, true));
                    weakSpots.Add(new WeakSpot("Wings", new Vector2(50,0), 100, WeakSpotType.strong, true));
                    break;
                case NPCID.CultistBoss:
                    weakSpots.Add(new WeakSpot("General", Vector2.Zero, 500, WeakSpotType.weak, true));
                    break;
                case NPCID.CultistDragonHead:
                case NPCID.CultistDragonTail:
                    weakSpots.Add(new WeakSpot("General", Vector2.Zero, 500, WeakSpotType.weak, true));
                    break;
                case NPCID.CultistDragonBody1:
                case NPCID.CultistDragonBody2:
                case NPCID.CultistDragonBody3:
                case NPCID.CultistDragonBody4:
                    weakSpots.Add(new WeakSpot("General", Vector2.Zero, 500, WeakSpotType.strong, true));
                    break;
                case NPCID.MoonLordCore:
                case NPCID.MoonLordHead:
                case NPCID.MoonLordLeechBlob:
                    weakSpots.Add(new WeakSpot("General", Vector2.Zero, 500, WeakSpotType.weak, true));
                    break;
                case NPCID.MoonLordFreeEye:
                case NPCID.MoonLordHand:
                    weakSpots.Add(new WeakSpot("General", Vector2.Zero, 500, WeakSpotType.strong, true));
                    break;
                case NPCID.DukeFishron:
                    weakSpots.Add(new WeakSpot("Head", new Vector2(100,0), 75, WeakSpotType.strong, true));
                    weakSpots.Add(new WeakSpot("Tail", new Vector2(-100,0), 75, WeakSpotType.weak, true));
                    weakSpots.Add(new WeakSpot("Tail", new Vector2(0,-50), 75, WeakSpotType.bulletImmune, true));
                    break;
            }

        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            int _hit = WeakSpotHit(projectile.Center, npc);
            if (weakSpots.Count > 0 && _hit >= 0)
            {
                return !(weakSpots[_hit].type == WeakSpotType.bulletImmune && projectile.DamageType==DamageClass.Ranged);
            }
            return base.CanBeHitByProjectile(npc, projectile);
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitByItem(npc, player, item, ref modifiers);
            int _hit = WeakSpotHit(player.Center, npc);
            if (weakSpots.Count > 0 && _hit >= 0)
            {
                Vector2 _vel = npc.Center - player.Center;
                switch (weakSpots[_hit].type)
                {
                    case WeakSpotType.strong:
                        modifiers.ScalingArmorPenetration += -0.2f;
                        modifiers.FinalDamage *= 0.25f;
                        CreateDust(_vel, npc, _hit,Color.White, WeakSpotType.strong);
                        break;
                    case WeakSpotType.bulletImmune:
                        modifiers.ScalingArmorPenetration += -1;
                        modifiers.FinalDamage *= 0.05f;
                        CreateDust(_vel, npc, _hit,Color.White, WeakSpotType.bulletImmune);
                        break;
                    case WeakSpotType.weak:
                        CreateDust(_vel, npc, _hit, Color.White, WeakSpotType.weak);
                        modifiers.ScalingArmorPenetration += 1;
                        modifiers.FlatBonusDamage += item.damage / 2;
                        break;
                    default:
                        break;
                }
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            int _hit = WeakSpotHit(projectile.Center, npc);
            if (weakSpots.Count > 0 && _hit >= 0)
            {
                Vector2 _vel = projectile.velocity;
                switch (weakSpots[_hit].type)
                {
                    case WeakSpotType.strong:
                        CreateDust(_vel, npc, _hit, Color.White, WeakSpotType.strong);
                        modifiers.FinalDamage *= 0.5f;
                        break;
                    case WeakSpotType.bulletImmune:
                        CreateDust(_vel, npc, _hit, Color.White, WeakSpotType.bulletImmune);
                        modifiers.ScalingArmorPenetration += -0.5f;

                        if (modifiers.DamageType == DamageClass.Ranged || modifiers.DamageType==DamageClass.Magic || modifiers.DamageType==DamageClass.Summon){
                            modifiers.FinalDamage *= 0;
                        }

                        break;
                    case WeakSpotType.weak:
                        CreateDust(_vel, npc, _hit,Color.White, WeakSpotType.weak);
                        modifiers.ScalingArmorPenetration += 1;
                        modifiers.FinalDamage *= 3f;
                        break;
                    default:
                        break;
                }
            }
            base.ModifyHitByProjectile(npc, projectile, ref modifiers);
        }

        public void CreateDust(Vector2 _vel, NPC npc, int _hit,Color _clr, WeakSpotType dustType)
        {
            int _dustType;
            int _ammount;
            switch (dustType)
            {
                default:
                case WeakSpotType.weak:
                    _dustType = ModContent.DustType<Dusts.WeakSpotHitSparkle>();
                    _ammount = 5;
                    break;
                case WeakSpotType.strong:
                    _dustType = ModContent.DustType<Dusts.StrongSpotHitSparkle>();
                    _ammount = 3;
                    break;
                case WeakSpotType.bulletImmune:
                    _ammount = 1;
                    _dustType = ModContent.DustType<Dusts.ImmuneSpotHitSparkle>();
                    break;
            }
            Vector2 _rotatedVel;
            for (int i = 0; i < _ammount; i++)
            {
                _rotatedVel = Vector2.Normalize(-_vel).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4/3, MathHelper.PiOver4/3))*Main.rand.NextFloat(1,7);
                Dust.NewDust(npc.Center + GetWeakSpotPosition(_hit, npc), (int)npc.scale, (int)npc.scale, _dustType, SpeedX:(int)(_rotatedVel.X ),SpeedY: (int)(_rotatedVel.Y ),newColor:_clr);
            }
        }

        public int WeakSpotHit(Vector2 _pos,NPC npc)
        {
            for (int i = 0; i < weakSpots.Count; i++)
            {
                if (Vector2.Distance(_pos, npc.Center + GetWeakSpotPosition(i,npc)) < weakSpots[i].range)
                {
                    return i;
                }
            }
            return -1;
        }

        public Vector2 GetWeakSpotPosition(int _spot,NPC npc)
        {
            Vector2 _weakSpotPos = weakSpots[_spot].position.RotatedBy(npc.rotation);
            _weakSpotPos *= (weakSpots[_spot].scaleWithNPCSize) ? npc.scale : 1f;
            return _weakSpotPos;
        }

    }
}
