using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober.Items
{
	public class TerraGolfClub : ModItem
	{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.UltimateGolfClub.hjson file.

        public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.GolfPlay;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.scale = 2.25f;

			Item.shoot = ModContent.ProjectileType<Projectiles.TerraGolfBlubProjectile>();
			Item.autoReuse = true;
			Item.channel = true;
			Item.noMelee = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GolfClubWedge);
			recipe.AddIngredient(ItemID.GolfClubDriver);
			recipe.AddIngredient(ItemID.GolfClubIron);
			recipe.AddIngredient(ItemID.BrokenHeroSword);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}