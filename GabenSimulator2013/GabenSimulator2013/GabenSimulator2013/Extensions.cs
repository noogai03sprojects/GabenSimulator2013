using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GabenSimulator2013
{
    static class Extensions
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }

        public static void SetBottom(this Rectangle rectangle, int bottom)
        {
            rectangle.Y = bottom - rectangle.Height;
        }

        public static Vector2 ToVector(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Rectangle Lerp(this Rectangle rectangle, Rectangle other, float amount)
        {
            rectangle.X = (int)MathHelper.Lerp(rectangle.X, other.X, amount);
            rectangle.Y = (int)MathHelper.Lerp(rectangle.Y, other.Y, amount);
            rectangle.Width = (int)MathHelper.Lerp(rectangle.Width, other.Width, amount);
            rectangle.Height = (int)MathHelper.Lerp(rectangle.Height, other.Height, amount);

            return rectangle;
        }
    }
}
