using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace Tmodtober.NPCs
{
    [AutoloadHead]
    public class Sorceress:ModNPC
    {
        public const string ShopName = "Shop";
        public int NumberOfTimesTalkedTo = 0;

        private static int ShimmerHeadIndex;
        private static Profiles.StackedNPCProfile NPCProfile;

        public override void Load()
        {
            ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 21;
            NPCID.Sets.ExtraFramesCount[Type] = 2;

            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 2;

            NPCID.Sets.AttackTime[Type] = 90;
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4;

            NPCID.Sets.ShimmerTownTransform[NPC.type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Like) // Example Person prefers the forest.
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike) // Example Person dislikes the snow
                .SetNPCAffection(NPCID.Merchant, AffectionLevel.Love) // Loves living near the dryad.
                .SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Like) // Likes living near the guide.
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Dislike) // Dislikes living near the merchant.
                .SetNPCAffection(NPCID.Wizard, AffectionLevel.Hate) // Hates living near the demolitionist.
            ;

            NPCProfile = new Profiles.StackedNPCProfile(
                new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
                new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex, Texture + "_Shimmer_Party")
            );
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true; // Sets NPC to be a Town NPC
            NPC.friendly = true; // NPC Will not attack player
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 100;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Dryad;
        }
        public override bool CanTownNPCSpawn(int numTownNPCs)
        { 
            return Main.hardMode;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("A sorceress of the far away land of ''Deande'', with some pyromaniac tendences."),

				// You can add multiple elements if you really wanted to
				// You can also use localization keys (see Localization/en-US.lang)
				new FlavorTextBestiaryInfoElement("Mods.SorceressTownNPC.Bestiary.Sorceress")
            });
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>(){
                "Kairas",
                "Pyra",
                "Morgana"
            };
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
            if (partyGirl >= 0 && Main.rand.NextBool(4))
            {
                chat.Add(Language.GetTextValue("Mods.SorceressTownNPC.Dialogue.Sorceress.PartyGirlDialogue", Main.npc[partyGirl].GivenName));
            }
            int guide = NPC.FindFirstNPC(NPCID.Guide);
            if (guide >= 0 && Main.rand.NextBool(4))
            {
                chat.Add(Language.GetTextValue("Mods.SorceressTownNPC.Dialogue.Sorceress.GuideDialogue", Main.npc[guide].GivenName));
            }
            // These are things that the NPC has a chance of telling you when you talk to it.
            chat.Add(Language.GetTextValue("Mods.SorceressTownNPC.Dialogue.Sorceress.StandardDialogue1"));
            chat.Add(Language.GetTextValue("Mods.SorceressTownNPC.Dialogue.Sorceress.StandardDialogue2"));
            chat.Add(Language.GetTextValue("Mods.SorceressTownNPC.Dialogue.Sorceress.StandardDialogue3"));
            chat.Add(Language.GetTextValue("Mods.SorceressTownNPC.Dialogue.Sorceress.CommonDialogue"), 5.0);
            chat.Add(Language.GetTextValue("Mods.SorceressTownNPC.Dialogue.Sorceress.RareDialogue"), 0.1);

            NumberOfTimesTalkedTo++;
            if (NumberOfTimesTalkedTo >= 10)
            {
                //This counter is linked to a single instance of the NPC, so if ExamplePerson is killed, the counter will reset.
                chat.Add(Language.GetTextValue("Mods.SorceressTownNPC.Dialogue.Sorceress.TalkALot"));
            }

            if (NPC.IsShimmerVariant)
            {
                chat.Add(Language.GetTextValue("Mods.SorceressTownNPC.Dialogue.Sorceress.KairasDialogue1"));
                chat.Add(Language.GetTextValue("Mods.SorceressTownNPC.Dialogue.Sorceress.KairasDialogue2"));
                chat.Add(Language.GetTextValue("Mods.SorceressTownNPC.Dialogue.Sorceress.KairasDialogue3"));
            }

            return chat; // chat is implicitly cast to a string.
        }

        public override void SetChatButtons(ref string button, ref string button2)
        { // What the chat buttons are when you open up the chat UI
            button = Language.GetTextValue("LegacyInterface.28");
        }


        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = ShopName; // Name of the shop tab we want to open.
            }
        }

        // Not completely finished, but below is what the NPC will sell
        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName)
                .Add(ItemID.Flamelash)
                .Add(ItemID.WandofSparking)
                .Add(ItemID.WandofFrosting)
                .Add(ItemID.DiamondStaff)
                .Add(ItemID.ThunderStaff)
                .Add(ItemID.Vilethorn)
                .Add(ItemID.AquaScepter)
                .Add(ItemID.FlowerofFire)
                .Add(ItemID.FlaskofFire,Condition.IsNpcShimmered)
                .Add(ItemID.AngelStatue,Condition.IsNpcShimmered)
                .Add(ItemID.NimbusRod,Condition.IsNpcShimmered)
                .Add(ItemID.BlackAndWhiteDye,Condition.IsNpcShimmered);

            npcShop.Register(); // Name of this shop tab
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.WaterBucket,maximumDropped:3));
        }

        public override bool CanGoToStatue(bool toKingStatue) => true;



        public override void LoadData(TagCompound tag)
        {
            NumberOfTimesTalkedTo = tag.GetInt("numberOfTimesTalkedTo");
        }

        public override void SaveData(TagCompound tag)
        {
            tag["numberOfTimesTalkedTo"] = NumberOfTimesTalkedTo;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 100;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.Fireball;
            attackDelay = 2;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
            // SparklingBall is not affected by gravity, so gravityCorrection is left alone.
        }

        public override int? PickEmote(Player closestPlayer, List<int> emoteList, WorldUIAnchor otherAnchor)
        {
            // If the NPC is talking to the Demolitionist, it will be more likely to react with angry emote
            if (otherAnchor.entity is NPC { type: NPCID.Wizard })
            {
                int type = EmoteID.EmotionAnger;
                for (int i = 0; i < 4; i++)
                {
                    emoteList.Add(type);
                }
            }
            if (otherAnchor.entity is NPC { type: NPCID.DD2Bartender })
            {
                int type = EmoteID.EmoteSilly;
                for (int i = 0; i < 4; i++)
                {
                    emoteList.Add(type);
                }
            }

            for(int i = 0; i < 10; i++)
            {
                emoteList.Add(EmoteID.MiscFire);
            }

            // Use this or return null if you don't want to override the emote selection totally
            return base.PickEmote(closestPlayer, emoteList, otherAnchor);
        }
    }
}
