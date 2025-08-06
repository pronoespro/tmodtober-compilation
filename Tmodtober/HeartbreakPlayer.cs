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
    public class HeartbreakPlayer:ModPlayer
    {

        private bool isHeartBroken;

        public void SetHeartbroken() {
            isHeartBroken = true;
        }

        public override void ResetEffects()
        {
            isHeartBroken = false;
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            base.ModifyWeaponDamage(item, ref damage);
            if (isHeartBroken)
            {
                damage *= 0.5f;
            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (isHeartBroken)
            {
                Player.lifeRegen -= 20;
            }
        }

    }
}
