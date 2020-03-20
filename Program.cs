using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace ChangerWallpaper
{
	class Program
	{
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		const int SW_HIDE = 0;

		const int SPI_SETDESKWALLPAPER = 20;
		const int SPIF_UPDATEINIFILE = 0x01;
		const int SPIF_SENDWININICHANGE = 0x02;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int SystemParametersInfo
			(int uAction, int uParam, string lpvParam, int fuWinIni);

		static void Main(string[] args)
		{
			SystemEvents.SessionEnding += OnSessionEnding;

			var handle = GetConsoleWindow();
			ShowWindow(handle, SW_HIDE);

			CheckStartUp();

			Console.Read();
		}

		static void CheckStartUp()
		{
			if(File.Exists(@"C:\Users\ReJ\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\ChangerWallpaper.exe") == false)
			{
				File.Copy(Directory.GetCurrentDirectory() + @"\ChangerWallpaper.exe", @"C:\Users\ReJ\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\ChangerWallpaper.exe");
			}
		}

		static void ChangeWallpaper(string path, int style, int tile)
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
			key.SetValue("WallpaperStyle", style.ToString());
			key.SetValue("TileWallpaper", tile.ToString());
			SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path,
			SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
			key.Close();
		}

		static void OnSessionEnding(object sender, SessionEndingEventArgs args)
		{
			if (args.Reason == SessionEndReasons.SystemShutdown)
			{
				Random rnd = new Random();
				ChangeWallpaper("T:/Projects/ChangerWallpaper/Pictures/" + rnd.Next(1,10) + ".jpg", 0, 0);
			}
		}
	}
}
