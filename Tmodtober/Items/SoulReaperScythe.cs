using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober.Items
{
	public class SoulReaperScythe : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.DeathScytheUpgrade.hjson file.

		int combo;

		public override void SetDefaults()
		{
			Item.scale = 2.5f;
			Item.damage = 150;
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
			Item.useTurn = true;

        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < combo*combo; i++)
			{
				int _proj = Projectile.NewProjectile(source, position, velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) , type, damage, knockback, player.whoAmI);
			}

			for (int i = 0; i < combo; i++) {
				int _proj=Projectile.NewProjectile(source, position, velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4/2,MathHelper.PiOver4/2))*1.5f,type,damage,knockback,player.whoAmI);
				Main.projectile[_proj].scale = 1.5f;
			}
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override bool CanUseItem(Player player)
		{
			DeathScythePlayer _soulReaper = player.GetModPlayer<DeathScythePlayer>();
			if (_soulReaper.GetIsSoulDashing())
			{
				return false;
			}

			combo++;
            if (combo == 5)
			{
				_soulReaper.SetSoulDashing(Item.useTime * 5);
				combo = 0;
            }
			return base.CanUseItem(player);
        }

        public override bool? UseItem(Player player)
        {
			return true;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DeathSickle);
			recipe.AddIngredient(ItemID.SoulofMight,20);
			recipe.AddIngredient(ItemID.SoulofNight,20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

	}
}