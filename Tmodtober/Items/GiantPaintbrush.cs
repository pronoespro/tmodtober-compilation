using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Tmodtober.Projectiles;

namespace Tmodtober.Items
{
	public class GiantPaintbrush : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.PaintbrushUpgrade.hjson file.

		private byte curPaint;

		public override void SetDefaults()
		{
			Item.damage = 50;
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

            Item.shoot = ModContent.ProjectileType<PaintProjectile>();
			Item.shootSpeed = 15f;

        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Paintbrush);
			recipe.AddIngredient(ItemID.BlackPaint,10);
			recipe.AddIngredient(ItemID.WhitePaint,10);
			recipe.AddTile(TileID.DyeVat);
			recipe.Register();
		}

        public override bool CanUseItem(Player player)
		{
			curPaint = PaintID.None;
			Item _paint = player.FindPaintOrCoating();
			if (_paint != null)
			{
				curPaint = _paint.paint;
				player.ConsumeItem(_paint.type, includeVoidBag: true);
			}

			Item.color = ColourHelper.ConvertCurPaintToColor(curPaint);

			return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

			for (int i = -2; i < 2; i++)
			{
				int _shotProj =Projectile.NewProjectile(source, position, velocity.RotatedBy(i*MathHelper.PiOver4/2f),type,damage,knockback,player.whoAmI);
				PaintProjectile _proj = (PaintProjectile)Main.projectile[_shotProj].ModProjectile;
				_proj.SetCurrentPaint(curPaint);
			}

			return false;
        }
    }
}