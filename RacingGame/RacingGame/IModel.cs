using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Xna.Framework;

namespace RacingGame
{
	public interface IModel
	{
		Vector3 Position { get; set; }

		void Draw(Matrix _viewMatrix, Matrix _projectionMatrix, Vector3 _carPosition);
        void Update();
		
	}
}
