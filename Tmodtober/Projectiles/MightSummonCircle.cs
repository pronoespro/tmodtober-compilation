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
using Microsoft.Xna.Framework.Graphics;

namespace Tmodtober.Projectiles
{
    public class MightSummonCircle:ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height= 64;
            Projectile.damage=0;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            
            Projectile.penetrate = -1;

            Projectile.scale = 0.1f;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {


            Projectile.scale=Math.Min(4f, Math.Min( Math.Max(200f-Projectile.timeLeft,0.1f)*5f,(Projectile.timeLeft*2f))/100f*3f);

            Projectile.rotation = Math.Max(0,((float)Projectile.timeLeft) * ((float)Projectile.timeLeft)-100*100)/900f*MathHelper.PiOver4;

            if (Projectile.owner >= 0)
            {
                Player _p = Main.player[Projectile.owner];
                Projectile.Center = _p.Center;
                Projectile.velocity = Vector2.Zero;
            }

            int _dustAmmount =(int)( Main.rand.Next(-2, 2) * Projectile.scale);
            for (int i = 0; i < _dustAmmount; i++)
            {
                Dust.NewDust(Projectile.Center - new Vector2(Projectile.width, Projectile.height) * Projectile.scale*8 / 2f, (int)(Projectile.width * Projectile.scale)*8, (int)(Projectile.height * Projectile.scale)*8, DustID.TerraBlade, SpeedX: 0, SpeedY: -Main.rand.Next(0, (int)(Projectile.scale*8)));
            }

            if (Projectile.timeLeft == 50){
                EntitySource_Parent _s = new EntitySource_Parent(Projectile);

                TerrariaMightPlayer _mightPlayer = Main.player[Projectile.owner].GetModPlayer<TerrariaMightPlayer>();

                _mightPlayer.RecieveBlessing();

            }

            base.AI();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D _texture = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
            Main.spriteBatch.Draw(_texture, Projectile.Center-Main.screenPosition, _rect, lightColor, Projectile.rotation, new Vector2(_texture.Width / 2, _texture.Height / 2),Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

    }
}
