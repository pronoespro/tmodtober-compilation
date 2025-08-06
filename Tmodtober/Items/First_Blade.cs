using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Tmodtober.Items
{
    public class First_Blade:ModItem
    {

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Handgun);

            Item.useTime = (int)(Item.useTime * 0.8f);
            Item.useAnimation= (int)(Item.useAnimation* 0.8f);
            Item.damage-=7;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.Bullet)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.PartyBullet, damage, knockback, player.whoAmI);
                return false;
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(player, target, hit, damageDone);
            target.velocity = target.velocity + new Vector2(0, -20);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Heavenstone_Bar>(15)
                .AddIngredient(ItemID.Handgun)
                .AddTile(TileID.Anvils)
                .Register();
        }

    }
}
