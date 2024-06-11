using System;
using System.IO;
using System.Drawing;
using System.Collections;

namespace Invasion
{
	/// <summary>
	/// Summary description for Ufo.
	/// </summary>
	public class Ufo
	{
		private int		x=0;
		private int		y=0;
		private int		mov=0;
		private int		amDown=0;
		private int		inc=1;
		private int		iExtra=0;	
		private int		iFrame=0;

		private Rectangle rcLastPos;
		private int		ArgPosX=0;
		private int		ArgPosY=0;
		private int		iExplosionFrame=0;
		private int		iQtdeDown=80;
		private ShipState	iState=ShipState.Ok;
		private int		iStr=1;
		private int		iType=0;
		private int		iLastShoot=0;
		private Random	rand=new Random();

		private int		iDiffRight=0;
		private int		iDiffLeft=0;
		private int		iDiffBottom=0;

		public Ufo(int r)
		{
			//
			// TODO: Add constructor logic here
			//
			iLastShoot = 1000 + r;
		}

		public bool DestroyMe()
		{
			if (iState == ShipState.Exploding &&
				iExplosionFrame == 20)
				return true;
			return false;
		}

		public int CreateExtra(int iFlags, int iPercent)
		{
			iExtra = 0;		
			if ((iPercent == 0 || iPercent == 25 || iPercent == 45) && (iFlags != 2))
				iExtra = 2;
			if (iPercent > 0 && iPercent < 11)
				iExtra = 3;
			if (iPercent > 10 && iPercent < 25)
				iExtra = 1;
			if (iPercent > 25 && iPercent < 45)
				iExtra = 4;
			if (iPercent == 45 || iPercent == 46)
				iExtra = 5;
			return iExtra;
		}

		public void SetXY(int xpos, int ypos)
		{
			x=xpos;
			y=ypos;
			int newx = x;
			int newy = y;
			if (newy + 70 > 460)
				newy = 460 - 70;
			if (newx + 70 > 640)
				newx= 640 - 70;
			rcLastPos = new Rectangle(newx,newy,70,70);
		}

		public int Inc // Get_Inc and Set_Inc
		{
			get { return inc; }
			set 
			{
				inc = value;
			}
		}
		public ShipState State // Get_Inc and Set_Inc
		{
			get { return iState; }
			set 
			{
				iState = value;
			}
		}
		public int Mov // Get_Inc and Set_Inc
		{
			get { return mov; }
			set 
			{
				mov = value;
			}
		}
		public int Extra // Get_Inc and Set_Inc
		{
			get { return iExtra; }
		}
		public int Type // Get_Inc and Set_Inc
		{
			get { return iType; }
			set 
			{
				iType = value;
			}
		}
		public int Str // Get_Inc and Set_Inc
		{
			get { return iStr; }
			set 
			{
				iStr = value;
			}
		}
		public int X // Get_Inc and Set_Inc
		{
			get { return x+iDiffLeft; }
			set 
			{
				x = value;
				iDiffLeft=0;
			}
		}		
		public int Y // Get_Inc and Set_Inc
		{
			get { return y+iDiffBottom; }
			set 
			{
				y = value;
				iDiffBottom=0;
			}
		}		
		public int AmDown // Get_Inc and Set_Inc
		{
			get { return amDown; }
			set 
			{
				amDown = value;
			}
		}
		public int QtdeDown // Get_Inc and Set_Inc
		{
			get { return iQtdeDown; }
			set 
			{
				iQtdeDown = value;
			}
		}
		public int LastShoot // Get_Inc and Set_Inc
		{
			get { return iLastShoot; }
			set 
			{
				iLastShoot = value;
			}
		}
		public void LowStr(int iQtde)
		{
			iStr -= iQtde;
		} 				
		public bool isExploding()
		{
			if (iState == ShipState.Ok)
				return false;
			else
				return true;
		}
		public Rectangle Draw()
		{
			Console.WriteLine("ufo draw");
			Rectangle srcRect;
			srcRect = new Rectangle(ArgPosX,ArgPosY,70,70);

			if (x+70>640)
			{
				iDiffRight = x+70-640;
				//						rcRect.right -= iDiffRight;
			}
			else
				iDiffRight = 0;

			if (x<0)
			{
				iDiffLeft = x * (-1);
				//						rcRect.left += iDiffLeft;
			}
			else
				iDiffLeft = 0;

			if (y < 0)
			{
				iDiffBottom = y * (-1);
				//						rcRect.top += iDiffBottom;
			}
			else
				iDiffBottom = 0;

			switch(iState)
			{
				case ShipState.Ok:
		
					
					break;
				case ShipState.Exploding:
					/*
					iDiffBottom = 0;
*/
					if (iExplosionFrame < 5)
					{
						ArgPosX = iExplosionFrame * 70;
						ArgPosY = 0;
					}
					if (iExplosionFrame > 4 &&
						iExplosionFrame < 9)
					{
						ArgPosX = (iExplosionFrame-5) * 70;
						ArgPosY = 70;
					}
					if (iExplosionFrame > 9 &&
						iExplosionFrame < 14)
					{
						ArgPosX = (iExplosionFrame-10) * 70;
						ArgPosY = 140;
					}
					if (iExplosionFrame > 14 &&
						iExplosionFrame < 20)
					{
						ArgPosX = (iExplosionFrame-15) * 70;
						ArgPosY = 210;
					}
//					srcRect = new Rectangle(ArgPosX+iDiffLeft,ArgPosY+iDiffBottom,70,70);
					Console.WriteLine("ufopos {0} {1}",ArgPosX,ArgPosY);
					iExplosionFrame++;

					break;
			}
			srcRect = new Rectangle(ArgPosX+iDiffLeft,ArgPosY+iDiffBottom,70-iDiffRight-iDiffLeft,70-iDiffBottom);
	
			ArgPosX += 70;
			if (ArgPosX == 350)
			{
				ArgPosX = 0;
				ArgPosY += 70;
				if (ArgPosY == 700)
					ArgPosY = 0;
			}
			

			return srcRect;
		}

