using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.UI;

namespace Tmodtober
{

    public enum ArianelChoiseSelection
    {
        none,
        noMoreKilling,
        murderer
    }

    public class ArianelleDeffeatedSystem:ModSystem
    {

        public const string ArianelDefeatSaveKey = "ArianelDefeated";
        public const string ArianelSummonSaveKey = "ArianelSummoned";
        public const string BunniesKilledSaveKey= "ArianelModBunniesKilled";

        public static bool arianelSummoned,downedArianel;
        public static int bunniesKilled = 0;

        private static ArianelChoiseSelection curPlayerChoise;

        public override void SaveWorldData(TagCompound tag)
        {
            base.SaveWorldData(tag);

            SaveToTag(ref tag, downedArianel, ArianelDefeatSaveKey);
            SaveToTag(ref tag, arianelSummoned, ArianelSummonSaveKey);
            tag.Add(BunniesKilledSaveKey, bunniesKilled);
        }

        public void SaveToTag(ref TagCompound _tag, bool _var,string _saveKey)
        {
            if (_var){
                _tag.Add(_saveKey, true);
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            base.LoadWorldData(tag);

            downedArianel = tag.ContainsKey(ArianelDefeatSaveKey);
            arianelSummoned = tag.ContainsKey(ArianelSummonSaveKey);
            bunniesKilled = (tag.ContainsKey(BunniesKilledSaveKey)) ? tag.GetAsInt(BunniesKilledSaveKey) : 0;
        }

        public static void ResetChoise()
        {
            curPlayerChoise = ArianelChoiseSelection.none;
        }

        public static void SetArianelChoise(ArianelChoiseSelection _choise)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bunnygirl_Default>()))
            {
                if (curPlayerChoise == ArianelChoiseSelection.none)
                {
                    curPlayerChoise = _choise;
                }
            }
            else
            {
                curPlayerChoise = ArianelChoiseSelection.none;
            }
        }

        public static ArianelChoiseSelection GetSelection()
        {
            return curPlayerChoise;
        }

        public static void DoGeneralTextMessage(string _text, Vector2 _pos,Color _textColor)
        {
            AdvancedPopupRequest _request = new AdvancedPopupRequest();

            _request.Text = _text;

            _request.DurationInFrames = 240;
            _request.Color =_textColor;

            PopupText.NewText(_request, _pos);

            Main.NewText(_request.Text, _textColor);
        }

        private string[] _layersToRemove = new string[]
        {
            "Vanilla: Mouse Item / NPC Head",
            "Vanilla: Entity Markers",
            "Vanilla: Inventory",
            "Vanilla: Hotbar",
            "Vanilla: Radial Hotbars",
            "Vanilla: Death Text",
            "Vanilla: Mouse Over",
            "Vanilla: Interact Item Icon",
            "Vanilla: Map / Minimap"
        };

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            ArianellePlayer _ArPlayer = Main.player[Main.myPlayer].GetModPlayer<ArianellePlayer>();
            if (_ArPlayer.IsMidCutscene())
            {
                int layerIndex;
                foreach (String _index in _layersToRemove)
                {
                    layerIndex = layers.FindIndex(layer => layer.Name == _index);
                    if (layerIndex != -1)
                    {
                        layers.RemoveAt(layerIndex);
                    }
                }
            }
            base.ModifyInterfaceLayers(layers);
        }

    }
}
