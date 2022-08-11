using System.Numerics;

namespace matthiasffm.Common.Math;

// disable stupid rule
#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA2225 // Provide friendly name for numeric operator


public record Vec3<T>(T X, T Y, T Z) :
    IComparable, IComparable<Vec3<T>>, IComparisonOperators<Vec3<T>, Vec3<T>>,
    INumberBase<Vec3<T>>
    where T : IComparable, IComparable<T>, IComparisonOperators<T, T>,
              INumberBase<T>
{
    #region INumberBase<T> Properties

    public static Vec3<T> One => new(T.One, T.One, T.One);

    public static Vec3<T> Zero => new(T.Zero, T.Zero, T.Zero);

    public static Vec3<T> AdditiveIdentity => Zero;

    public static Vec3<T> MultiplicativeIdentity => One;

    #endregion

    /// <summary>Länge des Vektors im Quadrat.</summary>
    public T LengthSquared => X * X + Y * Y + Z * Z;

    #region IComparable, IComparable<T>, IComparisonOperators<T, T>

    public int CompareTo(object? obj)
    {
        if(obj == null || (obj is not Vec3<T> vec2))
        {
            return 1;
        }
        else
        {
            return LengthSquared.CompareTo(vec2.LengthSquared);
        }
    }

    public int CompareTo(Vec3<T>? obj)
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

    public static bool operator <(Vec3<T> left, Vec3<T> right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    public static bool operator <=(Vec3<T> left, Vec3<T> right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    public static bool operator >(Vec3<T> left, Vec3<T> right)
    {
        return left is not null && left.CompareTo(right) > 0;
    }

    public static bool operator >=(Vec3<T> left, Vec3<T> right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }

    #endregion

    #region INUmberBase<T> Operatoren und Methoden

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        charsWritten = 0;

        if(destination.Length < 9)
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

        if(!yWritten || destination.Length < charsWritten + 2)
        {
            return false;
        }

        destination[charsWritten++] = ',';
        destination[charsWritten++] = ' ';

        var zWritten = Z.TryFormat(destination.Slice(charsWritten), out int zCharsWritten, null, null);
        charsWritten += zCharsWritten;

        if(!zWritten || destination.Length < charsWritten + 1)
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

    public static Vec3<T> operator +(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Vec3<T> operator checked +(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Vec3<T> operator +(Vec3<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Vec3<T>(value.X, value.Y, value.Z);
    }

    public static Vec3<T> operator ++(Vec3<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value + One;
    }

    public static Vec3<T> operator checked ++(Vec3<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value + One;
    }

    public static Vec3<T> operator -(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static Vec3<T> operator checked -(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static Vec3<T> operator -(Vec3<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Vec3<T>(-value.X, -value.Y, -value.Z);
    }

    public static Vec3<T> operator checked -(Vec3<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Vec3<T>(-value.X, -value.Y, -value.Z);
    }

    public static Vec3<T> operator --(Vec3<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value - One;
    }

    public static Vec3<T> operator checked --(Vec3<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value - One;
    }

    /// <summary>
    /// Hadamard product
    /// </summary>
    public static Vec3<T> operator *(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    public static Vec3<T> operator checked *(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    /// <summary>
    /// Hadamard quotient
    /// </summary>
    public static Vec3<T> operator /(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    public static Vec3<T> operator checked /(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    #endregion

    public Vec3<T> Abs()
    {
        return new Vec3<T>(X < T.Zero ? -X : X, Y < T.Zero ? -Y : Y, Z < T.Zero ? -Z : Z);
    }

    public static Vec3<T> operator *(Vec3<T> vec, T scalar)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec3<T>(vec.X * scalar, vec.Y * scalar, vec.Z * scalar);
    }

    public static Vec3<T> operator *(T scalar, Vec3<T> vec)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec3<T>(scalar * vec.X, scalar * vec.Y, scalar * vec.Z);
    }

    public static Vec3<T> operator /(Vec3<T> vec, T scalar)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec3<T>(vec.X / scalar, vec.Y / scalar, vec.Z / scalar);
    }

    public T Dot(Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(right);

        return X * right.X + Y * right.Y + Z * right.Z;
    }

    public bool In(Vec3<T> bottomLeft, Vec3<T> topRight)
    {
        ArgumentNullException.ThrowIfNull(bottomLeft);
        ArgumentNullException.ThrowIfNull(topRight);

        return X >= bottomLeft.X && X <= topRight.X &&
               Y >= bottomLeft.Y && Y <= topRight.Y &&
               Z >= bottomLeft.Z && Z <= topRight.Z;
    }

    public static Vec3<T> Lerp(Vec3<T> min, Vec3<T> max, T amount)
    {
        ArgumentNullException.ThrowIfNull(min);
        ArgumentNullException.ThrowIfNull(max);

        return new Vec3<T>(min.X + amount * (max.X - min.X),
                           min.Y + amount * (max.Y - min.Y),
                           min.Z + amount * (max.Z - min.Z));
    }

    /// <summary>
    /// Berechnet die Entfernung² zwischen dem durch diesen Vektor bestimmten 3D-Punkt und dem durch <i>right</i>
    /// festgelegten 3D-Punkt.
    /// </summary>
    public T SquaredDistance(Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(right);

        var dx = X - right.X;
        var dy = Y - right.Y;
        var dz = Z - right.Z;

        return dx * dx + dy * dy + dz * dz;
    }

    /// <summary>
    /// Berechnet die Manhattan-Entfernung zwischen dem durch diesen Vektor bestimmten 3D-Punkt und dem durch <i>right</i>
    /// festgelegten 3D-Punkt.
    /// </summary>
    public T ManhattanDistance(Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(right);

        var dx = X - right.X;
        var dy = Y - right.Y;
        var dz = Z - right.Z;

        return (dx > T.Zero ? dx : -dx) +
               (dy > T.Zero ? dy : -dy) +
               (dz > T.Zero ? dz : -dz);
    }

    public override string ToString() => $"({X}, {Y}, {Z})";
}

#pragma warning restore CA1000 // Do not declare static members on generic types
#pragma warning restore CA2225 // Provide friendly name for numeric operator
