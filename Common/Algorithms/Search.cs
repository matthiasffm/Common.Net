namespace matthiasffm.Common.Algorithms;

public static class Search
{
    /// <summary>
    /// Sucht den Pfad von <i>start</i> zu <i>goal</i> innerhalb von <i>searchMap</i>. Dabei verwendet die Methode eine
    /// Breitensuchstrategie, wobei zuerst alle direkten Folgeknoten des Ausgangsknotens besucht und dann die weiteren.
    /// 
    /// Die von einem Knoten erreichbaren weiteren Knoten gibt der Functor <i>surroundings</i> vor.
    /// </summary>
    /// <param name="searchMap"></param>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <param name="surroundings"></param>
    /// <returns>Der Knoten auf dem Pfad von <i>start</i> zu <i>goal</i> oder die leere Menge, wenn kein Pfad gefunden wird.</returns>
    /// <remarks>Läuft in O(n * logn)?</remarks>
    public static IEnumerable<TElem> BreadthFirstSearch<TMap, TElem>(this TMap searchMap,
                                                                     TElem start,
                                                                     TElem goal,
                                                                     Func<TMap, TElem, IEnumerable<TElem>> surroundings)
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(goal);
        ArgumentNullException.ThrowIfNull(surroundings);

        var nodes = new Queue<TElem>(); // TODO: viel schnelleren Fibonacci Heap verwenden
        nodes.Enqueue(start);

        var visited = new HashSet<TElem>();

        // für Merken des Pfades
        var parents = new Dictionary<TElem, TElem>();

        do
        {
            var nextNode = nodes.Dequeue();
            visited.Add(nextNode);

            if(goal.Equals(nextNode))
            {
                IList<TElem> path = new List<TElem>();
                var parent = goal;
                while(!start.Equals(parent))
                {
                    parent = parents[parent];
                    path.Add(parent);
                }
                path.Add(goal);
                return path.Reverse();
            }

            foreach(var neighbor in surroundings(searchMap, nextNode).Except(visited).Except(nodes))
            {
                nodes.Enqueue(neighbor);
                parents[neighbor] = nextNode;
            }
        }
        while(nodes.Any());

        // keinen Pfad von start nach goal gefunden
        return Array.Empty<TElem>();
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

        while(openSet.Any())
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