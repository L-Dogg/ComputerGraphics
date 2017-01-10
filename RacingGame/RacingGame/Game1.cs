using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RacingGame
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		GraphicsDevice device;
		Effect effect;
		Texture2D texture;

		Quaternion cameraRotation = Quaternion.Identity;

		VertexPositionTexture[] vertices;

		Matrix viewMatrix;
		Matrix projectionMatrix;

		int[,] floorPlan;

		VertexBuffer cityVertexBuffer;

		Texture2D sceneryTexture;

		int[] buildingHeights = { 0, 0, 0, 0, 0, 0 };

		Model xwingModel;

		Vector3 lightDirection = new Vector3(3, -2, 5);

		Vector3 xwingPosition = new Vector3(8, 0.037f, -3);
		Quaternion xwingRotation = Quaternion.Identity;

		float gameSpeed = 1.0f;

		BoundingBox[] buildingBoundingBoxes;
		BoundingBox completeCityBox;

		enum CollisionType { None, Building }
		

		Texture2D[] skyboxTextures;
		Model skyboxModel;

		//Model carModel;
		Texture2D[] carTextures;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			graphics.PreferredBackBufferWidth = 500;
			graphics.PreferredBackBufferHeight = 500;
			graphics.IsFullScreen = false;
			graphics.ApplyChanges();
			Window.Title = "Riemer's XNA Tutorials -- 3D Series 2";
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

			//targetModel = LoadModel("target");

			SetUpCamera();

			SetUpVertices();

			xwingModel = LoadModel("car", out carTextures);

			SetUpBoundingBoxes();

			skyboxModel = LoadModel("skybox2", out skyboxTextures);
		}

		private void DrawSkybox()
		{
			SamplerState ss = new SamplerState();
			ss.AddressU = TextureAddressMode.Clamp;
			ss.AddressV = TextureAddressMode.Clamp;
			device.SamplerStates[0] = ss;

			DepthStencilState dss = new DepthStencilState();
			dss.DepthBufferEnable = false;
			device.DepthStencilState = dss;

			Matrix[] skyboxTransforms = new Matrix[skyboxModel.Bones.Count];
			skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
			int i = 0;
			foreach (ModelMesh mesh in skyboxModel.Meshes)
			{
				foreach (Effect currentEffect in mesh.Effects)
				{
					Matrix worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(xwingPosition);
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

		private CollisionType CheckCollision(BoundingSphere sphere)
		{
			for (int i = 0; i < buildingBoundingBoxes.Length; i++)
				if (buildingBoundingBoxes[i].Contains(sphere) != ContainmentType.Disjoint)
					return CollisionType.Building;

			return CollisionType.None;
		}

		private void SetUpBoundingBoxes()
		{
			int cityWidth = floorPlan.GetLength(0);
			int cityLength = floorPlan.GetLength(1);


			List<BoundingBox> bbList = new List<BoundingBox>();
			for (int x = 0; x < cityWidth; x++)
			{
				for (int z = 0; z < cityLength; z++)
				{
					int buildingType = floorPlan[x, z];
					if (buildingType != 0)
					{
						int buildingHeight = buildingHeights[buildingType];
						Vector3[] buildingPoints = new Vector3[2];
						buildingPoints[0] = new Vector3(x, 0, -z);
						buildingPoints[1] = new Vector3(x + 1, buildingHeight, -z - 1);
						BoundingBox buildingBox = BoundingBox.CreateFromPoints(buildingPoints);
						bbList.Add(buildingBox);
					}
				}
			}
			buildingBoundingBoxes = bbList.ToArray();


			Vector3[] boundaryPoints = new Vector3[2];
			boundaryPoints[0] = new Vector3(0, 0, 0);
			boundaryPoints[1] = new Vector3(cityWidth, 20, -cityLength);
			completeCityBox = BoundingBox.CreateFromPoints(boundaryPoints);
		}

		Vector3 lightPos;
		float lightPower;
		float ambientPower;

		private void UpdateLightData()
		{
			lightPos = new Vector3(-10, 4, -2);
			lightPower = 1.0f;
			ambientPower = 0.2f;
		}

		private void DrawModel()
		{
			Matrix worldMatrix = Matrix.CreateScale(0.0005f, 0.0005f, 0.0005f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateFromQuaternion(xwingRotation) * Matrix.CreateTranslation(xwingPosition);

			Matrix[] xwingTransforms = new Matrix[xwingModel.Bones.Count];
			xwingModel.CopyAbsoluteBoneTransformsTo(xwingTransforms);
			foreach (ModelMesh mesh in xwingModel.Meshes)
			{
				foreach (Effect currentEffect in mesh.Effects)
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
			Matrix[] modelTransforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(modelTransforms);
			int i = 0;
			foreach (ModelMesh mesh in model.Meshes)
			{
				foreach (Effect currentEffect in mesh.Effects)
				{
					Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
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
		
		private Model LoadModel(string assetName, out Texture2D[] textures)
		{
			Model newModel = Content.Load<Model>(assetName);
			textures = new Texture2D[7];
			int i = 0;
			foreach (ModelMesh mesh in newModel.Meshes)
				foreach (BasicEffect currentEffect in mesh.Effects)
					textures[i++] = currentEffect.Texture;

			foreach (ModelMesh mesh in newModel.Meshes)
				foreach (ModelMeshPart meshPart in mesh.MeshParts)
					meshPart.Effect = effect.Clone();

			return newModel;
		}

		private void UpdateCamera()
		{
			cameraRotation = Quaternion.Lerp(cameraRotation, xwingRotation, 0.1f);

			Vector3 campos = new Vector3(0, 0.33f, 0.88f);
			campos = Vector3.Transform(campos, Matrix.CreateFromQuaternion(cameraRotation));
			campos += xwingPosition;

			Vector3 camup = new Vector3(0, 1, 0);
			camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(cameraRotation));

			viewMatrix = Matrix.CreateLookAt(campos, xwingPosition, camup);
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 500.0f);
		}

		private void SetUpCamera()
		{
			viewMatrix = Matrix.CreateLookAt(new Vector3(20, 13, -5), new Vector3(8, 0, -7), new Vector3(0, 1, 0));
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 500.0f);
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

		private void SetUpVertices()
		{
			int differentBuildings = buildingHeights.Length - 1;
			float imagesInTexture = 1 + differentBuildings * 2;

			int cityWidth = floorPlan.GetLength(0);
			int cityLength = floorPlan.GetLength(1);


			List<VertexPositionNormalTexture> verticesList = new List<VertexPositionNormalTexture>();
			for (int x = 0; x < cityWidth; x++)
			{
				for (int z = 0; z < cityLength; z++)
				{
					int currentbuilding = floorPlan[x, z];

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

			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				device.SetVertexBuffer(cityVertexBuffer);
				device.DrawPrimitives(PrimitiveType.TriangleList, 0, cityVertexBuffer.VertexCount / 3);
			}
		}
		
		private void ProcessKeyboard(GameTime gameTime)
		{
			float leftRightRot = 0;

			float turningSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
			turningSpeed *= 1.6f * gameSpeed;
			KeyboardState keys = Keyboard.GetState();
			if (keys.IsKeyDown(Keys.Right))
				leftRightRot += turningSpeed;
			if (keys.IsKeyDown(Keys.Left))
				leftRightRot -= turningSpeed;


			float upDownRot = 0;
			if (keys.IsKeyDown(Keys.Up))
				moveSpeed += 0.0005f;
			else if (keys.IsKeyDown(Keys.Down))
				moveSpeed -= 0.0005f;
			else if (moveSpeed > 0.0f)
				moveSpeed -= 0.0005f;

			Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, -1, 0), leftRightRot);
			xwingRotation *= additionalRot;
		}

		private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
		{
			Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);
			position += addVector * speed;
		}

		private float moveSpeed = 0;

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			ProcessKeyboard(gameTime);
			MoveForward(ref xwingPosition, xwingRotation, moveSpeed);

			BoundingSphere xwingSpere = new BoundingSphere(xwingPosition, 0.04f);
			if (CheckCollision(xwingSpere) != CollisionType.None)
			{
				xwingPosition = new Vector3(8, 0.037f, -3);
				xwingRotation = Quaternion.Identity;
				moveSpeed = 0f;
				gameSpeed /= 1.1f;
			}

			UpdateCamera();
			UpdateLightData();
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);

			DrawSkybox();
			DrawCity();
			Matrix car1Matrix = Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateFromQuaternion(xwingRotation) * Matrix.CreateTranslation(xwingPosition);
			DrawModel(xwingModel, carTextures, car1Matrix, "Simplest");

			base.Draw(gameTime);
		}
	}
}