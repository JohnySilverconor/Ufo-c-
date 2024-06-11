using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.DirectX;
using System.Reflection;
using Microsoft.DirectX.DirectDraw;
using Microsoft.DirectX.DirectSound;


namespace Invasion
{
	public class AlphaClass
	{
		public int start;
		public int end;

		public AlphaClass(int s, int e)
		{
			start = s;
			end = e;
		}
	};

	enum Weapon {Laser1, Laser2, Laser3, Photon1, Photon2, Photon3};
	enum AppState {MainMenu, LevelScreen, GameScreen, GameOver, Credits, HelpScreen};
	public enum ShipState {Ok, Exploding, Destroyed};

	/// <summary>
	/// Summary description for Invasion.
	/// </summary>
	public class Invasion : System.Windows.Forms.Form
	{
		// constants
		const int screenWidth = 640; // Screen width.
		const int screenHeight = 480; // Screen height.
		
		// Define the application states
		const int appMainMenu = 0;
		const int appGameScreen = 1;
		const int appCredits = 2;
		const int appHelpScreen = 3;

		const string nameFile = "invasion.png"; // Bitmap to load.
		const string nameAlphaFile = "alpha.png";
		const string nameShipFile = "ship.png";
		const string nameBackDropFile = "backdrop2.png";
		const string nameSelectedFile = "select.png";
		const string nameShipExplodeFile = "shipexplode.png";
		const string nameShootFile = "shoot.png";
		const string nameShoot2File = "shoot2.png";
		const string nameUfoShootFile = "shootufo.png";
		const string nameUfoFile = "ufo.png";
		const string nameUfo2File = "ufo2.png";
		const string nameUfo3File = "ufo3.png";
		const string nameBoomFile = "explosion.png";
		const string nameBoom2File = "explosion2.png";
		const string nameBoom3File = "explosion3.png";
		const string nameExtraFile = "extras.png";
		const string nameStatusBarFile = "status.png";
		
		AlphaClass[] AA = null;
		
		private Assembly loadedAssembly = null;
		Microsoft.DirectX.DirectDraw.Device displayDevice = null; // The Direct Draw Device.
		Microsoft.DirectX.DirectSound.Device soundDevice = null;
		Surface front = null; // The front surface.
		Surface back = null; // The back surface.
		Surface background = null; // The surface that contains the sprites bitmap.
		Surface backdrop = null; // The surface that contains the sprites bitmap.

		AppState iAppState = AppState.MainMenu;

		Surface surfaceUfo = null;
		Surface Ship = null;
		Surface Shoot = null;
		Surface Shoot2 = null;
		Surface UfoShoot = null;
		Surface Boom = null;
		Surface ShipBoom = null;
		Surface Alphabet = null;
		Surface Selected = null;
		Surface StatusBar = null;
		Surface surfaceExtra = null;

		private SecondaryBuffer bufferSkid = null;
		private SecondaryBuffer bufferSelect = null;
		private SecondaryBuffer bufferBlaster = null;
		private SecondaryBuffer bufferEnter = null;
		private SecondaryBuffer bufferBoom = null;
		private SecondaryBuffer bufferUfoShoot = null;
		private SecondaryBuffer bufferExtra = null;
		private SecondaryBuffer bufferGameOver = null;

		// link lists for the objects to draw
		ArrayList listBullet = null;
		ArrayList listUfo = null;
		ArrayList listExtra = null;

		bool needRestore = false; // Flag to determine the app needs to restore the surfaces.
		Clipper clip = null; // Clipper to prevent the app from drawing over the top of the dialog.
		int iOption = 0;
		int iSelectFrame=0;

		bool bShipStop=false;
		int iShipPos = 285;
		int ArgPosY = 0;
		int ArgPosX = 0;
		int incFrame = 70;
		int iShipMov = 0;
		ShipState iShipState = ShipState.Ok;
		int iShield=0;
		int iLevel=0;
		int iScore=0;
		Weapon iWeapon=Weapon.Laser1;
		Weapon iMaxWeapon=Weapon.Laser1;
		int iLaserAmmo=100;
		int iPhotonAmmo=0;
		bool bShootEnable=true;
		bool bShoot=false;
		int shootChance=1000;
		
		// create a random number generator seeded to time
		private Random	rand=new Random();

		// setup the delay variables and constants;
		int tickLast = 0; // Holds the value of the last call to GetTick.
		int delayStart = 0;
		int delayOffset = 0;
		const int delayTime = 18;
		const int delayLevel = 3000;
		const int delayCredits = 6000;
		const int delayHelp = 6000;
		const int delayBlink = 800;

		private System.Windows.Forms.Timer timerShoot;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Initialize the positions of the alphabet characters
		/// </summary>
		void InitAlphabet()
		{
			AA = new AlphaClass[40];
			
			AA[0] = new AlphaClass(0,14);
			AA[1] = new AlphaClass(19,27);
			AA[2] = new AlphaClass(31,46);
			AA[3] = new AlphaClass(47,61);
			AA[4] = new AlphaClass(65,78);
			AA[5] = new AlphaClass(80,94);
			AA[6] = new AlphaClass(96,110);
			AA[7] = new AlphaClass(114,125);
			AA[8] = new AlphaClass(128,143);
			AA[9] = new AlphaClass(145,159);
			AA[10] = new AlphaClass(160,175);
			AA[11] = new AlphaClass(176,191);
			AA[12] = new AlphaClass(193,206);
			AA[13] = new AlphaClass(207,222);
			AA[14] = new AlphaClass(224,237);
			AA[15] = new AlphaClass(238,252);
			AA[16] = new AlphaClass(253,267);
			AA[17] = new AlphaClass(269,284);
			AA[18] = new AlphaClass(285,291);
			AA[19] = new AlphaClass(292,307);
			AA[20] = new AlphaClass(308,323);
			AA[21] = new AlphaClass(324,338);
			AA[22] = new AlphaClass(340,364);
			AA[23] = new AlphaClass(365,380);
			AA[24] = new AlphaClass(382,397);
			AA[25] = new AlphaClass(398,413);
			AA[26] = new AlphaClass(414,429);
			AA[27] = new AlphaClass(430,444);
			AA[28] = new AlphaClass(447,460);
			AA[29] = new AlphaClass(464,475);
			AA[30] = new AlphaClass(477,492);
			AA[31] = new AlphaClass(494,508);
			AA[32] = new AlphaClass(509,533);
			AA[33] = new AlphaClass(533,548);
			AA[34] = new AlphaClass(550,564);
			AA[35] = new AlphaClass(564,579);
			AA[36] = new AlphaClass(579,599);
			AA[37] = new AlphaClass(600,610);
			AA[38] = new AlphaClass(611,615);
			AA[39] = new AlphaClass(617,620);
		}

		/// <summary>
		/// Constructor for the Invasion main class
		/// </summary>
		public Invasion()
		{
			loadedAssembly = Assembly.GetCallingAssembly();
			
			// get the resource names for debugging purposes
			string [] resourceNames = loadedAssembly.GetManifestResourceNames();

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Create a new DrawDevice.
			InitDirectDraw();

			// load the game resources
			InitAlphabet();
			LoadSurfaces();
			LoadSounds();

			// Keep looping until told to quit.
			while (Created)
			{               
				ProcessNextFrame();     // Process and draw the next frame.
				Application.DoEvents(); // Make sure the app has time to process messages.
			}
		}


