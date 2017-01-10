using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RacingGame
{
	public class Game1 : Game
	{
		#region Private Fields
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		GraphicsDevice device;
		Effect effect;
		Texture2D texture;

		VertexPositionTexture[] vertices;

		Quaternion cameraRotation = Quaternion.Identity;
		Vector3 lightDirection = new Vector3(3, -2, 5);
		Matrix viewMatrix;
		Matrix projectionMatrix;

		int[,] floorPlan;

		VertexBuffer cityVertexBuffer;
		Texture2D sceneryTexture;
		int[] buildingHeights = { 0, 0, 0, 0, 0, 0 };
		BoundingBox[] buildingBoundingBoxes;

		Model skyboxModel;
		Texture2D[] skyboxTextures;

		Model xwingModel;
		Texture2D[] carTextures;

		Vector3 carPosition = new Vector3(8, 0.037f, -3);
		Quaternion carRotation = Quaternion.Identity;
		private float moveSpeed = 0;
		private float acceleration = 0.0005f;

        float gameSpeed = 1.0f;
		enum CollisionType { None, Building }

		Vector3 lightPos;
		float lightPower;
		float ambientPower;

		#endregion

		#region Update and Process Keyboard

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			ProcessKeyboard(gameTime);
			MoveForward(ref carPosition, carRotation, moveSpeed);

			var xwingSpere = new BoundingSphere(carPosition, 0.04f);
			if (CheckCollision(xwingSpere) != CollisionType.None)
			{
				moveSpeed = 0f;
				carRotation = Quaternion.Identity;
				carPosition = new Vector3(8, 0.037f, -3);
				gameSpeed /= 1.1f;
			}

			UpdateCamera();
			UpdateLightData();
			base.Update(gameTime);
		}

		private void UpdateLightData()
		{
			lightPos = new Vector3(-10, 4, -2);
			lightPower = 1.0f;
			ambientPower = 0.2f;
		}

		private void UpdateCamera()
		{
			cameraRotation = Quaternion.Lerp(cameraRotation, carRotation, 0.08f);

			var campos = new Vector3(0, 0.33f, 0.88f);
			campos = Vector3.Transform(campos, Matrix.CreateFromQuaternion(cameraRotation));
			campos += carPosition;

			var camup = new Vector3(0, 1, 0);
			camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(cameraRotation));

			viewMatrix = Matrix.CreateLookAt(campos, carPosition, camup);
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 500.0f);
		}

		private void ProcessKeyboard(GameTime gameTime)
		{
			float leftRightRot = 0;

			var turningSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
			turningSpeed *= 1.6f * gameSpeed;
			var keys = Keyboard.GetState();
			if (keys.IsKeyDown(Keys.Right) && moveSpeed != 0)
				leftRightRot += turningSpeed;
			if (keys.IsKeyDown(Keys.Left) && moveSpeed != 0)
				leftRightRot -= turningSpeed;

			if (keys.IsKeyDown(Keys.Up))
				moveSpeed += acceleration;
			else if (keys.IsKeyDown(Keys.Down))
				moveSpeed -= acceleration;
			else if (moveSpeed > 0.0f)
				moveSpeed -= acceleration;
			else if (moveSpeed < 0.0f)
				moveSpeed += acceleration;

			var additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, -1, 0), leftRightRot);
			carRotation *= additionalRot;
		}

		private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
		{
			var addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);
			position += addVector * speed;
		}

		private CollisionType CheckCollision(BoundingSphere sphere)
		{
			for (var i = 0; i < buildingBoundingBoxes.Length; i++)
				if (buildingBoundingBoxes[i].Contains(sphere) != ContainmentType.Disjoint)
					return CollisionType.Building;

			return CollisionType.None;
		}

		#endregion

		#region Initialize Load SetUp

		protected override void Initialize()
		{
			graphics.PreferredBackBufferWidth = 500;
			graphics.PreferredBackBufferHeight = 500;
			graphics.IsFullScreen = false;
			graphics.ApplyChanges();
			Window.Title = "Racing";
			LoadFloorPlan();
			lightDirection.Normalize();
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			device = graphics.GraphicsDevice;

			effect = Content.Load<Effect>("effects");
			sceneryTexture = Content.Load<Texture2D>("texturemap");
			skyboxModel = LoadModel("skybox2", out skyboxTextures);
			xwingModel = LoadModel("car", out carTextures);

			SetUpCamera();
			SetUpVertices();

			SetUpBoundingBoxes();

		}

		private Model LoadModel(string assetName, out Texture2D[] textures)
		{
			var newModel = Content.Load<Model>(assetName);
			textures = new Texture2D[7];
			var i = 0;
			foreach (var mesh in newModel.Meshes)
				foreach (BasicEffect currentEffect in mesh.Effects)
					textures[i++] = currentEffect.Texture;

			foreach (var mesh in newModel.Meshes)
				foreach (var meshPart in mesh.MeshParts)
					meshPart.Effect = effect.Clone();

			return newModel;
		}

		private void LoadFloorPlan()
		{
			floorPlan = new int[,]
			 {
				  {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
				  {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			 };
		}

		private void SetUpBoundingBoxes()
		{
			var cityWidth = floorPlan.GetLength(0);
			var cityLength = floorPlan.GetLength(1);


			var bbList = new List<BoundingBox>();
			for (var x = 0; x < cityWidth; x++)
			{
				for (var z = 0; z < cityLength; z++)
				{
					var buildingType = floorPlan[x, z];
					if (buildingType != 0)
					{
						var buildingHeight = buildingHeights[buildingType];
						var buildingPoints = new Vector3[2];
						buildingPoints[0] = new Vector3(x, 0, -z);
						buildingPoints[1] = new Vector3(x + 1, buildingHeight, -z - 1);
						var buildingBox = BoundingBox.CreateFromPoints(buildingPoints);
						bbList.Add(buildingBox);
					}
				}
			}
			buildingBoundingBoxes = bbList.ToArray();


			var boundaryPoints = new Vector3[2];
			boundaryPoints[0] = new Vector3(0, 0, 0);
			boundaryPoints[1] = new Vector3(cityWidth, 20, -cityLength);
			BoundingBox.CreateFromPoints(boundaryPoints);
		}

		private void SetUpCamera()
		{
			viewMatrix = Matrix.CreateLookAt(new Vector3(20, 13, -5), new Vector3(8, 0, -7), new Vector3(0, 1, 0));
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 500.0f);
		}

		private void SetUpVertices()
		{
			var differentBuildings = buildingHeights.Length - 1;
			float imagesInTexture = 1 + differentBuildings * 2;

			var cityWidth = floorPlan.GetLength(0);
			var cityLength = floorPlan.GetLength(1);


			var verticesList = new List<VertexPositionNormalTexture>();
			for (var x = 0; x < cityWidth; x++)
			{
				for (var z = 0; z < cityLength; z++)
				{
					var currentbuilding = floorPlan[x, z];

					//floor or ceiling
					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z), new Vector3(0, 1, 0), new Vector2(currentbuilding * 2 / imagesInTexture, 1)));
					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 1)));

					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, buildingHeights[currentbuilding], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 0)));
					verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, buildingHeights[currentbuilding], -z), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 1)));
				}
			}

			cityVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, verticesList.Count, BufferUsage.WriteOnly);
			cityVertexBuffer.SetData<VertexPositionNormalTexture>(verticesList.ToArray());
		}

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}
		#endregion

		#region Draw

		protected override void Draw(GameTime gameTime)
		{
			device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);

			DrawSkybox();
			DrawCity();
			var car1Matrix = Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateFromQuaternion(carRotation) * Matrix.CreateTranslation(carPosition);
			DrawModel(xwingModel, carTextures, car1Matrix, "Simplest");

			base.Draw(gameTime);
		}

		// Old Drawing method
		private void DrawModel()
		{
			var worldMatrix = Matrix.CreateScale(0.0005f, 0.0005f, 0.0005f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateFromQuaternion(carRotation) * Matrix.CreateTranslation(carPosition);

			var xwingTransforms = new Matrix[xwingModel.Bones.Count];
			xwingModel.CopyAbsoluteBoneTransformsTo(xwingTransforms);
			foreach (var mesh in xwingModel.Meshes)
			{
				foreach (var currentEffect in mesh.Effects)
				{
					currentEffect.CurrentTechnique = currentEffect.Techniques["Colored"];
					currentEffect.Parameters["xWorld"].SetValue(xwingTransforms[mesh.ParentBone.Index] * worldMatrix);
					currentEffect.Parameters["xView"].SetValue(viewMatrix);
					currentEffect.Parameters["xProjection"].SetValue(projectionMatrix);
					currentEffect.Parameters["xEnableLighting"].SetValue(true);
					currentEffect.Parameters["xLightDirection"].SetValue(lightDirection);
					currentEffect.Parameters["xAmbient"].SetValue(0.5f);
				}
				mesh.Draw();
			}
		}

		private void DrawModel(Model model, Texture2D[] textures, Matrix wMatrix, string technique)
		{
			var modelTransforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(modelTransforms);
			var i = 0;
			foreach (var mesh in model.Meshes)
			{
				foreach (var currentEffect in mesh.Effects)
				{
					var worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
					currentEffect.CurrentTechnique = currentEffect.Techniques[technique];
					currentEffect.Parameters["xWorldViewProjection"].SetValue(worldMatrix * viewMatrix * projectionMatrix);
					currentEffect.Parameters["xTexture"].SetValue(textures[i++]);
					currentEffect.Parameters["xWorld"].SetValue(modelTransforms[mesh.ParentBone.Index] * worldMatrix);
					currentEffect.Parameters["xLightPos"].SetValue(lightPos);
					currentEffect.Parameters["xLightPower"].SetValue(lightPower);
					currentEffect.Parameters["xAmbient"].SetValue(ambientPower);
				}
				mesh.Draw();
			}
		}

		private void DrawSkybox()
		{
			var ss = new SamplerState();
			ss.AddressU = TextureAddressMode.Clamp;
			ss.AddressV = TextureAddressMode.Clamp;
			device.SamplerStates[0] = ss;

			var dss = new DepthStencilState { DepthBufferEnable = false };
			device.DepthStencilState = dss;

			var skyboxTransforms = new Matrix[skyboxModel.Bones.Count];
			skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
			var i = 0;
			foreach (var mesh in skyboxModel.Meshes)
			{
				foreach (var currentEffect in mesh.Effects)
				{
					var worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(carPosition);
					currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
					currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
					currentEffect.Parameters["xView"].SetValue(viewMatrix);
					currentEffect.Parameters["xProjection"].SetValue(projectionMatrix);
					currentEffect.Parameters["xTexture"].SetValue(skyboxTextures[i++]);
				}
				mesh.Draw();
			}

			dss = new DepthStencilState();
			dss.DepthBufferEnable = true;
			device.DepthStencilState = dss;
		}

		private void DrawCity()
		{
			effect.CurrentTechnique = effect.Techniques["Textured"];
			effect.Parameters["xWorld"].SetValue(Matrix.Identity);
			effect.Parameters["xView"].SetValue(viewMatrix);
			effect.Parameters["xProjection"].SetValue(projectionMatrix);
			effect.Parameters["xTexture"].SetValue(sceneryTexture);
			effect.Parameters["xEnableLighting"].SetValue(true);
			effect.Parameters["xLightDirection"].SetValue(lightDirection);
			effect.Parameters["xAmbient"].SetValue(0.5f);

			foreach (var pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				device.SetVertexBuffer(cityVertexBuffer);
				device.DrawPrimitives(PrimitiveType.TriangleList, 0, cityVertexBuffer.VertexCount / 3);
			}
		}

		#endregion
	}
}