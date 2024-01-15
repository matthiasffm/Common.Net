using System.Numerics;

namespace matthiasffm.Common.Math;


/// <summary>
/// Generic immutable 2D vector
/// </summary>
/// <typeparam name="T">type of the vector coordinates</typeparam>
public record struct Vec2<T>(T X, T Y) :
    IAdditiveIdentity<Vec2<T>, Vec2<T>>, IMultiplicativeIdentity<Vec2<T>, Vec2<T>>,
    IEquatable<Vec2<T>>, IEqualityOperators<Vec2<T>, Vec2<T>, bool>,
    IFormattable, ISpanFormattable,
    IAdditionOperators<Vec2<T>, Vec2<T>, Vec2<T>>,
    ISubtractionOperators<Vec2<T>, Vec2<T>, Vec2<T>>,
    IMultiplyOperators<Vec2<T>, Vec2<T>, Vec2<T>>,
    IDivisionOperators<Vec2<T>, Vec2<T>, Vec2<T>>,
    IUnaryNegationOperators<Vec2<T>, Vec2<T>>, IUnaryPlusOperators<Vec2<T>, Vec2<T>>
    where T : INumber<T>
{
    #pragma warning disable CA1000 // operators are static members
    #pragma warning disable CA2225 // not providing this and its not needed for INumber implementation

    #region INumberBase<T> Properties

    /// <summary>
    /// Unit vector (all coordinates are of value INumberBase.One)
    /// </summary>
    public static Vec2<T> One => new(T.One, T.One);

    /// <summary>
    /// Zero vector (all coordinates are of value INumberBase.Zero)
    /// </summary>
    public static Vec2<T> Zero => new(T.Zero, T.Zero);

    /// <summary>
    /// v1 + AdditiveIdentity == v1
    /// </summary>
    public static Vec2<T> AdditiveIdentity => new(T.AdditiveIdentity, T.AdditiveIdentity);

    /// <summary>
    /// v1 .* MultiplicativeIdentity == v1
    /// </summary>
    public static Vec2<T> MultiplicativeIdentity => new(T.MultiplicativeIdentity, T.MultiplicativeIdentity);

    #endregion

    /// <summary>
    /// length of the vector squared
    /// </summary>
    public readonly T LengthSquared => X * X + Y * Y;

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

        if(destination.Length < 6)
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

        if(!yWritten || destination.Length < charsWritten + 1)
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
    public override readonly string ToString() => $"({X}, {Y})";

    #endregion

    #region IAdditionOperators

    /// <summary>
    /// Adds the coordinates of two vectors.
    /// </summary>
    public static Vec2<T> operator +(Vec2<T> left, Vec2<T> right)
    {
        return new Vec2<T>(left.X + right.X, left.Y + right.Y);
    }

    /// <summary>
    /// Adds the coordinates of two vectors.
    /// </summary>
    public static Vec2<T> operator checked +(Vec2<T> left, Vec2<T> right)
    {
        return new Vec2<T>(left.X + right.X, left.Y + right.Y);
    }

    /// <summary>
    /// Adds a scalar value to both coordinates of a vector.
    /// </summary>
    public static Vec2<T> operator +(Vec2<T> value)
    {
        return new Vec2<T>(value.X, value.Y);
    }

    #endregion

    #region ISubtractionOperators

    /// <summary>
    /// Subtracts the coordinates of two vectors.
    /// </summary>
    public static Vec2<T> operator -(Vec2<T> left, Vec2<T> right)
    {
        return new Vec2<T>(left.X - right.X, left.Y - right.Y);
    }

    /// <summary>
    /// Subtracts the coordinates of two vectors.
    /// </summary>
    public static Vec2<T> operator checked -(Vec2<T> left, Vec2<T> right)
    {
        return new Vec2<T>(left.X - right.X, left.Y - right.Y);
    }

    /// <summary>
    /// Changes the sign of all coordinates a vector.
    /// </summary>
    public static Vec2<T> operator -(Vec2<T> value)
    {
        return new Vec2<T>(-value.X, -value.Y);
    }

    /// <summary>
    /// Changes the sign of all coordinates a vector.
    /// </summary>
    public static Vec2<T> operator checked -(Vec2<T> value)
    {
        return new Vec2<T>(-value.X, -value.Y);
    }

    #endregion

    #region IMultiplyOperators

    /// <summary>
    /// Multiplies the coordinates of two vectors (Hadamard product).
    /// </summary>
    public static Vec2<T> operator *(Vec2<T> left, Vec2<T> right)
    {
        return new Vec2<T>(left.X * right.X, left.Y * right.Y);
    }

    /// <summary>
    /// Multiplies the coordinates of two vectors (Hadamard product).
    /// </summary>
    public static Vec2<T> operator checked *(Vec2<T> left, Vec2<T> right)
    {
        return new Vec2<T>(left.X * right.X, left.Y * right.Y);
    }

    #endregion

    #region IDivisionOperators

    /// <summary>
    /// Divides the coordinates of two vectors (Hadamard division).
    /// </summary>
    public static Vec2<T> operator /(Vec2<T> left, Vec2<T> right)
    {
        return new Vec2<T>(left.X / right.X, left.Y / right.Y);
    }

    /// <summary>
    /// Divides the coordinates of two vectors (Hadamard division).
    /// </summary>
    public static Vec2<T> operator checked /(Vec2<T> left, Vec2<T> right)
    {
        return new Vec2<T>(left.X / right.X, left.Y / right.Y);
    }

    #endregion

    /// <summary>
    /// Returns the absolute value of the vector by calculating the absolute value of each coordinate.
    /// </summary>
    public readonly Vec2<T> Abs()
    {
        return new Vec2<T>(X.Abs(), Y.Abs());
    }

    /// <summary>
    /// Multiplies the coordinates of a vector by a scalar value.
    /// </summary>
    public static Vec2<T> operator *(Vec2<T> vec, T scalar)
    {
        return new Vec2<T>(vec.X * scalar, vec.Y * scalar);
    }

    /// <summary>
    /// Multiplies the coordinates of a vector by a scalar value.
    /// </summary>
    public static Vec2<T> operator *(T scalar, Vec2<T> vec)
    {
        return new Vec2<T>(scalar * vec.X, scalar * vec.Y);
    }

    /// <summary>
    /// Divides the coordinates of a vector by a scalar value.
    /// </summary>
    public static Vec2<T> operator /(Vec2<T> vec, T scalar)
    {
        return new Vec2<T>(vec.X / scalar, vec.Y / scalar);
    }

    /// <summary>
    /// Calculates the dot product of the vector with <paramref name="right"/> (dot = X1 * X2 + Y1 * Y2)
    /// </summary>
    public readonly T Dot(Vec2<T> right)
    {
        return X * right.X + Y * right.Y;
    }

    /// <summary>
    /// Checks if the 2D point specified by this vector lies within the 2D space defined by
    /// <paramref name="bottomLeft"/> and <paramref name="topRight"/>.
    /// </summary>
    public readonly bool In(Vec2<T> bottomLeft, Vec2<T> topRight)
    {
        return X >= bottomLeft.X && X <= topRight.X &&
               Y >= bottomLeft.Y && Y <= topRight.Y;
    }

    /// <summary>
    /// Creates a new vector so that it fulfills: new vec = min + (max - min) * amount
    /// </summary>
    public static Vec2<T> Lerp(Vec2<T> min, Vec2<T> max, T amount)
    {
        return new Vec2<T>(min.X + amount * (max.X - min.X),
                           min.Y + amount * (max.Y - min.Y));
    }

    /// <summary>
    /// Calculates the Euclidian distance² between the 2D point specified by this vector and <paramref name="right"/>.
    /// </summary>
    public readonly T SquaredDistance(Vec2<T> right)
    {
        var dx = X - right.X;
        var dy = Y - right.Y;

        return dx * dx + dy * dy;
    }

    /// <summary>
    /// Calculates the Manhattan distance between the 2D point specified by this vector and <paramref name="right"/>.
    /// </summary>
    public readonly T ManhattanDistance(Vec2<T> right)
    {
        var dx = X - right.X;
        var dy = Y - right.Y;

        return dx.Abs() + dy.Abs();
    }
}

#pragma warning restore CA1000 // Do not declare static members on generic types
