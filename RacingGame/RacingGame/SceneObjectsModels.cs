using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RacingGame
{
	public class SceneObjectsModels : IModel
	{
		public Vector3 Position { get; set; }

		private Model _fenceModel;
		private Model _treeModel;
		private Model _bambooHouseModel;
		private Model _slrModel;

		private readonly Vector3[] _treePositions =
		{
			new Vector3(32.25059f, 0, -10.68214f),
			new Vector3(32.25059f, 0, -10.68214f),
			new Vector3(14.1391f, 0, -29.09725f),
			new Vector3(29.83624f, 0, -46.26336f),
			new Vector3(55.27253f, 0, -52.69949f),
			new Vector3(63.2239f, 0, -24.0257f),
			new Vector3(48.31152f, 0, -24.0257f)
		};
		private readonly float[] _treeHeights = { 0.37f, 0.63f, 0.25f, 0.15f, 0.44f };

		private readonly Vector3 _waterTankPosition = new Vector3(15, 0.3f, -17);

		private readonly Vector3[] _houseesPositions =
		{
			new Vector3(41.17903f, 0, -15.565f),
			new Vector3(46.55457f, 0, -12.63017f)
		};

		private readonly GraphicsDeviceManager _graphics;

		public SceneObjectsModels(GraphicsDeviceManager graphics, Model _fenceModel, Model _treeModel, Model _bambooHouseModel, Model _slrModel)
		{
			this._graphics = graphics;
			this._fenceModel = _fenceModel;
			this._treeModel = _treeModel;
			this._bambooHouseModel = _bambooHouseModel;
			this._slrModel = _slrModel;
		}

		public void Draw(Matrix _viewMatrix, Matrix _projectionMatrix, Vector3 _carPosition)
		{
			
		}

		public void Update()
		{
			
		}

		private void DrawFences(Matrix _viewMatrix, Matrix _projectionMatrix)
		{
			var tmpMatrix = Matrix.CreateScale(0.005f) *
							Matrix.CreateRotationY(MathHelper.PiOver2) *
							Matrix.CreateFromQuaternion(Quaternion.Identity);
			for (var x = 0f; x < 63.8f; x += 0.65f)
			{
				var fenceMatrix = tmpMatrix * Matrix.CreateTranslation(0.1f + x, 0, 0.01f);
				DrawFence(_fenceModel, fenceMatrix, _viewMatrix, _projectionMatrix);
				fenceMatrix = tmpMatrix * Matrix.CreateTranslation(0.1f + x, 0, -60f);
				DrawFence(_fenceModel, fenceMatrix, _viewMatrix, _projectionMatrix);
			}

			tmpMatrix = Matrix.CreateScale(0.005f) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(Quaternion.Identity);

			for (var x = 0f; x < 63.8f; x += 0.65f)
			{
				var fenceMatrix = tmpMatrix * Matrix.CreateTranslation(0.1f, 0, 0.01f - x);
				DrawFence(_fenceModel, fenceMatrix, _viewMatrix, _projectionMatrix);
				fenceMatrix = tmpMatrix * Matrix.CreateTranslation(65f, 0, -60f + x);
				DrawFence(_fenceModel, fenceMatrix, _viewMatrix, _projectionMatrix);
			}
		}

		private void DrawTrees(Matrix _viewMatrix, Matrix _projectionMatrix)
		{
			foreach (var startVector in _treePositions)
			{
				for (var i = 0; i < 5; i++)
				{
					var treeMatrix = Matrix.CreateScale(_treeHeights[i % _treeHeights.Length]) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(Quaternion.Identity) *
							Matrix.CreateTranslation(startVector + new Vector3(0.5f * i, 0, 0));
					DrawModel(_treeModel, treeMatrix, _viewMatrix, _projectionMatrix);
				}
			}

			for (var i = 0; i < 12; i++)
			{
				var treeMatrix = Matrix.CreateScale(_treeHeights[i % _treeHeights.Length]) *
						Matrix.CreateRotationY(MathHelper.Pi) *
						Matrix.CreateFromQuaternion(Quaternion.Identity) *
						Matrix.CreateTranslation(_treePositions.Last() + new Vector3(-0.59f - 0.2f * i, 0, 0.25f * i));
				DrawModel(_treeModel, treeMatrix, _viewMatrix, _projectionMatrix);
			}

		}

		private void DrawHousesAndCars(Matrix _viewMatrix, Matrix _projectionMatrix)
		{
			foreach (var startVector in _houseesPositions)
			{
				for (var i = 0; i < 3; i++)
				{
					var houseMatrix = Matrix.CreateScale(0.13f) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(Quaternion.Identity) *
							Matrix.CreateTranslation(startVector + new Vector3(1.35f * i, 0, 0));

					var carMatrix = Matrix.CreateScale(0.05f) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(Quaternion.Identity) *
							Matrix.CreateTranslation(startVector + new Vector3(1.35f * i, 0, 2.15f));

					DrawModel(_bambooHouseModel, houseMatrix, _viewMatrix, _projectionMatrix);
					DrawModel(_slrModel, carMatrix, _viewMatrix, _projectionMatrix);
				}
			}
		}

		private void DrawFence(Model model, Matrix wMatrix, Matrix _viewMatrix, Matrix _projectionMatrix)
		{
			var modelTransforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(modelTransforms);


			var prevBlend = _graphics.GraphicsDevice.BlendState;
			var prevDepth = _graphics.GraphicsDevice.DepthStencilState;

			_graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
			_graphics.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
			int i = 0;
			foreach (var mesh in model.Meshes)
			{
				if (i == 1)
				{
					_graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
					_graphics.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
				}
				foreach (var currentEffect in mesh.Effects)
				{
					var worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
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
				if (i == 1)
				{
					_graphics.GraphicsDevice.BlendState = prevBlend;
					_graphics.GraphicsDevice.DepthStencilState = prevDepth;
				}
				mesh.Draw();
				i++;
			}
			_graphics.GraphicsDevice.BlendState = prevBlend;
			_graphics.GraphicsDevice.DepthStencilState = prevDepth;
		}

		private void DrawModel(Model model, Matrix wMatrix, Matrix _viewMatrix, Matrix _projectionMatrix)
		{
			var modelTransforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(modelTransforms);

			foreach (var mesh in model.Meshes)
			{
				foreach (var currentEffect in mesh.Effects)
				{
					var worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
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
	}
}
