using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace Tmodtober.Items
{
    public class StarBullet_Item:ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

		public override void SetDefaults()
		{
			Item.damage = 18; // The damage for projectiles isn't actually 12, it actually is the damage combined with the projectile and the item together.
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true; // This marks the item as consumable, making it automatically be consumed when it's used as ammunition, or something else, if possible.
			Item.knockBack = 3f;
			Item.value = 10;
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<Projectiles.StarBullet>(); // The projectile that weapons fire when using this item as ammunition.
			Item.shootSpeed = 18f; // The speed of the projectile.
			Item.ammo = AmmoID.Bullet; // The ammo class this ammo belongs to.
		}

        public override void AddRecipes()
        {
			CreateRecipe(200)
				.AddIngredient(ModContent.ItemType<Heavenstone_Bar>())
				.AddIngredient(ItemID.EmptyBullet,200)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			CreateRecipe(50)
				.AddIngredient(ItemID.FallenStar,10)
				.AddIngredient(ItemID.EmptyBullet, 50)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

    }
}
