using System;
using System.Drawing;
using System.Collections;

namespace Invasion
{
	/// <summary>
	/// Bullet class
	/// </summary>
	public class Bullet
	{
		private int	x=0;
		private int y=0;
		private Rectangle rcLastPos;
		private int iType=0;
		private int	frame=0;
		private int	iClipTop;

		public Bullet()
		{
			// 
			// TODO: Add constructor logic here
			//
		}
		public void SetXY(int xpos, int ypos)
		{
			x=xpos;
			y=ypos;
			rcLastPos = new Rectangle(x,y,20,20);
		}
		public int Type // Get_Inc and Set_Inc
		{
			get { return iType; }
			set 
			{
				iType = value;
			}
		}
		public int X // Get_Inc and Set_Inc
		{
			get { return x; }
			set 
			{
				x = value;
			}
		}		
		public int Y // Get_Inc and Set_Inc
		{
			get { return y+iClipTop; }
			set 
			{
				y = value;
			}
		}	
		public int Power // Get_Inc and Set_Inc
		{
			get 
			{ 
				switch(iType)
				{
					case 0:
						return 2;
					case 2:
						return 1;
				}
				return 1;			}
		}	

		public int ClipTop // Get_Inc and Set_Inc
		{
			get { return iClipTop; }
		}
		public void Move(int Qtde)
		{
			int newy=y;
			int newx=x;
			if (newy < 0)
				newy = 0;
			if (newy+20 > 455)
				newy = 455-20;
			rcLastPos = new Rectangle(x,y,20,20);
			y -= Qtde;
		}
		public Rectangle Draw()
		{
			Console.WriteLine("bullet draw");
			Rectangle rcRect = new Rectangle(0,0,0,0);
			int	iClipBottom;
		
			if (y < 0)
				iClipTop = y * -1;
			else
				iClipTop = 0;

			if (y+20 > 455)
				iClipBottom = y+20-455;
			else
				iClipBottom = 0;

			if (iType == 2)
				frame = 0;

			if(iClipTop<20)
			{
				rcRect = new Rectangle(frame * 20,iClipTop,20,20-iClipBottom-iClipTop);

				frame++;

				if (frame == 11 && iType == 0)
					frame = 0;
				if (frame == 20 && iType == 1)
					frame = 0;
			}
			//hRet = lpOrigin->BltFast(x + iClipTop, y, lpSource, &rcRect, DDBLTFAST_SRCCOLORKEY);
			return rcRect;
		}
	}
}