		/// <summary>
		/// Entry point for the application.
		/// </summary>
		public static void Main()
		{
			Invasion app = new Invasion();
			Application.Exit();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Restore all the surfaces, and redraw the sprites surfaces.
		/// </summary>
		void RestoreSurfaces()
		{
			SurfaceDescription description = new SurfaceDescription();

			displayDevice.RestoreAllSurfaces();

			background.Dispose();
			background = null;
			Bitmap bmp = new Bitmap(GetType(),nameFile);
			background = new Surface(bmp, description, displayDevice);

			description.Clear();
			Alphabet.Dispose();
			Alphabet = null;
			Bitmap bmpalpha = new Bitmap(GetType(),nameAlphaFile);
			Alphabet = new Surface(bmpalpha, description, displayDevice);

			ColorKey keyalpha = new ColorKey();
			keyalpha.ColorSpaceHighValue = keyalpha.ColorSpaceLowValue = 0;
			Alphabet.SetColorKey(ColorKeyFlags.SourceDraw, keyalpha);

			description.Clear();
			Ship.Dispose();
			Ship = null;
			Bitmap bmpship = new Bitmap(GetType(),nameShipFile);
			Ship = new Surface(bmpship, description, displayDevice);

			ColorKey keyship = new ColorKey();
			keyship.ColorSpaceHighValue = keyship.ColorSpaceLowValue = 0;
			Ship.SetColorKey(ColorKeyFlags.SourceDraw, keyship);

			description.Clear();
			ShipBoom.Dispose();
			ShipBoom = null;
			Bitmap bmpshipboom = new Bitmap(GetType(),nameShipExplodeFile);
			ShipBoom = new Surface(bmpshipboom, description, displayDevice);

			ColorKey keyshipboom = new ColorKey();
			keyshipboom.ColorSpaceHighValue = keyshipboom.ColorSpaceLowValue = 0;
			ShipBoom.SetColorKey(ColorKeyFlags.SourceDraw, keyshipboom);

			description.Clear();
			surfaceUfo.Dispose();
			surfaceUfo = null;
			Bitmap bmpufo = new Bitmap(GetType(),nameUfoFile);
			surfaceUfo = new Surface(bmpufo, description, displayDevice);
			ColorKey keyufo = new ColorKey();
			keyufo.ColorSpaceHighValue = keyufo.ColorSpaceLowValue = 0;
			surfaceUfo.SetColorKey(ColorKeyFlags.SourceDraw, keyufo);

			description.Clear();
			surfaceExtra.Dispose();
			surfaceExtra = null;
			Bitmap bmpextra = new Bitmap(GetType(),nameExtraFile);
			surfaceExtra = new Surface(bmpextra, description, displayDevice);
			ColorKey keyextra = new ColorKey();
			keyextra.ColorSpaceHighValue = keyextra.ColorSpaceLowValue = 0;
			surfaceExtra.SetColorKey(ColorKeyFlags.SourceDraw, keyextra);

			description.Clear();
			Boom.Dispose();
			Boom = null;
			Bitmap bmpufoboom = new Bitmap(GetType(),nameBoomFile);
			Boom = new Surface(bmpufoboom, description, displayDevice);
			ColorKey keyufoboom = new ColorKey();
			keyufoboom.ColorSpaceHighValue = keyufoboom.ColorSpaceLowValue = 0;
			Boom.SetColorKey(ColorKeyFlags.SourceDraw, keyshipboom);

			description.Clear();
			backdrop.Dispose();
			backdrop = null;
			Bitmap bmpdrop = new Bitmap(GetType(),nameBackDropFile);
			backdrop = new Surface(bmpdrop, description, displayDevice);

			ColorKey key = new ColorKey();
			background.SetColorKey(ColorKeyFlags.SourceDraw, key);

			description.Clear();
			Selected.Dispose();
			Selected = null;
			Bitmap bmpSelected = new Bitmap(GetType(),nameSelectedFile);
			Selected = new Surface(bmpSelected, description, displayDevice);

			ColorKey keySelected = new ColorKey();
			Selected.SetColorKey(ColorKeyFlags.SourceDraw, keySelected);

			description.Clear();
			Shoot.Dispose();
			Shoot = null;
			Bitmap bmpShoot = new Bitmap(GetType(),nameShootFile);
			Shoot = new Surface(bmpShoot, description, displayDevice);

			ColorKey keyShoot = new ColorKey();
			Shoot.SetColorKey(ColorKeyFlags.SourceDraw, keyShoot);

			description.Clear();
			Shoot2.Dispose();
			Shoot2 = null;
			Bitmap bmpShoot2 = new Bitmap(GetType(),nameShoot2File);
			Shoot2 = new Surface(bmpShoot2, description, displayDevice);

			ColorKey keyShoot2 = new ColorKey();
			Shoot2.SetColorKey(ColorKeyFlags.SourceDraw, keyShoot2);

			description.Clear();
			UfoShoot.Dispose();
			UfoShoot = null;
			Bitmap bmpUfoShoot = new Bitmap(GetType(),nameUfoShootFile);
			UfoShoot = new Surface(bmpUfoShoot, description, displayDevice);

			ColorKey keyUfoShoot = new ColorKey();
			UfoShoot.SetColorKey(ColorKeyFlags.SourceDraw, keyUfoShoot);

			description.Clear();
			StatusBar.Dispose();
			StatusBar = null;
			Bitmap bmpStatusBar = new Bitmap(GetType(),nameStatusBarFile);
			StatusBar = new Surface(bmpStatusBar, description, displayDevice);

			ColorKey keyStatusBar = new ColorKey();
			StatusBar.SetColorKey(ColorKeyFlags.SourceDraw, keyStatusBar);

			return;
		}
 

		/// <summary>
		/// Move the sprites, blt them to the back buffer, then 
		/// flips the back buffer to the primary buffer
		/// </summary>
		private void ProcessNextFrame()
		{
			// Figure how much time has passed since the last time.
			int tickCurrent = Environment.TickCount;
			int tickDifference = tickCurrent - tickLast;

			// Don't update if no time has passed.
			if (delayTime + delayOffset >= tickDifference)
				return; 

			tickLast = tickCurrent;

			//Draw the sprites and text to the screen.
			DisplayFrame();
		}


		/// <summary>
		/// Draw the sprites and text to the screen.
		/// </summary>
		private void DisplayFrame()
		{
			string buffer;
			if (null == front)
				return;

			if (false == displayDevice.TestCooperativeLevel())
			{
				needRestore = true;
				return;
			}

			if (true == needRestore)
			{
				needRestore = false;
				// The surfaces were lost so restore them .
				RestoreSurfaces();
			}

			// Fill the back buffer with the appropriate things.
			switch (iAppState)
			{
				case AppState.MainMenu:
					back.DrawFast(0,0,background, DrawFastFlags.Wait);
					bltText("START GAME",190,280);
					bltText("CREDITS",190,320);
					bltText("HELP",190,360);
					bltText("QUIT",190,400);

					Rectangle rcRect = new Rectangle(iSelectFrame * 32,0,32,20);
					try
					{
						back.Draw(new Rectangle(150, 275+(40 * iOption),32,20), Selected, rcRect, DrawFlags.DoNotWait | DrawFlags.KeySource);
					}
					catch(SurfaceLostException)
					{
						RestoreSurfaces();
					}
					iSelectFrame++;
					if (iSelectFrame > 19)
						iSelectFrame = 0;

					break;
				case AppState.GameScreen:
					back.DrawFast(0,0,backdrop, DrawFastFlags.Wait);
					DrawAmmo();
					DrawShield();
					DrawScore(510,460);
					DrawWeapon();
					if (listUfo.Count == 0 && listExtra.Count == 0)
					{
						bufferEnter.Play(0,BufferPlayFlags.Default);
						InitLevel(true);
						DrawScore(510,459);
						return;
					}
					
					DrawExtra(true);
					DrawUfo();
					DrawBullet();
					DrawShip();
					bShoot = false;
					break;
				case AppState.LevelScreen:
					back.ColorFill(0);
					
					char padding = '0';
					string level = iLevel.ToString().PadLeft(3,padding);
					buffer = "LEVEL " + level;
					bltText(buffer,250,200);

					if(delayStart + delayLevel < tickLast)
						iAppState = AppState.GameScreen;
					break;

				case AppState.GameOver:
					back.ColorFill(0);
					
					buffer = "GAME OVER";
					bltText(buffer,250,200);

					if(delayStart + delayLevel < tickLast)
						iAppState = AppState.MainMenu;
					break;

				case AppState.Credits:
					back.ColorFill(0);
					bltText("THIS GAME IS FREEWARE",145,40);
					bltText("IF YOU WANT TO MAIL US",130,90);
					bltText("OR YOU ARE FROM A GAME",120,140);
					bltText("COMPANY AND WANT TO",140,190);
					bltText("HIRE US, EMAIL US AT",150,240);
					bltText("ORIGINAL VERSION",190,300);	
					bltText("MAURICIORITTER@HOTMAIL.COM",120,330);	
					bltText("C-SHARP VERSION",190,380);	
					bltText("STEVE@YSGARD.COM",175,410);	
					
					if(delayStart + delayCredits < tickLast)
						iAppState = AppState.MainMenu;
					break;

				case AppState.HelpScreen:
					back.ColorFill(0);
					bltText("PHOTON AMMO",90,45);
					bltText("WEAPON ADVANCE0",90,95);
					bltText("100 POINTS BONUS",90,145);
					bltText("LASER AMMO",90,195);	
					bltText("SHIELD CHARGE",90,245);	
					bltText("LEFT, RIGHT - MOVE SHIP",50,290);	
					bltText("DOWN OR ENTER - STOP SHIP",50,325);
					bltText("CTRL - CHANGE WEAPON",50,360);
					
					DrawExtra(false);

					if(((tickLast - delayStart)/delayBlink)%2==1)
						bltText("PRESS ANY KEY TO CONTINUE",140,450);

					break;
			}

			front.Draw(back, DrawFlags.Wait);
//			front.Flip(back, FlipFlags.DoNotWait);
		}

		/// <summary>
		/// Initializes DirectDraw and DirectSound.
		/// </summary>
		private void InitDirectDraw()
		{
			Cursor.Hide();

			displayDevice = new Microsoft.DirectX.DirectDraw.Device(); // Create a new DirectDrawDevice.            
			displayDevice.SetCooperativeLevel(this, CooperativeLevelFlags.FullscreenExclusive); // Set the cooperative level.

			soundDevice = new Microsoft.DirectX.DirectSound.Device();
			soundDevice.SetCooperativeLevel(this, CooperativeLevel.Priority);

			displayDevice.SetDisplayMode(screenWidth, screenHeight, 16, 0, false); // Set the display mode width and height, and 8 bit color depth.

			SurfaceDescription description = new SurfaceDescription(); // Describes a surface.

			description.SurfaceCaps.Flip = description.SurfaceCaps.Complex = description.SurfaceCaps.PrimarySurface = true;
			description.BackBufferCount = 1; // Create 1 backbuffer.
			front = new Surface(description, displayDevice); // Create the surface using the description above.

			SurfaceCaps caps = new SurfaceCaps();
			caps.BackBuffer = true; // Caps of the surface.
			back = front.GetAttachedSurface(caps); // Get the attached surface that matches the caps, which will be the backbuffer.
            
			clip = new Clipper(displayDevice); // Create a clipper object.
			clip.Window = this; // Set the clippers window handle to the window handle of the main window.
			front.Clipper = clip; // Tell the front surface to use the clipper object.
		}

		/// <summary>
		/// Load the bitmaps and sufaces with the png resources
		/// </summary>
		void LoadSurfaces()
		{
			SurfaceDescription description = new SurfaceDescription(); // Describes a surface.

			description.Clear(); // Clear out the SurfaceDescription structure.
			Bitmap bmp = new Bitmap(GetType(),nameFile);
			background = new Surface(bmp, description, displayDevice); // Create the sprites bitmap surface.

			ColorKey ck = new ColorKey(); // Create a new colorkey.
			background.SetColorKey(ColorKeyFlags.SourceDraw, ck); // Set the colorkey to the bitmap surface. 0 is used for the colorkey, which is what the ColorKey struct is initialized to.

			description.Clear();
			Bitmap bmpship = new Bitmap(GetType(),nameShipFile);
			Ship = new Surface(bmpship, description, displayDevice);
			ColorKey keyship = new ColorKey();
			Ship.SetColorKey(ColorKeyFlags.SourceDraw, keyship);

			description.Clear();
			Bitmap bmpshipboom = new Bitmap(GetType(),nameShipExplodeFile);
			ShipBoom = new Surface(bmpshipboom, description, displayDevice);
			ColorKey keyshipboom = new ColorKey();
			ShipBoom.SetColorKey(ColorKeyFlags.SourceDraw, keyshipboom);

			description.Clear();
			Bitmap bmpufo = new Bitmap(GetType(),nameUfoFile);
			surfaceUfo = new Surface(bmpufo, description, displayDevice);
			ColorKey keyufo = new ColorKey();
			keyufo.ColorSpaceHighValue = keyufo.ColorSpaceLowValue = 0;
			surfaceUfo.SetColorKey(ColorKeyFlags.SourceDraw, keyufo);

			description.Clear();
			Bitmap bmpextra = new Bitmap(GetType(),nameExtraFile);
			surfaceExtra = new Surface(bmpextra, description, displayDevice);
			ColorKey keyextra = new ColorKey();
			keyextra.ColorSpaceHighValue = keyextra.ColorSpaceLowValue = 0;
			surfaceExtra.SetColorKey(ColorKeyFlags.SourceDraw, keyextra);


			description.Clear();
			Bitmap bmpufoboom = new Bitmap(GetType(),nameBoomFile);
			Boom = new Surface(bmpufoboom, description, displayDevice);
			ColorKey keyufoboom = new ColorKey();
			keyufoboom.ColorSpaceHighValue = keyufoboom.ColorSpaceLowValue = 0;
			Boom.SetColorKey(ColorKeyFlags.SourceDraw, keyshipboom);

			description.Clear();

			Bitmap bmpdrop = new Bitmap(GetType(),nameBackDropFile);
			backdrop = new Surface(bmpdrop, description, displayDevice);
			
			description.Clear();

			Bitmap bmpalpha = new Bitmap(GetType(),nameAlphaFile);
			Alphabet = new Surface(bmpalpha, description, displayDevice);

			ColorKey keyalpha = new ColorKey();
			keyalpha.ColorSpaceHighValue = keyalpha.ColorSpaceLowValue = 0;
			Alphabet.SetColorKey(ColorKeyFlags.SourceDraw, keyalpha);

			description.Clear();
			Bitmap bmpSelected = new Bitmap(GetType(),nameSelectedFile);
			Selected = new Surface(bmpSelected, description, displayDevice);

			ColorKey keySelected = new ColorKey();
			Selected.SetColorKey(ColorKeyFlags.SourceDraw, keySelected);
            
			description.Clear();
			Bitmap bmpShoot = new Bitmap(GetType(),nameShootFile);
			Shoot = new Surface(bmpShoot, description, displayDevice);

			ColorKey keyShoot = new ColorKey();
			Shoot.SetColorKey(ColorKeyFlags.SourceDraw, keyShoot);

			description.Clear();
			Bitmap bmpShoot2 = new Bitmap(GetType(),nameShoot2File);
			Shoot2 = new Surface(bmpShoot2, description, displayDevice);

			ColorKey keyShoot2 = new ColorKey();
			Shoot2.SetColorKey(ColorKeyFlags.SourceDraw, keyShoot2);
			
			description.Clear();
			Bitmap bmpUfoShoot = new Bitmap(GetType(),nameUfoShootFile);
			UfoShoot = new Surface(bmpUfoShoot, description, displayDevice);

			ColorKey keyUfoShoot = new ColorKey();
			UfoShoot.SetColorKey(ColorKeyFlags.SourceDraw, keyUfoShoot);

			description.Clear();
			Bitmap bmpStatusBar = new Bitmap(GetType(),nameStatusBarFile);
			StatusBar = new Surface(bmpStatusBar, description, displayDevice);

			ColorKey keyStatusBar = new ColorKey();
			StatusBar.SetColorKey(ColorKeyFlags.SourceDraw, keyStatusBar);

			if(listExtra == null)
				listExtra = new ArrayList();
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timerShoot = new System.Windows.Forms.Timer(this.components);
			// 
			// timerShoot
			// 
			this.timerShoot.Tick += new System.EventHandler(this.timerShoot_Tick);
			// 
			// Invasion
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(272, 88);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Invasion";
			this.Text = "Invasion";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Invasion_KeyDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Invasion_Closing);

		}
		#endregion


