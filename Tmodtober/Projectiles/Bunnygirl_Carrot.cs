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
    public class Bunnygirl_Carrot:ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.scale = 2f;
            Projectile.width =26;
            Projectile.height=63/2;

            Projectile.damage = 30;

            Projectile.hostile = true;
            Projectile.friendly = false;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            //Projectile.Center += Projectile.velocity;
            Projectile.rotation += MathHelper.PiOver4 *Math.Sign(Projectile.velocity.X);
        }

    }
}
