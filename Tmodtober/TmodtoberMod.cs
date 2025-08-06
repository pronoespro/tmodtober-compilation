using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Tmodtober
{
	public class TmodtoberMod : Mod
    {
        public static ModKeybind TriggerPowersKeybind;
        public static ModKeybind SingKeybind;
        public static ModKeybind TriggerBlessing;
        public static ModKeybind mightTriggerKeybing;


        public static int PlayerInvisibilityLegsTexture, PlayerInvisibilityHeadTexture, PlayerInvisibilityFaceTexture, PlayerInvisibilityTorsoTexture;

        public static int PlayerFlameTexture;

        public static int PartyChance;

        public static TmodtoberMod Instance;

        public override void Load()
        {
            base.Load();
            Instance = this;
            SingKeybind = KeybindLoader.RegisterKeybind(this, "Sing", "Q");

            TriggerPowersKeybind = KeybindLoader.RegisterKeybind(this, "Trigger Phantom Powers (if available)", "Q");

            PlayerInvisibilityTorsoTexture = EquipLoader.AddEquipTexture(this, "Tmodtober/PlayerSprites/invisibleTorso", EquipType.Body, name: "flameBody");
            PlayerInvisibilityLegsTexture = EquipLoader.AddEquipTexture(this, "Tmodtober/PlayerSprites/InvisibleLegs", EquipType.Legs, name: "invisibleHead");
            PlayerInvisibilityFaceTexture = EquipLoader.AddEquipTexture(this, "Tmodtober/PlayerSprites/InvisibleLegs", EquipType.Face, name: "invisibleFace");
            PlayerInvisibilityHeadTexture = EquipLoader.AddEquipTexture(this, "Tmodtober/PlayerSprites/InvisibleLegs", EquipType.Head, name: "invisibleLegs");

            PlayerFlameTexture = EquipLoader.AddEquipTexture(this, "Tmodtober/PlayerSprites/flameTorso", EquipType.Body, name: "flameBody");
            TriggerBlessing = KeybindLoader.RegisterKeybind(this, "Trigger Blessing (if available)", "Q");
            mightTriggerKeybing = KeybindLoader.RegisterKeybind(this, "Terraria Trigger", "G");

        }

        public override void Unload()
        {
            Instance = null;
            SingKeybind = null;
            TriggerPowersKeybind = null;
            TriggerBlessing = null;
            mightTriggerKeybing = null;

        }


        public void AddWeaknessToEnemy(NPC npc, string name, Vector2 position, float range, int type, bool _scaleWithNPC = false)
        {
            BossWeaknessOverride weaknessNPC = npc.GetGlobalNPC<BossWeaknessOverride>();

            if (weaknessNPC.weakSpots == null)
            {
                weaknessNPC.weakSpots = new List<WeakSpot>();
            }

            WeakSpot _newSpot = new WeakSpot(name, position, range, (WeakSpotType)MathHelper.Clamp(type, 0, 4), _scaleWithNPC);
            weaknessNPC.weakSpots.Add(_newSpot);
        }

    }
}