using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Primitives
{
    public class Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2()
        {            
        }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 Zero
        {
            get { return new Vector2(); }
        }

        public double Length
        {
            get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)); }
        }

        public Vector2 Normal
        {
            get 
            {
                if (this.Length == 0)
                    return Vector2.Zero;

                return this.Divide(this.Length);
            }
        }

        public Vector2 Add(Vector2 v)
        {
            return new Vector2(X + v.X, Y + v.Y);
        }

        public Vector2 Subtract(Vector2 v)
        {
            return new Vector2(X - v.X, Y - v.Y);
        }

        public Vector2 Divide(double v)
        {
            return new Vector2(X/v, Y/v);
        }

        public Vector2 Multiply(double v)
        {
            return new Vector2(X * v, Y * v);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            if (Object.ReferenceEquals(v1, null) || Object.ReferenceEquals(v2,null))
            {
                return Object.ReferenceEquals(v1,v2);
            }

            return v1.Equals(v2);
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1 == v2);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                var v = obj as Vector2;
                if (v != null)
                {
                    return (this.X.Equals(v.X) && this.Y.Equals(v.Y));
                }
            }
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return "{{X:{0},Y:{1}}}".fmt(X, Y);
        }
    }
}
