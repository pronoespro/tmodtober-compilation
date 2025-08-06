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

namespace Tmodtober.Dusts
{
    public class Heavensward_GroundCircle:ModDust
    {

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, 0, 32, 13);

            dust.velocity = Vector2.Zero;

            dust.customData = 1;
        }

        public override bool MidUpdate(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.noLightEmittence = false;
            dust.rotation = 0;

            if (dust.customData != null && dust.customData is int)
            {
                dust.customData = ((int)dust.customData) + 1;

                int time = (int)dust.customData;

                float power= MathF.Min(time / 10f, 1) * MathF.Min(1, (60f*2f - time) / 40f);
                dust.alpha =(int)((255f-power*255f*0.65f));
                dust.scale = MathF.Min(time / 15f, 1)*4f;
                dust.velocity = Vector2.Zero;

                dust.frame = new Rectangle(0, 13* ((time / 3)%6), 32, 13);

                if (time >=60*2f)
                {
                    dust.active = false;
                }

            }

            return true;
        }


    }
}
