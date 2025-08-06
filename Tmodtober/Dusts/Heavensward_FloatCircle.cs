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

namespace Tmodtober.Dusts
{
    public class Heavensward_FloatCircle:ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, 0, 16, 16);

            dust.customData =new Vector2(0,dust.velocity.X);
        }

        public override bool MidUpdate(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.noLightEmittence = false;

            if (dust.customData != null && dust.customData is Vector2)
            {
                dust.customData =new Vector2(((Vector2)dust.customData).X + 1,((Vector2)dust.customData).Y);

                int time = (int)((Vector2)dust.customData).X;

                float power = MathF.Min(time / 10f, 1) * MathF.Min(1, (60f * 2f - time) / 40f);
                dust.alpha = (int)(255f - power * 255f);

                dust.scale = 1f;

                dust.velocity = new Vector2(MathF.Sin(time/5f* 1.25f) *1.25f* 7f * (((Vector2)dust.customData).Y > 0 ? 1f : -1f), -1f);
                dust.rotation = -dust.velocity.ToRotation();

                dust.color = Color.Cyan;

                if (time >= 60 * 2f)
                {
                    dust.active = false;
                }

            }

            return true;
        }

        public override bool PreDraw(Dust dust)
        {
            Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>(Texture).Value;
            Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
            Main.spriteBatch.Draw(_texture, dust.position, _rect, Color.White * 0.5f, 0, Vector2.Zero, dust.scale, SpriteEffects.None, 0);

            return true;
        }


    }
}
