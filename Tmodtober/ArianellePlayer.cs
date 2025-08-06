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
using Terraria.Graphics.CameraModifiers;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;

namespace Tmodtober
{
    public class ArianellePlayer:ModPlayer
    {

        public int cutsceneTime;
        private bool showChoise;

        private bool wasPressingRight, wasPressingLeft;

        public override void PreUpdateMovement()
        {
            if (cutsceneTime >= 0)
            {
                Player.velocity = Vector2.Zero;
                Player.TryInterruptingItemUsage();
                if (Player.heldProj >= 0 && Main.projectile[Player.heldProj].active)
                {
                    Main.projectile[Player.heldProj].Kill();
                }
            }
        }

        public override void PostUpdate()
        {
            cutsceneTime--;
            if (cutsceneTime < 0)
            {
                showChoise = false;
            }
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (IsMidCutscene())
            {
                return true;
            }
            return false;
        }

        public override bool CanUseItem(Item item)
        {
            return cutsceneTime<0;
        }

        public void SetMidCutscene()
        {
            cutsceneTime = 3;
        }

        public bool IsMidCutscene()
        {
            return cutsceneTime >= 0;
        }

        public void SetShowingChoices(bool _show)
        {
            showChoise = _show;
        }

        public override void ModifyScreenPosition()
        {

            if (IsMidCutscene())
            {
                int _bunny = NPC.FindFirstNPC(ModContent.NPCType<NPCs.Bunnygirl_Default>());
                if (_bunny >= 0)
                {
                    NPC _bunnyNPC = Main.npc[_bunny];

                    Main.screenPosition += (_bunnyNPC.Center - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) - Main.screenPosition) * 0.35f;
                }
            }

        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (triggersSet.MouseRight)
            {
                if (!wasPressingRight){
                    ArianelleDeffeatedSystem.SetArianelChoise(ArianelChoiseSelection.noMoreKilling);
                }
                wasPressingRight = true;
            }
            else
            {
                wasPressingRight = false;
            }
            if (triggersSet.MouseLeft)
            {
                if (!wasPressingLeft)
                {
                    ArianelleDeffeatedSystem.SetArianelChoise(ArianelChoiseSelection.murderer);
                }
                wasPressingLeft = true;
            }
            else
            {
                wasPressingLeft = false;
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (showChoise)
            {
                Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>("Tmodtober/ExtraSprites/Choise_background");
                Rectangle _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
                Vector2 _pivot = new Vector2(_texture.Width / 2, _texture.Height / 2);

                Main.spriteBatch.Draw(_texture, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), _rect, Color.White * 0.35f, 0, _pivot, 2.5f, SpriteEffects.None, 0);

                _texture = (Texture2D)ModContent.Request<Texture2D>("Tmodtober/ExtraSprites/Choise_foreground");
                Main.spriteBatch.Draw(_texture, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), _rect, Color.White * 0.9f, 0, _pivot, 2.5f, SpriteEffects.None, 0);
            }

            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }

    }
}
