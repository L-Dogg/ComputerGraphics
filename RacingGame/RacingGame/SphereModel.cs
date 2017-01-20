using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RacingGame
{
	public class SphereModel : IModel
	{
		public Vector3 Position { get; set; }

		private Model _model;
		private float _scale = 0.002f;

		public SphereModel(Model model, Vector3 position, float scale)
		{
			this._model = model;
			this.Position = position;
		}

		public void Draw(Matrix _viewMatrix, Matrix _projectionMatrix, Vector3 _carPosition)
		{
			var sphereMatrix = Matrix.CreateScale(_scale) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(Quaternion.Identity) *
							Matrix.CreateTranslation(Position);


			foreach (var mesh in _model.Meshes)
			{
				foreach (var currentEffect in mesh.Effects)
				{
					currentEffect.CurrentTechnique = currentEffect.Techniques[Lights.CurrentModel];

					currentEffect.Parameters["xWorld"].SetValue(sphereMatrix);
					currentEffect.Parameters["xView"].SetValue(_viewMatrix);
					currentEffect.Parameters["xProjection"].SetValue(_projectionMatrix);

					currentEffect.Parameters["Ka"].SetValue(0.5f);
					currentEffect.Parameters["Ks"].SetValue(0.5f);
					currentEffect.Parameters["Kd"].SetValue(0.75f);
					currentEffect.Parameters["A"].SetValue(15f);

					currentEffect.Parameters["xCamUp"].SetValue(Cameras.CameraUp);
					currentEffect.Parameters["xCamPos"].SetValue(Cameras.CameraPosition);
					currentEffect.Parameters["xLightPositions"].SetValue(Lights.LightPositions);
					currentEffect.Parameters["xLightColors"].SetValue(Lights.LightColors);
					currentEffect.Parameters["xLightCount"].SetValue(Lights.LightsCount);
					currentEffect.Parameters["xCarLightPositions"].SetValue(Lights.CarLightPositions);
					currentEffect.Parameters["xCarLightColors"].SetValue(Lights.CarLightColors);

				}
				mesh.Draw();
			}
		}

		public void Update()
		{
			return;
		}
	}
}
