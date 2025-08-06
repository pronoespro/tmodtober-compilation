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
    public class MoonsterPlayer : ModPlayer
    {

        public const string MOONSTER_DRANK_SAVE_TAG = "drank_moonster_drink";

        public bool drankMoonsterDrink;

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            if (drankMoonsterDrink)
            {
                tag.Add(MOONSTER_DRANK_SAVE_TAG, true);
            }
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            if (tag.ContainsKey(MOONSTER_DRANK_SAVE_TAG)){
                drankMoonsterDrink = true;
            }
        }

        public override void PostUpdateBuffs()
        {
            if (drankMoonsterDrink)
            {
                Player.runAcceleration += 0.05f;
                Player.maxRunSpeed += 0.35f;
            }
            base.PostUpdateBuffs();
        }

    }
}
