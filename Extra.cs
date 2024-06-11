using System;
using System.Drawing;

namespace Invasion
{
	/// <summary>
	/// Extra Class
	/// </summary>
	public class Extra
	{
		private int	x=0;
		private int y=0;
		private int iType=0;
		private int	iClipTop=0;
		private int	iClipBottom=0;
		private int	frame=0;

		public Extra()
		{
			// 
			// TODO: Add constructor logic here
			//
		}
		public void SetXY(int xpos, int ypos)
		{
			x=xpos;
			y=ypos;
		}
		public int Type
		{
			get { return iType; }
			set { iType = value; }
		}
		public void Move(int Qtde)
		{
			y -= Qtde;
		}
		public Rectangle Draw()
		{
			Rectangle rcRect;
		
			if (y < 0)
				iClipTop = y * -1;
			else
				iClipTop = 0;

			if (y+25 > 455)
				iClipBottom = y+25-455;
			else
				iClipBottom = 0;

			rcRect = new Rectangle(frame*25,((iType - 1) * 25) + iClipTop,25,25-iClipBottom-iClipTop);

			frame++;

			if (frame == 19)
				frame = 0;
		
			return rcRect;
/*			while (1)
			{
				hRet = lpOrigin->BltFast(x + iClipTop, y, lpSource, &rcRect, DDBLTFAST_SRCCOLORKEY);
			
				if (hRet == DD_OK)
				{
					break;
				}
				if (hRet == DDERR_SURFACELOST)
				{
					return FALSE;
				}
				if (hRet != DDERR_WASSTILLDRAWING)
					break;
			}
			return TRUE;
*/
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

	}
}
