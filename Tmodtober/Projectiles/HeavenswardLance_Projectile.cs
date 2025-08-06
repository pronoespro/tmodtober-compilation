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
    public class HeavenswardLance_Projectile:ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;
            Projectile.ignoreWater= true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 15;

            Projectile.scale = 1.1f;

        }

        public bool IsLineClear(Point _pos,int _height)
        {
            for (int i = 0; i < _height; i++)
            {
                if(WorldGen.SolidTile(_pos.X, _pos.Y+i))
                {
                    return false;
                }
            }
            return true;
        }

        public override void AI()
        {

            Vector2 _desPos = Main.player[Projectile.owner].Center + new Vector2(0, Main.player[Projectile.owner].height/2);
            if (IsLineClear(_desPos.ToTileCoordinates(),6))
            {
                Main.player[Projectile.owner].Center += new Vector2(0, 16 * 6);
                Main.player[Projectile.owner].velocity = Vector2.Zero;
            }
            else if(Projectile.ai[0]==0)
            {
                Main.player[Projectile.owner].velocity = new Vector2(0, 16 *6);
                MakeEffects();
            }
            if (Projectile.timeLeft==1)
            {
                MakeEffects();
            }

            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.velocity = Main.player[Projectile.owner].velocity;
            Projectile.rotation = MathHelper.PiOver4 +MathHelper.Pi;
        }

        public void MakeEffects()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = 1;

                Dust.NewDust(Projectile.Center + new Vector2(-Projectile.width / 2, Projectile.height / 4), 0, 0, ModContent.DustType<Dusts.Heavensward_GroundCircle>());

                Dust.NewDust(Projectile.Center + new Vector2(0, Projectile.height / 2), 0, 0, ModContent.DustType<Dusts.Heavensward_FloatCircle>(), -1, -1);
                Dust.NewDust(Projectile.Center + new Vector2(0, Projectile.height / 2+15), 0, 0, ModContent.DustType<Dusts.Heavensward_FloatCircle>(), 1, 1);

                Dust.NewDust(Projectile.Center + new Vector2(0, -Projectile.height * 1.5f), 0, 0, ModContent.DustType<Dusts.Heavensward_Slash>());

                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_14"), Projectile.Center);
                SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_30"), Projectile.Center);
            }
        }

    }
}
