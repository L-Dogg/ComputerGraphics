using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RacingGame
{
	public enum CameraType
	{
		Static,
		Following,
		Dynamic
	}

	public static class Cameras
	{
		public static CameraType CameraType { get; set; } = CameraType.Dynamic;
		public static Vector3 CameraUp { get; set; }
		public static Vector3 CameraPosition { get; set; }
		public static Quaternion CameraRotation { get; set; } = Quaternion.Identity;
		public static float CameraChangeAngle { get; } = 0.005f;
		public static float CameraAngle { get; set; } = 0;

		public static Matrix UpdateCamera(Vector3 position)
		{
			Cameras.CameraRotation = Quaternion.Lerp(Cameras.CameraRotation, position, 0.08f);

			Cameras.CameraPosition = new Vector3(0, 0.3f, 0.8f);
			Cameras.CameraPosition = Vector3.Transform(Cameras.CameraPosition, Matrix.CreateFromAxisAngle(new Vector3(0, -1, 0), Cameras.CameraAngle));
			Cameras.CameraPosition = Vector3.Transform(Cameras.CameraPosition, Matrix.CreateFromQuaternion(Cameras.CameraRotation));
			Cameras.CameraPosition += position;

			Cameras.CameraUp = new Vector3(0, 1, 0);
			Cameras.CameraUp = Vector3.Transform(Cameras.CameraUp, Matrix.CreateFromQuaternion(Cameras.CameraRotation));

			return Matrix.CreateLookAt(Cameras.CameraPosition, position, Cameras.CameraUp);
		}

		public static Matrix UpdateCameraFollowing(Vector3 position)
		{
			Cameras.CameraRotation = Quaternion.Lerp(Cameras.CameraRotation, position, 0.08f);

			Cameras.CameraPosition = new Vector3(30, 5.5f, 0.8f);
			Cameras.CameraPosition = Vector3.Transform(Cameras.CameraPosition, Matrix.CreateFromAxisAngle(new Vector3(0, -1, 0), 0));
			Cameras.CameraPosition = Vector3.Transform(Cameras.CameraPosition, Matrix.CreateFromQuaternion(Quaternion.Identity));

			Cameras.CameraUp = new Vector3(0, 1, 0);
			Cameras.CameraUp = Vector3.Transform(Cameras.CameraUp, Matrix.CreateFromQuaternion(Cameras.CameraRotation));

			return Matrix.CreateLookAt(Cameras.CameraPosition, position, Cameras.CameraUp);
		}

		public static Matrix UpdateCameraStatic(Vector3 position)
		{
			Cameras.CameraPosition = new Vector3(30, 5.5f, 0.8f);
			Cameras.CameraUp = new Vector3(0, 1, 0);
			return Matrix.CreateLookAt(Cameras.CameraPosition, position + new Vector3(2, 2.4f, 0), Cameras.CameraUp);
		}
	}
}
