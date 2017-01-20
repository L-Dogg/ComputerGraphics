using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RacingGame
{
	

	public class Waypoint
	{
		public float X { get; }
		public float Z { get; }
		public float Angle { get; } = 0;
		public Waypoint(float x, float z, float angle = 0)
		{
			this.X = x;
			this.Z = z;
			this.Angle = angle;
		}
	}

	public class Game : Microsoft.Xna.Framework.Game
	{
		#region Fields
		private bool gamePaused = true;

		private readonly GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private SpriteFont _font;
		private GraphicsDevice _device;
		private Effect _effect;

		private Matrix _viewMatrix;
		private Matrix _projectionMatrix;
		
		private Texture2D _speedometerTexture;
		private Texture2D _speedometerNeedleTexture;
		
		private Model _carModel;
		
		private readonly float _acceleration = 0.0005f;
		private readonly float _topSpeed = 0.07f;
		private readonly float _speedEpsilon = 0.005f;
		private float _moveSpeed;
		private float _gameSpeed = 1.0f;
		

		

		private Waypoint[] _waypoints;
		private readonly float _aiMoveSpeed = 0.078f;
		private int _waypointIndex = 0;

		private bool _plusPressed;
		private bool _minusPressed;
		private bool _hornPressed;

		private MeterComponent _speedoMeter;

		private SoundEffect _engineSound;
		private SoundEffectInstance _soundEngineInstance;
		#endregion

		#region Update

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();

			ProcessKeyboard(gameTime);

			if (!gamePaused)
			{
				MoveForward(ref _carPosition, _carRotation, _moveSpeed);

				var carSpere = new BoundingSphere(_carPosition, 0.04f);
				if (CheckCollision(carSpere) != CollisionType.None)
				{
					_moveSpeed = 0f;
					_carRotation = Quaternion.Identity;
					_carPosition = _carStartPosition;
					_gameSpeed /= 1.1f;
				}
			}

			switch (Cameras.CameraType)
			{
				case CameraType.Dynamic:
					Cameras.UpdateCamera();
					break;
				case CameraType.Following:
					Cameras.UpdateCameraFollowing();
					break;
				default:
					Cameras.UpdateCameraStatic();
					break;
			}

			_speedoMeter.Update(_moveSpeed, _topSpeed);


			
			var l1 = _carPosition + Vector3.Transform(new Vector3(Lights.CarLightXOffset, Lights.CarLightYOffset, Lights.CarLightZOffset), Matrix.CreateFromQuaternion(_carRotation));
			var l2 = _carPosition + Vector3.Transform(new Vector3(-Lights.CarLightXOffset, Lights.CarLightYOffset, Lights.CarLightZOffset), Matrix.CreateFromQuaternion(_carRotation));
			Lights.CarLightPositions = new Matrix(
				l1.X, l1.Y, l1.Z, 0,
				l2.X, l2.Y, l2.Z, 0,
				0, 0, 0, 0,
				0, 0, 0, 0
			);

			base.Update(gameTime);
		}

		

		private void ProcessKeyboard(GameTime gameTime)
		{
			float leftRightRot = 0;

			var turningSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
			turningSpeed *= 1.6f * _gameSpeed;
			var keys = Keyboard.GetState();

			// Pause
			if (gamePaused && keys.IsKeyDown(Keys.F2))
			{
				gamePaused = false;
			}
			else if (!gamePaused && keys.IsKeyDown(Keys.F1))
			{
				gamePaused = true;
			}
			

			// Change camera angle:
			if (keys.IsKeyDown(Keys.L))
				Cameras.CameraAngle -= Cameras.CameraChangeAngle;
			else if (keys.IsKeyDown(Keys.J))
				Cameras.CameraAngle += Cameras.CameraChangeAngle;
			else if (keys.IsKeyDown(Keys.K))
				Cameras.CameraAngle = 0;

			if (keys.IsKeyDown(Keys.I))
				Cameras.CameraType = CameraType.Dynamic;
			else if (keys.IsKeyDown(Keys.O))
				Cameras.CameraType = CameraType.Following;
			else if (keys.IsKeyDown(Keys.P))
				Cameras.CameraType = CameraType.Static;

			// Change light count:
			if (keys.IsKeyDown(Keys.OemPlus) && Lights.LightsCount < Lights.MaxLights && !_plusPressed)
			{
				Lights.LightsCount++;
				_plusPressed = true;
			}
			else if (keys.IsKeyUp(Keys.OemPlus) && _plusPressed)
			{
				_plusPressed = false;
			}
			if (keys.IsKeyDown(Keys.OemMinus) && Lights.LightsCount > 0 && !_minusPressed)
			{
				Lights.LightsCount--;
				_minusPressed = true;
			}
			else if (keys.IsKeyUp(Keys.OemMinus) && _minusPressed)
			{
				_minusPressed = false;
			}

			// Change light model:
			if (keys.IsKeyDown(Keys.D1))
				Lights.LightingModel = "Phong";
			else if (keys.IsKeyDown(Keys.D2))
				Lights.LightingModel = "Blinn";

			// Change shading model:
			if (keys.IsKeyDown(Keys.Q))
				Lights.ShadingModel = "Flat";
			else if (keys.IsKeyDown(Keys.W))
				Lights.ShadingModel = "Gouraud";
			else if (keys.IsKeyDown(Keys.E))
				Lights.ShadingModel = "Phong";

			if (gamePaused)
				return;
			
			// Turn
			if (keys.IsKeyDown(Keys.Right) && _moveSpeed > 0.00005f || keys.IsKeyDown(Keys.Left) && _moveSpeed < -0.00005f)
				leftRightRot += turningSpeed;
			if (keys.IsKeyDown(Keys.Left) && _moveSpeed > 0.00005f || keys.IsKeyDown(Keys.Right) && _moveSpeed < -0.00005f)
				leftRightRot -= turningSpeed;

			// Accelerate
			if (keys.IsKeyDown(Keys.Up) && _moveSpeed <= _topSpeed)
				_moveSpeed += _acceleration;
			else if (keys.IsKeyDown(Keys.Down) && _moveSpeed >= -_topSpeed)
				_moveSpeed -= _acceleration;
			else if (_moveSpeed > 0.0f && _moveSpeed >= _speedEpsilon)
				_moveSpeed -= _acceleration;
			else if (_moveSpeed < 0.0f && _moveSpeed <= -_speedEpsilon)
				_moveSpeed += _acceleration;
			else if (Math.Abs(_moveSpeed) < _speedEpsilon)
				_moveSpeed = 0;

			// Horn:
			if (keys.IsKeyDown(Keys.H) && !_hornPressed)
			{
				_soundEngineInstance.Play();
				_hornPressed = true;
			}
			else if (keys.IsKeyDown(Keys.H) && _hornPressed)
			{
				_hornPressed = false;
			}

			var additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, -1, 0), leftRightRot);
			_carRotation *= additionalRot;
		}

		private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
		{
			var addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);
			position += addVector * speed;
		}


		#endregion

		#region Setup

		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = 1920;
			_graphics.PreferredBackBufferHeight = 1080;
			_graphics.IsFullScreen = false;
			_graphics.ApplyChanges();
			Window.Title = "Project Cars";

			LoadFloorPlan();
			SetUpWaypoints();
			
			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_font = Content.Load<SpriteFont>("Courier-New");
			_device = _graphics.GraphicsDevice;

			_effect = Content.Load<Effect>("effects");

			_sceneryTexture = Content.Load<Texture2D>("texturemap");
			_speedometerNeedleTexture = Content.Load<Texture2D>("needle");
			_speedometerTexture = Content.Load<Texture2D>("meter");

			_skyboxModel = LoadModel("skybox");
			_carModel = LoadModel("car");
			_fenceModel = LoadModel("fence");
			_treeModel = LoadModel("Tree");
			_sphereModel = LoadModel("Highfbx");
			_bambooHouseModel = LoadModel("Bambo_House");
			_slrModel = LoadModel("SLR");
			
			SetUpCamera();
			SetUpVertices();

			SetUpBoundingBoxes();

			Lights.SetUpLightData(_carStartPosition);

			_speedoMeter = new MeterComponent(
				new Vector2(GraphicsDevice.Viewport.Width * 3 / 4.0f, GraphicsDevice.Viewport.Height * 5 / 8.0f),
				_speedometerTexture, _speedometerNeedleTexture, _spriteBatch, 0.6f);

			_engineSound = Content.Load<SoundEffect>("engineSound");
			_soundEngineInstance = _engineSound.CreateInstance();
			_soundEngineInstance.IsLooped = false;
		}

		private Model LoadModel(string assetName)
		{
			Model newModel = Content.Load<Model>(assetName);
			foreach (ModelMesh mesh in newModel.Meshes)
			{
				foreach (ModelMeshPart meshPart in mesh.MeshParts)
				{
					if ((meshPart.Effect as BasicEffect).Texture == null)
					{
						_effect.Parameters["xUseColors"].SetValue(true);
						var clr = (meshPart.Effect as BasicEffect).DiffuseColor;
						if ((double) clr.X == 1.0 && (double) clr.Y == 1.0)
						{
							if (assetName == "car")
								_effect.Parameters["xDiffuseColor"].SetValue(new Vector4(249/255f, 187/255f, 0.0f, 1f));
							else
								_effect.Parameters["xDiffuseColor"].SetValue(new Vector4(249 / 255f, 0, 0.2f, 1f));
						}
						else
							_effect.Parameters["xDiffuseColor"].SetValue(new Vector4((meshPart.Effect as BasicEffect).DiffuseColor, 1));
					}
					else
					{
						_effect.Parameters["xUseColors"].SetValue(false);
						_effect.Parameters["xTexture"].SetValue((meshPart.Effect as BasicEffect).Texture);
					}

					meshPart.Effect = _effect.Clone();
				}
			}
			return newModel;
		}
		

		

		private void SetUpCamera()
		{
			_viewMatrix = Matrix.CreateLookAt(new Vector3(20, 13, -5), new Vector3(8, 0, -7), new Vector3(0, 1, 0));
			_projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _device.Viewport.AspectRatio, 0.2f, 500.0f);
		}

		

		

		private void SetUpWaypoints()
		{
			var wplist = new List<Waypoint>();
			wplist.Add(new Waypoint(30.66f, -5.966733f));
			// 1
			for (float i = 0; i - 5.966733f >= -17.0f; i -= 0.05f)
				wplist.Add(new Waypoint(30.66f, i - 5.966733f));

			//2
			var angle = -0.35f;
			var z =  wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle > -1f; i -= 0.0075f)
			{
				wplist.Add(new Waypoint(30.66f + i, z + i, angle));
				angle -= 0.025f;
			}

			//3
			var x = wplist.Last().X;
            for (float i = 0; i < 12; i += _aiMoveSpeed)
				wplist.Add(new Waypoint(x - i, wplist.Last().Z, -1.55f));
			
			//4
			angle = -1.55f;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle < 0f; i += 0.0075f)
			{
				wplist.Add(new Waypoint(x - i * 2, z - i, angle));
				angle += 0.025f;
			}

			//5
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 12.75f; i += _aiMoveSpeed * 1.1f)
				wplist.Add(new Waypoint(x, z - i));

			//6
			angle = 0f;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle < 1f; i -= 0.0075f)
			{
				wplist.Add(new Waypoint(x - i, z + i, angle));
				angle += 0.025f;
			}

			//7
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 19.25f; i += _aiMoveSpeed)
				wplist.Add(new Waypoint(x + i, z, 1.55f));

			//8
			angle = 1.45f;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle > 0; i -= 0.0075f)
			{
				wplist.Add(new Waypoint(x - i, z + i, angle));
				angle -= 0.025f;
			}

			//9
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 17.85; i += 0.1f)
				wplist.Add(new Waypoint(x, z - i));

			// 10
			angle = 0f;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle < 1f; i -= 0.0075f)
			{
				wplist.Add(new Waypoint(x - i, z + i, angle));
				angle += 0.025f;
			}

			// 11
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 8.89f; i += 0.1f)
				wplist.Add(new Waypoint(x + i, z, 1.55f));

			//12
			angle = 1.55f;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle < MathHelper.Pi; i -= 0.0075f)
			{
				wplist.Add(new Waypoint(x - i, z - i, angle));
				angle += 0.025f;
			}

			//13
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 13.038; i += 0.1f)
				wplist.Add(new Waypoint(x, z + i, MathHelper.Pi));

			//14
			angle = MathHelper.Pi;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle > 1.55f; i += 0.0075f)
			{
				wplist.Add(new Waypoint(x + i, z + i, angle));
				angle -= 0.025f;
			}

			//15
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 9.7f; i += 0.1f)
				wplist.Add(new Waypoint(x + i, z, 1.55f));

			//16
			angle = 1.55f;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle < MathHelper.Pi; i -= 0.0075f)
			{
				wplist.Add(new Waypoint(x - i, z - i, angle));
				angle += 0.025f;
			}

			//17
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 14.75f; i += 0.1f)
				wplist.Add(new Waypoint(x, z + i, MathHelper.Pi));

			//18
			angle = MathHelper.Pi;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle > 1.55f; i += 0.0075f)
			{
				wplist.Add(new Waypoint(x + i, z + i, angle));
				angle -= 0.025f;
			}

			//19
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 2f; i += 0.1f)
				wplist.Add(new Waypoint(x + i, z, 1.55f));

			//20
			angle = 1.55f;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle < MathHelper.Pi; i -= 0.0075f)
			{
				wplist.Add(new Waypoint(x - i, z - i, angle));
				angle += 0.025f;
			}

			//21
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 4.9f; i += 0.1f)
				wplist.Add(new Waypoint(x, z + i, MathHelper.Pi));

			//22
			angle = MathHelper.Pi;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle > 1.55f; i += 0.0075f)
			{
				wplist.Add(new Waypoint(x + i, z + i, angle));
				angle -= 0.025f;
			}

			//23 zepsuta numeracja
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 0.3f; i += _aiMoveSpeed)
				wplist.Add(new Waypoint(x + i, z, 1.55f));
			angle = 1.55f;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle < MathHelper.Pi; i -= 0.0075f)
			{
				wplist.Add(new Waypoint(x - i, z - i, angle));
				angle += 0.025f;
			}

			//24
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 8f; i += 0.1f)
				wplist.Add(new Waypoint(x, z + i, MathHelper.Pi));

			//25
			angle = MathHelper.Pi;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle > 1.55f; i -= 0.0075f)
			{
				wplist.Add(new Waypoint(x + i, z - i, -angle));
				angle -= 0.025f;
			}

			//26
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 30.1; i += 0.135f)
				wplist.Add(new Waypoint(x - i, z, -1.55f));

			//27
			angle = -1.55f;
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i >= -2.35072f && angle < 0f; i += 0.0075f)
			{
				wplist.Add(new Waypoint(x - i * 2, z - i, angle));
				angle += 0.025f;
			}

			//28
			x = wplist.Last().X;
			z = wplist.Last().Z;
			for (float i = 0; i < 1; i += 0.1f)
				wplist.Add(new Waypoint(x, z - i));

			_waypoints = wplist.ToArray();
		}

		public Game()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		#endregion

		#region Draw

		protected override void Draw(GameTime gameTime)
		{
			_device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);

			DrawHousesAndCars();
			DrawFences();
			DrawTrees();
			DrawAI();

			DrawHUD();

			base.Draw(gameTime);
		}

		private void DrawHUD()
		{
			var prevBlend = _graphics.GraphicsDevice.BlendState;
			var prevDepth = _graphics.GraphicsDevice.DepthStencilState;
			var text = Lights.LightsCount > 1 ? "lights" : "light";
            _spriteBatch.Begin();
			_spriteBatch.DrawString(_font, $"Lighting: {Lights.LightingModel} ({Lights.LightsCount} {text})\nShading: {Lights.ShadingModel}",
				new Vector2(GraphicsDevice.Viewport.Width * 1 / 19.0f, GraphicsDevice.Viewport.Height * 7 / 8.0f), Color.Red);
			_spriteBatch.End();
			_speedoMeter.Draw();
			_graphics.GraphicsDevice.BlendState = prevBlend;
			_graphics.GraphicsDevice.DepthStencilState = prevDepth;
		}

		

		private void DrawModel(Model model, Matrix wMatrix)
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
	
		private void DrawAI()
		{
			Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, -1, 0), _waypoints[_waypointIndex].Angle);

			var scale = 0.025f;
			var carMatrix = Matrix.CreateScale(scale) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(Quaternion.Identity * additionalRot) *
							Matrix.CreateTranslation(new Vector3(_waypoints[_waypointIndex].X, 0, _waypoints[_waypointIndex].Z));

			if (!gamePaused)
				_waypointIndex = (_waypointIndex + 1)%_waypoints.Length;

            DrawModel(_carModel, carMatrix);

		}
		#endregion
	}
}
