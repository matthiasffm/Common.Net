using System.Numerics;

namespace matthiasffm.Common.Math;

// disable stupid rule
#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA2225 // Provide friendly name for numeric operator


public record Vec2<T>(T X, T Y) :
    IComparable, IComparable<Vec2<T>>, IComparisonOperators<Vec2<T>, Vec2<T>>,
    INumberBase<Vec2<T>>
    where T : IComparable, IComparable<T>, IComparisonOperators<T, T>,
              INumberBase<T>
{
    #region INumberBase<T> Properties

    public static Vec2<T> One => new(T.One, T.One);

    public static Vec2<T> Zero => new(T.Zero, T.Zero);

    public static Vec2<T> AdditiveIdentity => Zero;

    public static Vec2<T> MultiplicativeIdentity => One;

    #endregion

    /// <summary>Länge des Vektors im Quadrat.</summary>
    public T LengthSquared => X * X + Y * Y;

    #region IComparable, IComparable<T>, IComparisonOperators<T, T>

    public int CompareTo(object? obj)
    {
        if(obj == null || (obj is not Vec2<T> vec2))
        {
            return 1;
        }
        else
        {
            return LengthSquared.CompareTo(vec2.LengthSquared);
        }
    }

    public int CompareTo(Vec2<T>? obj)
    {
        if(obj == null)
        {
            return 1;
        }
        else
        {
            return LengthSquared.CompareTo(obj.LengthSquared);
        }
    }

    public static bool operator <(Vec2<T> left, Vec2<T> right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    public static bool operator <=(Vec2<T> left, Vec2<T> right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    public static bool operator >(Vec2<T> left, Vec2<T> right)
    {
        return left is not null && left.CompareTo(right) > 0;
    }

    public static bool operator >=(Vec2<T> left, Vec2<T> right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }

    #endregion

    #region INUmberBase<T> Operatoren und Methoden

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        charsWritten = 0;

        if(destination.Length < 6)
        {
            return false;
        }

        destination[charsWritten++] = '(';

        var xWritten = X.TryFormat(destination.Slice(charsWritten), out int xCharsWritten, null, null);
        charsWritten += xCharsWritten; 

        if(!xWritten || destination.Length < charsWritten + 2)
        {
            return false;
        }

        destination[charsWritten++] = ',';
        destination[charsWritten++] = ' ';

        var yWritten = Y.TryFormat(destination.Slice(charsWritten), out int yCharsWritten, null, null);
        charsWritten += yCharsWritten;

        if(!yWritten || destination.Length < charsWritten + 1)
        {
            return false;
        }

        destination[charsWritten++] = ')';
        return true;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return this.ToString();
    }

    public static Vec2<T> operator +(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X + right.X, left.Y + right.Y);
    }

    public static Vec2<T> operator checked +(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X + right.X, left.Y + right.Y);
    }

    public static Vec2<T> operator +(Vec2<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Vec2<T>(value.X, value.Y);
    }

    public static Vec2<T> operator ++(Vec2<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value + One;
    }

    public static Vec2<T> operator checked ++(Vec2<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value + One;
    }

    public static Vec2<T> operator -(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X - right.X, left.Y - right.Y);
    }

    public static Vec2<T> operator checked -(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X - right.X, left.Y - right.Y);
    }

    public static Vec2<T> operator -(Vec2<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Vec2<T>(-value.X, -value.Y);
    }

    public static Vec2<T> operator checked -(Vec2<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Vec2<T>(-value.X, -value.Y);
    }

    public static Vec2<T> operator --(Vec2<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value - One;
    }

    public static Vec2<T> operator checked --(Vec2<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value - One;
    }

    /// <summary>
    /// Hadamard product
    /// </summary>
    public static Vec2<T> operator *(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X * right.X, left.Y * right.Y);
    }

    public static Vec2<T> operator checked *(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X * right.X, left.Y * right.Y);
    }

    /// <summary>
    /// Hadamard quotient
    /// </summary>
    public static Vec2<T> operator /(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X / right.X, left.Y / right.Y);
    }

    public static Vec2<T> operator checked /(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X / right.X, left.Y / right.Y);
    }

    #endregion

    public Vec2<T> Abs()
    {
        return new Vec2<T>(X < T.Zero ? -X : X, Y < T.Zero ? -Y : Y);
    }

    public static Vec2<T> operator *(Vec2<T> vec, T scalar)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec2<T>(vec.X * scalar, vec.Y * scalar);
    }

    public static Vec2<T> operator *(T scalar, Vec2<T> vec)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec2<T>(scalar * vec.X, scalar * vec.Y);
    }

    public static Vec2<T> operator /(Vec2<T> vec, T scalar)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec2<T>(vec.X / scalar, vec.Y / scalar);
    }

    public T Dot(Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(right);

        return X * right.X + Y * right.Y;
    }

    public bool In(Vec2<T> bottomLeft, Vec2<T> topRight)
    {
        ArgumentNullException.ThrowIfNull(bottomLeft);
        ArgumentNullException.ThrowIfNull(topRight);

        return X >= bottomLeft.X && X <= topRight.X &&
               Y >= bottomLeft.Y && Y <= topRight.Y;
    }

    public static Vec2<T> Lerp(Vec2<T> min, Vec2<T> max, T amount)
    {
        ArgumentNullException.ThrowIfNull(min);
        ArgumentNullException.ThrowIfNull(max);

        return new Vec2<T>(min.X + amount * (max.X - min.X),
                           min.Y + amount * (max.Y - min.Y));
    }

    /// <summary>
    /// Berechnet die Entfernung² zwischen dem durch diesen Vektor bestimmten 2D-Punkt und dem durch <i>right</i>
    /// festgelegten 2D-Punkt.
    /// </summary>
    public T SquaredDistance(Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(right);

        var dx = X - right.X;
        var dy = Y - right.Y;

        return dx * dx + dy * dy;
    }

    /// <summary>
    /// Berechnet die Manhattan-Entfernung zwischen dem durch diesen Vektor bestimmten 2D-Punkt und dem durch <i>right</i>
    /// festgelegten 2D-Punkt.
    /// </summary>
    public T ManhattanDistance(Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(right);

        var dx = X - right.X;
        var dy = Y - right.Y;

        return (dx > T.Zero ? dx : -dx) +
               (dy > T.Zero ? dy : -dy);
    }

    public override string ToString() => $"({X}, {Y})";
}

#pragma warning restore CA1000 // Do not declare static members on generic types
#pragma warning restore CA2225 // Provide friendly name for numeric operator
