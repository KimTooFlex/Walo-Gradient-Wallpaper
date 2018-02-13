// ***********************************************************************
// Assembly         : walo gradient wallpaper
// Author           : KIM TOO FLEX
// Created          : 02-11-2018
//
// Last Modified By : KIM TOO FLEX
// Last Modified On : 02-11-2018
// ***********************************************************************
// <copyright file="core.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace walo_gradient_wallpaper
{
    /// <summary>
    /// Class core.
    /// </summary>
    public static  class core
    {


        /// <summary>
        /// Sets the wall paper.
        /// </summary>
        /// <param name="img">The img.</param>
        public static void SetWallPaper(Image img)
        {
            Wallpaper.Set(img, Wallpaper.Style.Stretched);
        }
          static  Random r = new Random();

        public static Color getRandomColor()
        {
            return Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
        }


        /// <summary>
        /// Generates the image.
        /// </summary>
        /// <param name="Top">The top.</param>
        /// <param name="Bottom">The bottom.</param>
        /// <param name="Left">The left.</param>
        /// <param name="Right">The right.</param>
        /// <returns>Image.</returns>
        public static Image GenerateImage(Bunifu.Framework.UI.BunifuColorTransition Mixer, Color Top,Color Bottom,Color Left,Color Right)
        {
            //CREATE A BITMAP of same screen size

            Bitmap img = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            //loop top bottom and create color range
            for (int i = 0; i < img.Height; i++)
            {
                //get pass
                int pass = (int)Math.Round(((double)i /(double)img.Height) * 100,0);
                //create color
                Mixer.Color1 = Top;
                Mixer.Color2 = Bottom;
                Mixer.ProgessValue = pass;

                //replace colors
                Color vcol = Mixer.Value;
                for (int j = 0; j < img.Width; j++)
                {
                    int pass2 = (int)Math.Round(((double)j / (double)img.Width) * 100, 0);

                    Mixer.Color1 = Left;
                    Mixer.Color2 = Right;
                    Mixer.ProgessValue = pass2;

                    Color hcol = Mixer.Value;

                  //  img.SetPixel(j, i, hcol);
                  //  img.SetPixel(j, i,vcol);
                   img.SetPixel(j, i, Blend(vcol, hcol, 0.5));
                }

            }

            //loop left right and create color range


            //retun image 

            return img;
        }


        /// <summary>Blends the specified colors together.</summary>
        /// <param name="color">Color to blend onto the background color.</param>
        /// <param name="backColor">Color to blend the other color onto.</param>
        /// <param name="amount">How much of <paramref name="color"/> to keep,
        /// “on top of” <paramref name="backColor"/>.</param>
        /// <returns>The blended colors.</returns>
        public static Color Blend(this Color color, Color backColor, double amount)
        {
            byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
            byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
            byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
            return Color.FromArgb(r, g, b);
        }

    }


    /// <summary>
    /// Class Wallpaper. This class cannot be inherited.
    /// </summary>
    public sealed class Wallpaper
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="Wallpaper"/> class from being created.
        /// </summary>
        Wallpaper() { }

        /// <summary>
        /// The spi setdeskwallpaper
        /// </summary>
        const int SPI_SETDESKWALLPAPER = 20;
        /// <summary>
        /// The spif updateinifile
        /// </summary>
        const int SPIF_UPDATEINIFILE = 0x01;
        /// <summary>
        /// The spif sendwininichange
        /// </summary>
        const int SPIF_SENDWININICHANGE = 0x02;

        /// <summary>
        /// Systems the parameters information.
        /// </summary>
        /// <param name="uAction">The u action.</param>
        /// <param name="uParam">The u parameter.</param>
        /// <param name="lpvParam">The LPV parameter.</param>
        /// <param name="fuWinIni">The fu win ini.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        /// <summary>
        /// Enum Style
        /// </summary>
        public enum Style : int
        {
            /// <summary>
            /// The tiled
            /// </summary>
            Tiled,
            /// <summary>
            /// The centered
            /// </summary>
            Centered,
            /// <summary>
            /// The stretched
            /// </summary>
            Stretched
        }

        /// <summary>
        /// Sets the specified img.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="style">The style.</param>
        public static void Set(Image img, Style style)
        {
          
            string tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
            img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (style == Style.Stretched)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Style.Centered)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Style.Tiled)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }

            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                tempPath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
