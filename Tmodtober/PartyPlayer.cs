using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
using Tmodtober.Items;
using Terraria.GameInput;

namespace Tmodtober
{
    public class PartyPlayer : ModPlayer
    {

        public bool IsPlayerBirthday;
        private bool wasNight;
        private List<int> _npcsGivenPresents;

        private int lastSavedNPCSGivenPresentCount = 0;

        public override void ModifyLuck(ref float luck)
        {
            base.ModifyLuck(ref luck);
            if (IsPlayerBirthday)
            {
                luck *= 1.5f;
            }
        }

        public override void ModifyNursePrice(NPC nurse, int health, bool removeDebuffs, ref int price)
        {
            base.ModifyNursePrice(nurse, health, removeDebuffs, ref price);
            if (IsPlayerBirthday)
            {
                price = price * 3 / 2;
            }
        }

        public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (IsPlayerBirthday)
            {
                if (_npcsGivenPresents == null)
                {
                    _npcsGivenPresents = new List<int>();
                }

                if (!_npcsGivenPresents.Contains(vendor.type))
                {
                    EntitySource_Parent _s = new EntitySource_Parent(vendor);
                    int _presentItem = Item.NewItem(_s, Player.Center, ModContent.ItemType<PlayerPartyPresent>());
                    PlayerPartyPresent _present = (PlayerPartyPresent)Main.item[_presentItem].ModItem;

                    _present.RecievePresent(vendor.type);

                    _npcsGivenPresents.Add(vendor.type);
                }
            }
            base.PostBuyItem(vendor, shopInventory, item);
        }

        public override void PostSellItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (IsPlayerBirthday)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(Player.position, Player.width, Player.height, DustID.Confetti, Main.rand.Next(-5, 5), Main.rand.Next(-10, -2), Scale: Main.rand.Next(1, 5) / 2f);
                }
            }
            base.PostSellItem(vendor, shopInventory, item);
        }

        public override void PreUpdate()
        {
            if (wasNight && Main.dayTime && Main.rand.Next(TmodtoberMod.PartyChance)==0 && NPC.AnyNPCs(NPCID.PartyGirl)){
                IsPlayerBirthday = true;
                Main.NewText("Today is your birthday!! HAPPY BIRTHDAY!!! <3", Color.HotPink);
            }
            if (!Main.dayTime )
            {
                if (IsPlayerBirthday)
                {
                    Main.NewText("Hope you had a great birthday", Color.HotPink);
                    IsPlayerBirthday = false;
                    if (_npcsGivenPresents == null)
                    {
                        _npcsGivenPresents = new List<int>();
                    }
                    _npcsGivenPresents.Clear();
                }
            }
            wasNight = !Main.dayTime;
        }

        public const string PLAYER_BIRTHDAY_SAVE_KEY = "is_player_birthday_today";
        public const string NPC_GIVEN_PRESENTS_COUNT_SAVE_KEY = "npcs_given_present_count";
        public const string NPC_GIVEN_PRESENT_SAVE_KEY = "npcs_given_present_";

        public override void SaveData(TagCompound tag)
        {
            for (int i = 0; i < lastSavedNPCSGivenPresentCount; i++)
            {
                if (tag.ContainsKey(NPC_GIVEN_PRESENT_SAVE_KEY + i.ToString()))
                {
                    tag.Remove(NPC_GIVEN_PRESENT_SAVE_KEY + i.ToString());
                }
            }

            if (IsPlayerBirthday)
            {
                tag.Add(PLAYER_BIRTHDAY_SAVE_KEY, IsPlayerBirthday);
            }

            if (_npcsGivenPresents != null)
            {
                tag.Add(NPC_GIVEN_PRESENTS_COUNT_SAVE_KEY, _npcsGivenPresents.Count);

                for (int i = 0; i < _npcsGivenPresents.Count; i++)
                {
                    tag.Add(NPC_GIVEN_PRESENT_SAVE_KEY + i.ToString(), _npcsGivenPresents[i]);
                }
            }
            base.SaveData(tag);

        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            IsPlayerBirthday = tag.ContainsKey(PLAYER_BIRTHDAY_SAVE_KEY);

            _npcsGivenPresents = new List<int>();
            if (tag.ContainsKey(NPC_GIVEN_PRESENTS_COUNT_SAVE_KEY))
            {
                lastSavedNPCSGivenPresentCount = (int)tag[NPC_GIVEN_PRESENTS_COUNT_SAVE_KEY];
                string _curKey;

                for (int i = 0; i < lastSavedNPCSGivenPresentCount; i++)
                {
                    _curKey = NPC_GIVEN_PRESENT_SAVE_KEY + i.ToString();
                    if (tag.ContainsKey(_curKey))
                    {
                        _npcsGivenPresents.Add((int)(tag[_curKey]));
                    }
                }
            }
        }

    }
}
