using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Tmodtober.Items
{

    public class HeavenswardLance:ModItem
    {
        int useDelay;
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 52;

            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(silver: 25);

            Item.useTime = 12;
            Item.useAnimation= 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item34;
            Item.autoReuse = true;
            Item.holdStyle = -1;
            Item.noMelee = true;

            Item.shootSpeed = 3.7f;
            Item.damage = 25;
            Item.knockBack = 6.5f;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ModContent.ProjectileType<Projectiles.HeavenswardLance_Projectile>();
        }

        public override bool CanUseItem(Player player)
        {
            if (useDelay >= 0)
            {
                return false;
            }

            Player _p = Main.player[Item.playerIndexTheItemIsReservedFor];
            if (player.whoAmI==Main.LocalPlayer.whoAmI && player.ownedProjectileCounts[Item.shoot] < 1)
            {
                DragoonPlayer _dp = (DragoonPlayer)player.GetModPlayer<DragoonPlayer>();
                int _target = -1;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].damage > 0 && Vector2.DistanceSquared(Main.npc[i].Center, _p.Center) < 600 * 600)
                    {
                        if (_target < 1 || Vector2.DistanceSquared(Main.npc[i].Center, _p.Center) < Vector2.DistanceSquared(Main.npc[_target].Center, _p.Center)){
                            _target = i;
                        }
                    }
                }

                if (_target != -1)
                {

                    int i;
                    Point _desPos;
                    for (i = 0; i < 75; i++)
                    {
                        _desPos = (Main.npc[_target].Center + new Vector2(16 * i, 0)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(_desPos.X, _desPos.Y)){
                            i--;
                            break;
                        }
                    }
                    _desPos = (Main.npc[_target].Center - new Vector2(0, 16 * i)).ToTileCoordinates();
                    if (!WorldGen.SolidOrSlopedTile(_desPos.X, _desPos.Y))
                    {
                        MakeClones(_dp, player,7);
                        _dp.Jump((int)(Item.useTime * 1.5f), Item.useTime * 2, Main.npc[_target].Center);
                        Main.player[Item.playerIndexTheItemIsReservedFor].Center = Main.npc[_target].Center - new Vector2(0, 16 * i);
                    }
                }
                else
                {
                    
                    Point _desPos;
                    int i;
                    for (i = 0; i < 75; i++)
                    {
                        _desPos = (_p.Center + new Vector2(16 * i, 0)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(_desPos.X, _desPos.Y))
                        {
                            i--;
                            break;
                        }
                    }

                    _desPos = ( _p.Center+new Vector2(_p.direction * 100, -16 * i - _p.height / 2)).ToTileCoordinates();
                    if (!WorldGen.SolidOrSlopedTile(_desPos.X, _desPos.Y))
                    {
                        MakeClones(_dp, player,7);
                        _dp.Jump((int)(Item.useTime * 1.5f), Item.useTime * 2, _p.Center + new Vector2(_p.direction * 100, 0));

                        Main.player[Item.playerIndexTheItemIsReservedFor].Center += new Vector2(_p.direction * 100, -16*i-_p.height/2);
                    }
                }
                useDelay = (int)(Item.useTime *4f);

                return true;
            }

            return false;
        }

        public void MakeClones(DragoonPlayer _dp, Player _p,int _ammount)
        {
            for(int i = 0; i < _ammount; i++)
            {
                _dp.AddClone(_p.Center-new Vector2(0,16*5*i),(float)i/_ammount);
            }
        }

        public override void UpdateInventory(Player player)
        {
            useDelay--;
        }

        public override bool? UseItem(Player player)
        {

            // Because we're skipping sound playback on use animation start, we have to play it ourselves whenever the item is actually used.
            if (!Main.dedServ && Item.UseSound.HasValue)
            {
                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Heavenstone_Bar>(15)
                .AddTile(TileID.Hellforge)
                .Register();
        }

    }
}
