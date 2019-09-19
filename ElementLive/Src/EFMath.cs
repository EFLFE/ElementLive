using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ElementLive.Src
{
    internal static class EFMath
    {
        public enum DegreesAngle
        {
            Left = 0,
            Up = 90,
            Right = 180,
            Down = 270,
        }

        /// <summary> static random. </summary>
        public static readonly Random Random;

        private static byte[] rndSingleBuffer = new byte[1];
        private static float[] absSinArray = new float[120];

        static EFMath()
        {
            Random = new Random();

            for (int i = 0; i < absSinArray.Length; i++)
            {
                // 0..1..0
                absSinArray[i] = (float)Math.Sin(Math.PI * i / absSinArray.Length);
            }
        }

        public static Vector2 HalfVector => new Vector2(0.5f);

        public static int GetAbsSinLenght => absSinArray.Length;

        /// <summary> Получить случайный цвет из стандартной палитры. </summary>
        public static Color GetRandomPaletteColor
        {
            get
            {
                switch (Random.Next(7))
                {
                    case 0:
                        return Color.Blue;

                    case 1:
                        return Color.Red;

                    case 2:
                        return Color.Green;

                    case 3:
                        return Color.Yellow;

                    case 4:
                        return Color.Magenta;

                    case 5:
                        return Color.White;

                    case 6:
                        return Color.Cyan;
                }

                return Color.Black;
            }
        }

        /// <summary> Rnd by X,Y - Random.Next(-offX, offX + 1). </summary>
        public static Vector2 RandomOffsetVector2(Vector2 vec, int offX, int offY)
        {
            vec.X += Random.Next(-offX, offX + 1);
            vec.Y += Random.Next(-offY, offY + 1);
            return vec;
        }

        /// <summary> Возвращает случайное true или false по формуле: Random.Next() % 2 == 0. </summary>
        public static bool RandomBool()
        {
            return Random.Next() % 2 == 0;
        }

        /// <summary> Возвращает случайный угол (degress). </summary>
        public static float RandomDegAngle()
        {
            return (float)Random.Next(628) * 0.01f; // 6.28
        }

        public static float RandomSingle()
        {
            Random.NextBytes(rndSingleBuffer);
            float v = rndSingleBuffer[0] / 256f;
            return v;
        }

        // fast
        public static float Pow(float x, int y)
        {
            float temp;
            if (y == 0)
                return 1;
            temp = Pow(x, y / 2);
            if (y % 2 == 0)
                return temp * temp;
            else
            {
                if (y > 0)
                    return x * temp * temp;
                else
                    return (temp * temp) / x;
            }
        }

        /// <summary>
        /// Округлить к целому. Использовать только когда это необходимо.
        /// </summary>
        public static int RoundToEven(float a)
        {
            return (int)Math.Round(a, 0, MidpointRounding.ToEven);
        }

        public static float AbsSin(int per)
        {
            return absSinArray[per % absSinArray.Length];
        }

        /// <summary> Фиксация в сетке/клетке. </summary>
        public static float RoundToGrid(float x, float gridScale)
        {
            x -= gridScale / 2f;
            x = (float)Math.Round(x / gridScale) * gridScale;
            return x;
        }

        /// <summary> Фиксация в сетке/клетке. </summary>
        public static Vector2 RoundToGrid(Vector2 pos, float gridScale)
        {
            pos -= new Vector2(gridScale / 2f);
            pos.X = (float)Math.Round(pos.X / gridScale) * gridScale;
            pos.Y = (float)Math.Round(pos.Y / gridScale) * gridScale;
            return pos;
        }

        /// <summary> Фиксация в сетке/клетке. </summary>
        public static void RoundToGrid(ref Vector2 pos, float gridScale)
        {
            pos -= new Vector2(gridScale / 2f);
            pos.X = (float)Math.Round(pos.X / gridScale) * gridScale;
            pos.Y = (float)Math.Round(pos.Y / gridScale) * gridScale;
        }

        public static float AngleBetweenVectors(Vector2 a, Vector2 b)
        {
            return (float)Math.Atan2(a.Y - b.Y, a.X - b.X);
        }

        /// <summary> Посчитать процент (float). </summary>
        /// <param name="current"> Текущее число. </param>
        /// <param name="max"> Максимальное число. </param>
        /// <param name="inPercentage"> Процент отношение текушего числа к максимальной. </param>
        /// <returns>float result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalcPercentageF(float current, float max, float inPercentage = 100f)
        {
            return current * inPercentage / max;
        }

        /// <summary> Посчитать процент. </summary>
        /// <param name="current"> Текущее число. </param>
        /// <param name="max"> Максимальное число. </param>
        /// <param name="inPercentage"> Процент отношение текушего числа к максимальной. </param>
        /// <returns>float result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CalcPercentage(int current, int max, int inPercentage = 100)
        {
            return current * inPercentage / max;
        }

        /// <summary> Возвращает true, если столкнулись две окружности. </summary>
        /// <param name="x1">Позиция первого по х</param>
        /// <param name="y1">Позиция первого по у</param>
        /// <param name="radius1">Радиус первого</param>
        /// <param name="x2">Позиция второго по х</param>
        /// <param name="y2">Позиция второго по у</param>
        /// <param name="radius2">Радиус второго</param>
        public static bool CirclesColliding(int x1, int y1, int radius1, int x2, int y2, int radius2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var radii = radius1 + radius2;
            return (dx * dx) + (dy * dy) < radii * radii;
        }

        /// <summary> Дистанция двух векторов. </summary>
        /// <param name="x1">Позиция первого по х</param>
        /// <param name="y1">Позиция первого по y</param>
        /// <param name="x2">Позиция второго по x</param>
        /// <param name="y2">Позиция второго по y</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Pow(x2 - x1, 2) + Pow(y2 - y1, 2));
        }

        /// <summary> Дистанция двух точек. </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(float x, float y)
        {
            return Math.Abs(x - y);
        }

        /// <summary>
        /// Дистанция двух точке.
        /// </summary>
        /// <param name="p1">Позиция первого.</param>
        /// <param name="p2">Позиция второго.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector2 p1, Vector2 p2)
        {
            return (float)Math.Sqrt(Pow(p2.X - p1.X, 2) + Pow(p2.Y - p1.Y, 2));
        }

        /// <summary> Градусы в радианы. </summary>
        /// <param name="degrees"> Градус. </param>
        /// <returns> Радиан </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DegreesToRadians(float degrees)
        {
            return ((float)Math.PI / 180f) * degrees;
        }

        /// <summary> Градусы в радианы. </summary>
        /// <param name="degrees"> Градус. </param>
        /// <returns> Радиан </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DegreesToRadians(double degrees)
        {
            return (Math.PI / 180.0) * degrees;
        }

        /// <summary> Радианы в градусы. </summary>
        /// <param name="radians"> Радиан. </param>
        /// <returns> Градус </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RadiansToDegrees(float radians)
        {
            return (180f / (float)Math.PI) * radians;
        }

        /// <summary>
        /// альтернатива прозрачности текстуры
        /// </summary>
        public static Color MultiplyColor(Color color, float scale)
        {
            if (scale < 0.0f)
                scale = 0f;
            else if (scale > 1.0f)
                scale = 1.0f;

            byte r = (byte)MathHelper.Clamp(color.R * scale, 0f, 255f);
            byte g = (byte)MathHelper.Clamp(color.G * scale, 0f, 255f);
            byte b = (byte)MathHelper.Clamp(color.B * scale, 0f, 255f);
            return new Color(r, g, b, (byte)MathHelper.Clamp(color.A * scale, 0f, 255f));
        }

        /// <summary> Функция перевода угла в вектор </summary>
        /// <param name="angle">Угол</param>
        /// <param name="length">Длина</param>
        public static Vector2 AngleToVector(float angle, float length, float ger)
        {
            angle -= DegreesToRadians(ger);
            return new Vector2((float)Math.Cos(angle) * length, (float)Math.Sin(angle) * length);
        }

        public static Vector2 AngleToVector(float angle, float ger)
        {
            angle -= DegreesToRadians(ger);
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -vector.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float VectorToAngle(Point point)
        {
            return (float)Math.Atan2(point.X, -point.Y);
        }

        /// <summary> Задать скорость движения к цели </summary>
        /// <param name="Position">Поизия</param>
        /// <param name="goal">Цель</param>
        /// <param name="speed">Скорость</param>
        public static Vector2 Velocity(Vector2 Position, Vector2 goal, float speed)
        {
            float dX = goal.X - Position.X;
            float dY = goal.Y - Position.Y;

            double radians = Math.Atan2(dY, dX);

            return new Vector2((float)Math.Cos(radians) * speed, (float)Math.Sin(radians) * speed);
        }

        public static float AngleToTarget(float x1, float y1, float x2, float y2, float radian = ANGLE_IN_RADIAN_90)
        {
            var dX = x1 - x2;
            var dY = y1 - y2;
            return (float)(Math.Atan2(dY, dX)) - radian;
        }

        public static float AngleToTarget(Vector2 a, Vector2 b, float radian = ANGLE_IN_RADIAN_90)
        {
            return AngleToTarget(a.X, a.Y, b.X, b.Y, radian);
        }

        /// <summary>
        /// Плавно менять угол. ex: Angle = SmoothLookTo(..)
        /// </summary>
        /// <param name="currentAngle">Текущий угол</param>
        /// <param name="speed">Скорость вращения (от 0.0 до 1.0).</param>
        public static float SmoothLookTo(float currentAngle, Vector2 center, Vector2 target, float speed, float radian = ANGLE_IN_RADIAN_90)
        {
            var atarget = AngleToTarget(center, target, radian);
            var delta = atarget - currentAngle;

            delta += (delta > MathHelper.Pi)
                ? -MathHelper.TwoPi
                : (delta < -MathHelper.Pi)
                    ? MathHelper.TwoPi
                    : 0;

            currentAngle += delta * speed;
            return MathHelper.WrapAngle(currentAngle);
        }

        /// <summary>
        /// Плавно менять угол. ex: Angle = SmoothLookTo(..)
        /// </summary>
        /// <param name="currentAngle">Текущий угол</param>
        /// <param name="speed">Скорость вращения (от 0.0 до 1.0).</param>
        public static float SmoothLookTo(float currentAngle, float atarget, float speed, float radian = ANGLE_IN_RADIAN_90)
        {
            var delta = atarget - currentAngle;

            delta += (delta > MathHelper.Pi)
                ? -MathHelper.TwoPi
                : (delta < -MathHelper.Pi)
                    ? MathHelper.TwoPi
                    : 0;

            currentAngle += delta * speed;
            return MathHelper.WrapAngle(currentAngle);
        }

        public static float AngleToTargetRad(float x1, float y1, float x2, float y2, float rad)
        {
            var dX = x1 - x2;
            var dY = y1 - y2;
            return (float)Math.Atan2(dY, dX) - rad;
        }

        /// <summary> Дистанция двух углов.
        /// <remarks> Пример сравнения: </remarks>
        /// <code>if (res меньше = dis И res больше = -dis)</code>.
        /// </summary>
        /// <param name="a1"> Угол 1. </param>
        /// <param name="a2"> Угол 2. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleDistance(float a1, float a2)
        {
            return (a1 - a2 + 180f) % 360 - 180f;
        }

        public static Vector2[] GenerateHullFromTexture(Texture2D texture)
        {
            // Calculate center of texture
            var texCenter = new Vector2(texture.Height / 2f, texture.Width / 2f);

            // Dynamic List to hold each vertices specified in screen/pixel space
            var vertexList = new List<Vector2>();

            // Store texture's pixels in a 1D array
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);

            // Loop through pixels and store any magenta ones as vertices
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    // The 2D cooridnate of each pixel in screen/pixel space
                    Color pixelXY = pixels[x + y * texture.Width];

                    // If the pixel is magenta, add its coordinate to the vertexList
                    if (pixelXY.R == 255 && pixelXY.G == 0 && pixelXY.B == 255) // 254? no
                        vertexList.Add(new Vector2(x, y));
                }
            }

            // Convert the List of vertices to an array
            Vector2[] vertexArray = vertexList.ToArray();

            // Store angles between texture's center and vertex position vector
            float[] angles = new float[vertexArray.Length];

            // Calculate the angle between each vertex and the texture's /center
            for (int i = 0; i < vertexArray.Length; i++)
            {
                angles[i] = AngleBetweenVectors(vertexArray[i], texCenter);
                vertexArray[i] -= texCenter; // Offset vertex about texture center
            }

            // Sort angles into ascending order, use to put vertices in clockwise order
            Array.Sort(angles, vertexArray);

            return vertexArray; // Return the ordered vertex array
        }

        public static unsafe bool NearEqual(float a, float b)
        {
            if (Math.Abs(a - b) < 1E-06f) // float is zero
            {
                return true;
            }

            int num = *(int*)(&a);
            int num2 = *(int*)(&b);
            if (num < 0 != num2 < 0)
            {
                return false;
            }

            int num3 = Math.Abs(num - num2);
            return num3 <= 4;
        }

        public static Vector2 GetImageSizeKeepAspectRatio(float width, float height, float maxWidth, float maxHeight)
        {
            Vector2 resize = new Vector2(width, height);

            if (width > maxWidth || height > maxHeight)
            {
                if (width > height)
                {
                    resize.X = maxWidth;
                    resize.Y = (float)Math.Floor((height / width) * maxWidth);
                }
                else
                {
                    resize.X = (float)Math.Floor((width / height) * maxHeight);
                    resize.Y = maxHeight;
                }
            }

            return resize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RSqrt(float a)
        {
            return (float)(1.0 / Math.Sqrt(a));
        }

        // https://github.com/Unity-Technologies/Unity.Mathematics/blob/e83c4209e0a3d94caa9722928893ddaf662b4137/src/Unity.Mathematics/quaternion.cs#L345
        public static float Slepr(float current, float target, float speed)
        {
            float dt = current * target;
            if (dt < 0f)
            {
                dt *= -1f;
                target *= -1f;
            }

            if (dt < 0.9995f)
            {
                float ange = (float)Math.Acos(dt);
                float s = RSqrt(1f - dt * dt); // 1.0f / sin(angle)
                float w1 = (float)Math.Sin(ange * (1f - speed)) * s;
                float w2 = (float)Math.Sin(ange * speed) * s;
                return current * w1 + target * w2;
            }
            else
            {
                // if the angle is small, use linear interpolation
                return NLepr(current, target, speed);
            }
        }

        public static float NLepr(float current, float target, float speed)
        {
            float dt = current * target;
            if (dt < 0f)
            {
                target *= -1f;
            }

            return MathHelper.Lerp(current, target, speed);
        }

        #region ANGLE IN RADIAN

        /// <summary> Угол 45 в радианах. </summary>
        public const float ANGLE_IN_RADIAN_45 = 0.7853982f;

        /// <summary> Угол 90 в радианах. </summary>
        public const float ANGLE_IN_RADIAN_90 = 1.57079637f;

        /// <summary> Угол 135 в радианах. </summary>
        public const float ANGLE_IN_RADIAN_135 = 2.3561945f;

        /// <summary> Угол 180 в радианах. </summary>
        public const float ANGLE_IN_RADIAN_180 = 3.14159274f;

        /// <summary> Угол 225 в радианах. </summary>
        public const float ANGLE_IN_RADIAN_225 = 3.92699075f;

        /// <summary> Угол 270 в радианах. </summary>
        public const float ANGLE_IN_RADIAN_270 = 4.712389f;

        /// <summary> Угол 315 в радианах. </summary>
        public const float ANGLE_IN_RADIAN_315 = 5.497787f;

        /// <summary> Угол 360 в радианах. </summary>
        public const float ANGLE_IN_RADIAN_360 = 6.28318548f;

        #endregion
    }
}