		/// <summary>
		/// Draw text on the background surface
		/// </summary>
		/// <param name="buffer">string to draw</param>
		/// <param name="x">x position</param>
		/// <param name="y">y position</param>
		void bltText(string buffer, int  x, int y)
		{
			// Draw text to the screen
			int cnt=0;
			int		pos;
			int		nextx;

			nextx = x;
			while (cnt < buffer.Length)
			{
				if (buffer[cnt] != ' ')
				{
					if (buffer[cnt] >= 48 &&
						buffer[cnt] <= 57)
						pos = (int) buffer[cnt] - 48;
					else
						pos = (int) buffer[cnt] - 55;
					if (buffer[cnt] == '@')
						pos = 36;
					if (buffer[cnt] == '-')
						pos = 37;
					if (buffer[cnt] == ',')
						pos = 38;
					if (buffer[cnt] == '.')
						pos = 39;

					Rectangle rcRect = new Rectangle(AA[pos].start,0,AA[pos].end-AA[pos].start+1,15);

					try
					{
						back.Draw(new Rectangle(nextx, y,AA[pos].end-AA[pos].start+1,15), Alphabet, rcRect, DrawFlags.DoNotWait | DrawFlags.KeySource);
					}
					catch(SurfaceLostException)
					{
						RestoreSurfaces();
					}

					nextx += (AA[pos].end - AA[pos].start) + 1;
				}
				else
					nextx += 15;
				
				cnt++;
			}
		}

