using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RacingGame
{
	public class Game1 : Game
	{
		#region Private Fields

		private readonly GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private SpriteFont _font;
		private GraphicsDevice _device;
		private Effect _effect;

		private Matrix _viewMatrix;
		private Matrix _projectionMatrix;

		private int[,] _floorPlan;
		private VertexBuffer _vertexBuffer;
		private Texture2D _sceneryTexture;
		private BoundingBox _raceTrackBox;

		private Model _skyboxModel;
		private Texture2D[] _skyboxTextures;

		private Model _carModel;
		private Vector3 _carPosition = new Vector3(8, 0.037f, -3);
		private Quaternion _carRotation = Quaternion.Identity;

		private Vector3 _cameraUp;
		private Vector3 _cameraPosition;
		private Quaternion _cameraRotation = Quaternion.Identity;
		private float _cameraAngle = 0f;
		private const float _cameraChangeAngle = 0.005f;

		private readonly float _acceleration = 0.0005f;
		private float _moveSpeed;
		private float _gameSpeed = 1.0f;
		private enum CollisionType { None, Boundary }
		
		private float _ambientPower;

		private Matrix _lightPositions;

		private string _shadingModel = "Flat";
		private string _lightingModel = "Phong";
		private string CurrentModel => $"{_shadingModel}{_lightingModel}";

		#endregion

		#region Update

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();

			ProcessKeyboard(gameTime);
			MoveForward(ref _carPosition, _carRotation, _moveSpeed);

			var carSpere = new BoundingSphere(_carPosition, 0.04f);
			if (CheckCollision(carSpere) != CollisionType.None)
			{
				_moveSpeed = 0f;
				_carRotation = Quaternion.Identity;
				_carPosition = new Vector3(8, 0.037f, -3);
				_gameSpeed /= 1.1f;
			}

			UpdateCamera();
			base.Update(gameTime);
		}

		private void UpdateCamera()
		{
			_cameraRotation = Quaternion.Lerp(_cameraRotation, _carRotation, 0.08f);

			_cameraPosition = new Vector3(_cameraAngle, 0.33f, 0.88f);
			_cameraPosition = Vector3.Transform(_cameraPosition, Matrix.CreateFromQuaternion(_cameraRotation));
			_cameraPosition += _carPosition;

			_cameraUp = new Vector3(0, 1, 0);
			_cameraUp = Vector3.Transform(_cameraUp, Matrix.CreateFromQuaternion(_cameraRotation));

			_viewMatrix = Matrix.CreateLookAt(_cameraPosition, _carPosition, _cameraUp);
			//_projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _device.Viewport.AspectRatio, 0.2f, 500.0f);
		}

		private void ProcessKeyboard(GameTime gameTime)
		{
			float leftRightRot = 0;

			var turningSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
			turningSpeed *= 1.6f * _gameSpeed;
			var keys = Keyboard.GetState();

			// Turn
			if (keys.IsKeyDown(Keys.Right) && (_moveSpeed > 0.0005f || _moveSpeed < -0.0005f))
				leftRightRot += turningSpeed;
			if (keys.IsKeyDown(Keys.Left) && (_moveSpeed > 0.0005f || _moveSpeed < -0.0005f))
				leftRightRot -= turningSpeed;

			// Accelerate
			if (keys.IsKeyDown(Keys.Up))
				_moveSpeed += _acceleration;
			else if (keys.IsKeyDown(Keys.Down))
				_moveSpeed -= _acceleration;
			else if (_moveSpeed > 0.0f)
				_moveSpeed -= _acceleration;
			else if (_moveSpeed < 0.0f)
				_moveSpeed += _acceleration;

			// Change camera angle:
			if (keys.IsKeyDown(Keys.J))
				_cameraAngle -= _cameraChangeAngle;
			else if (keys.IsKeyDown(Keys.L))
				_cameraAngle += _cameraChangeAngle;
			else if (keys.IsKeyDown(Keys.K))
				_cameraAngle = 0;

			// Change light model:
			if (keys.IsKeyDown(Keys.D1))
				_lightingModel = "Phong";
			else if (keys.IsKeyDown(Keys.D2))
				_lightingModel = "Blinn";

			// Change shading model:
			if (keys.IsKeyDown(Keys.Q))
				_shadingModel = "Flat";
			else if (keys.IsKeyDown(Keys.W))
				_shadingModel = "Gouraud";
			else if (keys.IsKeyDown(Keys.E))
				_shadingModel = "Phong";

			var additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, -1, 0), leftRightRot);
			_carRotation *= additionalRot;
		}

		private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
		{
			var addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);
			position += addVector * speed;
		}

		private CollisionType CheckCollision(BoundingSphere sphere)
		{
			return _raceTrackBox.Contains(sphere) != ContainmentType.Contains ? CollisionType.Boundary : CollisionType.None;
		}

		#endregion

		#region Setup

		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = 1280;
			_graphics.PreferredBackBufferHeight = 720;
			_graphics.IsFullScreen = false;
			_graphics.ApplyChanges();
			Window.Title = "Project Cars";

			LoadFloorPlan();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_font = Content.Load<SpriteFont>("Courier-New");
			_device = _graphics.GraphicsDevice;

			_effect = Content.Load<Effect>("effects");
			_sceneryTexture = Content.Load<Texture2D>("texturemap");
			_skyboxModel = LoadModel("skybox2", out _skyboxTextures);
			_carModel = LoadModel("car");

			SetUpCamera();
			SetUpVertices();

			SetUpBoundingBoxes();
			SetUpLightData();
		}

		private Model LoadModel(string assetName, out Texture2D[] textures)
		{
			var newModel = Content.Load<Model>(assetName);
			textures = new Texture2D[7];

			var i = 0;
			foreach (var mesh in newModel.Meshes)
			{
				foreach (BasicEffect currentEffect in mesh.Effects)
				{
					textures[i++] = currentEffect.Texture;
					//this.effect.Parameters["Ka"].SetValue(currentEffect.Parameters["SpecularPower"].GetValueSingle());
				}
			}

			foreach (var mesh in newModel.Meshes)
			{
				foreach (var meshPart in mesh.MeshParts)
				{
					meshPart.Effect = _effect.Clone();
				}
			}

			return newModel;
		}

		private Model LoadModel(string assetName)
		{

			Model newModel = Content.Load<Model>(assetName);
			foreach (ModelMesh mesh in newModel.Meshes)
				foreach (ModelMeshPart meshPart in mesh.MeshParts)
				{
					if ((meshPart.Effect as BasicEffect).Texture == null)
					{
						_effect.Parameters["UseColors"].SetValue(true);
						_effect.Parameters["DiffuseColor"].SetValue(new Vector4((meshPart.Effect as BasicEffect).DiffuseColor, 1));
					}
					else
					{
						_effect.Parameters["xTexture"].SetValue((meshPart.Effect as BasicEffect).Texture);
					}
					meshPart.Effect = _effect.Clone();
				}
			return newModel;
		}

		private void LoadFloorPlan()
		{
			_floorPlan = new[,]
			 {
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			 };
		}

		private void SetUpBoundingBoxes()
		{
			var raceTrackWidth = _floorPlan.GetLength(0);
			var raceTrackLength = _floorPlan.GetLength(1);

			var boundaryPoints = new Vector3[2];
			boundaryPoints[0] = new Vector3(0, -20, 0);
			boundaryPoints[1] = new Vector3(raceTrackWidth, 20, -raceTrackLength);
			_raceTrackBox = BoundingBox.CreateFromPoints(boundaryPoints);
		}

		private void SetUpCamera()
		{
			_viewMatrix = Matrix.CreateLookAt(new Vector3(20, 13, -5), new Vector3(8, 0, -7), new Vector3(0, 1, 0));
			_projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _device.Viewport.AspectRatio, 0.2f, 500.0f);
		}

		private void SetUpVertices()
		{
			var raceTrackWidth = _floorPlan.GetLength(0);
			var raceTrackLength = _floorPlan.GetLength(1);

			var verticesList = new List<VertexPositionNormalTexture>();
			for (var x = 0; x < raceTrackWidth; x++)
			{
				for (var z = 0; z < raceTrackLength; z++)
				{
					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(0, 1, 0), new Vector2(0, 1)));
					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z - 1), new Vector3(0, 1, 0), new Vector2(0, 0)));
					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(0, 1, 0), new Vector2(1 / 11.0f, 1)));

					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z - 1), new Vector3(0, 1, 0), new Vector2(0, 0)));
					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z - 1), new Vector3(0, 1, 0), new Vector2(1 / 11.0f, 0)));
					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(0, 1, 0), new Vector2(1 / 11.0f, 1)));
				}
			}

			_vertexBuffer = new VertexBuffer(_device, VertexPositionNormalTexture.VertexDeclaration, verticesList.Count, BufferUsage.WriteOnly);
			_vertexBuffer.SetData(verticesList.ToArray());
		}

		private void SetUpLightData()
		{
			_lightPositions = new Matrix
				(5, 10, 5, 0,
				1, 30, 3, 0,
				0, 0, 0, 0,
				0, 0, 0 , 0);
			_ambientPower = 0.3f;
		}

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		#endregion

		#region Draw

		protected override void Draw(GameTime gameTime)
		{
			_device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);

			DrawSkybox();
			DrawRaceTrack();
			var carMatrix = Matrix.CreateScale(0.1f, 0.1f, 0.1f) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(_carRotation) *
							Matrix.CreateTranslation(_carPosition);
			DrawModel(_carModel, null, carMatrix);

			//_spriteBatch.Begin();
			//_spriteBatch.DrawString(_font, $"{(int)(_moveSpeed * 1000)} km/h",
			//    new Vector2(GraphicsDevice.Viewport.Width * 2 / 3.0f, GraphicsDevice.Viewport.Height * 7 / 8.0f), Color.Red);
			//_spriteBatch.End();

			base.Draw(gameTime);
		}

		private void DrawModel(Model model, Texture2D[] textures, Matrix wMatrix)
		{
			var modelTransforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(modelTransforms);

			//var i = 0;
			foreach (var mesh in model.Meshes)
			{
				foreach (var currentEffect in mesh.Effects)
				{
					var worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
					currentEffect.CurrentTechnique = currentEffect.Techniques[CurrentModel];

					currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
					currentEffect.Parameters["xView"].SetValue(_viewMatrix);
					currentEffect.Parameters["xProjection"].SetValue(_projectionMatrix);
					//currentEffect.Parameters["xTexture"].SetValue(textures[i++]);

					currentEffect.Parameters["Ka"].SetValue(0.5f);
					currentEffect.Parameters["Ks"].SetValue(0.5f);
					currentEffect.Parameters["Kd"].SetValue(0.75f);
					currentEffect.Parameters["A"].SetValue(15f);

					_effect.Parameters["xCamUp"].SetValue(_cameraUp);
					_effect.Parameters["xCamPos"].SetValue(_cameraPosition);
					currentEffect.Parameters["AmbientIntensity"].SetValue(_ambientPower);

					_effect.Parameters["xLightPositions"].SetValue(_lightPositions);

				}
				mesh.Draw();
			}
		}

		private void DrawSkybox()
		{
			var ss = new SamplerState { AddressU = TextureAddressMode.Clamp, AddressV = TextureAddressMode.Clamp };
			_device.SamplerStates[0] = ss;

			var dss = new DepthStencilState { DepthBufferEnable = false };
			_device.DepthStencilState = dss;

			var skyboxTransforms = new Matrix[_skyboxModel.Bones.Count];
			_skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);

			var i = 0;
			foreach (var mesh in _skyboxModel.Meshes)
			{
				foreach (var currentEffect in mesh.Effects)
				{
					var worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(_carPosition);
					currentEffect.CurrentTechnique = currentEffect.Techniques[CurrentModel];
					currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
					currentEffect.Parameters["xView"].SetValue(_viewMatrix);
					currentEffect.Parameters["xProjection"].SetValue(_projectionMatrix);
					currentEffect.Parameters["xTexture"].SetValue(_skyboxTextures[i++]);

					currentEffect.Parameters["Ka"].SetValue(0.5f);
					currentEffect.Parameters["Ks"].SetValue(0.5f);
					currentEffect.Parameters["Kd"].SetValue(0.75f);
					currentEffect.Parameters["A"].SetValue(15f);

					currentEffect.Parameters["xCamUp"].SetValue(_cameraUp);
					currentEffect.Parameters["xCamPos"].SetValue(_cameraPosition);
					currentEffect.Parameters["AmbientIntensity"].SetValue(_ambientPower);

					_effect.Parameters["xLightPositions"].SetValue(_lightPositions);
				}
				mesh.Draw();
			}

			dss = new DepthStencilState { DepthBufferEnable = true };
			_device.DepthStencilState = dss;
		}

		private void DrawRaceTrack()
		{
			_effect.CurrentTechnique = _effect.Techniques[CurrentModel];
			_effect.Parameters["xWorld"].SetValue(Matrix.Identity);
			_effect.Parameters["xView"].SetValue(_viewMatrix);
			_effect.Parameters["xProjection"].SetValue(_projectionMatrix);
			_effect.Parameters["xTexture"].SetValue(_sceneryTexture);

			_effect.Parameters["Ka"].SetValue(0.5f);
			_effect.Parameters["Ks"].SetValue(0.5f);
			_effect.Parameters["Kd"].SetValue(0.75f);
			_effect.Parameters["A"].SetValue(15f);

			_effect.Parameters["xCamUp"].SetValue(_cameraUp);
			_effect.Parameters["xCamPos"].SetValue(_cameraPosition);
			_effect.Parameters["AmbientIntensity"].SetValue(_ambientPower);

			_effect.Parameters["xLightPositions"].SetValue(_lightPositions);

			foreach (var pass in _effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				_device.SetVertexBuffer(_vertexBuffer);
				_device.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertexBuffer.VertexCount / 3);
			}
		}

		#endregion
	}
}