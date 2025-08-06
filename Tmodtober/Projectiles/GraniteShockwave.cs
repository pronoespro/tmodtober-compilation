using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober.Projectiles
{
    public class GraniteShockwave : ModProjectile
    {

        public override void SetDefaults()
		{
			Projectile.width = 16; // The width of projectile hitbox
			Projectile.height = 16; // The height of projectile hitbox

			Projectile.friendly = true; // Can the projectile deal damage to enemies?
			Projectile.hostile = false; // Can the projectile deal damage to the player?
			Projectile.DamageType = DamageClass.Ranged; // Is the projectile shoot by a ranged weapon?

			Projectile.penetrate = 1000; // How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.timeLeft = 80; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.alpha = 100; // The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			Projectile.light = 0.65f; // How much light emit around the projectile
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false; // Can the projectile collide with tiles?

			AIType = ProjectileID.Bullet; // Act exactly like default Bullet


			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

		}

        public override void AI()
        {

			Projectile.scale *=Math.Max(1f,0.75f+((Projectile.timeLeft-2)/60f)*0.3f);
			Projectile.alpha += 5;

			Projectile.velocity *= 0.9f;
			Projectile.rotation = Projectile.velocity.ToRotation();

        }
    }
}