		/// <summary>
		/// Draw the player's ship
		/// </summary>
		void DrawShip()
		{
			if (bShipStop == true)
			{
				bufferSkid.SetCurrentPosition(0);
				bufferSkid.Play(0, BufferPlayFlags.Default);
				bShipStop = false;
			}

			iShipPos += iShipMov;

			if (iShipPos <= 1) 
			{
				iShipMov = 0;
				iShipPos = 1;
			}
			if (iShipPos >= 569) 
			{	
				iShipMov = 0;
				iShipPos = 569;	
			}

			Rectangle rcRect = new Rectangle(ArgPosX,ArgPosY,70,70);

			if (iShipState == ShipState.Ok && CheckHitShip() == true)
			{
				iShield = iShield - 20;
			}

			if (iShield <= 0 && iShipState == ShipState.Ok)
			{
				iShield = 0;
				iShipState = ShipState.Exploding;
				iShipMov = 0;
				ArgPosX = 0;
				ArgPosY = 0;
				rcRect = new Rectangle(ArgPosX,ArgPosY,70,70);
				incFrame = 70;
			}

			if (iShipState == ShipState.Ok && listExtra.Count != 0)
			{
				int iType;

				// check if we hit an extra
				// if so, select the kinf of extra
				iType = CheckHitExtra();
				switch (iType)
				{
					case 1:
						iPhotonAmmo += 25;
						if (iPhotonAmmo > 999)
							iPhotonAmmo = 999;

						break;
					case 2:
						if(iMaxWeapon < Weapon.Photon3)
							iMaxWeapon++;

						iWeapon = iMaxWeapon;

						break;
					case 3:
						iScore += 100;
						break;
					case 4:
						iLaserAmmo += 40;
						if (iLaserAmmo > 999)
							iLaserAmmo = 999;
						break;
					case 5:
						iShield += 10;
						if (iShield > 50)
							iShield = 50;
						break;
				}
				if(iType != 0)
				{
					bufferExtra.SetCurrentPosition(0);
					bufferExtra.Play(0,BufferPlayFlags.Default);
				}
			}

			// Draw the ship sprite
			try
			{
				Console.WriteLine("draw ship");
				if(iShipState == ShipState.Ok)
					back.Draw(new Rectangle(iShipPos, 380,70,70), Ship, rcRect, DrawFlags.DoNotWait | DrawFlags.KeySource);
				else
					back.Draw(new Rectangle(iShipPos, 380,70,70), ShipBoom, rcRect, DrawFlags.DoNotWait | DrawFlags.KeySource);

			}
			catch(SurfaceLostException)
			{
				RestoreSurfaces();
			}

			switch (iShipState)
			{
				case ShipState.Ok:
					ArgPosX += incFrame;
					if (ArgPosX == 350 && incFrame > 0)
					{
						ArgPosX = 0;
						ArgPosY += incFrame;
						if (ArgPosY == 350)
						{
							ArgPosX=280;
							ArgPosY=280;
							incFrame = -70;
							return;
						}

					}

					if (ArgPosX == -70 && incFrame < 0)
					{
						ArgPosX = 280;
						ArgPosY += incFrame;
						if (ArgPosY == -70)
						{
							incFrame = 70;
							ArgPosX = 0;
							ArgPosY = 0;
						}
					}
					break;
				case ShipState.Exploding:
					ArgPosX += incFrame;
					if (ArgPosX == 490 && incFrame > 0)
					{
						ArgPosX = 0;
						ArgPosY += incFrame;
						if (ArgPosY == 210)
						{
							iShipState = ShipState.Destroyed;
							iAppState = AppState.GameOver;
							delayStart = Environment.TickCount;
							bufferGameOver.Play(0,BufferPlayFlags.Default);
							return;
						}
					}
					break;
			}
		}

