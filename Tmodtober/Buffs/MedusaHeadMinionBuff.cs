using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace Tmodtober.Buffs
{
    public class MedusaHeadMinionBuff : ModBuff
    {

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
            tip = "Your uglyness turns enemies into stone";
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override bool RightClick(int buffIndex)
        {
            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 100;

            if (player.whoAmI == Main.myPlayer)
            {
                bool minionExists = false;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<Projectiles.MedusaSight>()
                        && Main.projectile[i].owner == player.whoAmI)
                    {
                        minionExists = true;
                        break;
                    }
                }
                if (!minionExists)
                {
                    Projectile.NewProjectile(new EntitySource_Buff(player, Type, buffIndex), player.Center, player.velocity, ModContent.ProjectileType<Projectiles.MedusaSight>(), 100, 1f);
                }
            }
        }

    }
}
