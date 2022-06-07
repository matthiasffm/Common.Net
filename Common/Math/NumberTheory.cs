using System.Numerics;

namespace matthiasffm.Common.Math;

/// <summary>
/// Methoden aus der Zahlentheorie zur Bestimmung zB simultaner Kongruenzen.
/// </summary>
public static class NumberTheory
{
    // TODO: alle Methoden zu einer einzigen generischen zusammenfassen, sobald BigInteger INumber implementiert

    /// <summary>
    /// Löst die linearen Kongruenzen ganzer Zahlen aus n Tupel mit je x = [a, mod m]:
    /// x = [a(1), mod m(1)]
    /// x = [a(2), mod m(2)]
    /// ...
    /// x = [a(n), mod m(n)]
    /// 
    /// Liefert als Lösung das kleinste x, dass alle Kongruenzen erfüllt.
    /// </summary>
    /// <param name="input">Tupel aus rest a und mod m</param>
    /// <returns>Das Tupel aus x und mod m', wobei m' das kgV aller m darstellt und x _alle_ Input-Tupel erfüllt</returns>
    /// <remarks>siehe https://de.wikipedia.org/wiki/Chinesischer_Restsatz</remarks>
    public static (long, long) CalcSimultaneousCongruences(params (long, long)[] input)
    {
        return CalcSimultaneousCongruences((IEnumerable<(long, long)>)input);
    }

    /// <summary>
    /// Löst die linearen Kongruenzen ganzer Zahlen aus n Tupel mit je x = [a, mod m]:
    /// x = [a(1), mod m(1)]
    /// x = [a(2), mod m(2)]
    /// ...
    /// x = [a(n), mod m(n)]
    /// 
    /// Liefert als Lösung das kleinste x, dass alle Kongruenzen erfüllt.
    /// </summary>
    /// <param name="input">Tupel aus rest a und mod m</param>
    /// <returns>Das Tupel aus x und mod m', wobei m' das kgV aller m darstellt und x _alle_ Input-Tupel erfüllt</returns>
    /// <remarks>siehe https://de.wikipedia.org/wiki/Chinesischer_Restsatz</remarks>
    /// <exception cref="OverflowException">Im Falle eines numerischen Überlaufs besser die Methode mit BigInteger-Argumenten verwenden.</exception>
    public static (long, long) CalcSimultaneousCongruences(IEnumerable<(long, long)> input)
    {
        ArgumentNullException.ThrowIfNull(input);

        // löst die Kongruenzen paarweise

        var first = input.First();
        return input.Skip(1)
                    .Aggregate(first, (aggr, next) => CalcSimultaneousCongruence(aggr.Item1, aggr.Item2, next.Item1, next.Item2));
    }

    /// <summary>
    /// Löst die linearen Kongruenzen großer ganzer Zahlen aus n Tupel mit je x = [a, mod m]:
    /// x = [a(1), mod m(1)]
    /// x = [a(2), mod m(2)]
    /// ...
    /// x = [a(n), mod m(n)]
    /// 
    /// Liefert als Lösung das kleinste x, dass alle Kongruenzen erfüllt.
    /// </summary>
    /// <param name="input">Tupel aus rest a und mod m</param>
    /// <returns>Das Tupel aus x und mod m', wobei m' das kgV aller m darstellt und x _alle_ Input-Tupel erfüllt</returns>
    /// <remarks>siehe https://de.wikipedia.org/wiki/Chinesischer_Restsatz</remarks>
    public static (BigInteger, BigInteger) CalcSimultaneousCongruences(IEnumerable<(BigInteger, BigInteger)> input)
    {
        ArgumentNullException.ThrowIfNull(input);

        // löst die Kongruenzen paarweise

        var first = input.First();
        return input.Skip(1)
                    .Aggregate(first, (aggr, next) => CalcSimultaneousCongruence(aggr.Item1, aggr.Item2, next.Item1, next.Item2));
    }

    /// <summary>Löst x = a mod n und x = b mod m</summary>
    /// <returns>Tupel aus x und m', wobei m' das kgV von n und m ist</returns>
    /// <remarks>Benutzt dazu den erweiterten euklidischen Algorithmus für die Ermittlung des größten gemeinsamen Teilers</remarks>
    private static (long, long) CalcSimultaneousCongruence(long a, long n, long b, long m)
    {
        // zuerst kgV durch erweiterten Euklid bestimmen

        var (gcd, y, _) = Euclid.GcdExt(n, m);
        var lcm = checked(m * n / gcd);

        // dann x damit direkt ausrechnen nach chin. Restsatz (https://de.wikipedia.org/w/index.php?title=Chinesischer_Restsatz&section=8)

        var x = checked(a - ((a - b) / gcd) * y * n) % lcm;
        
        return x switch
        {
            < 0 => (x + lcm, lcm),
            _   => (x, lcm),
        };
    }

    /// <summary>Löst x = a mod n und x = b mod m für sehr große Zahlen</summary>
    /// <returns>Tupel aus x und m', wobei m' das kgV von n und m ist</returns>
    /// <remarks>Benutzt dazu den erweiterten euklidischen Algorithmus für die Ermittlung des größten gemeinsamen Teilers</remarks>
    private static (BigInteger, BigInteger) CalcSimultaneousCongruence(BigInteger a, BigInteger n, BigInteger b, BigInteger m)
    {
        // zuerst kgV durch erweiterten Euklid bestimmen

        var (gcd, y, _) = Euclid.GcdExt(n, m);
        var lcm = m * n / gcd;

        // dann x damit direkt ausrechnen nach chin. Restsatz (https://de.wikipedia.org/w/index.php?title=Chinesischer_Restsatz&section=8)

        var x = (a - (a - b) / gcd * y * n) % lcm;

        return x < BigInteger.Zero ? (x + lcm, lcm) : (x, lcm);
    }
}