		/// <summary>
		/// Draw the ammo remaining
		/// </summary>
		void DrawAmmo()
		{
			string ammobuf;
			int	iAmmo;

			// blt everything in reverse order if we are doing destination transparency
			// calculate score string
			if (iWeapon < Weapon.Photon1)
				iAmmo = iLaserAmmo;
			else
				iAmmo = iPhotonAmmo;
			
			ammobuf = iAmmo.ToString().PadLeft(3,'0');
			bltText(ammobuf, 241, 460);
		}
		
		/// <summary>
		/// Draw the player's score
		/// </summary>
		/// <param name="x">x position</param>
		/// <param name="y">y position</param>
		void DrawScore(int x, int y)
		{
			char padding = '0';
			string score = iScore.ToString().PadLeft(8,padding);
			bltText(score, x, y);
		}

		/// <summary>
		/// Handler for the KeyDown Event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Invasion_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(iAppState == AppState.Credits || iAppState == AppState.HelpScreen)
			{
				iAppState = AppState.MainMenu;
				listExtra.Clear();
				delayOffset=0;
				return;
			}
			Bullet pBullet;
			switch(e.KeyCode)
			{
				case Keys.Space:
					if (iAppState != AppState.GameScreen)
						break;
					// If ship is not ok, get out
					if (iShipState != ShipState.Ok)
						break;

					// Check if we can shoot or we are waiting for our weapon
					// recharge
					if (bShootEnable == false)
						break;

					// Start shooting flag
					bShoot = true;
				
					// Check the weapon we are using
					switch (iWeapon)
					{
						case Weapon.Laser1:
							// using single laser

							// if we are out of ammo, get energy from the shields
							if (iLaserAmmo == 0)
								iShield -= 1;
							else
								iLaserAmmo--;

							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+30, 380);
							pBullet.Type = 2;
							listBullet.Add(pBullet);
							pBullet = null;
							break;
						case Weapon.Laser2:

							// If we are out of ammo, not shoot
							if (iLaserAmmo < 2)
								return;

							iLaserAmmo-=2;

							// Create a new bullet object and add it to bullet list
							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+17, 385);
							pBullet.Type = 2;
							listBullet.Add(pBullet);
							pBullet = null;

							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+43, 385);
							pBullet.Type = 2;
							listBullet.Add(pBullet);
							pBullet = null;
	
							break;
						case Weapon.Laser3:

							// If we are out of ammo, not shoot
							if (iLaserAmmo < 3)
								return;

