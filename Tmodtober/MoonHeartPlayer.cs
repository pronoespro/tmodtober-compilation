using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Tmodtober
{
    public class MoonHeartPlayer:ModPlayer
    {

        public const string MOONHEART_SAVE_TAG = "moon_heart_amount";

        private int moonHeartAmmount;

        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            health = new StatModifier(1, health.Multiplicative,health.Flat + (50f* moonHeartAmmount), health.Base);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add(MOONHEART_SAVE_TAG, moonHeartAmmount);
            base.SaveData(tag);
        }

        public override void LoadData(TagCompound tag)
        {

            if (tag.ContainsKey(MOONHEART_SAVE_TAG))
            {
                moonHeartAmmount = tag.GetInt(MOONHEART_SAVE_TAG);
            }

            base.LoadData(tag);
        }

        public void IncreaseMoonHeartAmmount()
        {
            moonHeartAmmount++;
            Main.NewText(moonHeartAmmount.ToString());
        }

    }
}
