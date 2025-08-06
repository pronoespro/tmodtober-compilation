using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace Tmodtober.Items
{
	public class HolyCrossBlade : ModItem
	{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Heavenstone.hjson file.

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6.5f;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.scale = 1.5f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.RainCloud, 10);
			recipe.AddIngredient(ItemID.Cloud, 10);
			recipe.AddIngredient(ModContent.ItemType<Heavenstone_Bar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			EntitySource_Parent _s = new EntitySource_Parent(player);
			Projectile.NewProjectile(_s, target.Center + new Vector2(target.velocity.X * 12, -550), new Vector2(0, 25), ProjectileID.Starfury, damageDone, Item.knockBack);
        }
    }
}