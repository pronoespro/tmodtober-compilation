using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tmodtober.Projectiles
{
	public class GraniteLazer : ModProjectile
	{

		private const float BeamTileColWidth = 1f;
		private const float BeamHitboxColWidth = 22f;

		private const int NumSamplePoints = 3;

		private float BeamLength=24;

		public override void SetDefaults()
		{
			Projectile.width = 24; // The width of projectile hitbox
			Projectile.height = 12; // The height of projectile hitbox

			Projectile.friendly = true; // Can the projectile deal damage to enemies?
			Projectile.hostile = false; // Can the projectile deal damage to the player?
			Projectile.DamageType = DamageClass.Ranged; // Is the projectile shoot by a ranged weapon?

			Projectile.penetrate = -1; // How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.timeLeft = 50; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.alpha = 255; // The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			Projectile.light = 0.65f; // How much light emit around the projectile
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
			Projectile.tileCollide = true; // Can the projectile collide with tiles?


			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			// If something has gone wrong with either the beam or the host Prism, destroy the beam.
			if (Projectile.type != ModContent.ProjectileType<GraniteLazer>())
			{
				Projectile.Kill();
				return;
			}

			Color beamColor = Projectile.GetAlpha(Color.Blue);
			beamColor.A = 64;

			Projectile.Opacity = 1f;

			// This trigonometry calculates where the beam is supposed to be pointing.
			Vector2 yVec = new Vector2(4f, 6f);

			// Calculate the beam's emanating position. Start with the Prism's center.
			Projectile.Center = player.HandPosition.Value;
			// Add a fixed offset to align with the Prism's sprite sheet.
			Projectile.position += Vector2.Normalize(Projectile.velocity)* 16f;

			if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
			{
				Projectile.velocity = -Vector2.UnitY;
			}
			Projectile.rotation =Vector2.Normalize(Projectile.velocity).ToRotation();

			// Update the beam's length by performing a hitscan collision check.
			float hitscanBeamLength = PerformBeamHitscan(Projectile.Center);
			BeamLength = MathHelper.Lerp(BeamLength, hitscanBeamLength, 1f);

			// This Vector2 stores the beam's hitbox statistics. X = beam length. Y = beam width.
			Vector2 beamDims = new Vector2(Projectile.velocity.Length() * BeamLength, Projectile.width * Projectile.scale);

			// Only produce dust and cause water ripples if the beam is above a certain charge level.

			ProduceWaterRipples(beamDims);

			// Make the beam cast light along its length. The brightness of the light scales with the charge.
			// v3_1 is an unnamed decompiled variable which is the color of the light cast by DelegateMethods.CastLight.
			Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * BeamLength, beamDims.Y, new Utils.TileActionAttempt(DelegateMethods.CastLight));
			
		}


		private float PerformBeamHitscan(Vector2 prismPos)
		{
			// By default, the hitscan interpolation starts at the Projectile's center.
			// If the host Prism is fully charged, the interpolation starts at the Prism's center instead.
			Vector2 samplingPoint = Projectile.Center;

			// Overriding that, if the player shoves the Prism into or through a wall, the interpolation starts at the player's center.
			// This last part prevents the player from projecting beams through walls under any circumstances.
			Player player = Main.player[Projectile.owner];
			if (!Collision.CanHitLine(player.Center, 0, 0, prismPos, 0, 0))
			{
				samplingPoint = player.Center;
			}

			// Perform a laser scan to calculate the correct length of the beam.
			// Alternatively, if you want the beam to ignore tiles, just set it to be the max beam length with the following line.
			// return MaxBeamLength;
			float[] laserScanResults = new float[NumSamplePoints];
			Collision.LaserScan(samplingPoint, Projectile.velocity, 0 * Projectile.scale, 25f, laserScanResults);
			float averageLengthSample = 0f;
			for (int i = 0; i < laserScanResults.Length; ++i)
			{
				averageLengthSample += laserScanResults[i];
			}
			averageLengthSample /= NumSamplePoints;

			return averageLengthSample;
		}


		public override bool PreDraw(ref Color lightColor)
		{
			// If the beam doesn't have a defined direction, don't draw anything.
			if (Projectile.velocity == Vector2.Zero)
			{
				return false;
			}

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 centerFloored = Projectile.Center.Floor() + Vector2.Normalize(Projectile.velocity) * Projectile.scale * 10.5f;
			Vector2 drawScale = new Vector2(Projectile.scale);

			// Reduce the beam length proportional to its square area to reduce block penetration.
			float visualBeamLength = BeamLength - 14.5f * Projectile.scale * Projectile.scale;

			DelegateMethods.f_1 = 1f; // f_1 is an unnamed decompiled variable whose function is unknown. Leave it at 1.
			Vector2 startPosition = centerFloored - Main.screenPosition;
			Vector2 endPosition = startPosition + Projectile.velocity * visualBeamLength;

			// Draw the outer beam.
			DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale,Color.Blue* Projectile.Opacity);

			// Draw the inner beam, which is half size.
			drawScale *= 0.5f;
			DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, Color.White * Projectile.Opacity);

			// Returning false prevents Terraria from trying to draw the Projectile itself.
			return false;
		}


		private void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Vector2 drawScale, Color beamColor)
		{
			Utils.LaserLineFraming lineFraming = new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw);

			// c_1 is an unnamed decompiled variable which is the render color of the beam drawn by DelegateMethods.RainbowLaserDraw.
			DelegateMethods.c_1 = beamColor;
			Utils.DrawLaser(spriteBatch, texture, startPosition, endPosition, drawScale, lineFraming);
		}

		private void ProduceWaterRipples(Vector2 beamDims)
		{
			WaterShaderData shaderData = (WaterShaderData)Filters.Scene["WaterDistortion"].GetShader();

			// A universal time-based sinusoid which updates extremely rapidly. GlobalTime is 0 to 3600, measured in seconds.
			float waveSine = 0.1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
			Vector2 ripplePos = Projectile.position + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(Projectile.rotation);

			// WaveData is encoded as a Color. Not really sure why.
			Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
			shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, Projectile.rotation);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// If the target is touching the beam's hitbox (which is a small rectangle vaguely overlapping the host Prism), that's good enough.
			if (projHitbox.Intersects(targetHitbox))
			{
				return true;
			}

			// Otherwise, perform an AABB line collision check to check the whole beam.
			float _ = float.NaN;
			Vector2 beamEndPos = Projectile.Center + Vector2.Normalize(Projectile.velocity) * BeamLength;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos, BeamHitboxColWidth * Projectile.scale, ref _);
		}



	}
}
