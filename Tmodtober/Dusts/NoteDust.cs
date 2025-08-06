using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Tmodtober.Dusts
{
    public class NoteDust:ModDust
    {

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, 0, 32, 32);

            dust.velocity = Vector2.Zero;

            dust.customData = 50;
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override bool MidUpdate(Dust dust)
        {
            dust.alpha = 0;

            dust.scale = 1f;

            if (dust.customData != null && dust.customData is int)
            {
                dust.customData=((int)dust.customData)-1;

                int time = (int)dust.customData;

                dust.alpha = (int)((50-time) / 50f * 255f);

                dust.position += new Vector2((float)Math.Cos(time / 3f), -2f);
                dust.rotation += (float)Math.Cos(time / 3f) * 0.1f;
                if (time <= 0)
                {
                    dust.active = false;
                }

            }

            return true;
        }

    }
}
