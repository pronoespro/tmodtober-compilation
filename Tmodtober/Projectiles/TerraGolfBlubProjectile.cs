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
using Terraria.Audio;

namespace Tmodtober.Projectiles
{

    public enum ClubState
    {
        SwingBack,
        FullCharge,
        Swinging,
        Aftermath
    }

    public class TerraGolfBlubProjectile:ModProjectile
    {

        private const int maxPower=60;
        private ClubState curState;

        private const int SwingTime = 10;
        private const int SwingAftermathTime = 20;

        public override string Texture => "Tmodtober/Items/TerraGolfClub";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 32*4;
            Projectile.height= 32*4;
            Projectile.friendly = true;
            Projectile.hostile= false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Throwing;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            player.SetDummyItemTime(2);
            Projectile.direction = player.direction;
            Projectile.spriteDirection = Projectile.direction;

            if (!player.active || player.dead || player.CCed || Vector2.DistanceSquared(Projectile.Center, player.Center) > 900 * 900)
            {
                Projectile.Kill();
                return;
            }

            if(Main.myPlayer==Projectile.owner && Main.mapFullscreen)
            {
                Projectile.Kill();
                return;
            }

            Vector2 mountedCenter = player.MountedCenter;

            Projectile.Center = mountedCenter+new Vector2(Projectile.direction*Projectile.width/2f,0);

            switch (curState)
            {
                default:
                case ClubState.SwingBack:
                    Projectile.ai[1] = 0;
                    Projectile.damage = 0;
                    Projectile.Center = mountedCenter;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        if (!player.channel)
                        {
                            Projectile.Kill();
                            return;
                        }
                        Projectile.ai[0] = Math.Min(maxPower, Projectile.ai[0] + 1);
                        if (Projectile.ai[0] >= maxPower)
                        {
                            curState = ClubState.FullCharge;
                            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/MaxMana"),player.Center);
                        }
                    }
                    Projectile.rotation =Projectile.ai[0] / maxPower * MathHelper.Pi;
                    break;
                case ClubState.FullCharge:
                    Projectile.rotation = MathHelper.Pi;
                    Projectile.damage = 0;
                    Projectile.Center = mountedCenter;
                    Projectile.ai[1] = 1;
                    if (!player.channel)
                    {
                        Projectile.ai[0] = 0;
                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_1"), player.Center);
                        curState = ClubState.Swinging;
                        Projectile.netUpdate = true;
                        break;
                    }
                    break;
                case ClubState.Swinging:
                    Projectile.damage = 100+100*(int)Projectile.ai[1];
                    Projectile.ai[0]=Math.Min(Projectile.ai[0]+1f,maxPower);
                    Projectile.rotation =-MathHelper.Pi +(maxPower- Projectile.ai[0]) / maxPower * MathHelper.TwoPi*4;
                    if (Projectile.ai[0] >=SwingTime ){
                        Projectile.ai[0] = 0;
                        curState = ClubState.Aftermath;
                        Projectile.netUpdate = true;
                        break;
                    }
                    break;
                case ClubState.Aftermath:
                    Projectile.rotation = -MathHelper.PiOver2;
                    Projectile.damage = 0;
                    Projectile.Center = mountedCenter;
                    Projectile.ai[0]++;
                    if (Projectile.ai[0] >= SwingAftermathTime)
                    {
                        Projectile.Kill();
                        return;
                    }
                    break;
            }
            Projectile.rotation *= Projectile.direction;
            player.itemRotation = Projectile.rotation;
            Projectile.rotation += MathHelper.PiOver2;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {

            if (curState != ClubState.Swinging)
            {
                return false;
            }

            return base.Colliding(projHitbox, targetHitbox);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            if (curState != ClubState.Swinging)
            {
                return false;
            }

            return base.CanCutTiles();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (!target.boss)
            {
                target.velocity = new Vector2(target.velocity.X * Main.player[Projectile.owner].direction, target.velocity.Y);
                target.AddBuff(ModContent.BuffType<Buffs.LaunchedDebuff>(), 60);
            }
        }

    }
}
