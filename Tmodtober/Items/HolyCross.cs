using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober.Items
{
	public class HolyCross : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.CrossWeapon.hjson file.

		int combo;
		int lastUsed;

		public override void SetDefaults()
		{
			Item.damage = 75;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			Item.shoot = ModContent.ProjectileType<Projectiles.HolyCross_projectile>();
			Item.shootSpeed = 15;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrossNecklace);
			recipe.AddIngredient(ItemID.PixieDust,10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

        public override bool CanUseItem(Player player)
        {
			lastUsed = Item.useTime + 10;

            switch (combo)
            {
				default:
					Item.useStyle = ItemUseStyleID.Swing;
					break;
				case 3:
				case 5:
					Item.useStyle = ItemUseStyleID.Shoot;
					break;
				case 6:
					Item.useStyle = ItemUseStyleID.HoldUp;
					break;
            }

			combo=(combo+1)%7;

			return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            switch (combo)
            {
				default:
					break;
				case 3:
				case 5:
					for(int i = -2; i <= 2; i++)
                    {
                        if (i != 0)
                        {
							int _proj=Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.PiOver4 * i), Item.shoot, damage, knockback, player.whoAmI);
							Main.projectile[_proj].scale = 1.5f;
                        }
                    }
					break;
				case 6:
					for (int i = 0; i <= 20; i++)
					{
						int _proj=Projectile.NewProjectile(source, position, new Vector2(Item.shootSpeed,0).RotatedBy(MathHelper.TwoPi/20f * i), Item.shoot, damage, knockback, player.whoAmI);
						Main.projectile[_proj].scale = 2f;
					}
					return false;
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override bool? UseItem(Player player)
        {
			return true;
        }

        public override void UpdateInventory(Player player)
        {
			if (combo != 0)
			{
				lastUsed -= 1;
				if (lastUsed < 0)
				{
					combo = 0;
				}
			}
            base.UpdateInventory(player);
        }

    }
}