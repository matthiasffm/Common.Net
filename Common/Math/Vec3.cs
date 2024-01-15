using System.Numerics;

namespace matthiasffm.Common.Math;

/// <summary>
/// Generic immutable 3D vector
/// </summary>
/// <typeparam name="T">type of the vector coordinates</typeparam>
public record struct Vec3<T>(T X, T Y, T Z) :
    IAdditiveIdentity<Vec3<T>, Vec3<T>>, IMultiplicativeIdentity<Vec3<T>, Vec3<T>>,
    IEquatable<Vec3<T>>, IEqualityOperators<Vec3<T>, Vec3<T>, bool>,
    IFormattable, ISpanFormattable,
    IAdditionOperators<Vec3<T>, Vec3<T>, Vec3<T>>,
    ISubtractionOperators<Vec3<T>, Vec3<T>, Vec3<T>>,
    IMultiplyOperators<Vec3<T>, Vec3<T>, Vec3<T>>,
    IDivisionOperators<Vec3<T>, Vec3<T>, Vec3<T>>,
    IUnaryNegationOperators<Vec3<T>, Vec3<T>>, IUnaryPlusOperators<Vec3<T>, Vec3<T>>
    where T : INumber<T>
{
    #pragma warning disable CA1000 // operators are static members
    #pragma warning disable CA2225 // not providing this and its not needed for INumber implementation

    #region INumberBase<T> Properties

    /// <summary>
    /// Unit vector (all coordinates are of value INumberBase.One)
    /// </summary>
    public static Vec3<T> One => new(T.One, T.One, T.One);

    /// <summary>
    /// Zero vector (all coordinates are of value INumberBase.Zero)
    /// </summary>
    public static Vec3<T> Zero => new(T.Zero, T.Zero, T.Zero);

    /// <summary>
    /// v1 + AdditiveIdentity == v1
    /// </summary>
    public static Vec3<T> AdditiveIdentity => new(T.AdditiveIdentity, T.AdditiveIdentity, T.AdditiveIdentity);

    /// <summary>
    /// v1 .* MultiplicativeIdentity == v1
    /// </summary>
    public static Vec3<T> MultiplicativeIdentity => new(T.MultiplicativeIdentity, T.MultiplicativeIdentity, T.MultiplicativeIdentity);

    #endregion

    /// <summary>
    /// length of the vector squared
    /// </summary>
    public readonly T LengthSquared => X * X + Y * Y + Z * Z;

    #region IFormattable, ISpanFormattable

    /// <summary>
    /// Writes the vector coordinates to the destination char array.
    /// </summary>
    /// <param name="destination">writes the formatted vector coordinates here</param>
    /// <param name="charsWritten">number of chars written to destination</param>
    /// <param name="format">is ignored</param>
    /// <param name="provider">is ignored</param>
    /// <returns>
    /// <i>true</i>if the destination array has enough space left for the formatted vector coordinates
    /// <i>false</i> else.
    /// </returns>
    public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        charsWritten = 0;

        if(destination.Length < 9)
        {
            return false;
        }

        destination[charsWritten++] = '(';

        var xWritten = X.TryFormat(destination[charsWritten..], out int xCharsWritten, null, null);
        charsWritten += xCharsWritten;

        if(!xWritten || destination.Length < charsWritten + 2)
        {
            return false;
        }

        destination[charsWritten++] = ',';
        destination[charsWritten++] = ' ';

        var yWritten = Y.TryFormat(destination[charsWritten..], out int yCharsWritten, null, null);
        charsWritten += yCharsWritten;

        if(!yWritten || destination.Length < charsWritten + 2)
        {
            return false;
        }

        destination[charsWritten++] = ',';
        destination[charsWritten++] = ' ';

        var zWritten = Z.TryFormat(destination[charsWritten..], out int zCharsWritten, null, null);
        charsWritten += zCharsWritten;

        if(!zWritten || destination.Length < charsWritten + 1)
        {
            return false;
        }

        destination[charsWritten++] = ')';
        return true;
    }

    /// <summary>
    /// Returns the vector coordinates formatted in a string.
    /// </summary>
    /// <param name="format">is ignored</param>
    /// <param name="formatProvider">is ignored</param>
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        return this.ToString();
    }

    #endregion

    #region object Overrides

    /// <summary>
    /// Returns the vector coordinates formatted in a string.
    /// </summary>
    public override readonly string ToString() => $"({X}, {Y}, {Z})";

    #endregion

    #region IAdditionOperators

    /// <summary>
    /// Adds the coordinates of two vectors.
    /// </summary>
    public static Vec3<T> operator +(Vec3<T> left, Vec3<T> right)
    {
        return new Vec3<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    /// <summary>
    /// Adds the coordinates of two vectors.
    /// </summary>
    public static Vec3<T> operator checked +(Vec3<T> left, Vec3<T> right)
    {
        return new Vec3<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    /// <summary>
    /// Adds a scalar value to both coordinates of a vector.
    /// </summary>
    public static Vec3<T> operator +(Vec3<T> value)
    {
        return new Vec3<T>(value.X, value.Y, value.Z);
    }

    #endregion

    #region ISubtractionOperators

    /// <summary>
    /// Subtracts the coordinates of two vectors.
    /// </summary>
    public static Vec3<T> operator -(Vec3<T> left, Vec3<T> right)
    {
        return new Vec3<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    /// <summary>
    /// Subtracts the coordinates of two vectors.
    /// </summary>
    public static Vec3<T> operator checked -(Vec3<T> left, Vec3<T> right)
    {
        return new Vec3<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    /// <summary>
    /// Changes the sign of all coordinates a vector.
    /// </summary>
    public static Vec3<T> operator -(Vec3<T> value)
    {
        return new Vec3<T>(-value.X, -value.Y, -value.Z);
    }

    /// <summary>
    /// Changes the sign of all coordinates a vector.
    /// </summary>
    public static Vec3<T> operator checked -(Vec3<T> value)
    {
        return new Vec3<T>(-value.X, -value.Y, -value.Z);
    }

    #endregion

    #region IMultiplyOperators

    /// <summary>
    /// Multiplies the coordinates of two vectors (Hadamard product).
    /// </summary>
    public static Vec3<T> operator *(Vec3<T> left, Vec3<T> right)
    {
        return new Vec3<T>(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    /// <summary>
    /// Multiplies the coordinates of two vectors (Hadamard product).
    /// </summary>
    public static Vec3<T> operator checked *(Vec3<T> left, Vec3<T> right)
    {
        return new Vec3<T>(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    #endregion

    #region IDivisionOperators

    /// <summary>
    /// Divides the coordinates of two vectors (Hadamard division).
    /// </summary>
    public static Vec3<T> operator /(Vec3<T> left, Vec3<T> right)
    {
        return new Vec3<T>(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    /// <summary>
    /// Divides the coordinates of two vectors (Hadamard division).
    /// </summary>
    public static Vec3<T> operator checked /(Vec3<T> left, Vec3<T> right)
    {
        return new Vec3<T>(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    #endregion

    /// <summary>
    /// Returns the absolute value of the vector by calculating the absolute value of each coordinate.
    /// </summary>
    public readonly Vec3<T> Abs()
    {
        return new Vec3<T>(X.Abs(), Y.Abs(), Z.Abs());
    }

    /// <summary>
    /// Multiplies the coordinates of a vector by a scalar value.
    /// </summary>
    public static Vec3<T> operator *(Vec3<T> vec, T scalar)
    {
        return new Vec3<T>(vec.X * scalar, vec.Y * scalar, vec.Z * scalar);
    }

    /// <summary>
    /// Multiplies the coordinates of a vector by a scalar value.
    /// </summary>
    public static Vec3<T> operator *(T scalar, Vec3<T> vec)
    {
        return new Vec3<T>(scalar * vec.X, scalar * vec.Y, scalar * vec.Z);
    }

    /// <summary>
    /// Divides the coordinates of a vector by a scalar value.
    /// </summary>
    public static Vec3<T> operator /(Vec3<T> vec, T scalar)
    {
        return new Vec3<T>(vec.X / scalar, vec.Y / scalar, vec.Z / scalar);
    }

    /// <summary>
    /// Calculates the dot product of the vector with <paramref name="right"/> (dot = X1 * X2 + Y1 * Y2 + Z1 * Z2)
    /// </summary>
    public readonly T Dot(Vec3<T> right)
    {
        return X * right.X + Y * right.Y + Z * right.Z;
    }

    /// <summary>
    /// Checks if the 3D point specified by this vector lies within the 3D space defined by
    /// <paramref name="bottomLeft"/> and <paramref name="topRight"/>.
    /// </summary>
    public readonly bool In(Vec3<T> bottomLeft, Vec3<T> topRight)
    {
        return X >= bottomLeft.X && X <= topRight.X &&
               Y >= bottomLeft.Y && Y <= topRight.Y &&
               Z >= bottomLeft.Z && Z <= topRight.Z;
    }


    /// <summary>
    /// Creates a new vector so that it fulfills: new vec = min + (max - min) * amount
    /// </summary>
    public static Vec3<T> Lerp(Vec3<T> min, Vec3<T> max, T amount)
    {
        return new Vec3<T>(min.X + amount * (max.X - min.X),
                           min.Y + amount * (max.Y - min.Y),
                           min.Z + amount * (max.Z - min.Z));
    }

    /// <summary>
    /// Calculates the Euclidian distance² between the 3D point specified by this vector and <paramref name="right"/>.
    /// </summary>
    public readonly T SquaredDistance(Vec3<T> right)
    {
        var dx = X - right.X;
        var dy = Y - right.Y;
        var dz = Z - right.Z;

        return dx * dx + dy * dy + dz * dz;
    }

    /// <summary>
    /// Calculates the Manhattan distance between the 3D point specified by this vector and <paramref name="right"/>.
    /// </summary>
    public readonly T ManhattanDistance(Vec3<T> right)
    {
        var dx = X - right.X;
        var dy = Y - right.Y;
        var dz = Z - right.Z;

        return dx.Abs() + dy.Abs() + dz.Abs();
    }
}

#pragma warning restore CA1000 // Do not declare static members on generic types
