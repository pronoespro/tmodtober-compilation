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
    public class Bunnygirl_tree : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 113;
            Projectile.height = 55;

            Projectile.damage = 45;

            Projectile.hostile = true;
            Projectile.friendly = false;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.velocity += new Vector2(0, 0.5f);
            Projectile.rotation += MathHelper.PiOver4/2f * Math.Sign(Projectile.velocity.X);
        }

    }
}