		public bool Move(int iFlags)
		{
			switch(iType)
			{
				case 0:
				switch (mov)
				{
					case 0:
						x += inc;
						if (y > 300)
						{
							if (inc > 0)
							{
								if (x>650)
								{
									x = 11;
									y = -70;
									mov = 1;
									inc = 1;
									AmDown = 0;
								}
							}
							else
							{
								if (x<-80)
								{
									x = 560;
									y = -70;
									mov = 1;
									inc = 1;
									AmDown = 0;
								}
							}
						}
						else
						{
							if (x == 560 ||
								x == 10)
							{
								AmDown = 0;
								mov = 1;
								inc = 1;
							}
						}
						break;
					case 1:
						y += inc;
						AmDown += inc;
						if (y > 0)
							if (AmDown >= iQtdeDown)
							{
								mov = 0;
								if (x == 0)
								{
									inc = Math.Abs(inc);
								}

								if (x == 560)
								{
									inc = Math.Abs(inc) * (-1);
								}
							}
						break;
				}
					break;
				case 1:
				switch(mov)
				{
					case 0:
						x -= inc;
						y -= inc;
						if (x <= 0)
						{
							x = 0;
							mov = 1;
						}
						if (y <= 0)
						{
							y = 0;
							mov = 3;
						}
						break;
					case 1:
						x += inc;
						y -= inc;
						if (x >= 560)
						{
							x = 560;
							mov = 0;
						}
						if (y <= 0)
						{
							y = 0;
							mov = 2;
						}
						break;
					case 2:
						x += inc;
						y += inc;
						if (x >= 560)
						{
							x = 560;
							mov = 3;
						}
						if (y >= 300)
						{
							y = 300;
							mov = 1;
						}
						break;
					case 3:
						x -= inc;
						y += inc;
						if (x <= 0)
						{
							x = 0;
							mov = 2;
						}
						if (y >= 300)
						{
							y = 300;
							mov = 0;
						}
						break;
				}
					break;
				case 2:
					Console.WriteLine("iflags {0} {1} {2} {3}",iFlags,mov,x,inc);
				switch(mov)
				{
					case 0:
						if (x <= iFlags )
						{
							if (iFlags != -1)
								mov = 1;
							else
								x += inc;
						}
						else
							x +=inc;
						if (x >= 650)
						{
							x = -80;
							mov = 0;
						}
						break;
					case 1:
						if (x >= iFlags)
						{
							if (iFlags != -1)
								mov = 0;
							else
								x -= inc;
						}
						else
						{
							x -=inc;
						}
						if (x <= -80)
						{
							x = 650;
							mov = 1;
						}
						break;
				}
					break;
			}
			return true;
		}
		public void Crash()
		{
			switch (iType)
			{
				case 0:
					inc = inc * (-1);
					break;
				case 2:
					if (mov == 1)
						mov = 0;
					else
						mov = 1;
					break;
			}
		}
		public bool DropExtra()
		{
			if (iState == ShipState.Exploding &&
				iExplosionFrame == 12 &&
				iExtra != 0)
				return true;
			else
				return false;
		}

	
	}
}
