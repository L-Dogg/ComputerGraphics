using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RacingGame
{
	public class CarModel : IModel
	{
		public Vector3 Position { get; set; }
		public Vector3 CarStartPosition { get; } = new Vector3(30.39419f, 0, -5.966733f);
		public Quaternion CarRotation { get; set; }= Quaternion.Identity;


		private Model model;
		private float _scale;

		public CarModel(Model model, Vector3 position, Quaternion rotation, float scale)
		{
			this.model = model;
			this.Position = position;
		}


		public void Draw(Matrix _viewMatrix, Matrix _projectionMatrix, Vector3 _carPosition)
		{
			var carMatrix = Matrix.CreateScale(_scale, _scale, _scale) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(CarRotation) *
							Matrix.CreateTranslation(_carPosition);

			var modelTransforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(modelTransforms);

			foreach (var mesh in model.Meshes)
			{
				foreach (var currentEffect in mesh.Effects)
				{
					var worldMatrix = modelTransforms[mesh.ParentBone.Index]* carMatrix;
					currentEffect.CurrentTechnique = currentEffect.Techniques[Lights.CurrentModel];

					currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
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
			throw new NotImplementedException();
		}
	}
}
