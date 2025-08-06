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

namespace Tmodtober.NPCs
{
    public class ArianelExplosiveBunny:ModNPC
    {

        public override string Texture => "Terraria/Images/NPC_614";

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 7;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0;
            NPC.width = 48;
            NPC.height = 280 / 7;

            NPC.lifeMax = 1;
            NPC.immortal = true;
            NPC.friendly = true;

            NPC.value = 0;
        }

        public override void AI()
        {
            if (NPC.ai[0] == 0)
            {
                if ((Math.Abs(NPC.velocity.Y) < 2 && NPC.oldVelocity.Y > 4 )||(NPC.oldVelocity== NPC.velocity && NPC.position==NPC.oldPosition))
                {
                    NPC.ai[0]++;
                    NPC.rotation = 0;
                }
                else
                {
                    NPC.rotation += MathHelper.PiOver4 / 2f;
                }
            }else {
                NPC.TargetClosest();
                Player _target = Main.player[NPC.target];

                float _moveSpeed = 1f - NPC.ai[0] / 120f;

                NPC.ai[1] +=_moveSpeed;
                NPC.velocity += new Vector2(Math.Sign(_target.Center.X-NPC.Center.X)*_moveSpeed*0.5f,0);

                NPC.ai[0]++;
            }

            if (NPC.ai[0] == 60 * 2)
            {
                int _proj = Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileID.Explosives, 10, 2f);
                Main.projectile[_proj].friendly = true;
                Main.projectile[_proj].hostile = true;

                NPC.immortal = false;
            }

            NPC.frameCounter = (int)(NPC.ai[1] % 7);

        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = (int)(frameHeight * NPC.frameCounter);
        }



    }
}
