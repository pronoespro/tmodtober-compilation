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
    class Paulatena_Bow : ModItem
    {

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MoltenFury);
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
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.JestersArrow, damage, knockback, player.whoAmI);
                return false;
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

    }
}
