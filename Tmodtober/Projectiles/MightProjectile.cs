using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace Tmodtober.Projectiles
{
    public class MightProjectile:ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 30;

            Projectile.DamageType = DamageClass.Generic;

            Projectile.friendly = true;
            Projectile.hostile= false;

            Projectile.ignoreWater = true;

            Projectile.timeLeft = 100;

        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Dust.NewDust(Projectile.Center, Projectile.whoAmI, Projectile.height, DustID.Asphalt, (int)(Projectile.velocity.X / 2), (int)(Projectile.velocity.Y / 2));

        }

    }
}
