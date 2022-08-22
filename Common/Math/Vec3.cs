using System.Numerics;

namespace matthiasffm.Common.Math;

// unpassende Regeln abstellen
#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA2225 // Provide friendly name for numeric operator

/// <summary>
/// 3D-Vektor mit immutable Charakteristik
/// </summary>
/// <typeparam name="T">Typ der Koordinaten, muss INumber implementieren</typeparam>
public record Vec3<T>(T X, T Y, T Z) :
    IAdditiveIdentity<Vec3<T>, Vec3<T>>, IMultiplicativeIdentity<Vec3<T>, Vec3<T>>,
    IEquatable<Vec3<T>>, IEqualityOperators<Vec3<T>, Vec3<T>>,
    IFormattable, ISpanFormattable,
    IAdditionOperators<Vec3<T>, Vec3<T>, Vec3<T>>,
    ISubtractionOperators<Vec3<T>, Vec3<T>, Vec3<T>>,
    IMultiplyOperators<Vec3<T>, Vec3<T>, Vec3<T>>,
    IDivisionOperators<Vec3<T>, Vec3<T>, Vec3<T>>,
    IUnaryNegationOperators<Vec3<T>, Vec3<T>>, IUnaryPlusOperators<Vec3<T>, Vec3<T>>
    where T : INumber<T>
{
    #region IAdditiveIdentity, IMultiplicativeIdentity Properties

    /// <summary>
    /// Einheitsvektor, d.h. alle Koordinaten sind vom Wert One des Typs T
    /// </summary>
    public static Vec3<T> One => new(T.One, T.One, T.One);

    /// <summary>
    /// Null-Vektor, d.h. alle Koordinaten sind vom Wert Zero des Typs T
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

    /// <summary>Länge² des Vektors</summary>
    public T LengthSquared => X * X + Y * Y + Z * Z;

    #region IFormattable, ISpanFormattable

    /// <summary>
    /// Schreibt die 3 Koordinatenwerte des Vektors formatiert in das durch den Aufrufer bereitgestellte char-Array.
    /// </summary>
    /// <param name="destination">hier hinein schreibt die Methode den formatierten Wert des Vektors</param>
    /// <param name="charsWritten">Anzahl an in destination geschriebene Zeichen</param>
    /// <param name="format">wird ignoriert</param>
    /// <param name="provider">wird ignoriert</param>
    /// <returns><i>true</i>, wenn destination ausreichend Speicher für alle Zeichen der Formatierung bereitstellt, sonst <i>false</i></returns>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
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
    /// Gibt die 3 Koordinatenwerte des Vektors als Zeichenkette aus.
    /// </summary>
    /// <param name="format">wird ignoriert</param>
    /// <param name="provider">wird ignoriert</param>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return this.ToString();
    }

    #endregion

    #region object Overrides

    /// <summary>
    /// Gibt die 3 Koordinatenwerte des Vektors als Zeichenkette aus.
    /// </summary>
    public override string ToString() => $"({X}, {Y}, {Z})";

    #endregion

    #region IAdditionOperators

    /// <summary>
    /// Addiert die 2 Vektoren koordinatenweise.
    /// </summary>
    public static Vec3<T> operator +(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    /// <summary>
    /// Addiert die 2 Vektoren koordinatenweise.
    /// </summary>
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

    #endregion

    #region ISubtractionOperators

    /// <summary>
    /// Subtrahiert die 2 Vektoren koordinatenweise.
    /// </summary>
    public static Vec3<T> operator -(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    /// <summary>
    /// Addiert die 2 Vektoren koordinatenweise.
    /// </summary>
    public static Vec3<T> operator checked -(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    /// <summary>
    /// Ändert das Vorzeichen aller Koordinaten des Vektors.
    /// </summary>
    public static Vec3<T> operator -(Vec3<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Vec3<T>(-value.X, -value.Y, -value.Z);
    }

    /// <summary>
    /// Ändert das Vorzeichen aller Koordinaten des Vektors.
    /// </summary>
    public static Vec3<T> operator checked -(Vec3<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Vec3<T>(-value.X, -value.Y, -value.Z);
    }

    #endregion

    #region IMultiplyOperators

    /// <summary>
    /// Multipliziert 2 Vektoren koordinatenweise und bildet damit das Hadamard-Produkt.
    /// </summary>
    public static Vec3<T> operator *(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    /// <summary>
    /// Multipliziert 2 Vektoren koordinatenweise und bildet damit das Hadamard-Produkt.
    /// </summary>
    public static Vec3<T> operator checked *(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    #endregion

    #region IDivisionOperators

    /// <summary>
    /// Dividiert 2 Vektoren koordinatenweise und bildet damit den Hadamard-Quotienten.
    /// </summary>
    public static Vec3<T> operator /(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    /// <summary>
    /// Dividiert 2 Vektoren koordinatenweise und bildet damit den Hadamard-Quotienten.
    /// </summary>
    public static Vec3<T> operator checked /(Vec3<T> left, Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec3<T>(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    #endregion

    /// <summary>
    /// Bildet den absoluten Wert dieses Vektoren durch koordinatenweise absoluten Betrag.
    /// </summary>
    public Vec3<T> Abs()
    {

        return new Vec3<T>(Abs(X), Abs(Y), Abs(Z));
    }

    /// <summary>
    /// Hack, weil es kein Interface a la T.IAbsOperation gibt und IComparable für T nicht vorausgesetzt wird
    /// </summary>
    private static T Abs(T number) => number >= T.Zero ? number : -number;

    /// <summary>
    /// Multipliziert die Koordinaten des Vektors jeweils mit dem Skalarwert.
    /// </summary>
    public static Vec3<T> operator *(Vec3<T> vec, T scalar)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec3<T>(vec.X * scalar, vec.Y * scalar, vec.Z * scalar);
    }

    /// <summary>
    /// Multipliziert die Koordinaten des Vektors jeweils mit dem Skalarwert.
    /// </summary>
    public static Vec3<T> operator *(T scalar, Vec3<T> vec)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec3<T>(scalar * vec.X, scalar * vec.Y, scalar * vec.Z);
    }

    /// <summary>
    /// Dividiert die Koordinaten des Vektors jeweils durch den Skalarwert.
    /// </summary>
    public static Vec3<T> operator /(Vec3<T> vec, T scalar)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec3<T>(vec.X / scalar, vec.Y / scalar, vec.Z / scalar);
    }

    /// <summary>
    /// Bildet das Kreuzprodukt des Vektors mit X²+Y²+Z²
    /// </summary>
    public T Dot(Vec3<T> right)
    {
        ArgumentNullException.ThrowIfNull(right);

        return X * right.X + Y * right.Y + Z * right.Z;
    }

    /// <summary>
    /// Prüft ob der durch den Vektor festgelegte 3D-Punkt innerhalb des Raumes liegt, der
    /// durch bottomLeft und topRight aufgespannt wird.
    /// </summary>
    public bool In(Vec3<T> bottomLeft, Vec3<T> topRight)
    {
        ArgumentNullException.ThrowIfNull(bottomLeft);
        ArgumentNullException.ThrowIfNull(topRight);

        return X >= bottomLeft.X && X <= topRight.X &&
               Y >= bottomLeft.Y && Y <= topRight.Y &&
               Z >= bottomLeft.Z && Z <= topRight.Z;
    }


    /// <summary>
    /// Erstellt einen neuen Vektor genau zwischen min und max nach min + (max - min) * amount
    /// </summary>
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

        return Abs(dx) + Abs(dy) + Abs(dz);
    }
}

#pragma warning restore CA1000 // Do not declare static members on generic types
#pragma warning restore CA2225 // Provide friendly name for numeric operator
