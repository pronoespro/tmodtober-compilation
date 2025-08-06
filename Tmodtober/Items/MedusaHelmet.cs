using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.UI;
using Microsoft.Xna.Framework;

namespace Tmodtober.Items
{
    class MedusaHelmet:ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.scale = 2f;

            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;

            Item.damage = 100;
            Item.knockBack = 1;
            Item.rare = ItemRarityID.LightRed;

            Item.shoot = ModContent.ProjectileType<Projectiles.MedusaSight>();
            Item.shootSpeed = 15;
            
            Item.noMelee = true;
            Item.holdStyle = ItemHoldStyleID.HoldUp;
            Item.DamageType = DamageClass.Magic;
            Item.UseSound = SoundID.Pixie;

            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.value = 10000;

        }

        public override void AddRecipes()
        {
            Recipe.Create(Type).
                AddIngredient(ItemID.MedusaHead).
                AddIngredient(ItemID.LunarBar,1).
                AddTile(TileID.WorkBenches).
                Register();

        }

        public override bool CanUseItem(Player player)
        {
            Main.NewText("Can use?");
            return player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.MedusaSight>()] <= 0;
        }

        public override bool? UseItem(Player player)
        {
            Main.NewText("Can use?");
            if (player.whoAmI == Main.myPlayer)
            {

                player.AddBuff(ModContent.BuffType<Buffs.MedusaHeadMinionBuff>(), 100);


            }
            return true;
        }

    }
}
