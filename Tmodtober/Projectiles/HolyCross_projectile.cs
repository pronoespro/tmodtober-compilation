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
    public class HolyCross_projectile:ModProjectile
    {

        public override void SetDefaults()
        {

            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = true;
            Projectile.penetrate = -1;

        }

        public override void AI()
        {
            if (Main.rand.Next(3) == 0){
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.AncientLight, Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(-10, -3));
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4+MathHelper.Pi;
            Lighting.AddLight(Projectile.Center, new Vector3(1, 1, 1));
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.AncientLight,Main.rand.NextFloat(-10,10),Main.rand.NextFloat(-10,-3));
            return base.OnTileCollide(oldVelocity);
        }

    }
}
