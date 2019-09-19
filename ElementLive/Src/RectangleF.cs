using System;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ElementLive.Src
{
    struct RectangleF : IEquatable<RectangleF>
    {
        private float _left;

        private float _top;

        private float _right;

        private float _bottom;

        public static readonly RectangleF Empty;

        public static readonly RectangleF Infinite;

        public float Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public float Top
        {
            get { return _top; }
            set { _top = value; }
        }

        public float Right
        {
            get { return _right; }
            set { _right = value; }
        }

        public float Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        public float X
        {
            get { return _left; }
            set
            {
                _right = value + Width;
                _left = value;
            }
        }

        public float Y
        {
            get { return _top; }
            set
            {
                _bottom = value + Height;
                _top = value;
            }
        }

        public float Width
        {
            get { return _right - _left; }
            set { _right = _left + value; }
        }

        public float Height
        {
            get { return _bottom - _top; }
            set { _bottom = _top + value; }
        }

        public Vector2 Location
        {
            get { return new Vector2(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vector2 Center => new Vector2(X + Width / 2f, Y + Height / 2f);

        public bool IsEmpty => Width == 0f && Height == 0f && X == 0f && Y == 0f;

        public Vector2 TopLeft => new Vector2(_left, _top);

        public Vector2 TopRight => new Vector2(_right, _top);

        public Vector2 BottomLeft => new Vector2(_left, _bottom);

        public Vector2 BottomRight => new Vector2(_right, _bottom);

        static RectangleF()
        {
            Empty = new RectangleF(0f, 0f, 0f, 0f);
            Infinite = new RectangleF
            {
                Left = float.NegativeInfinity,
                Top = float.NegativeInfinity,
                Right = float.PositiveInfinity,
                Bottom = float.PositiveInfinity
            };
        }

        public RectangleF(float x, float y, float width, float height)
        {
            _left = x;
            _top = y;
            _right = x + width;
            _bottom = y + height;
        }

        public Vector2[] GetLines()
        {
            return new[] { TopLeft, TopRight, TopRight, BottomRight, BottomRight, BottomLeft, BottomLeft, TopLeft, };
        }

        public void Offset(Point amount)
        {
            Offset(amount.X, amount.Y);
        }

        public void Offset(Vector2 amount)
        {
            Offset(amount.X, amount.Y);
        }

        public void Offset(float offsetX, float offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        public void Inflate(float horizontalAmount, float verticalAmount)
        {
            X -= horizontalAmount;
            Y -= verticalAmount;
            Width += horizontalAmount * 2f;
            Height += verticalAmount * 2f;
        }

        public void Contains(ref Vector2 value, out bool result)
        {
            result = X <= value.X && value.X < Right && Y <= value.Y && value.Y < Bottom;
        }

        public bool Contains(Rectangle value)
        {
            return X <= value.X && value.Right <= Right && Y <= value.Y && value.Bottom <= Bottom;
        }

        public void Contains(ref RectangleF value, out bool result)
        {
            result = X <= value.X && value.Right <= Right && Y <= value.Y && value.Bottom <= Bottom;
        }

        public bool Contains(float x, float y)
        {
            return x >= _left && x <= _right && y >= _top && y <= _bottom;
        }

        public bool Contains(Vector2 vector2D)
        {
            return Contains(vector2D.X, vector2D.Y);
        }

        public bool Contains(Point point)
        {
            return Contains(point.X, point.Y);
        }

        public bool Intersects(ref RectangleF value)
        {
            return
                value.X < _right &&
                value.Y < _bottom &&
                X < value.Right &&
                Y < value.Bottom;
        }

        public bool Intersects(RectangleF value)
        {
            return value.X < _right && X < value.Right && value.Y < _bottom && Y < value.Bottom;
        }

        public bool Intersects(Rectangle value)
        {
            return value.X < _right && X < value.Right && value.Y < _bottom && Y < value.Bottom;
        }

        public static RectangleF Intersect(RectangleF value1, RectangleF value2)
        {
            Intersect(ref value1, ref value2, out RectangleF result);
            return result;
        }

        public static void Intersect(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
        {
            float num = value1.X > value2.X ? value1.X : value2.X;
            float num2 = value1.Y > value2.Y ? value1.Y : value2.Y;
            float num3 = value1.Right < value2.Right ? value1.Right : value2.Right;
            float num4 = value1.Bottom < value2.Bottom ? value1.Bottom : value2.Bottom;
            if (num3 > num && num4 > num2)
            {
                result = new RectangleF(num, num2, num3 - num, num4 - num2);
                return;
            }

            result = Empty;
        }

        public static RectangleF Union(RectangleF value1, RectangleF value2)
        {
            Union(ref value1, ref value2, out RectangleF result);
            return result;
        }

        public static void Union(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
        {
            float num = Math.Min(value1.Left, value2.Left);
            float num2 = Math.Max(value1.Right, value2.Right);
            float num3 = Math.Min(value1.Top, value2.Top);
            float num4 = Math.Max(value1.Bottom, value2.Bottom);
            result = new RectangleF(num, num3, num2 - num, num4 - num3);
        }

        public override bool Equals(object obj)
        {
            return !(obj is null) && !(obj.GetType() != typeof(RectangleF)) && Equals((RectangleF)obj);
        }

        public bool Equals(RectangleF other)
        {
            return EFMath.NearEqual(other.Left, Left) && EFMath.NearEqual(other.Right, Right)
                && EFMath.NearEqual(other.Top, Top) && EFMath.NearEqual(other.Bottom, Bottom);
        }

        public override int GetHashCode()
        {
            int num = _left.GetHashCode();
            num = num * 397 ^ _top.GetHashCode();
            num = num * 397 ^ _right.GetHashCode();
            return num * 397 ^ _bottom.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "X:{0} Y:{1} Width:{2} Height:{3}", X, Y, Width, Height);
        }

        // EFLFE
        public Rectangle ToRectangle()
        {
            return new Rectangle(
                (int)X,
                (int)Y,
                (int)Width,
                (int)Height);
        }

        // отрицательные ширина и высота сдвигают x и y
        public void AbsMove()
        {
            if (Width < 0f)
            {
                X += Width;
                Width *= -1f;
            }

            if (Height < 0f)
            {
                Y += Height;
                Height *= -1f;
            }
        }

        public static bool operator ==(RectangleF left, RectangleF right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RectangleF left, RectangleF right)
        {
            return !(left == right);
        }

        public static RectangleF operator +(RectangleF r1, RectangleF r2)
        {
            return new RectangleF(r1.X + r2.X, r1.Y + r2.Y, r1.Width + r2.Width, r1.Height + r2.Height);
        }

        public static RectangleF operator -(RectangleF r1, RectangleF r2)
        {
            return new RectangleF(r1.X - r2.X, r1.Y - r2.Y, r1.Width - r2.Width, r1.Height - r2.Height);
        }

        public static explicit operator Rectangle(RectangleF value)
        {
            return new Rectangle((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
        }
    }
}
