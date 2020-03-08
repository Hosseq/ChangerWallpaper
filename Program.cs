using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace ChangerWallpaper
{
	class Program
	{
		static void Main(string[] args)
		{
			while(true)
			{
				Console.WriteLine("What wallpaper you need? (a,b,c)");
				string wallpaper = Console.ReadLine();
				ChangeWallpaper("T:/Projects/ChangerWallpaper/Pictures/" + wallpaper + ".jpg", 0, 0);
			}
		}

		const int SPI_SETDESKWALLPAPER = 20;
		const int SPIF_UPDATEINIFILE = 0x01;
		const int SPIF_SENDWININICHANGE = 0x02;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int SystemParametersInfo
			(int uAction, int uParam, string lpvParam, int fuWinIni);

		public static void ChangeWallpaper(string path, int style, int tile)
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
			key.SetValue("WallpaperStyle", style.ToString());
			key.SetValue("TileWallpaper", tile.ToString());
			SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path,
			SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
		}
	}
}
