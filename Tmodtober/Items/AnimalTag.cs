using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober.Items
{
    public class AnimalTag:ModItem
    {

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 36;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = ItemHoldStyleID.HoldFront;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;

			Item.maxStack = 99999;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Book);
			recipe.AddIngredient(ItemID.Hay, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpellTome);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
		}

        public override bool CanUseItem(Player player)
        {

			for(int i = 0; i < Main.maxNPCs; i++)
            {
				if (Main.npc[i].active && Vector2.DistanceSquared(Main.MouseWorld, Main.npc[i].Center) < 50 * 50 && Main.npc[i].GetGlobalNPC<FarmAnimalNPC>().TransformIntoFarmAnimal(Main.npc[i]))
				{
					if (player.HeldItem != null)
					{
						Item _item = player.HeldItem;
						_item.stack -= 1;
						if (_item.stack <= 0)
						{
							_item.TurnToAir();
						}
					}else{
						player.ConsumeItem(Item.type);
					}
					Item.autoReuse = false;
					return true;
				}
            }

            return base.CanUseItem(player);
        }

    }
}
