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
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;

namespace Tmodtober.NPCs
{
    public class Spirit:ModNPC
    {

        List<Vector2> oldPos;
        public const int TRAIL_MAX_POSITIONS = 50;
        public const float TRACK_RANGE = 250;
        float sinOffset;
        Vector2 maxDistance;
        float moveSpeed;
        float rotateSpeed;
        float aiSpeed;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;
            Main.npcCatchable[Type] = true;

            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;

        }

        public override void SetDefaults()
        {
            NPC.width = 16;
            NPC.height = 16;
            NPC.scale = 2f;

            NPC.lifeMax = 10;
            NPC.friendly = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.catchItem =ModContent.ItemType<Items.Spirit_item>();
            NPC.lavaImmune = true;
            oldPos = new List<Vector2>();

            sinOffset = Main.rand.NextFloat(0, MathHelper.Pi * 2);
            maxDistance = new Vector2(Main.rand.NextFloat(25, 50), Main.rand.NextFloat(15, 25));
            moveSpeed = Main.rand.Next(3, 6);

            rotateSpeed = Math.Sign(Main.rand.Next(-10, 10)) * Main.rand.NextFloat(MathHelper.PiOver2 / 20, MathHelper.PiOver2/5);
            aiSpeed = Main.rand.NextFloat(1, 1.2f);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
                new FlavorTextBestiaryInfoElement("Ghostly apparitions that seem fond of you, probably for the dead you cause"));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneGraveyard?1f:0;
        }

        public override void AI()
        {

            Vector2 _desVel = new Vector2(MathF.Cos(NPC.ai[0])* maxDistance.X, MathF.Sin(NPC.ai[0] * 2+sinOffset)* maxDistance.Y) ;
            NPC.TargetClosest(faceTarget:false);
            NPC.direction = 1;
            if (NPC.HasPlayerTarget && Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center) < TRACK_RANGE * TRACK_RANGE)
            {
                NPC.rotation += rotateSpeed;
                Vector2 _despos = Main.player[NPC.target].Center + _desVel;
                Vector2 _desDir = _despos - NPC.Center;
                if ((MathF.Abs(_desDir.X)+ MathF.Abs(_desDir.Y))/2<= moveSpeed)
                {
                    NPC.ai[0] += 0.01f * moveSpeed* aiSpeed;
                    NPC.Center = _despos;
                    NPC.velocity = Vector2.Zero;
                }
                else
                {
                    _desDir = Vector2.Normalize(_desDir) * moveSpeed;
                    NPC.velocity= _desDir;
                }
            }
            else
            {
                NPC.rotation += rotateSpeed/3f;
                NPC.ai[0] += 0.005f * moveSpeed* aiSpeed;
                if (Vector2.DistanceSquared(Vector2.Zero, NPC.velocity) > Vector2.DistanceSquared(_desVel / 10f, Vector2.Zero))
                {
                    NPC.velocity = Vector2.Lerp(NPC.velocity, _desVel / 10f,0.2f);
                }
                else
                {
                    NPC.velocity = _desVel / 10f;
                }
            }

            oldPos.Add(NPC.Center+new Vector2(Main.rand.NextFloat(-1,1),Main.rand.NextFloat(-1,1)));

            if (oldPos.Count >= TRAIL_MAX_POSITIONS)
            {
                oldPos.RemoveAt(0);
            }

        }

        public override Color? GetAlpha(Color drawColor)
        {
            return drawColor * 0.5f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>(Texture).Value;
            Rectangle _rect = new Rectangle(0, _texture.Height / 3, _texture.Width, _texture.Height/3);

            for(int i=0;i<oldPos.Count;i++)
            {
                spriteBatch.Draw(_texture, oldPos[i] - screenPos, _rect, drawColor*((((float)i)/oldPos.Count))*0.2f, NPC.rotation-rotateSpeed*(1-i/oldPos.Count)/3, new Vector2(_texture.Width / 2, _texture.Height / 4), MathF.Max(0,NPC.scale - (1f-(((float)i)/oldPos.Count))*NPC.scale), SpriteEffects.None, 0);
            }

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Texture2D _texture = (Texture2D)ModContent.Request<Texture2D>(Texture).Value;
            Rectangle _rect = new Rectangle(0, _texture.Height / 3*2, _texture.Width, _texture.Height / 3);

            spriteBatch.Draw(_texture, NPC.Center - screenPos, _rect, drawColor, 0, new Vector2(_texture.Width / 2, _texture.Height / 3 / 2), NPC.scale, SpriteEffects.None, 0); ;
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

    }
}
