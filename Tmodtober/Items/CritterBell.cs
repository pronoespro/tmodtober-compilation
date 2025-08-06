using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober.Items
{
	public class CritterBell : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.FarmingHelper.hjson file.

		int reuseDelay;
		int afterUseDelay = 150;

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 36;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = ItemHoldStyleID.HoldFront;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item35;
			Item.autoReuse = true;
			Item.useTurn = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Bell);
			recipe.AddIngredient(ItemID.Hay,10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

        public override bool AltFunctionUse(Player player)
        {
			return true;
        }

        public override bool CanUseItem(Player player)
		{
            if (reuseDelay > 0){
				return false;
            }

			bool teleportFarmAnimals = player.altFunctionUse!=2;

			FarmAnimalNPC _animal;
			for (int i = 0; i < Main.maxNPCs; i++)
            {
				if(Main.npc[i].active && Main.npc[i].CountsAsACritter)
				{
					_animal = Main.npc[i].GetGlobalNPC<FarmAnimalNPC>();
					if (_animal.isFarmAnimal == teleportFarmAnimals)
					{
						Main.npc[i].Teleport(player.Center + new Vector2(Main.rand.NextFloat(-20, 20), -10), TeleportationStyleID.ShimmerTownNPCTransform);
						reuseDelay = afterUseDelay;
					}
                }
            }

			return true;
        }

        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);

			reuseDelay--;

			Item.color = Color.Lerp(Color.White,Color.LightBlue,Math.Max(0,(float)reuseDelay/ afterUseDelay));
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            base.HoldStyle(player, heldItemFrame);
			player.itemLocation -= new Vector2((heldItemFrame.Width-7)*player.direction,-heldItemFrame.Height/20);

        }

    }
}