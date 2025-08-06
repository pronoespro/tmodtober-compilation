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
    public class WeakSpotHitSparkle:ModDust
    {

        float initScale =1f;

        public override void OnSpawn(Dust dust)
        {
            dust.scale = initScale;
            dust.customData = 0.5f;
        }

        public override bool Update(Dust dust)
        {

            int frameX = (int)MathF.Max(0,(float)dust.customData+12f)/6;
            int frameY = 1-(int)MathF.Max(0, (float)dust.customData + 9f) / 6;
            dust.frame = new Rectangle(frameX*8, frameY*8, 8, 8);

            dust.rotation += 0.1f;

            dust.alpha += 5;
            dust.scale += (float)dust.customData;
            dust.customData = ((float)dust.customData) - 0.05f;

            dust.noLight = false;
            dust.noLightEmittence = false;

            dust.position += dust.velocity;

            if (dust.scale < 0.25f)
                dust.active = false;
            else
                dust.active = true;

            return false;
        }

    }

    public class ImmuneSpotHitSparkle : ModDust
    {

        float initScale = 4f;

        public override void OnSpawn(Dust dust)
        {
            dust.scale = initScale;
            dust.customData = 0.5f;
        }

        public override bool Update(Dust dust)
        {


            int frameX = (int)MathF.Max(0, (float)dust.customData + 12f) / 6;
            int frameY = 1 - (int)MathF.Max(0, (float)dust.customData + 9f) / 6;
            dust.frame = new Rectangle(frameX * 8, frameY * 8, 8, 8);

            dust.rotation = MathHelper.PiOver4;

            dust.alpha += 5;
            dust.scale +=(float)dust.customData;
            dust.customData =((float)dust.customData)- 0.05f;

            dust.noLight = false;
            dust.noLightEmittence = false;

            dust.position += dust.velocity;

            if (dust.scale < 0.25f)
                dust.active = false;
            else
                dust.active = true;

            return false;
        }

    }

    public class StrongSpotHitSparkle : ModDust
    {

        float initScale = 1.5f;

        public override void OnSpawn(Dust dust)
        {
            dust.scale = initScale;
            dust.customData = 0.5f;
        }

        public override bool Update(Dust dust)
        {


            int frameX = (int)MathF.Max(0, (float)dust.customData + 12f) / 6;
            int frameY = 1 - (int)MathF.Max(0, (float)dust.customData + 9f) / 6;
            dust.frame = new Rectangle(frameX * 8, frameY * 8, 8, 8);

            dust.rotation = MathHelper.PiOver4;

            dust.alpha += 5;
            dust.scale += (float)dust.customData;
            dust.customData = ((float)dust.customData) - 0.05f;

            dust.noLight = false;
            dust.noLightEmittence = false;

            dust.position += dust.velocity;

            if (dust.scale < 0.25f)
                dust.active = false;
            else
                dust.active = true;

            return false;
        }

    }
}
