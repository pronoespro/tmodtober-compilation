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

namespace Tmodtober.Projectiles
{
    public class BunnyProjectile:ModProjectile
    {

        float speed;

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height= 26;

            Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.direction = Main.rand.Next(2) == 0 ? -1 : 1;
            speed = Main.rand.NextFloat(1, 2);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Point _projPos = Projectile.Center.ToTileCoordinates();

            Projectile.velocity = oldVelocity/2;
            if (WorldGen.SolidTile(_projPos+new Point(0, 1*Math.Sign(oldVelocity.Y))))
            {
                Projectile.velocity = new Vector2(Projectile.velocity.X, -Projectile.velocity.Y);
            }
            if (WorldGen.SolidTile(_projPos + new Point( 1 * Math.Sign(oldVelocity.X),0)))
            {
                Projectile.velocity = new Vector2(-Projectile.velocity.X, Projectile.velocity.Y);
            }

            if (Vector2.DistanceSquared(Vector2.Zero, Projectile.velocity) < 5 * 5){
                Projectile.damage = 0;
            }

            return false;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.PiOver4 / 4*Projectile.direction*speed;
            Projectile.velocity += new Vector2(0, 0.1f);

            if(Vector2.DistanceSquared(Vector2.Zero, Projectile.velocity) < 5 * 5){
                Projectile.alpha += 5;
                if (Projectile.alpha >= 250)
                {
                    Projectile.timeLeft =Math.Min(Projectile.timeLeft, 1);
                }
            }
        }

    }
}
