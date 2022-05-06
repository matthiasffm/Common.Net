namespace matthiasffm.Common.Algorithms;

public static class Search
{
    /// <summary>
    /// Iteriert die Knoten eines Graphen per Breitensuchstrategie, wobei zuerst _alle_ Knotens auf gleicher Tiefe besucht werden und
    /// erst dann die weiteren Kindknoten.
    /// </summary>
    /// <param name="start">Startknoten</param>
    /// <param name="stopEnumeration">optionale Abbruchbedingung der Iteration. Darf null sein, dann wird der komplette Graph durchlaufen.</param>
    /// <param name="adjacentNodes">gibt die von einem Knoten direkt erreichbaren weiteren Nachbarknoten zurück</param>
    /// <returns>Alle Knoten im Graph von <i>start</i> ausgehend, dabei immer zuerst alle Knoten auf gleicher Tiefe, dann deren Kindknoten.</returns>
    /// <remarks>Läuft in O(n + m), wobei n die Anzahl der Knoten und m die Anzahl der Verbindungen zwischen den Knoten ist.</remarks>
    public static IEnumerable<TElem> BreadthFirstEnumerate<TElem>(TElem start,
                                                                  Func<TElem, bool> stopEnumeration,
                                                                  Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(adjacentNodes);

        // Die direkten Nachbarknoten werden in eine Queue eingefügt. Damit wird sichergestellt, dass von einem Knoten aus
        // in der Breite gesucht wird. Vergleiche dazu Tiefensuche, wo ein Stack verwendet wird.

        var nextToVisit = new Queue<TElem>();
        nextToVisit.Enqueue(start);

        var visited = new HashSet<TElem> { start };

        // für Algorithmus siehe Corman oder https://de.wikipedia.org/wiki/Breitensuche

        do
        {
            var nextNode = nextToVisit.Dequeue();
            yield return nextNode;

            if(stopEnumeration != null && stopEnumeration(nextNode))
            {
                break;
            }

            foreach(var adjacent in adjacentNodes(nextNode).Except(visited))
            {
                visited.Add(adjacent);
                nextToVisit.Enqueue(adjacent);
            }
        }
        while(nextToVisit.Any());
    }

