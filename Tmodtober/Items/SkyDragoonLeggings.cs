using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Tmodtober.Items
{
    [AutoloadEquip(EquipType.Legs)]
    public class SkyDragoonLeggings:ModItem
	{
		public static readonly int MoveSpeedBonus = 5;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeedBonus);

		public override void SetDefaults()
		{
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 8; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += MoveSpeedBonus / 100f; 
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<Heavenstone_Bar>(15)
				.AddTile(TileID.Hellforge)
				.Register();
		}
	}
}
