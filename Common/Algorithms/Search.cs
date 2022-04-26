namespace matthiasffm.Common.Algorithms;

public static class Search
{
    /// <summary>
    /// Führt eine Umgebungssuche innerhalb von <i>searchMap</i> ausgehend von <i>startPos</i> anhand
    /// der Kriterien in <i>surroundings</i> aus, wobei letzteres die 
    /// </summary>
    /// <remarks>Läuft in O(n * logn)?</remarks>
    public static IEnumerable<TElem> BreadthFirstSearch<TMap, TElem>(this TMap searchMap, TElem startPos, Func<TMap, TElem, IEnumerable<TElem>> surroundings)
    {
        var visited = new HashSet<TElem>
        {
            startPos
        };

        do
        {
            var newNodes = visited.SelectMany(pos => surroundings(searchMap, pos))
                                  .Except(visited)
                                  .ToArray();
            if(newNodes.Length == 0)
            {
                return visited;
            }

            visited.AddRange(newNodes);
        }
        while(true);
    }

    /// <summary>
    /// A*-Suche von start zu end unter Berücksichtigung der Distanz-Heuristik und Nachbarschaft von TPos.
    /// </summary>
    /// <param name="nodes">Liefert alle Knoten in der Karte/dem Graphen</param>
    /// <param name="start">Legt den Start-Knoten der Pfadsuche fest</param>
    /// <param name="finish">Legt den Ziel-Knoten der Pfadsuche und damit das Abbruchkriterium fest</param>
    /// <param name="Neighbors">diese Funktion muss alle direkten Nachbarn eines Knotens liefern</param>
    /// <param name="CalcCosts">diese Funktion liefert die tatsächlichen Kosten von einem Knoten zu einem Nachbarknoten</param>
    /// <param name="EstimateToFinish">diese Funktion liefert die geschätzten Kosten von einem Knoten zum Ziel-Knoten</param>
    /// <param name="maxCost">der Maximalwert von TCost zB int.MaxValue</param>
    /// <param name="Add">diese Funktion addiert zwei TCost-Werte</param>
    /// <typeparam name="TCost">Typ der Distanz</typeparam>
    /// <typeparam name="TPos">Elementetyp von <paramref name="nodes"/></typeparam>
    /// <returns>die Knoten bilden den besten Pfad von <paramref name="start"/> zum <paramref name="finish"/></returns>
    public static IEnumerable<TPos> AStar<TPos, TCost>(IEnumerable<TPos> nodes,
                                                       TPos start,
                                                       TPos finish,
                                                       Func<TPos, IEnumerable<TPos>> Neighbors,
                                                       Func<TPos, TPos, TCost> CalcCosts,
                                                       Func<TPos, TCost> EstimateToFinish,
                                                       TCost maxCost,
                                                       Func<TCost, TCost, TCost> Add)
                                                       where TPos  : notnull
                                                       where TCost : notnull, IComparable
    {
        ArgumentNullException.ThrowIfNull(Neighbors);
        ArgumentNullException.ThrowIfNull(CalcCosts);
        ArgumentNullException.ThrowIfNull(EstimateToFinish);
        ArgumentNullException.ThrowIfNull(Add);

        // init Maps für Pfade und Kosten mit dem Startknoten

        var openSet  = new HashSet<TPos>(new [] { start });
        var cameFrom = new Dictionary<TPos, TPos>();

        var minPathCosts = nodes.ToDictionary(coord => coord, coord => maxCost);
        minPathCosts[start] = default!;

        var estimatedCostToFinish = new Dictionary<TPos, TCost>(minPathCosts)
        {
            [start] = EstimateToFinish(start)
        };

        // besten Knoten aus openSet auswählen und mit dessen Kosten
        // minPathCosts und finishedPathCosts aktualisieren

        while (openSet.Any())
        {
            // TODO: Fibonacci-Heap für openSet verwenden
            var current = openSet.MinBy(o => estimatedCostToFinish[o])!;

            if(object.Equals(current, finish))
            {
                var bestPath = new[] { current }.ToList();
                while(cameFrom.ContainsKey(current))
                {
                    current = cameFrom[current];
                    bestPath.Insert(0, current);
                }
                return bestPath;
            }
            else
            {
                openSet.Remove(current);

                foreach(var neighbor in Neighbors(current))
                {
                    var costToNeighbor = Add(minPathCosts[current], CalcCosts(current, neighbor));

                    if(costToNeighbor.CompareTo(minPathCosts[neighbor]) < 0)
                    {
                        // dieser Pfad zu neighbor ist besser als alle bisherigen => merken

                        cameFrom[neighbor] = current;
                        minPathCosts[neighbor] = costToNeighbor;
                        estimatedCostToFinish[neighbor] = Add(costToNeighbor, EstimateToFinish(neighbor));

                        if(!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
        }

        return Array.Empty<TPos>();
    }
}