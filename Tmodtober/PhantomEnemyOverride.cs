using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace Tmodtober
{
    public class PhantomEnemyOverride:GlobalNPC
    {

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            DannyPhantomPlayer _danny = player.GetModPlayer<DannyPhantomPlayer>();
            _danny.AddPhantomPower(damageDone / 10);

            base.OnHitByItem(npc, player, item, hit, damageDone);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {

            if (projectile.owner >= 0)
            {
                Player player = Main.player[projectile.owner];
                DannyPhantomPlayer _danny = player.GetModPlayer<DannyPhantomPlayer>();
                _danny.AddPhantomPower(damageDone / 10);
            }

            base.OnHitByProjectile(npc, projectile, hit, damageDone);
        }

    }
}
