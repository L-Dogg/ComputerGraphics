using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RacingGame
{
	enum CameraType
	{
		Static,
		Following,
		Dynamic
	}

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

	public class Game1 : Game
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

		private int[,] _floorPlan;
		private Texture2D _sceneryTexture;
		private const int FloorTypes = 8;
		private BoundingBox _raceTrackBox;
		private VertexBuffer _vertexBuffer;

		private Model _skyboxModel;
		private Model _carModel;
		private Model _fenceModel;
		private Model _treeModel;
		private Model _waterTankModel;
		private Model _bambooHouseModel;
		private Model _slrModel;
		private Model[] _stoneModels = new Model[5];

		private readonly Vector3 _carStartPosition = new Vector3(30.39419f, 0, -5.966733f);
		private Vector3 _carPosition = new Vector3(30.39419f, 0, -5.966733f);
		private Quaternion _carRotation = Quaternion.Identity;

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
		private readonly float[] _treeHeights = {0.37f, 0.63f, 0.25f, 0.15f, 0.44f};

		private readonly Vector3 _waterTankPosition = new Vector3(15, 0, -17);

		private readonly Vector3[] _houseesPositions =
		{
			new Vector3(41.17903f, 0, -15.565f),
			new Vector3(46.55457f, 0, -12.63017f)
		};

		private CameraType _cameraType = CameraType.Dynamic;
		private Vector3 _cameraUp;
		private Vector3 _cameraPosition;
		private Quaternion _cameraRotation = Quaternion.Identity;
		private readonly float _cameraChangeAngle = 0.005f;
		private float _cameraAngle = 0;

		private readonly float _acceleration = 0.0005f;
		private readonly float _topSpeed = 0.07f;
		private readonly float _speedEpsilon = 0.005f;
		private float _moveSpeed;
		private float _gameSpeed = 1.0f;
		private enum CollisionType { None, Boundary }

		private Matrix _lightPositions;
		private int _lightsCount = 1;
		private readonly int MaxLights = 4;
		private string _shadingModel = "Flat";
		private string _lightingModel = "Phong";
		private string CurrentModel => $"{_shadingModel}{_lightingModel}";

		private Waypoint[] _waypoints;
		private readonly float _aiMoveSpeed = 0.078f;
		private int _waypointIndex = 0;

		private bool _plusPressed;
		private bool _minusPressed;
		private bool _escapePressed;
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

			if (_cameraType == CameraType.Dynamic)
				UpdateCamera();
			else if(_cameraType == CameraType.Following)
				UpdateCameraFollowing();
			else 
				UpdateCameraStatic();

			base.Update(gameTime);
		}

		private void UpdateCamera()
		{
			_cameraRotation = Quaternion.Lerp(_cameraRotation, _carRotation, 0.08f);

			_cameraPosition = new Vector3(0, 0.3f, 0.8f);
			_cameraPosition = Vector3.Transform(_cameraPosition, Matrix.CreateFromAxisAngle(new Vector3(0, -1, 0), _cameraAngle));
			_cameraPosition = Vector3.Transform(_cameraPosition, Matrix.CreateFromQuaternion(_cameraRotation));
			_cameraPosition += _carPosition;

			_cameraUp = new Vector3(0, 1, 0);
			_cameraUp = Vector3.Transform(_cameraUp, Matrix.CreateFromQuaternion(_cameraRotation));

			_viewMatrix = Matrix.CreateLookAt(_cameraPosition, _carPosition, _cameraUp);
		}

		private void UpdateCameraFollowing()
		{
			_cameraRotation = Quaternion.Lerp(_cameraRotation, _carRotation, 0.08f);

			_cameraPosition = new Vector3(30, 5.5f, 0.8f);
			_cameraPosition = Vector3.Transform(_cameraPosition, Matrix.CreateFromAxisAngle(new Vector3(0, -1, 0), 0));
			_cameraPosition = Vector3.Transform(_cameraPosition, Matrix.CreateFromQuaternion(Quaternion.Identity));
			
			_cameraUp = new Vector3(0, 1, 0);
			_cameraUp = Vector3.Transform(_cameraUp, Matrix.CreateFromQuaternion(_cameraRotation));

			_viewMatrix = Matrix.CreateLookAt(_cameraPosition, _carPosition, _cameraUp);
		}

		private void UpdateCameraStatic()
		{
			_cameraPosition = new Vector3(30, 5.5f, 0.8f);
			_cameraUp = new Vector3(0, 1, 0);
			_viewMatrix = Matrix.CreateLookAt(_cameraPosition, _carStartPosition + new Vector3(2, 2.4f, 0), _cameraUp);
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
				_cameraAngle -= _cameraChangeAngle;
			else if (keys.IsKeyDown(Keys.J))
				_cameraAngle += _cameraChangeAngle;
			else if (keys.IsKeyDown(Keys.K))
				_cameraAngle = 0;

			if (keys.IsKeyDown(Keys.I))
				_cameraType = CameraType.Dynamic;
			else if (keys.IsKeyDown(Keys.O))
				_cameraType = CameraType.Following;
			else if (keys.IsKeyDown(Keys.P))
				_cameraType = CameraType.Static;

			// Change light count:
			if (keys.IsKeyDown(Keys.OemPlus) && _lightsCount < MaxLights && !_plusPressed)
			{
				_lightsCount++;
				_plusPressed = true;
			}
			else if (keys.IsKeyUp(Keys.OemPlus) && _plusPressed)
			{
				_plusPressed = false;
			}
			if (keys.IsKeyDown(Keys.OemMinus) && _lightsCount > 1 && !_minusPressed)
			{
				_lightsCount--;
				_minusPressed = true;
			}
			else if (keys.IsKeyUp(Keys.OemMinus) && _minusPressed)
			{
				_minusPressed = false;
			}

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
			_skyboxModel = LoadModel("skybox");
			_carModel = LoadModel("car");
			
			_fenceModel = LoadModel("fence");
			_treeModel = LoadModel("Tree");
			_waterTankModel = LoadModel("Water_Tank_fbx");
			_bambooHouseModel = LoadModel("Bambo_House");
			_slrModel = LoadModel("SLR");
			
			//_stoneModels[2] = LoadModel($"stone_2");

			SetUpCamera();
			SetUpVertices();

			SetUpBoundingBoxes();
			SetUpLightData();
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
		
		private void LoadFloorPlan()
		{
			_floorPlan = new[,]
			 {
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,2,2,2,2,2,2,2,2,2,2,2,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,6,2,7,2,2,2,2,2,2,2,2,2,2,4,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,2,2,2,2,2,2,2,2,2,2,2,2,2,4,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,1,0,0,0,0,0,0,0,0,0,6,2,2,2,2,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,5,2,2,2,2,2,2,2,2,2,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},

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
			float textureCount = FloorTypes;
            for (var x = 0; x < raceTrackWidth; x++)
			{
				for (var z = 0; z < raceTrackLength; z++)
				{
					for (var i = 0; i < FloorTypes; i++)
					{
						if (_floorPlan[x, z] == i)
						{
							verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(0, 1, 0), new Vector2(i / textureCount, 1)));
							verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z - 1), new Vector3(0, 1, 0), new Vector2(i / textureCount, 0)));
							verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(0, 1, 0), new Vector2((i+1) / textureCount, 1)));

							verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z - 1), new Vector3(0, 1, 0), new Vector2(i / textureCount, 0)));
							verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z - 1), new Vector3(0, 1, 0), new Vector2((i + 1) / textureCount, 0)));
							verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(0, 1, 0), new Vector2((i + 1) / textureCount, 1)));
						}
                    }
				}
			}

			_vertexBuffer = new VertexBuffer(_device, VertexPositionNormalTexture.VertexDeclaration, verticesList.Count, BufferUsage.WriteOnly);
			_vertexBuffer.SetData(verticesList.ToArray());
		}

		private void SetUpLightData()
		{
			_lightPositions = new Matrix(
				_carStartPosition.X + 0.5f, _carStartPosition.Y + 13, _carStartPosition.Z, 0,
                18, 13, -17, 0,
				33.28685f, 13, -51.12287f, 0,
				62.82345f, 13, -18.72201f, 0
			);
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

			var scale = 0.025f;
			var carMatrix = Matrix.CreateScale(scale, scale,scale) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(_carRotation) *
							Matrix.CreateTranslation(_carPosition);

			var stopMatrix = Matrix.CreateScale(scale/10, scale/10, scale/10) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(Quaternion.Identity) *
							Matrix.CreateTranslation(_waterTankPosition);

			DrawModel(_carModel, carMatrix);
			DrawModel(_waterTankModel, stopMatrix);

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
			_spriteBatch.Begin();
			_spriteBatch.DrawString(_font, $"{Math.Round(_moveSpeed * 1833)}km/h",
				new Vector2(GraphicsDevice.Viewport.Width * 3 / 4.0f, GraphicsDevice.Viewport.Height * 7 / 8.0f), Color.Red);
			_spriteBatch.DrawString(_font, $"Lighting: {_lightingModel} ({_lightsCount} lights)\nShading: {_shadingModel}",
				new Vector2(GraphicsDevice.Viewport.Width * 1 / 19.0f, GraphicsDevice.Viewport.Height * 7 / 8.0f), Color.Red);
			_spriteBatch.End();
			_graphics.GraphicsDevice.BlendState = prevBlend;
			_graphics.GraphicsDevice.DepthStencilState = prevDepth;
		}

		private void DrawFences()
		{
			var tmpMatrix = Matrix.CreateScale(0.005f) *
			                Matrix.CreateRotationY(MathHelper.PiOver2) *
			                Matrix.CreateFromQuaternion(Quaternion.Identity);
            for (var x = 0f; x < 63.8f; x+=0.65f)
			{
				var fenceMatrix = tmpMatrix * Matrix.CreateTranslation(0.1f + x, 0, 0.01f);
				DrawFence(_fenceModel, fenceMatrix);
				fenceMatrix = tmpMatrix * Matrix.CreateTranslation(0.1f + x, 0, -60f);
				DrawFence(_fenceModel, fenceMatrix);
			}

			tmpMatrix = Matrix.CreateScale(0.005f) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(Quaternion.Identity);

			for (var x = 0f; x < 63.8f; x += 0.65f)
			{
				var fenceMatrix = tmpMatrix * Matrix.CreateTranslation(0.1f, 0, 0.01f - x);
				DrawFence(_fenceModel, fenceMatrix);
				fenceMatrix = tmpMatrix * Matrix.CreateTranslation(65f, 0, -60f+x);
				DrawFence(_fenceModel, fenceMatrix);
			}
		}

		private void DrawTrees()
		{
			foreach (var startVector in _treePositions)
			{
				for (var i = 0; i < 5; i++)
				{
                    var treeMatrix = Matrix.CreateScale(_treeHeights[i % _treeHeights.Length]) *
							Matrix.CreateRotationY(MathHelper.Pi) *
							Matrix.CreateFromQuaternion(Quaternion.Identity) *
							Matrix.CreateTranslation(startVector + new Vector3(0.5f * i, 0, 0));
					DrawModel(_treeModel, treeMatrix);
				}
			}

			for (var i = 0; i < 12; i++)
			{
				var treeMatrix = Matrix.CreateScale(_treeHeights[i % _treeHeights.Length]) *
						Matrix.CreateRotationY(MathHelper.Pi) *
						Matrix.CreateFromQuaternion(Quaternion.Identity) *
						Matrix.CreateTranslation(_treePositions.Last() + new Vector3(-0.59f - 0.2f * i, 0, 0.25f * i));
				DrawModel(_treeModel, treeMatrix);
			}
			
        }

		private void DrawHousesAndCars()
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

					DrawModel(_bambooHouseModel, houseMatrix);
					DrawModel(_slrModel, carMatrix);
				}
			}
		}

		private void DrawFence(Model model, Matrix wMatrix)
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
					currentEffect.CurrentTechnique = currentEffect.Techniques[CurrentModel];

					currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
					currentEffect.Parameters["xView"].SetValue(_viewMatrix);
					currentEffect.Parameters["xProjection"].SetValue(_projectionMatrix);

					currentEffect.Parameters["Ka"].SetValue(0.5f);
					currentEffect.Parameters["Ks"].SetValue(0.5f);
					currentEffect.Parameters["Kd"].SetValue(0.75f);
					currentEffect.Parameters["A"].SetValue(15f);

					currentEffect.Parameters["xCamUp"].SetValue(_cameraUp);
					currentEffect.Parameters["xCamPos"].SetValue(_cameraPosition);
					currentEffect.Parameters["xLightPositions"].SetValue(_lightPositions);
					currentEffect.Parameters["xLightCount"].SetValue(_lightsCount);

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

		private void DrawModel(Model model, Matrix wMatrix)
		{
			var modelTransforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(modelTransforms);

			foreach (var mesh in model.Meshes)
			{
				foreach (var currentEffect in mesh.Effects)
				{
					var worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
					currentEffect.CurrentTechnique = currentEffect.Techniques[CurrentModel];

					currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
					currentEffect.Parameters["xView"].SetValue(_viewMatrix);
					currentEffect.Parameters["xProjection"].SetValue(_projectionMatrix);

					currentEffect.Parameters["Ka"].SetValue(0.5f);
					currentEffect.Parameters["Ks"].SetValue(0.5f);
					currentEffect.Parameters["Kd"].SetValue(0.75f);
					currentEffect.Parameters["A"].SetValue(15f);

					currentEffect.Parameters["xCamUp"].SetValue(_cameraUp);
					currentEffect.Parameters["xCamPos"].SetValue(_cameraPosition);
					currentEffect.Parameters["xLightPositions"].SetValue(_lightPositions);
					currentEffect.Parameters["xLightCount"].SetValue(_lightsCount);
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

			foreach (var mesh in _skyboxModel.Meshes)
			{
				foreach (var currentEffect in mesh.Effects)
				{
					var worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(_carPosition);
					currentEffect.CurrentTechnique = currentEffect.Techniques[CurrentModel];
					currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
					currentEffect.Parameters["xView"].SetValue(_viewMatrix);
					currentEffect.Parameters["xProjection"].SetValue(_projectionMatrix);

					currentEffect.Parameters["Ka"].SetValue(0.5f);
					currentEffect.Parameters["Ks"].SetValue(0.5f);
					currentEffect.Parameters["Kd"].SetValue(0.75f);
					currentEffect.Parameters["A"].SetValue(15f);

					currentEffect.Parameters["xCamUp"].SetValue(_cameraUp);
					currentEffect.Parameters["xCamPos"].SetValue(_cameraPosition);
					currentEffect.Parameters["xLightPositions"].SetValue(_lightPositions);
					currentEffect.Parameters["xLightCount"].SetValue(_lightsCount);
				}
				mesh.Draw();
			}

			dss = new DepthStencilState { DepthBufferEnable = true };
			_device.DepthStencilState = dss;
		}

		private void DrawRaceTrack()
		{
			_effect.Parameters["xUseColors"].SetValue(false);
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
			_effect.Parameters["xLightPositions"].SetValue(_lightPositions);
			_effect.Parameters["xLightCount"].SetValue(_lightsCount);

			foreach (var pass in _effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				_device.SetVertexBuffer(_vertexBuffer);
				_device.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertexBuffer.VertexCount / 3);
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
