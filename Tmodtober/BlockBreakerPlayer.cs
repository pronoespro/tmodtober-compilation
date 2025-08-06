using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace Tmodtober
{
    class BlockBreakerPlayer:ModPlayer
    {

        public static int InitTimer = 800;
        public static int TileTimeLower= 10;

        public int timer;
        public int blockBreakLevel;

        private int lastShownTime;

        public static bool needsBreakBlocks;

        public override void OnEnterWorld()
        {
            base.OnEnterWorld();
            timer = InitTimer;
            blockBreakLevel = 0;
        }

        public override void PostUpdate()
        {
            base.PostUpdate();

            if (needsBreakBlocks)
            {
                timer--;
                if (timer <= 0)
                {
                    Player.statLife = 0;

                    PlayerDeathReason _reason = PlayerDeathReason.ByCustomReason("Have to break... will... break... myself");
                    Player.Hurt(_reason, 100000, Player.direction, dodgeable: false, armorPenetration: 100);
                }
            }
        }


        public override void UpdateDead()
        {
            if (needsBreakBlocks)
            {
                timer = 100000000;
                blockBreakLevel = 0;
            }
            base.UpdateDead();
        }

        public override void OnRespawn()
        {
            if (needsBreakBlocks)
            {
                timer = InitTimer - (TileTimeLower * blockBreakLevel);
            }
            base.OnRespawn();
        }

        public void BreakBlock()
        {
            if (needsBreakBlocks)
            {
                blockBreakLevel++;
                timer = InitTimer - Math.Min(TileTimeLower * blockBreakLevel, InitTimer - 1);
                Main.NewText("Block break need satisfied... for now");
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);

            if (needsBreakBlocks)
            {
                int secondsLength = 30;

                if (timer / secondsLength != lastShownTime)
                {
                    lastShownTime = timer / secondsLength;
                    Main.NewText((timer / secondsLength).ToString() + "/" + ((InitTimer - TileTimeLower * blockBreakLevel) / secondsLength).ToString());
                }
            }
        }

    }
}
