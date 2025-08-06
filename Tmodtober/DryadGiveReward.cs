using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Tmodtober
{
    public class DryadGiveReward:GlobalNPC
    {

        public static bool castingBlessing;
        public static int castingTime;

        public override bool PreChatButtonClicked(NPC npc, bool firstButton)
        {
                TerrariaMightPlayer _mightyPlayer = Main.player[Main.myPlayer].GetModPlayer<TerrariaMightPlayer>();
            if (npc.type == NPCID.Dryad && !firstButton && (WorldGen.tEvil==0 && WorldGen.tGood == 0) && !_mightyPlayer.recievedBlessing)
            {

                if (!_mightyPlayer.recievedBlessing)
                {
                    Main.npcChatText = "Good job! Terraria is completelly safe thanks to you.\nTo conmemorate that, I will cast an old dryad blessing that should aid you in any of your adventures.";
                    castingBlessing = true;
                }
                return false;
            }
            return base.PreChatButtonClicked(npc, firstButton);
        }

        public override bool PreAI(NPC npc)
        {
            TerrariaMightPlayer _mightyPlayer = Main.player[Main.myPlayer].GetModPlayer<TerrariaMightPlayer>();
            if (npc.type == NPCID.Dryad && castingBlessing)
            {
                castingTime++;

                if (castingTime < 15)
                {
                    _mightyPlayer.Player.gravity = 0;
                    _mightyPlayer.Player.velocity = new Vector2(0, 1)*-3;
                }else{
                    _mightyPlayer.Player.velocity = new Vector2(0,-0.4f);
                    _mightyPlayer.Player.slowFall = true;
                    _mightyPlayer.Player.fallStart = 0;
                    _mightyPlayer.Player.fallStart2 = 0;
                    _mightyPlayer.Player.noFallDmg = true;
                }

                if (castingTime ==20)
                {
                    EntitySource_Parent _s = new EntitySource_Parent(npc);
                    Projectile.NewProjectile(_s, _mightyPlayer.Player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.MightSummonCircle>(), 0, 0, _mightyPlayer.Player.whoAmI);
                }

                if (castingTime >= 300){
                    castingBlessing = false;
                    castingTime = 0;
                }
                return false;
            }
            return base.PreAI(npc);
        }

        public override void FindFrame(NPC npc, int frameHeight)
        {
            if (npc.type == NPCID.Dryad && castingBlessing)
            {
                npc.frame.Y = (Main.time%10<5?19:20) * frameHeight;
            }
            else {
                base.FindFrame(npc, frameHeight);
            }
        }

    }
}