    /// <summary>
    /// Sucht den (kürzesten) Pfad von <i>start</i> zu <i>goal</i>. Dabei verwendet die Suche eine
    /// Breitensuchstrategie, wobei zuerst _alle_ Knoten einer Tiefe und dann erst deren Kindknoten besucht werden.
    /// </summary>
    /// <param name="start">Startknoten</param>
    /// <param name="goal">Zielknoten</param>
    /// <param name="adjacentNodes">gibt die von einem Knoten direkt erreichbaren weiteren Nachbarknoten zurück</param>
    /// <returns>Der Knoten auf dem Pfad von <i>start</i> zu <i>goal</i> oder die leere Menge, wenn kein Pfad gefunden wird.</returns>
    /// <remarks>Läuft in O(n + m), wobei n die Anzahl der Knoten und m die Anzahl der Verbindungen zwischen den Knoten ist.</remarks>
    public static IEnumerable<TElem> BreadthFirstSearch<TElem>(TElem start,
                                                               TElem goal,
                                                               Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
        => BreadthFirstSearch(start, node => goal.Equals(node), adjacentNodes);

    /// <summary>
    /// Sucht den (kürzesten) Pfad von <i>start</i> zu <i>goalReached()</i>. Dabei verwendet die Suche eine
    /// Breitensuchstrategie, wobei zuerst _alle_ Knoten einer Tiefe und dann erst deren Kindknoten besucht werden.
    /// </summary>
    /// <param name="start">Startknoten</param>
    /// <param name="goalReached">Bedingung für Erreichen des Zielknotens</param>
    /// <param name="adjacentNodes">gibt die von einem Knoten direkt erreichbaren weiteren Nachbarknoten zurück</param>
    /// <returns>Der Knoten auf dem Pfad von <i>start</i> zu <i>goalReached()</i> oder die leere Menge, wenn kein Pfad gefunden wird.</returns>
    /// <remarks>Läuft in O(n + m), wobei n die Anzahl der Knoten und m die Anzahl der Verbindungen zwischen den Knoten ist.</remarks>
    public static IEnumerable<TElem> BreadthFirstSearch<TElem>(TElem start,
                                                               Func<TElem, bool> goalReached,
                                                               Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(goalReached);
        ArgumentNullException.ThrowIfNull(adjacentNodes);

        // Die direkten Nachbarknoten werden in eine Queue eingefügt. Damit wird sichergestellt, dass von einem Knoten aus
        // in der Breite gesucht wird. Vergleiche dazu Tiefensuche, wo ein Stack verwendet wird.

        var nextToVisit = new Queue<TElem>();
        nextToVisit.Enqueue(start);

        var visited = new HashSet<TElem> { start };

        // für Merken des Pfades (siehe BuildPath)

        var parents = new Dictionary<TElem, TElem>();

        // für Algorithmus siehe Corman oder https://de.wikipedia.org/wiki/Breitensuche

        do
        {
            var nextNode = nextToVisit.Dequeue();

            if(goalReached(nextNode))
            {
                return BuildPath(parents, start, nextNode);
            }

            foreach(var adjacent in adjacentNodes(nextNode).Except(visited))
            {
                visited.Add(adjacent);
                nextToVisit.Enqueue(adjacent);

                parents[adjacent] = nextNode;
            }
        }
        while(nextToVisit.Any());

        // keinen Pfad gefunden

        return Array.Empty<TElem>();
    }

    // TODO: optionales Abbruchkriterium für max. Tiefe bei Tiefensuche
    // TODO: damit iterative Tiefensuche implementieren

    /// <summary>
    /// Iteriert die Knoten eines Graphen per Tiefensuchstrategie, wobei Kindknoten vor den anderen Knoten auf gleicher Tiefe besucht werden.
    /// </summary>
    /// <param name="start">Startknoten</param>
    /// <param name="stopEnumeration">optionale Abbruchbedingung der Iteration. Darf null sein, dann wird der komplette Graph durchlaufen.</param>
    /// <param name="adjacentNodes">gibt die von einem Knoten direkt erreichbaren weiteren Nachbarknoten zurück</param>
    /// <returns>Von <i>start</i> ausgehend werden Kindknoten ausgegeben, dann erst Knoten auf gleicher Tiefe.</returns>
    /// <remarks>Läuft in O(n + m), wobei n die Anzahl der Knoten und m die Anzahl der Verbindungen zwischen den Knoten ist.</remarks>
    public static IEnumerable<TElem> DepthFirstEnumerate<TElem>(TElem start,
                                                                Func<TElem, bool> stopEnumeration,
                                                                Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(adjacentNodes);

        // Die direkten Nachbarknoten werden in einen Stack eingefügt. Damit wird sichergestellt, dass von einem Knoten aus
        // in der Tiefe gesucht wird. Vergleiche dazu Breitensuche, wo eine Queue verwendet wird.

        var nextToVisit = new Stack<TElem>();
        nextToVisit.Push(start);

        var visited = new HashSet<TElem> { start };

        // für Algorithmus siehe Corman oder https://de.wikipedia.org/wiki/Tiefensuche

        do
        {
            var nextNode = nextToVisit.Pop();
            yield return nextNode;

            if(stopEnumeration != null && stopEnumeration(nextNode))
            {
                break;
            }

            foreach(var adjacent in adjacentNodes(nextNode).Except(visited))
            {
                visited.Add(adjacent);
                nextToVisit.Push(adjacent);
            }
        }
        while(nextToVisit.Any());
    }

    /// <summary>
    /// Sucht den (kürzesten) Pfad von <i>start</i> zu <i>goal</i>. Dabei verwendet die Suche eine
    /// Tiefensuchstrategie, wobei zuerst Kindknoten und dann erst andere Knoten auf gleicher Tiefe besucht werden.
    /// </summary>
    /// <param name="start">Startknoten</param>
    /// <param name="goal">Zielknoten</param>
    /// <param name="adjacentNodes">gibt die von einem Knoten direkt erreichbaren weiteren Nachbarknoten zurück</param>
    /// <returns>Der Knoten auf dem Pfad von <i>start</i> zu <i>goal</i> oder die leere Menge, wenn kein Pfad gefunden wird.</returns>
    /// <remarks>Läuft in O(n + m), wobei n die Anzahl der Knoten und m die Anzahl der Verbindungen zwischen den Knoten ist.</remarks>
    public static IEnumerable<TElem> DepthFirstSearch<TElem>(TElem start,
                                                             TElem goal,
                                                             Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
        => DepthFirstSearch(start, node => goal.Equals(node), adjacentNodes);

    /// <summary>
    /// Sucht den (kürzesten) Pfad von <i>start</i> zu <i>goalReached()</i>. Dabei verwendet die Suche eine
    /// Tiefensuchstrategie, wobei zuerst Kindknoten und dann erst andere Knoten auf gleicher Tiefe besucht werden.
    /// </summary>
    /// <param name="start">Startknoten</param>
    /// <param name="goalReached">Bedingung für Erreichen des Zielknotens</param>
    /// <param name="adjacentNodes">gibt die von einem Knoten direkt erreichbaren weiteren Nachbarknoten zurück</param>
    /// <returns>Der Knoten auf dem Pfad von <i>start</i> zu <i>goalReached()</i> oder die leere Menge, wenn kein Pfad gefunden wird.</returns>
    /// <remarks>Läuft in O(n + m), wobei n die Anzahl der Knoten und m die Anzahl der Verbindungen zwischen den Knoten ist.</remarks>
    public static IEnumerable<TElem> DepthFirstSearch<TElem>(TElem start,
                                                             Func<TElem, bool> goalReached,
                                                             Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(goalReached);
        ArgumentNullException.ThrowIfNull(adjacentNodes);

        // Die direkten Nachbarknoten werden in einen Stack eingefügt. Damit wird sichergestellt, dass von einem Knoten aus
        // in der Tiefe gesucht wird. Vergleiche dazu Breitensuche, wo eine Queue verwendet wird.

        var nextToVisit = new Stack<TElem>();
        nextToVisit.Push(start);

        var visited = new HashSet<TElem> { start };

        // für Merken des Pfades (siehe BuildPath)

        var parents = new Dictionary<TElem, TElem>();

        // für Algorithmus siehe Corman oder https://de.wikipedia.org/wiki/Tiefensuche

        do
        {
            var nextNode = nextToVisit.Pop();

            if(goalReached(nextNode))
            {
                return BuildPath(parents, start, nextNode);
            }

            foreach(var adjacent in adjacentNodes(nextNode).Except(visited))
            {
                visited.Add(adjacent);
                nextToVisit.Push(adjacent);

                parents[adjacent] = nextNode;
            }
        }
        while(nextToVisit.Any());

        // keinen Pfad gefunden

        return Array.Empty<TElem>();
    }

    /// <summary>
    /// Erstellt aus den in <i>parents</i> gemerkten Eltern-Beziehungen den Pfad als Liste von Knoten von <i>start</i> zu <i>goal</i>.
    /// </summary>
    /// <param name="parents">Abbildung von Kind => Elternknoten im (besten) Pfad</param>
    /// <param name="start">Startknoten</param>
    /// <param name="goal">Zielknoten</param>
    /// <returns>Liste der Knoten im (besten) Pfad von <i>start</i> zu <i>goal</i> inkl. dieser beiden Knoten.</returns>
    private static IEnumerable<TElem> BuildPath<TElem>(IDictionary<TElem, TElem> parents, TElem start, TElem goal) where TElem : notnull
    {
        IList<TElem> path = new List<TElem> { goal };
        var parent = goal;

        while(!start.Equals(parent))
        {
            parent = parents[parent];
            path.Add(parent);
        }

        return path.Reverse();
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