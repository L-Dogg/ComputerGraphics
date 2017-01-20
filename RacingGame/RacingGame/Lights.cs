using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RacingGame
{
	public static class Lights
	{
		public static Matrix LightPositions { get; set; }
		public static Matrix LightColors { get; set; } = new Matrix
			(
				1, 1, 1, 1,
				0.2f, 0.8f, 0.1f, 1,
				0.1f, 0.1f, 0.8f, 1,
				0.7f, 0.3f, 0.3f, 1
			);
		public static int LightsCount { get; set; } = 0;
		public static int MaxLights { get;  }  = 4;
		public static string ShadingModel { get; set; } = "Flat";
		public static string LightingModel { get; set; } = "Phong";
		public static string CurrentModel => $"{ShadingModel}{LightingModel}";

		public static Matrix CarLightPositions { get; set; }
		public static Matrix CarLightColors { get; set; } = new Matrix
			(
				1, 0, 0, 1,
				0, 0, 1, 1,
				0, 0, 0, 1,
				0, 0, 0, 1
			);
		public static float CarLightXOffset { get; set; } = -0.11f;
		public static float CarLightYOffset { get; set; } = 0.17f;
		public static float CarLightZOffset { get; set; } = -0.22f;

		public static void SetUpLightData(Vector3 _carStartPosition)
		{
			LightPositions = new Matrix(
				_carStartPosition.X - 0.5f, _carStartPosition.Y + 10, _carStartPosition.Z, 0,
				18, 13, -17, 0,
				33.28685f, 13, -51.12287f, 0,
				62.82345f, 13, -18.72201f, 0
			);
			
			CarLightPositions = new Matrix(
				_carStartPosition.X + CarLightXOffset, _carStartPosition.Y + CarLightYOffset, _carStartPosition.Z + CarLightZOffset, 0,
				_carStartPosition.X - CarLightXOffset, _carStartPosition.Y + CarLightYOffset, _carStartPosition.Z + CarLightZOffset, 0,
				0, 0, 0, 0,
				0, 0, 0, 0
			);
			
		}
	}
}
