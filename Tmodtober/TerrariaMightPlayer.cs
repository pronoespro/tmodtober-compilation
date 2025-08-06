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
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace Tmodtober
{
    public class TerrariaMightPlayer:ModPlayer
    {

        public const string RECIEVED_BLESSING_SAVE_KEY= "Dryad_Blessing_Final_Reward_Recieved";
        public bool recievedBlessing;

        public bool usingBlessing;

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            if (recievedBlessing){
                tag.Add(RECIEVED_BLESSING_SAVE_KEY, true);
            }
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            if (tag.ContainsKey(RECIEVED_BLESSING_SAVE_KEY))
            {
                recievedBlessing = true;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            base.ProcessTriggers(triggersSet);
            if(TmodtoberMod.TriggerBlessing.JustPressed && recievedBlessing){
                usingBlessing = !usingBlessing;

                TriggerBlessing();
            }
        }

        public override void UpdateDead()
        {
            base.UpdateDead();
            usingBlessing = false;
        }

        public override void OnEnterWorld()
        {

            if (recievedBlessing)
            {
                TriggerBlessing();
            }
        }

        public void RecieveBlessing()
        {
            recievedBlessing = true;
            usingBlessing = !usingBlessing;

            TriggerBlessing();

        }

        public void TriggerBlessing()
        {
            if (usingBlessing && ProjectileCount(ModContent.ProjectileType<Projectiles.Terrarias_Might>())==0)
            {
                EntitySource_Parent _s = new EntitySource_Parent(Player);
                Projectile.NewProjectile(_s, Player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Terrarias_Might>(),300, 1,Player.whoAmI);
            }
        }

        public int ProjectileCount(int _type)
        {
            int _count = 0;

            for(int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == _type)
                {
                    _count++;
                }
            }

            return _count;
        }

    }
}
