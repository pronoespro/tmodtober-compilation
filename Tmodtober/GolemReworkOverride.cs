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
using Microsoft.Xna.Framework.Graphics;

namespace Tmodtober
{
    public class GolemReworkOverride:GlobalNPC
    {

        public const float maxGolemHeadDistance = 600;

        public override bool PreAI(NPC npc)
        {

            if (npc.type == NPCID.Golem)
            {

                npc.TargetClosest(false);

                if (npc.target < 0 || !Main.player[npc.target].active || Main.player[npc.target].dead)
                {
                    npc.velocity = new Vector2(0, -100);
                    npc.rotation += MathHelper.PiOver2;
                    return false;
                }

                if (!NPC.AnyNPCs(NPCID.GolemHead))
                {
                    if (npc.life == npc.lifeMax)
                    {
                        EntitySource_Parent _s = new EntitySource_Parent(npc);
                        NPC.NewNPC(_s, (int)npc.Center.X, (int)npc.Center.Y, NPCID.GolemHead, Target: npc.target);
                        NPC.NewNPC(_s, (int)npc.Center.X, (int)npc.Center.Y, NPCID.GolemFistLeft, Target: npc.target);
                        NPC.NewNPC(_s, (int)npc.Center.X, (int)npc.Center.Y, NPCID.GolemFistRight, Target: npc.target);

                        npc.life--;
                    }
                    else if (!NPC.AnyNPCs(NPCID.GolemHeadFree))
                    {
                        EntitySource_Parent _s = new EntitySource_Parent(npc);
                        NPC.NewNPC(_s, (int)npc.Center.X, (int)npc.Center.Y, NPCID.GolemHeadFree, Target: npc.target);

                    }
                }

                npc.immortal = NPC.AnyNPCs(NPCID.GolemHead);
                npc.friendly = false;
                npc.dontTakeDamageFromHostiles = true;

                return false;
            }
            if (npc.type == NPCID.GolemHead)
            {
                npc.friendly = false;
                npc.dontTakeDamageFromHostiles = true;
                if (!NPC.AnyNPCs(NPCID.Golem))
                {
                    npc.life = 0;
                    npc.checkDead();

                    return false;
                }

                int golemBody = NPC.FindFirstNPC(NPCID.Golem);
                if (golemBody <= 0 || !Main.npc[golemBody].active)
                {
                    npc.velocity = new Vector2(0, -100);
                    npc.timeLeft = Math.Min(npc.timeLeft, 10);
                    return false;
                }

                npc.Center = Main.npc[golemBody].Center + new Vector2(0, -75 * Main.npc[golemBody].scale);

                npc.ai[0]++;

                if (npc.target < 0 || !Main.player[npc.target].active || Main.player[npc.target].dead)
                {
                    npc.TargetClosest(false);
                }

                if (npc.target >= 0)
                {
                    float _desRot = (Main.player[npc.target].Center - npc.Center).ToRotation();
                    if (npc.ai[0] <= 50)
                    {
                        npc.rotation = _desRot - MathHelper.PiOver4;
                    }
                    else if (npc.ai[0] > 50 && npc.ai[0] < 200)
                    {
                        if (npc.ai[0] % 5 == 0)
                        {
                            npc.rotation += 0.1f * MathHelper.PiOver4;
                            EntitySource_Parent _s = new EntitySource_Parent(npc);
                            Vector2 _dir = npc.rotation.ToRotationVector2() * 15;
                            int _proj = Projectile.NewProjectile(_s, npc.Center, _dir, ProjectileID.EyeBeam, npc.damage / 4, 1f);
                            Main.projectile[_proj].friendly = false;

                            _proj = Projectile.NewProjectile(_s, npc.Center, -_dir, ProjectileID.EyeBeam, npc.damage / 4, 1f);
                            Main.projectile[_proj].friendly = false;
                        }
                    }
                    else if (npc.ai[0] <= 250)
                    {
                        npc.rotation = _desRot + MathHelper.PiOver4;
                    }
                    else
                    {
                        if (npc.ai[0] % 5 == 0)
                        {
                            npc.rotation -= 0.1f * MathHelper.PiOver4;
                            EntitySource_Parent _s = new EntitySource_Parent(npc);
                            Vector2 _dir = npc.rotation.ToRotationVector2() * 15;
                            int _proj = Projectile.NewProjectile(_s, npc.Center, _dir, ProjectileID.EyeBeam, npc.damage / 4, 1f);

                            _proj = Projectile.NewProjectile(_s, npc.Center, -_dir, ProjectileID.EyeBeam, npc.damage / 4, 1f);
                        }
                    }
                }

                if (npc.ai[0] >= 400)
                {
                    npc.ai[0] = 0;
                }

                return false;
            }

            if (npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight)
            {
                npc.friendly = false;
                npc.dontTakeDamageFromHostiles = true;
                if (!NPC.AnyNPCs(NPCID.Golem))
                {
                    npc.life = 0;
                    npc.checkDead();

                    return false;
                }
                npc.TargetClosest(false);
                int golemBody = NPC.FindFirstNPC(NPCID.Golem);
                if (golemBody <= 0 || !Main.npc[golemBody].active)
                {
                    npc.velocity = new Vector2(0, -100);
                    npc.timeLeft = Math.Min(npc.timeLeft, 10);
                }

                npc.ai[0]++;

                if (npc.type == NPCID.GolemFistLeft)
                {
                    Vector2 _ogPos = Main.npc[golemBody].Center + new Vector2(-100, 0) * Main.npc[golemBody].scale;

                    if (npc.ai[0] < 100)
                    {
                        npc.Center = Vector2.Lerp(npc.Center, _ogPos, 0.2f);
                    }
                    else
                    {
                        float _percentDone = (npc.ai[0] - 100f) / 50;
                        Vector2 _desPos = _ogPos - new Vector2( _percentDone * 150, 0).RotatedBy(MathHelper.PiOver2 * (_percentDone));
                        npc.Center = Vector2.Lerp(npc.Center, _desPos, 0.2f);
                    }
                }
                if (npc.type == NPCID.GolemFistRight)
                {
                    Vector2 _ogPos = Main.npc[golemBody].Center + new Vector2(100, 0) * Main.npc[golemBody].scale;
                    if (npc.ai[0] < 100)
                    {
                        npc.Center = Vector2.Lerp(npc.Center, _ogPos, 0.2f);
                    }
                    else
                    {
                        float _percentDone =(npc.ai[0] - 100f) / 50;
                        Vector2 _desPos= _ogPos+ new Vector2(_percentDone * 150, 0).RotatedBy(MathHelper.PiOver2 * (_percentDone));
                        npc.Center = Vector2.Lerp(npc.Center, _desPos, 0.2f);
                    }

                }

                if(npc.ai[0]>(NPC.AnyNPCs(NPCID.GolemHeadFree)?100: 300) && npc.ai[0]<400 && npc.ai[0] %  50 == 0 && npc.target>=0 && Main.player[npc.target].active && !Main.player[npc.target].dead)
                {
                    Vector2 _vel = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 7;
                    EntitySource_Parent _s = new EntitySource_Parent(npc);
                    int _proj = Projectile.NewProjectile(_s, npc.Center, _vel, ProjectileID.Fireball, npc.damage / 3, 1f);
                    Main.projectile[_proj].friendly = false;
                    Main.projectile[_proj].hostile= true;
                }

                if (npc.ai[0] >= 500){
                    npc.ai[0] = 0;
                }
                return false;
            }

            if (npc.type == NPCID.GolemHeadFree)
            {

                npc.friendly = false;
                npc.dontTakeDamageFromHostiles = true;

                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead && Vector2.DistanceSquared(Main.player[i].Center + Main.player[i].velocity, npc.Center) >= maxGolemHeadDistance * maxGolemHeadDistance)
                    {
                        Main.player[i].Center = npc.Center + Vector2.Normalize(Main.player[i].Center - npc.Center) * maxGolemHeadDistance;
                        Main.player[i].velocity *= -1;
                    }
                }