							iLaserAmmo-=3;

							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+17, 385);
							pBullet.Type = 2;
							listBullet.Add(pBullet);
							pBullet = null;

							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+43, 385);
							pBullet.Type = 2;
							listBullet.Add(pBullet);
							pBullet = null;

							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+30, 380);
							pBullet.Type = 2;
							listBullet.Add(pBullet);
							pBullet = null;
							break;
					
						case Weapon.Photon1:
							if (iPhotonAmmo == 0)
								return;

							iPhotonAmmo--;

							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+30, 380);
							listBullet.Add(pBullet);
							pBullet = null;
							break;
						case Weapon.Photon2:
							if (iPhotonAmmo < 2)
								return;
							iPhotonAmmo-=2;

							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+17, 385);
							listBullet.Add(pBullet);
							pBullet = null;

							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+43, 385);
							listBullet.Add(pBullet);
							pBullet = null;							
							break;

						case Weapon.Photon3:
						
							if (iPhotonAmmo < 3)
								return;
							iPhotonAmmo-=3;
							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+17, 385);
							listBullet.Add(pBullet);
							pBullet = null;

							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+43, 385);
							listBullet.Add(pBullet);
							pBullet = null;

							pBullet = new Bullet();
							pBullet.SetXY(iShipPos+30, 380);
							listBullet.Add(pBullet);
							pBullet = null;
							break;
					
					
					}
					bufferBlaster.Stop();
					bufferBlaster.Play(0,BufferPlayFlags.Default);
					bShootEnable = false;
					if (iWeapon == 0)
						timerShoot.Interval = 400;
					else
						timerShoot.Interval = 680;
					timerShoot.Enabled = true;
					//iShield=-1;
					break;
				case Keys.ControlKey:
					// Control key is used to change the weapon
					// If not in game screen, get out
					if (iAppState != AppState.GameScreen)
						break;

					if (iWeapon == iMaxWeapon)
						iWeapon = Weapon.Laser1;
					else
						// Increment weapon type
						iWeapon++;

					break;
				case Keys.Escape:
				case Keys.F12:
					if(iAppState == AppState.MainMenu)
						Close();
					else
						iAppState = AppState.MainMenu;
					break;
				case Keys.Return:
					switch(iAppState)
					{
						case AppState.MainMenu:
							switch(iOption)
							{
								case appGameScreen-1:
									iShield=50;
									iLevel=0;
									iScore=0;
									iWeapon=Weapon.Laser1;
									iLaserAmmo = 100;
									iPhotonAmmo = 0;
									iMaxWeapon=Weapon.Laser1;

									bufferEnter.Play(0, BufferPlayFlags.Default);

									iAppState = AppState.GameScreen;
									InitLevel(true);

									break;
								case appCredits-1:
									iAppState = AppState.Credits;
									delayStart = Environment.TickCount;
									break;
								case appHelpScreen-1:
									iAppState = AppState.HelpScreen;
									listExtra.Clear();
									for(int x=0;x<5;x++)
									{
										Extra pExtra = new Extra();
										pExtra.SetXY(50,40+50*x);
										pExtra.Type = x+1;
										listExtra.Add(pExtra);
									}
									delayOffset = 20;
									delayStart = Environment.TickCount;
									break;
								default:
									Close();
									break;
							}
							break;
						case AppState.GameScreen:
							iAppState = AppState.MainMenu;
							break;
					}
					break;
				case Keys.Up:
					if (iAppState != AppState.MainMenu)
						break;
					bufferSelect.SetCurrentPosition(0);
					bufferSelect.Play(0, BufferPlayFlags.Default);
					iOption--;
					// If we reach the first option, go the last one
					if (iOption < 0)
						iOption = 3;

					break;
				case Keys.Left:
					iShipMov = -4;
					break;
				case Keys.Right:
					iShipMov = 4;
					break;

				case Keys.Down:
					switch (iAppState)
					{
						case AppState.MainMenu:
							// Select the down key at main menu
							// increment option counter
							iOption++;
							bufferSelect.SetCurrentPosition(0);
							bufferSelect.Play(0, BufferPlayFlags.Default);
							if (iOption > 3)
								iOption = 0;
							break;
						case AppState.GameScreen:
							// Select down key at the game screen, stop the
							// ship if it´s ok and not destroyed

							// If the ship is not ok, doen´t need to stop it, 
							//							if (iShipState != SHIP_OK)
							//								break;

							// If the ship is already stopped, get out
							if (iShipMov == 0)
								break;

							bShipStop = true;
							iShipMov = 0;
							break;
					}
					break;
			}
		}

		/// <summary>
		/// Handler for the Closing Event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Invasion_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Cursor.Show();
		}

		/// <summary>
		/// Load the sounds from the wav resources
		/// </summary>
		/// <returns></returns>
		bool LoadSounds()
		{
			try
			{
				bufferSkid = new SecondaryBuffer(loadedAssembly.GetManifestResourceStream("Invasion.skid.wav"), soundDevice);
				bufferSelect = new SecondaryBuffer(loadedAssembly.GetManifestResourceStream("Invasion.tap.wav"), soundDevice);
				bufferBlaster = new SecondaryBuffer(loadedAssembly.GetManifestResourceStream("Invasion.blaster.wav"), soundDevice);
				bufferEnter = new SecondaryBuffer(loadedAssembly.GetManifestResourceStream("Invasion.blub.wav"), soundDevice);
				bufferBoom = new SecondaryBuffer(loadedAssembly.GetManifestResourceStream("Invasion.explosion.wav"), soundDevice);
				bufferUfoShoot = new SecondaryBuffer(loadedAssembly.GetManifestResourceStream("Invasion.ufoshoot.wav"), soundDevice);
				bufferExtra = new SecondaryBuffer(loadedAssembly.GetManifestResourceStream("Invasion.getextra.wav"), soundDevice);
				bufferGameOver = new SecondaryBuffer(loadedAssembly.GetManifestResourceStream("Invasion.gameover.wav"), soundDevice);
			}
			catch(SoundException)
			{
				Console.WriteLine("load sounds error");
				return false;
			}
			return true;
		}

		/// <summary>
		/// Draw the bullets
		/// </summary>
		public void DrawBullet()
		{
			// Draw bullets in the screen
			foreach(Bullet pBullet in listBullet)
			{
				Rectangle r;
				switch(pBullet.Type)
				{
					case 0:
						pBullet.Move(6);
						r = pBullet.Draw();
						
						if(r.Height > 0)
						{
							try
							{
								back.DrawFast(pBullet.X,pBullet.Y,Shoot,r,DrawFastFlags.DoNotWait | DrawFastFlags.SourceColorKey);
							}
							catch(InvalidRectangleException)
							{
								Console.WriteLine("bullet 0 InvalidRectangleException {0} {1} {2} {3}",r.Left,r.Top,r.Width,r.Height);
							}
						}

						break;
					case 1:
						pBullet.Move(-6);
						r = pBullet.Draw();
						Console.WriteLine("bullet1 {0} {1} {2}     {3} {4} {5} {6}",pBullet.X,pBullet.Y,pBullet.ClipTop,r.Left,r.Top,r.Width,r.Height);
						if(r.Height > 0)
						{
							try
							{
								back.DrawFast(pBullet.X,pBullet.Y,UfoShoot,r,DrawFastFlags.DoNotWait | DrawFastFlags.SourceColorKey);
							}
							catch(InvalidRectangleException)
							{
								Console.WriteLine("bullet 1 InvalidRectangleException {0} {1} {2} {3}",r.Left,r.Top,r.Width,r.Height);
							}
						}
						break;
					case 2:
						pBullet.Move(10);
						r = pBullet.Draw();
						Console.WriteLine("bullet2 {0} {1} {2}     {3} {4} {5} {6}",pBullet.X,pBullet.Y,pBullet.ClipTop,r.Left,r.Top,r.Width,r.Height);
						if(r.Height > 0)
						{
							try
							{
								back.DrawFast(pBullet.X,pBullet.Y,Shoot2,r,DrawFastFlags.DoNotWait | DrawFastFlags.SourceColorKey);
							}
							catch(InvalidRectangleException)
							{
								Console.WriteLine("bullet 2 InvalidRectangleException {0} {1} {2} {3}",r.Left,r.Top,r.Width,r.Height);
							}
						}
						break;
				}
			}
			if(listBullet.Count > 0)
			{
				for(int x=listBullet.Count-1;x>=0;x--)
				{
					Bullet pBullet = (Bullet)listBullet[x];
					if ((pBullet.Y <= -26) || (pBullet.Y > 455))
					{
						listBullet.Remove(pBullet);
					}
				}
			}
			
		}

		/// <summary>
		/// Handler for the Timer Events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerShoot_Tick(object sender, System.EventArgs e)
		{
			timerShoot.Enabled = false;
			bShootEnable = true;;
		}

		/// <summary>
		/// Draw the Shield power to the back buffer
		/// </summary>
		public void DrawShield()
		{
			if(iShield <= 0)
				return;

			// Draw the shield status
			Rectangle rcRect=new Rectangle(371,460,iShield,17);

			try
			{
				if (iShield <= 50 && iShield >  30)
				{
					back.ColorFill(rcRect,15<<6);
				}
				if (iShield <= 30 && iShield >  20)
					back.ColorFill(rcRect,255<<8);
				if (iShield <= 20 && iShield >= 0)
					back.ColorFill(rcRect,255<<12);
			}
			catch(SurfaceLostException)
			{
				RestoreSurfaces();
			}
		}

		/// <summary>
		/// Initialize the next level
		/// </summary>
		public void InitLevel(bool bStart)
		{
			int iLastExtra=0;
			int valinc=1;
			if(listBullet == null)
				listBullet = new ArrayList();
			if(listUfo == null)
				listUfo = new ArrayList();

			listBullet.Clear();
			listUfo.Clear();
			listExtra.Clear();

			iLevel++;
			LoadUfoSurface();

			switch(iLevel % 6)
			{
				case 0:
					int	i;
					i = iLevel / 6;

					Ufo pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(20,20);
					iLastExtra = pUFO.CreateExtra(0,rand.Next(150));
					pUFO.Inc = i;
					pUFO.Mov = 0;
					pUFO.Type = 2;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(110,90);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = i;
					pUFO.Mov = 0;
					pUFO.Type = 2;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(20,160);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = i;
					pUFO.Mov = 0;
					pUFO.Type = 2;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(540,20);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = i;
					pUFO.Mov = 1;
					pUFO.Type = 2;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(470,90);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = i;
					pUFO.Mov= 1;
					pUFO.Type = 2;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(540,160);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = i;
					pUFO.Mov = 1;
					pUFO.Type = 2;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					break;

				case 1:
					
					for (i=0; i<3; i++)
					{
						pUFO = new Ufo(rand.Next(500));
					
						pUFO.SetXY(15 + (i*30),(i * 65)+10);
						iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
						pUFO.Inc = 1;
						pUFO.Mov = 0;
						pUFO.QtdeDown = 1;
						pUFO.Str = (int)(iLevel /7)+1;
						listUfo.Add(pUFO);
						pUFO = null;
					}

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(45,195);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 1;
					pUFO.Mov = 0;
					pUFO.QtdeDown = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(15,260);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 1;
					pUFO.Mov = 0;
					pUFO.QtdeDown = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					for (i=0; i<3; i++)
					{
						pUFO = new Ufo(rand.Next(500));
						pUFO.SetXY(560 - (i*30),(i * 65)+10);
						iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
						pUFO.Inc = -1;
						pUFO.Mov = 0;
						pUFO.QtdeDown = 1;
						pUFO.Str = (int)(iLevel /7)+1;
						listUfo.Add(pUFO);
						pUFO = null;
					}

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(530,195);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = -1;
					pUFO.Mov = 0;
					pUFO.QtdeDown = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(560,260);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = -1;
					pUFO.Mov = 0;
					pUFO.QtdeDown = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					break;

				case 2:
					for (i=0; i<5; i++)
					{
						pUFO = new Ufo(rand.Next(500));
						pUFO.SetXY(15 + (i*30),(i * 65)+10);
						iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
						pUFO.Inc = 1;
						pUFO.Mov = 0;
						pUFO.QtdeDown = 1;
						pUFO.Str = (int)(iLevel /7)+1;
						listUfo.Add(pUFO);
						pUFO = null;
					}

					for (i=0; i<5; i++)
					{
						pUFO = new Ufo(rand.Next(500));
						pUFO.SetXY(560 - (i*30),(i * 65)+10);
						iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
						pUFO.Inc = -1;
						pUFO.Mov = 0;
						pUFO.QtdeDown = 1;
						pUFO.Str = (int)(iLevel /7)+1;
						listUfo.Add(pUFO);
						pUFO = null;
					}
					break;
				case 3:
					for (int j=0;j<3;j++)
					{
						for (i=0;i<8;i++)
						{
							pUFO = new Ufo(rand.Next(500));
							pUFO.SetXY((i * 77)+15,j*80);
							iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
							pUFO.Inc = valinc;
							pUFO.Mov = 0;
							pUFO.Str = (int)(iLevel /7)+1;
							listUfo.Add(pUFO);
							pUFO = null;
						}
						valinc = valinc * -1;
					}
					break;
				case 4:
					pUFO  = new Ufo(rand.Next(500));
					pUFO.SetXY(20,20);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 2;
					pUFO.Mov = 2;
					pUFO.Type = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(180,180);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 2;
					pUFO.Mov = 2;
					pUFO.Type = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;


					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(530,110);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 2;
					pUFO.Mov = 3;
					pUFO.Type = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(100,100);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 2;
					pUFO.Mov = 1;
					pUFO.Type = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(450,190);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 2;
					pUFO.Mov = 0;
					pUFO.Type = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;
					break;

				case 5:
					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(20,20);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 3;
					pUFO.Mov = 2;
					pUFO.Type = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(90,90);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 3;
					pUFO.Mov = 2;
					pUFO.Type = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(20,160);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 3;
					pUFO.Mov = 3;
					pUFO.Type = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(540,20);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 3;
					pUFO.Mov = 3;
					pUFO.Type = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(470,90);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 3;
					pUFO.Mov = 3;
					pUFO.Type = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;

					pUFO = new Ufo(rand.Next(500));
					pUFO.SetXY(540,160);
					iLastExtra = pUFO.CreateExtra(iLastExtra,rand.Next(150));
					pUFO.Inc = 3;
					pUFO.Mov = 2;
					pUFO.Type = 1;
					pUFO.Str = (int)(iLevel /7)+1;
					listUfo.Add(pUFO);
					pUFO = null;
					break;


			}

			iShipState = ShipState.Ok;
			iShipMov = 0;
			iShipPos = 285;
			delayStart = Environment.TickCount;
			iAppState = AppState.LevelScreen;

			shootChance = 180 - (40 * (int)(iLevel /7));
			if (shootChance < 50)
				shootChance = 50;

		}

		/// <summary>
		/// Load the UFO surface for the next level
		/// </summary>
		public bool LoadUfoSurface()
		{
			SurfaceDescription description = new SurfaceDescription();
			Bitmap bmp;
			Bitmap bmpboom;

			surfaceUfo.Dispose();
			surfaceUfo = null;
			Boom.Dispose();
			Boom = null;

			switch(iLevel % 6)
			{
				case 0:
					bmp = new Bitmap(GetType(),nameUfo3File);
					break;
				case 4:
				case 5:
					bmp = new Bitmap(GetType(),nameUfo2File);
					break;
				default:
					bmp = new Bitmap(GetType(),nameUfoFile);
					break;
			};
			if( bmp == null )
				return false;
			
			surfaceUfo = new Surface(bmp, description, displayDevice);
			ColorKey keyufo = new ColorKey();
			keyufo.ColorSpaceHighValue = keyufo.ColorSpaceLowValue = 0;
			surfaceUfo.SetColorKey(ColorKeyFlags.SourceDraw, keyufo);

			description.Clear();

			switch(iLevel % 6)
			{
				case 0:
					bmpboom = new Bitmap(GetType(),nameBoom3File);
					break;
				case 4:
				case 5:
					bmpboom = new Bitmap(GetType(),nameBoom2File);
					break;
				default:
					bmpboom = new Bitmap(GetType(),nameBoomFile);
					break;
			};
			if( bmpboom == null )
				return false;

			Boom = new Surface(bmpboom, description, displayDevice);
			ColorKey keyufoboom = new ColorKey();
			keyufoboom.ColorSpaceHighValue = keyufoboom.ColorSpaceLowValue = 0;
			Boom.SetColorKey(ColorKeyFlags.SourceDraw, keyufoboom);

			return true;
		}

		/// <summary>
		/// Draw all of the ufos in the list
		/// </summary>
		private void DrawUfo()
		{
			int	delayshoot=0;
		    
			// draw all the ufos
			foreach(Ufo pUFO in listUfo)
			{
				switch (pUFO.isExploding())
				{
				case false:
					switch (pUFO.Type)
					{
					case 0:
						delayshoot = 100;
						break;
					case 1:
						delayshoot = 10;
						break;
					case 2:
						delayshoot = 70;
						break;
					}

					if (bShoot == true)
					{
						pUFO.Move(iShipPos);
					}
					else
						pUFO.Move(-1);

					Rectangle rec=pUFO.Draw();
					if(rec.Height > 0 && rec.Width > 0)
					{
						try
						{
							back.DrawFast(pUFO.X,pUFO.Y,surfaceUfo,rec,DrawFastFlags.SourceColorKey | DrawFastFlags.DoNotWait);
						}
						catch(InvalidRectangleException)
						{
							Console.WriteLine("ufo InvalidRectangleException {0} {1} {2} {3}",rec.Left,rec.Top,rec.Width,rec.Height);
						}
					}

					if (CheckHit(pUFO) == true)
					{
						pUFO.State = ShipState.Exploding;
						iScore += 10;
						bufferBoom.SetCurrentPosition(0);
						bufferBoom.Play(0,BufferPlayFlags.Default);
					}

					if ( (Environment.TickCount - pUFO.LastShoot) > delayshoot)
					{
						if (rand.Next(shootChance) == 0)
						{
							Console.WriteLine("adding a bullet");
							pUFO.LastShoot = Environment.TickCount;
							
							Bullet pBullet = new Bullet();
							pBullet.SetXY(pUFO.X+24, pUFO.Y+50);
							pBullet.Type = 1;
							listBullet.Add(pBullet);

							bufferUfoShoot.SetCurrentPosition(0);
							bufferUfoShoot.Play(0,BufferPlayFlags.Default);
							
						}
					}
					break;
				case true:
					if (pUFO.DestroyMe() == false)
					{
						Rectangle r;
						r = pUFO.Draw();
						Console.WriteLine("ufo explosion {0} {1}     {2} {3} {4} {5}",pUFO.X,pUFO.Y,r.Left,r.Top,r.Width,r.Height);
						if(r.Height > 0 && r.Width > 0)
						{
							try
							{
								back.DrawFast(pUFO.X,pUFO.Y,Boom,pUFO.Draw(),DrawFastFlags.SourceColorKey | DrawFastFlags.DoNotWait);
							}
							catch(InvalidRectangleException)
							{
								Console.WriteLine("ufo InvalidRectangleException {0} {1} {2} {3}",r.Left,r.Top,r.Width,r.Height);
							}
						}
						if (pUFO.DropExtra() == true)
						{
							Extra pNewExtra = new Extra();
							pNewExtra.SetXY(pUFO.X+22, pUFO.Y+20);
							pNewExtra.Type = pUFO.Extra;
							listExtra.Add(pNewExtra);
						}
					}
					break;
				}
			} //** While UFO-NEXT;
			if(listUfo.Count > 0)
			{
				for(int x=listUfo.Count-1;x>=0;x--)
				{
					Ufo tUFO = (Ufo)listUfo[x];
					if (tUFO.DestroyMe() == true)
					{
						listUfo.Remove(tUFO);
					}
				}
			}
			
			foreach(Ufo pUFO in listUfo)
			{
				if (CheckCrash(pUFO) == true)
					pUFO.Crash();
			}
		}

		/// <summary>
		/// Check to see if a ufo has hit into another ufo
		/// </summary>
		/// <param name="pCheck">ufo to check against</param>
		/// <returns>true if there is a crash
		/// false if there is no crash</returns>
		bool CheckCrash(Ufo pCheck)
		{
			// Check if we have crashed into a UFO
			foreach(Ufo pUFO in listUfo)
			{
				if (pUFO.isExploding() == false      &&
					pUFO.Y == pCheck.Y    &&
					pUFO != pCheck)
				{
					if (pCheck.X < pUFO.X)
					{
						if (pUFO.X - pCheck.X <= 70 &&
							(
							(pCheck.Inc > 0 && pCheck.Type==0) ||
							(pCheck.Mov == 0 && pCheck.Type==2)   )
							)
							return true;
					}
					else
					{
						if (pCheck.X - pUFO.X <= 70 &&
							((pCheck.Inc < 0 &&
							pCheck.Type == 0) ||
							(pCheck.Mov == 1 &&
							pCheck.Type == 2))
							)
							return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Check if we have hit a UFO
		/// </summary>
		/// <param name="pCheckUfo">Ufo to check</param>
		/// <returns>true if ufo is hit
		/// false if not hit</returns>
		bool CheckHit(Ufo pCheckUfo)
		{
			if (pCheckUfo.isExploding() == true) 
				return false;

			foreach(Bullet pBullet in listBullet)
			{
				if ((pBullet.X > pCheckUfo.X    &&
					pBullet.X < pCheckUfo.X+64 &&
					pBullet.Y > pCheckUfo.Y    &&
					pBullet.Y < pCheckUfo.Y+56) &&
					(pBullet.Type != 1))
				{
					pCheckUfo.LowStr(pBullet.Power);
					pBullet.Y = -30;
				
					if (pCheckUfo.Str <= 0)
					{
						return true;
					}
					else
					{
						iScore += 1;
						bufferSelect.SetCurrentPosition(0);
						bufferSelect.Play(0,BufferPlayFlags.Default);
						return false;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Draw the bullets
		/// </summary>
		public void DrawExtra(bool moveIt)
		{
			if(listExtra.Count == 0)
				return;
			// Draw bullets in the screen
			foreach(Extra pExtra in listExtra)
			{
				Rectangle r;

				if(moveIt == true)
					pExtra.Move(-3);
				r = pExtra.Draw();

				try
				{
					if(r.Height > 0)
						back.DrawFast(pExtra.X,pExtra.Y,surfaceExtra,r,DrawFastFlags.DoNotWait | DrawFastFlags.SourceColorKey);
				}
				catch(InvalidRectangleException)
				{
				}
			}

			// remove bullets
			if(listExtra.Count > 0)
			{
				for(int x=listExtra.Count-1;x>=0;x--)
				{
					Extra pExtra = (Extra)listExtra[x];
					if (pExtra.Y > 455)
					{
						listExtra.Remove(pExtra);
					}
				}
			}
		}

		/// <summary>
		/// Check if the ship get an extra
		/// </summary>
		/// <returns>the value of the extra that was hit</returns>
		int CheckHitExtra()
		{
			int iReturn = 0;

			if(listExtra.Count > 0)
			{
				for(int x=listExtra.Count-1;x>=0;x--)
				{
					Extra pExtra = (Extra)listExtra[x];

					if ((pExtra.X >= iShipPos   &&
						pExtra.X  < iShipPos+54 &&
						pExtra.Y > 385 && pExtra.Y  < 425) &&
						(pExtra.Type != 0))
					{
						iReturn = pExtra.Type;
						listExtra.Remove(pExtra);
						return iReturn;
					}
				}
			}
			return iReturn;
		}

		/// <summary>
		/// Draw the weapon in the status bar
		/// </summary>
		void DrawWeapon()
		{
			Rectangle	rcRect = new Rectangle((int)iWeapon*87,0,87,20);

			// Draw the weapon we are using
			try
			{
				back.DrawFast(84,458,StatusBar,rcRect,DrawFastFlags.DoNotWait | DrawFastFlags.SourceColorKey);
			}
			catch(InvalidRectangleException)
			{
				Console.WriteLine("weapon InvalidRectangleException {0} {1} {2} {3}",rcRect.Left,rcRect.Top,rcRect.Width,rcRect.Height);
			}
		}

		/// <summary>
		/// check if the ship was hit
		/// </summary>
		/// <returns>true if the ship was hit
		/// else false</returns>
		bool CheckHitShip()
		{
			if (iShipState == ShipState.Exploding) 
				return false;

			if(listBullet.Count > 0)
			{
				for(int x=listBullet.Count-1;x>=0;x--)
				{
					Bullet pBullet = (Bullet)listBullet[x];
					if ((pBullet.X >= iShipPos && pBullet.X < iShipPos+54 &&
						pBullet.Y > 385 && pBullet.Y < 425) &&
						(pBullet.Type != 0))
					{
						listBullet.Remove(pBullet);
						return true;
					}
				}
			}
			return false;
		}


	}
}
