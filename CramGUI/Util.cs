using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Cram
{
    public class Util
    {
        public static int CompareFallbackSizes(Size a, Size b)
        {
            int areaA = a.Width * a.Height;
            int areaB = b.Width * b.Height;

            // Sort by area size, descending
            if (areaA > areaB)
                return -1;
            else if (areaA < areaB)
                return 1;

            // If that doesn't work, sort by width, descending
            else if (a.Width > b.Width)
                return -1;
            else if (a.Width < b.Width)
                return 1;

            return 0;
        }

        public static string FormatBytes(ulong bytes)
        {
            // Gigabytes of VRAM are a bit too much atm

            if (bytes >= 1000 * 1024)
            {
                return String.Format("{0:0.#} MB", (float)bytes / (1024 * 1024));
            }
            else if (bytes >= 1000)
            {
                return String.Format("{0:0.#} kB", (float)bytes / (1024));
            }
            else
            {
                return String.Format("{0} bytes", bytes);
            }
        }

        public static bool ParseSizeList(string str, out List<Size> result)
        {
            result = new List<Size>();
            char[] seps = new char[] {','};
            string[] parts = str.Split(seps);
            foreach (string part in parts)
            {
                Size size;
                if (!ParseSize(part.Trim(), out size))
                    return false;
                result.Add(size);
            }

            return true;
        }

        public static bool ParseSize(string str, out Size result)
        {
            result = new Size(0, 0);

            Regex regex = new Regex(@"^\s*([0-9]+)\s*[xX,: ]\s*([0-9]+)\s*$");
            Match match = regex.Match(str);
            if (!match.Success)
                return false;

            int w, h;
            if (!Int32.TryParse(match.Groups[1].Value, out w))
                return false;
            if (!Int32.TryParse(match.Groups[2].Value, out h))
                return false;
            if (w <= 0 || h <= 0)
                return false;
            result = new Size(w, h);
            return true;
        }

        public static string FormatColor(Color col)
        {
            return String.Format("{0:x2}{1:x2}{2:x2}{3:x2}", col.A, col.R, col.G, col.B);
        }

        public static bool ParseColor(string s, out Color col)
        {
            Regex r = new Regex("^[0-9A-Fa-f]+$");
            col = Color.White;
            if (!r.Match(s).Success)
                return false;
            // Allow with and without alpha versions
            if (s.Length != 6 &&
                s.Length != 8)
                return false;
            bool hasAlpha = s.Length == 8;
            int A, R, G, B;
            try
            {
                if (hasAlpha)
                {
                    A = Int32.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    R = Int32.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    G = Int32.Parse(s.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    B = Int32.Parse(s.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    A = 255;
                    R = Int32.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    G = Int32.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    B = Int32.Parse(s.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception)
            {
                return false;
            }

            col = Color.FromArgb(A, R, G, B);
            return true;
        }
    }
}
