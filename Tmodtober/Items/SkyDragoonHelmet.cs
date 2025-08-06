using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Tmodtober.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class SkyDragoonHelmet:ModItem
	{
		public static int AdditiveGenericDamageBonus=10;
		public static LocalizedText SetBonusText { get; private set; }

		public override void SetStaticDefaults()
		{
			// If your head equipment should draw hair while drawn, use one of the following:
			// ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
			// ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
			// ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
			// ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;

			SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs(AdditiveGenericDamageBonus);
		}

		public override void SetDefaults()
        {
            Item.defense = 8;
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Orange; // The rarity of the item
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<SkyDragoonChestpiece>() && legs.type == ModContent.ItemType<SkyDragoonLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = SetBonusText.Value; // This is the setbonus tooltip: "Increases dealt damage by 20%"
			player.GetDamage(DamageClass.Melee) += 1;
			player.buffImmune[BuffID.OnFire] = true;
			player.noFallDmg = true;
			player.wingRunAccelerationMult *= 2f;
			player.jumpBoost = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<Heavenstone_Bar>(10)
				.AddTile(TileID.Hellforge)
				.Register();
		}
	}
}
