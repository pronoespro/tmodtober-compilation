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

namespace Tmodtober.Items
{
    public class PlayerPartyPresent:ModItem
    {

        private int npcGivenPresent;

        public void RecievePresent(int _npcType)
        {
            npcGivenPresent = _npcType;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height= 32;
            Item.rare = ItemRarityID.Pink;
            Item.holdStyle = ItemHoldStyleID.HoldHeavy;
            Item.useStyle = ItemUseStyleID.None;

            Item.value = 0;

        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            int _givenItem;
            int _minStack=1,_maxStack = 0;
            string _giftText = "";
            switch (npcGivenPresent)
            {
                default:
                    _givenItem = ItemID.CopperCoin;
                    _minStack = 25;
                    _maxStack = 100;
                    break;
                case NPCID.Merchant:
                    _givenItem = ItemID.LesserHealingPotion;
                    _maxStack = 5;
                    _giftText = "Happy birthday, wish you wealth and prosperity (mainly so you can buy more stuff)";
                    break;
                case NPCID.Demolitionist:
                    _givenItem = ItemID.Grenade;
                    _minStack = 2;
                    _maxStack = 5;
                    _giftText = "Happy born day! Hope this explosive surprise doesn't blow up before you open it.";
                    break;
                case NPCID.DyeTrader:
                    _givenItem = ItemID.PinkGelDye;
                    _minStack = 3;
                    _maxStack = 10;
                    _giftText = "Greetings and happy birthday, hope this dye helps you look fabulous on this day";
                    break;
                case NPCID.BestiaryGirl:
                    _givenItem = ItemID.BunnyEars;
                    _giftText = "HAPPY BIRTHDAY!!! Wish you the best XOXO";
                    break;
                case NPCID.Dryad:
                    _givenItem = 4043;
                    _giftText = "I don't understand birthdays, but I do understand celebrating milestones, so happy birthday";
                    break;
                case NPCID.Painter:
                    _givenItem = ItemID.PinkPaint;
                    _minStack = 25;
                    _maxStack = 99;
                    _giftText = "Wish you a colourfull birthday, and a year filled with pretty vistas";
                    break;
                case NPCID.Golfer:
                    _givenItem = ItemID.GolfBallDyedPink;
                    _giftText = "Hey there, wish you a happy and fun birthday";
                    break;
                case NPCID.ArmsDealer:
                    _givenItem = ItemID.PartyBullet;
                    _maxStack = 100;
                    _minStack = 25;
                    _giftText = "Hey there, hope you have an action filled birthday and rest of your year";
                    break;
                case NPCID.DD2Bartender:
                    _givenItem = ItemID.Ale;
                    _maxStack = 10;
                    _minStack = 3;
                    _giftText = "Check in with me, and do your... I mean, happy birthday";
                    break;
                case NPCID.Stylist:
                    _givenItem = ItemID.PartyHairDye;
                    _giftText = "Happy birthday, cutie. Enjoy the hair dye! <3 <3 <3";
                    break;
                case NPCID.GoblinTinkerer:
                    _givenItem = ItemID.SpikyBall;
                    _maxStack = 1000;
                    _minStack = 100;
                    _giftText = "Happy birthday, it is tradition to give spiked balls among goblins... I have too many, so you can have them";
                    break;
                case NPCID.WitchDoctor:
                    _givenItem = ItemID.JungleWaterFountain;
                    _maxStack = 3;
                    _giftText = "Natural selection has not taken you yet, so we can all be happy about it";
                    break;
                case NPCID.Clothier:
                    _givenItem = ItemID.PartyBalloonAnimal;
                    _giftText = "Happy birthday! I'm really gratefull you helped me be happy and free again, I hope this helps you be happy too";
                    break;
                case NPCID.Mechanic:
                    _givenItem = ItemID.RainbowTorch;
                    _maxStack = 99;
                    _minStack = 9;
                    _giftText = "Happy birthday! I hope you can use this colourfull torches on your next contraption! (hint: it could be a rainbow cannon that actually shoots ;D )";
                    break;
                case NPCID.PartyGirl:
                    _givenItem = ItemID.PartyGirlGrenade;
                    _maxStack = 999;
                    _minStack = 99;
                    _giftText = "HAPPY BIRTHDAY!!! Hope you enjoy the party I'm preparing for you, and YOU CAN'T ESCAPE IT JUST BECAUSE THERE'S A GIANT EYEBALL ATTACKING OR SOMETHING, alright?";
                    break;
                case NPCID.Wizard:
                    _givenItem =ItemID.WizardsHat;
                    {
                        string _otherNPCName = "dear Timmie";
                        if (NPC.AnyNPCs(NPCID.Guide))
                        {
                            int _guideId = NPC.FindFirstNPC(NPCID.Guide);
                            _otherNPCName = Main.npc[_guideId].GivenOrTypeName;
                        }
                        else if (NPC.AnyNPCs(NPCID.Nurse))
                        {
                            int _nurseId = NPC.FindFirstNPC(NPCID.Nurse);
                            _otherNPCName = Main.npc[_nurseId].GivenOrTypeName;
                        }
                        else if (NPC.AnyNPCs(NPCID.Dryad))
                        {
                            int _dryadId = NPC.FindFirstNPC(NPCID.Dryad);
                            _otherNPCName = Main.npc[_dryadId].GivenOrTypeName;
                        }
                        else if (NPC.AnyNPCs(NPCID.GoblinTinkerer))
                        {
                            int _goblinId = NPC.FindFirstNPC(NPCID.GoblinTinkerer);
                            _otherNPCName = Main.npc[_goblinId].GivenOrTypeName;
                        }
                        _giftText = "I wish upon you a magic birthday " + _otherNPCName;
                    }
                    break;
                case NPCID.TaxCollector:
                    _givenItem = ItemID.Coal;
                    _giftText = "Take this and scram";
                    break;
                case NPCID.Truffle:
                    _givenItem = ItemID.DarkBlueSolution;
                    _maxStack = 99;
                    _minStack = 10;
                    if (NPC.AnyNPCs(NPCID.PartyGirl))
                    {
                        int _partygirl = NPC.FindFirstNPC(NPCID.PartyGirl);
                        _giftText = "Happy branchday! (please tell "+Main.npc[_partygirl].GivenOrTypeName+" I said it right, I really tried)";
                    }
                    else
                    {
                        _giftText = "Happy sproutday!";
                    }
                    break;
                case NPCID.Pirate:
                    _givenItem = ItemID.ExplosiveBunny;
                    _maxStack = 10;
                    _minStack =2;
                    _giftText = "Ahoy! Ye beard startin' ta grow, har har!";
                    break;
                case NPCID.Steampunker:
                    _givenItem = ItemID.SteampunkLantern;
                    _giftText = "Happy birthday! I hope this lantern will accompany you during late blueprint making sessions like it did for me";
                    break;
                case NPCID.Cyborg:
                    _givenItem = ItemID.FlaskofNanites;
                    _maxStack = 3;
                    _giftText = "H4PPY 81RTHD4Y!! Let's cheer with this nice beverage, since you seem to be the only one who can drink it besides myself";
                    break;
                case NPCID.SantaClaus:
                    _givenItem = ItemID.SantaHat;
                    _giftText = "Ho ho ho, merry daylight and a happy birthday! I'm not used to giving presents in the day, but it still brings me joy!";
                    break;
                case NPCID.TravellingMerchant:
                    _givenItem =ItemID.HeartHairpin;
                    _giftText = "Happy birthday, I hope this small gift will incentivize you to come buy more from me";
                    break;
                case NPCID.Princess:
                    _givenItem = ItemID.UglySweater;
                    _giftText = "Happy birthday! I was told that gifts that take efford are the best, so I knitted you this swater, enjoy!";
                    break;

            }

            if (_giftText != "")
            {
                Main.NewText("There's a note that says: ''"+_giftText+"''", Color.Pink);
            }

            EntitySource_Parent _s = new EntitySource_Parent(player);
            Item.NewItem(_s, player.Center, _givenItem,Stack:(_maxStack==0?1:Main.rand.Next(_minStack,_maxStack)), noGrabDelay: true, reverseLookup: true);
        }

    }
}
