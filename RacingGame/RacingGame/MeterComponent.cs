using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RacingGame
{
	public class MeterComponent
	{
		private SpriteBatch spriteBatch;

		private const float MAX_METER_ANGLE = 230;
		
		private float scale;
		private float lastAngle;

		private Vector2 meterPosition;
		private Vector2 meterOrigin;

		private Texture2D backgroundImage;
		private Texture2D needleImage;

		public float currentAngle = 0;

		/// <summary>
		/// Creates a new TextComponent for the HUD.
		/// </summary>
		/// <param name="position">Component position on the screen.</param>
		/// <param name="backgroundImage">Image for the background of the meter.</param>
		/// <param name="needleImage">Image for the neede of the meter.</param>
		/// <param name="spriteBatch">SpriteBatch that is required to draw the sprite.</param>
		/// <param name="scale">Factor to scale the graphics.</param>
		public MeterComponent(Vector2 position, Texture2D backgroundImage, Texture2D needleImage, SpriteBatch spriteBatch, float scale)
		{
			this.spriteBatch = spriteBatch;

			this.backgroundImage = backgroundImage;
			this.needleImage = needleImage;
			this.scale = scale;

			this.lastAngle = 0;

			meterPosition = new Vector2(position.X + backgroundImage.Width / 2, position.Y + backgroundImage.Height / 2);
			meterOrigin = new Vector2(250, 250);
		}

		
		/// <summary>
		/// Updates the current value of that should be displayed.
		/// </summary>
		/// <param name="currentValue">Value that to be displayed.</param>
		/// <param name="maximumValue">Maximum value that can be displayed by the meter.</param>
		public void Update(float currentValue, float maximumValue)
		{
			if (currentValue < 0)
				currentAngle = 0;
			else
				currentAngle = MathHelper.SmoothStep(lastAngle, (currentValue / maximumValue) * MAX_METER_ANGLE, 0.2f);
			lastAngle = currentAngle;
		}

		/// <summary>
		/// Draws the MeterComponent with the values set before.
		/// </summary>
		public void Draw()
		{
			spriteBatch.Begin();
			spriteBatch.Draw(backgroundImage, meterPosition, null, Color.White, 0, new Vector2(backgroundImage.Width / 2, backgroundImage.Height / 2), scale, SpriteEffects.None, 0); //Draw(backgroundImage, position, Color.White);
			spriteBatch.Draw(needleImage, meterPosition, null, Color.White, MathHelper.ToRadians(currentAngle) - MathHelper.PiOver2 - MathHelper.PiOver4 - 0.07f, meterOrigin, scale, SpriteEffects.None, 0);
			spriteBatch.End();
		}
	}
}
