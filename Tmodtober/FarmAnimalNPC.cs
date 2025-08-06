using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tmodtober
{
    public class FarmAnimalNPC:GlobalNPC{

        public override bool InstancePerEntity => true;

        public bool isFarmAnimal;
        public int framesUntilNextNameShow;

        public override void AI(NPC npc)
        {
            framesUntilNextNameShow--;
            if(npc.CountsAsACritter && isFarmAnimal)
            {
                npc.despawnEncouraged = false;
                npc.timeLeft = 10000;

                bool foundAnimal = false;
                for(int i = 0; i < FarmWorld.Instance._animals.Count; i++)
                {
                    if (FarmWorld.Instance._animals[i].npcID == npc.whoAmI)
                    {
                        FarmWorld.Instance._animals[i].pos = npc.Center;
                        foundAnimal = true;
                        break;
                    }
                }
                if (!foundAnimal)
                {
                    FarmWorld.Instance._animals.Add(new FarmAnimal(npc.FullName, npc.Center, npc.type, npc.whoAmI));
                }
            }
            base.AI(npc);
        }

        public bool TransformIntoFarmAnimal(NPC _npc)
        {
            if (_npc.CountsAsACritter && !isFarmAnimal)
            {
                isFarmAnimal = true;
                _npc.townNPC = true;
                return true;
            }
            return false;
        }

        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            if(npc.CountsAsACritter && isFarmAnimal)
            {
                return false;
            }
            return base.CanBeHitByItem(npc, player, item);
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            if (npc.CountsAsACritter && isFarmAnimal && projectile.friendly==true)
            {
                return false;
            }
            return base.CanBeHitByProjectile(npc, projectile);
        }

        public override void OnKill(NPC npc)
        {
            base.OnKill(npc);
            int _toRemove = -1;
            for (int i = 0; i < FarmWorld.Instance._animals.Count; i++)
            {
                if (FarmWorld.Instance._animals[i].npcID == npc.whoAmI)
                {
                    _toRemove = i;

                    EntitySource_Parent _s = new EntitySource_Parent(npc);
                    Item.NewItem(_s, npc.Center, ModContent.ItemType<Items.AnimalTag>(), noGrabDelay: true);
                    return;
                }
            }
            if (_toRemove >= 0)
            {
                FarmWorld.Instance._animals.RemoveAt(_toRemove);
            }
        }

        public override void OnCaughtBy(NPC npc, Player player, Item item, bool failed)
        {
            base.OnCaughtBy(npc, player, item, failed);
            if (!failed)
            {
                int _toRemove = -1;
                for (int i = 0; i < FarmWorld.Instance._animals.Count; i++)
                {
                    if (FarmWorld.Instance._animals[i].npcID == npc.whoAmI)
                    {
                        _toRemove = i;

                        EntitySource_Parent _s = new EntitySource_Parent(npc);
                        Item.NewItem(_s, npc.Center, ModContent.ItemType<Items.AnimalTag>(), noGrabDelay: true);
                        break;
                    }
                }
                if (_toRemove >= 0){
                    FarmWorld.Instance._animals.RemoveAt(_toRemove);
                }
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (isFarmAnimal && Vector2.Distance(npc.Center,Main.MouseWorld)<500 &&framesUntilNextNameShow<=0)
            {
                AdvancedPopupRequest _request = new AdvancedPopupRequest();
                _request.Color = Color.Yellow;
                _request.DurationInFrames = 200;
                _request.Text = npc.FullName;
                _request.Velocity = Vector2.Zero;

                Terraria.PopupText.NewText(_request,npc.Center+new Vector2(0,-50));
                framesUntilNextNameShow = 200;
            }
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }

    }
}
