using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober.Items
{

    public class GranitePuncher_Item:ModItem
    {


        int curMagazine = 0;

        Texture2D useTexture;
        Player owner;

        public override void SetDefaults()
        {

            Item.useAmmo = AmmoID.Bullet;

            Item.width = 32;
            Item.height = 16;
            Item.rare = ItemRarityID.Green;

            Item.damage = 30;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 4f;
            Item.noMelee = true;
            Item.shootSpeed = 12f;
            Item.UseSound = SoundID.Item11;
            Item.useStyle=ItemUseStyleID.Shoot;
            Item.value=100;


            useTexture = (Texture2D)ModContent.Request<Texture2D>("Tmodtober/Items/GranitePuncher");
        }

        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
            owner = player;
        }

        public override bool CanUseItem(Player player)
        {

            if (curMagazine < 7)
            {
                Item.useTime = 10;
                Item.useAnimation = 10;

                Item.shoot = ModContent.ProjectileType<Projectiles.GraniteBullet>();
            } else if (curMagazine < 9)
            {
                Item.useTime = 40;
                Item.useAnimation = 40;
                Item.shoot = ModContent.ProjectileType<Projectiles.GraniteShockwave>();
            }else{
                Item.useTime = 60;
                Item.useAnimation = 60;
                Item.shoot = ModContent.ProjectileType<Projectiles.GraniteShockwave>();
            }

            curMagazine = (curMagazine + 1) % 10;

            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            Projectile.NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI);

            if (curMagazine>=8)
            {
                int _bulletAmmount = Main.rand.Next(2, 5);
                for (int i = 0; i < _bulletAmmount; i++)
                {
                    Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.PiOver2*0.2f*Main.rand.Next(-20,20)/20f), ModContent.ProjectileType<Projectiles.GraniteBullet>(), damage, knockback, player.whoAmI);
                }
            }
            if (curMagazine == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.PiOver2 * (i-2f)/4f), ModContent.ProjectileType<Projectiles.GraniteShockwave>(), damage, knockback, player.whoAmI);
                }
            }

            return false;
        }

        public override void HoldItemFrame(Player player)
        {
            base.HoldItemFrame(player);
        }

        public override void AddRecipes()
        {
            Recipe _r = Recipe.Create(Type);

            _r.AddIngredient(ItemID.Granite, 10);
            _r.AddIngredient(ItemID.OnyxBlaster);
            _r.AddIngredient(ItemID.SoulofNight, 2);
            
            _r.Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }

        /*
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (useTexture != null && owner != null)
            {
                Rectangle _rect = new Rectangle(0, 16 * curMagazine, 32, 16);
                spriteBatch.Draw(useTexture, owner.HandPosition.Value - Main.screenPosition, _rect, lightColor, rotation, new Vector2(4, 6), scale, owner.direction >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                return false;
            }

            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (useTexture != null && owner != null)
            {
                Rectangle _rect = new Rectangle(0, 16 * (curMagazine%4), 32, 16);
                spriteBatch.Draw(useTexture, owner.HandPosition.Value - Main.screenPosition, _rect, drawColor, 0f, new Vector2(4, 6), scale, owner.direction >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                return false;
            }
            return false;
        }
        */
    }
}
