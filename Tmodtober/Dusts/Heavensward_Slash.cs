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
    public class Heavensward_Slash:ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, 0, 16, 48);

            dust.customData = 0;
        }

        public override bool MidUpdate(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.noLightEmittence = false;

            if (dust.customData != null && dust.customData is int)
            {
                dust.customData = (int)dust.customData + 1;

                int time = (int)dust.customData;

                float power = MathF.Min(1, (30f - time) / 20f);
                dust.alpha = (int)(255f - power * 255f);

                dust.scale = 1.5f;

                dust.velocity = new Vector2(0,MathF.Max(0,10-time)/60f*60*4);
                dust.rotation = 0;


                if (time >= 30)
                {
                    dust.active = false;
                }

            }

            return true;
        }

    }
}
