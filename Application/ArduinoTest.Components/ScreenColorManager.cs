﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ArduinoTest.Components.Abstract;
using Color = ArduinoTest.Components.Domain.Color;

namespace ArduinoTest.Components
{
    /// <summary>
    /// Figures out what the Screen Color is
    /// </summary>
    public class ScreenColorManager : IScreenColorManager
    {

        private static Bitmap _screenPixel = new Bitmap(1, 1);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int BitBlt(IntPtr hDc, int x, int y, int nWidth, int nHeight, IntPtr hSrcDc, int xSrc, int ySrc, int dwRop);

        /// <summary>
        /// Gets the color of the pixel at the center of the screen
        /// </summary>
        /// <returns></returns>
        public Color GetCenterScreenColor()
        {            

            //method to test
            using (var gdest = Graphics.FromImage(_screenPixel))
            {
                using (var gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    try
                    {
                        var hSrcDc = gsrc.GetHdc();
                        var hDc = gdest.GetHdc();
                        BitBlt(hDc, 0, 0, 1, 1, hSrcDc, 500, 540, (int)CopyPixelOperation.SourceCopy);
                        gdest.ReleaseHdc();
                        gsrc.ReleaseHdc();
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            return MapColor(_screenPixel.GetPixel(0, 0));
        }

        /// <summary>
        /// Maps a .Net Color Type to the Local Color Type
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static Color MapColor(System.Drawing.Color color)
        {
            var mappedColor = new Color
                {
                    Red = color.R,
                    Green = color.G,
                    Blue = color.B
                };
            return mappedColor;
        }
    }
}
