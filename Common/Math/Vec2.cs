using System.Numerics;

namespace matthiasffm.Common.Math;

// unpassende Regeln abstellen
#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA2225 // Provide friendly name for numeric operator

/// <summary>
/// 2D-Vektor mit immutable Charakteristik
/// </summary>
/// <typeparam name="T">Typ der Koordinaten, muss INumber implementieren</typeparam>
public record Vec2<T>(T X, T Y) :
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
    #region INumberBase<T> Properties

    /// <summary>
    /// Einheitsvektor, d.h. alle Koordinaten sind vom Wert One des Typs T
    /// </summary>
    public static Vec2<T> One => new(T.One, T.One);

    /// <summary>
    /// Null-Vektor, d.h. alle Koordinaten sind vom Wert Zero des Typs T
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

    /// <summary>Länge² des Vektors</summary>
    public T LengthSquared => X * X + Y * Y;

    #region IFormattable, ISpanFormattable

    /// <summary>
    /// Schreibt die 2 Koordinatenwerte des Vektors formatiert in das durch den Aufrufer bereitgestellte char-Array.
    /// </summary>
    /// <param name="destination">hier hinein schreibt die Methode den formatierten Wert des Vektors</param>
    /// <param name="charsWritten">Anzahl an in destination geschriebene Zeichen</param>
    /// <param name="format">wird ignoriert</param>
    /// <param name="provider">wird ignoriert</param>
    /// <returns><i>true</i>, wenn destination ausreichend Speicher für alle Zeichen der Formatierung bereitstellt, sonst <i>false</i></returns>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
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
    /// Gibt die 2 Koordinatenwerte des Vektors als Zeichenkette aus.
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
    /// Gibt die 2 Koordinatenwerte des Vektors als Zeichenkette aus.
    /// </summary>
    public override string ToString() => $"({X}, {Y}";

    #endregion

    #region IAdditionOperators

    /// <summary>
    /// Addiert die 2 Vektoren koordinatenweise.
    /// </summary>
    public static Vec2<T> operator +(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X + right.X, left.Y + right.Y);
    }

    /// <summary>
    /// Addiert die 2 Vektoren koordinatenweise.
    /// </summary>
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

    #endregion

    #region ISubtractionOperators

    /// <summary>
    /// Subtrahiert die 2 Vektoren koordinatenweise.
    /// </summary>
    public static Vec2<T> operator -(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X - right.X, left.Y - right.Y);
    }

    /// <summary>
    /// Subtrahiert die 2 Vektoren koordinatenweise.
    /// </summary>
    public static Vec2<T> operator checked -(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X - right.X, left.Y - right.Y);
    }

    /// <summary>
    /// Ändert das Vorzeichen aller Koordinaten des Vektors.
    /// </summary>
    public static Vec2<T> operator -(Vec2<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Vec2<T>(-value.X, -value.Y);
    }

    /// <summary>
    /// Ändert das Vorzeichen aller Koordinaten des Vektors.
    /// </summary>
    public static Vec2<T> operator checked -(Vec2<T> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Vec2<T>(-value.X, -value.Y);
    }

    #endregion

    #region IMultiplyOperators

    /// <summary>
    /// Multipliziert 2 Vektoren koordinatenweise und bildet damit das Hadamard-Produkt.
    /// </summary>
    public static Vec2<T> operator *(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X * right.X, left.Y * right.Y);
    }

    /// <summary>
    /// Multipliziert 2 Vektoren koordinatenweise und bildet damit das Hadamard-Produkt.
    /// </summary>
    public static Vec2<T> operator checked *(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X * right.X, left.Y * right.Y);
    }

    #endregion

    #region IDivisionOperators

    /// <summary>
    /// Dividiert 2 Vektoren koordinatenweise und bildet damit den Hadamard-Quotienten.
    /// </summary>
    public static Vec2<T> operator /(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X / right.X, left.Y / right.Y);
    }

    /// <summary>
    /// Dividiert 2 Vektoren koordinatenweise und bildet damit den Hadamard-Quotienten.
    /// </summary>
    public static Vec2<T> operator checked /(Vec2<T> left, Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Vec2<T>(left.X / right.X, left.Y / right.Y);
    }

    #endregion

    /// <summary>
    /// Bildet den absoluten Wert dieses Vektoren durch koordinatenweise absoluten Betrag.
    /// </summary>
    public Vec2<T> Abs()
    {
        return new Vec2<T>(Abs(X), Abs(Y));
    }

    /// <summary>
    /// Hack, weil es kein Interface a la T.IAbsOperation gibt und IComparable für T nicht vorausgesetzt wird
    /// </summary>
    private static T Abs(T number) => number >= T.Zero ? number : -number;

    /// <summary>
    /// Multipliziert die Koordinaten des Vektors jeweils mit dem Skalarwert.
    /// </summary>
    public static Vec2<T> operator *(Vec2<T> vec, T scalar)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec2<T>(vec.X * scalar, vec.Y * scalar);
    }

    /// <summary>
    /// Multipliziert die Koordinaten des Vektors jeweils mit dem Skalarwert.
    /// </summary>
    public static Vec2<T> operator *(T scalar, Vec2<T> vec)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec2<T>(scalar * vec.X, scalar * vec.Y);
    }

    /// <summary>
    /// Dividiert die Koordinaten des Vektors jeweils durch den Skalarwert.
    /// </summary>
    public static Vec2<T> operator /(Vec2<T> vec, T scalar)
    {
        ArgumentNullException.ThrowIfNull(vec);

        return new Vec2<T>(vec.X / scalar, vec.Y / scalar);
    }

    /// <summary>
    /// Bildet das Kreuzprodukt des Vektors mit X² + Y²
    /// </summary>
    public T Dot(Vec2<T> right)
    {
        ArgumentNullException.ThrowIfNull(right);

        return X * right.X + Y * right.Y;
    }

    /// <summary>
    /// Prüft ob der durch den Vektor festgelegte 3D-Punkt innerhalb des Raumes liegt, der
    /// durch bottomLeft und topRight aufgespannt wird.
    /// </summary>
    public bool In(Vec2<T> bottomLeft, Vec2<T> topRight)
    {
        ArgumentNullException.ThrowIfNull(bottomLeft);
        ArgumentNullException.ThrowIfNull(topRight);

        return X >= bottomLeft.X && X <= topRight.X &&
               Y >= bottomLeft.Y && Y <= topRight.Y;
    }

    /// <summary>
    /// Erstellt einen neuen Vektor genau zwischen min und max nach min + (max - min) * amount
    /// </summary>
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
}

#pragma warning restore CA1000 // Do not declare static members on generic types
#pragma warning restore CA2225 // Provide friendly name for numeric operator