                npc.TargetClosest(false);

                if (!NPC.AnyNPCs(NPCID.Golem))
                {
                    npc.life = 0;
                    npc.checkDead();

                    return false;
                }

                int golemBody = NPC.FindFirstNPC(NPCID.Golem);
                if (golemBody <= 0 || !Main.npc[golemBody].active)
                {
                    npc.velocity = new Vector2(0, -100);
                    npc.timeLeft = Math.Min(npc.timeLeft, 10);
                }

                Vector2 _desPos= Main.npc[golemBody].Center + new Vector2(MathF.Cos((float)Main.time/80)*250, -75 * Main.npc[golemBody].scale-(MathF.Sin((float)Main.time/40f)+1)*350);
                npc.Center =Vector2.Lerp(npc.Center, _desPos,0.05f);

                npc.ai[0]++;

                if (npc.ai[0] % 100 == 50)
                {
                    EntitySource_Parent _s = new EntitySource_Parent(npc);
                    Vector2 _vel = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 7;

                    for (int i = 0; i < 4; i++)
                    {
                        int _proj = Projectile.NewProjectile(_s, npc.Center, _vel.RotatedBy(MathHelper.PiOver2*i), ProjectileID.EyeBeam, npc.damage/4, 1f);
                    }
                }
                if (npc.ai[0] % 100 == 0)
                {
                    EntitySource_Parent _s = new EntitySource_Parent(npc);
                    Vector2 _vel = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 7;

                    for (int i = 0; i < 4; i++)
                    {
                        int _proj = Projectile.NewProjectile(_s, npc.Center, _vel.RotatedBy(MathHelper.PiOver2 * i + MathHelper.PiOver4), ProjectileID.EyeBeam, npc.damage/4, 1f);
                    }
                }

