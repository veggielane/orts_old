using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Primitives;
using System.Windows;

namespace WpfTester
{
    public static class Extensions
    {
        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }


        public static bool IsInside(this Vector2 v, Rect r)
        {
            return (v.X >= r.Left) && (v.X <= r.Right) && (v.Y >= r.Top) && (v.Y <= r.Bottom);
        }
    }
}
