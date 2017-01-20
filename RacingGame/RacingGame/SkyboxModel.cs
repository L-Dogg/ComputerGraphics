using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RacingGame
{
	public class SkyboxModel : IModel
	{
		private GraphicsDevice _device;
		private Model _skyboxModel;

		public SkyboxModel(GraphicsDevice device)
		{
			this._device = device;
		}

		public Vector3 Position
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public void Draw(Matrix _viewMatrix, Matrix _projectionMatrix, Vector3 _carPosition)
		{
			var ss = new SamplerState { AddressU = TextureAddressMode.Clamp, AddressV = TextureAddressMode.Clamp };
			_device.SamplerStates[0] = ss;

			var dss = new DepthStencilState { DepthBufferEnable = false };
			_device.DepthStencilState = dss;

			var skyboxTransforms = new Matrix[_skyboxModel.Bones.Count];
			_skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);

			foreach (var mesh in _skyboxModel.Meshes)
			{
				foreach (var currentEffect in mesh.Effects)
				{
					var worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(_carPosition);
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

			dss = new DepthStencilState { DepthBufferEnable = true };
			_device.DepthStencilState = dss;
		}

		public void Update()
		{
			return;
		}
	}
}