                return false;
            }

            return base.PreAI(npc);
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            if ((npc.type==NPCID.Golem|| npc.type == NPCID.GolemFistLeft|| npc.type == NPCID.GolemFistRight
                || npc.type == NPCID.GolemHead|| npc.type == NPCID.GolemHeadFree)&& projectile.type==ProjectileID.EyeBeam)
            {
                return false;
            }
            return base.CanBeHitByProjectile(npc, projectile);
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            if (npc.type == NPCID.Golem)
            {
                Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/NPC_245").Value;
                Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height / 7);
                spriteBatch.Draw(_texture, npc.Center-Main.screenPosition, _rect, drawColor, npc.rotation, new Vector2(_texture.Width / 2, _texture.Height / 7 / 2), npc.scale, SpriteEffects.None, 0);

            }
            if (npc.type == NPCID.GolemHead)
            {
                Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/NPC_249").Value;
                Rectangle _rect = new Rectangle(0, (int)(npc.ai[0]%100/50)*_texture.Height/2, _texture.Width, _texture.Height / 2);
                spriteBatch.Draw(_texture, npc.Center - Main.screenPosition, _rect, drawColor, npc.rotation, new Vector2(_texture.Width / 2, _texture.Height / 2/2), npc.scale, SpriteEffects.None, 0);

            }
            if (npc.type == NPCID.GolemFistLeft)
            {
                Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/NPC_247").Value;
                Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height / 2);
                spriteBatch.Draw(_texture, npc.Center - Main.screenPosition, _rect, drawColor, npc.rotation, new Vector2(_texture.Width / 2, _texture.Height /2), 1f, SpriteEffects.None, 0);

            }
            if (npc.type == NPCID.GolemFistRight)
            {
                Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/NPC_248").Value;
                Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height / 2);
                spriteBatch.Draw(_texture, npc.Center - Main.screenPosition, _rect, drawColor, npc.rotation, new Vector2(_texture.Width / 2, _texture.Height / 2), 1f, SpriteEffects.None, 0);
            }
            if (npc.type == NPCID.GolemHeadFree)
            {
                Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/NPC_246").Value;
                int _frame = ((npc.ai[0] + 10) % 100 < 40 ? 2 : ((npc.ai[0] + 10) % 50 < 40 ? 4 : 0));
                Rectangle _rect = new Rectangle(0, _texture.Height / 6*_frame, _texture.Width, _texture.Height / 6);
                spriteBatch.Draw(_texture, npc.Center - Main.screenPosition, _rect, drawColor, npc.rotation, new Vector2(_texture.Width / 2, _texture.Height / 6 / 2), npc.scale, SpriteEffects.None, 0);

                if (npc.target >= 0)
                {
                    _texture = (Texture2D)ModContent.Request<Texture2D>("Tmodtober/Sprites/gravity_field").Value;
                    _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
                    spriteBatch.Draw(_texture, npc.Center - Main.screenPosition, _rect, Color.White * 0.25f, npc.rotation, new Vector2(_texture.Width / 2, _texture.Height / 2), 2.5f*2f, SpriteEffects.None, 0);
                }
                return false;
            }

            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }

    }
}
