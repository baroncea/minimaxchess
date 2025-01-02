using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectIA
{
	public class Move
	{
		public int StartX { get; set; }
		public int StartY { get; set; }
		public int EndX { get; set; }
		public int EndY { get; set; }

		public Move(int startX, int startY, int endX, int endY)
		{
			StartX = startX;
			StartY = startY;
			EndX = endX;
			EndY = endY;
		}
	}
}
