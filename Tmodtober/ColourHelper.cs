using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober
{
    public static class ColourHelper
    {
        public static Color ConvertCurPaintToColor(byte curPaint)
        {

            switch (curPaint)
            {
                default:
                case PaintID.None:
                    return Color.White;
                case PaintID.BluePaint:
                case PaintID.DeepBluePaint:
                    return Color.Blue;
                case PaintID.BrownPaint:
                    return Color.Brown;
                case PaintID.CyanPaint:
                case PaintID.DeepCyanPaint:
                    return Color.Cyan;
                case PaintID.DeepGreenPaint:
                case PaintID.GreenPaint:
                    return Color.Green;
                case PaintID.DeepLimePaint:
                case PaintID.LimePaint:
                    return Color.Lime;
                case PaintID.DeepOrangePaint:
                case PaintID.OrangePaint:
                    return Color.Orange;
                case PaintID.DeepPinkPaint:
                case PaintID.PinkPaint:
                    return Color.Pink;
                case PaintID.DeepPurplePaint:
                case PaintID.PurplePaint:
                    return Color.Purple;
                case PaintID.DeepRedPaint:
                case PaintID.RedPaint:
                    return Color.Red;
                case PaintID.DeepSkyBluePaint:
                case PaintID.SkyBluePaint:
                    return Color.SkyBlue;
                case PaintID.DeepTealPaint:
                case PaintID.TealPaint:
                    return Color.Teal;
                case PaintID.DeepVioletPaint:
                case PaintID.VioletPaint:
                    return Color.Violet;
                case PaintID.DeepYellowPaint:
                case PaintID.YellowPaint:
                    return Color.Yellow;
                case PaintID.GrayPaint:
                    return Color.Gray;
                case PaintID.IlluminantPaint:
                    return Color.Transparent;
                case PaintID.WhitePaint:
                    return Color.White;
                case PaintID.ShadowPaint:
                case PaintID.BlackPaint:
                    return Color.Black;
                case PaintID.NegativePaint:
                    return Color.Bisque;
            }

        }
    }
}
