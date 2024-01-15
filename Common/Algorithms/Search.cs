using System.Numerics;

using matthiasffm.Common.Collections;

namespace matthiasffm.Common.Algorithms;

/// <summary>
/// Provides generic search and iteration algorithms for graph or tree like data structures.
/// </summary>
/// <remarks>
/// To define the search space the caller has to provide a lambda function to enumerate all direct neighbors of a 
/// element in the search space.
/// </remarks>
public static class Search
{
    /// <summary>
    /// Enumerates a search space by iterating over _all_ direct neighbors of the current elements
    /// before moving on to search at theses neighbors.
    /// </summary>
    /// <param name="start">
    /// Element to start the enumeration from
    /// </param>
    /// <param name="stopEnumeration">
    /// Optional condition to stop the enumeration. If it is <i>null</i> the whole graph will be iterated.
    /// </param>
    /// <param name="adjacentNodes">
    /// Provides an enumeration of all direct neighbors of an element.
    /// </param>
    /// <returns>
    /// Starting at <i>start</i> all of its direct neighbors, then the neighbors of these elements and so on.
    /// </returns>
    /// <remarks>
    /// Runs in O(n + m), where n is the number of elements in the search space and m is the number of neighbor relationships of these elements.
    /// </remarks>
    public static IEnumerable<TElem> BreadthFirstEnumerate<TElem>(TElem start,
                                                                  Func<TElem, bool> stopEnumeration,
                                                                  Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(adjacentNodes);

        // the neighbors are stored in a fifo queue
        // this ensures _all_ elements of a specific depth are looked at first before any element of depth + 1 is enumerated
        // see also depth first enumeration where a stack is used

        var nextToVisit = new Queue<TElem>();
        nextToVisit.Enqueue(start);

        var visited = new HashSet<TElem> { start };

        do
        {
            var nextNode = nextToVisit.Dequeue();
            yield return nextNode;

            if(stopEnumeration != null && stopEnumeration(nextNode))
            {
                break;
            }

            // TODO: check in Corman if O(n+m) is correct with Except calls
            foreach(var adjacent in adjacentNodes(nextNode).Except(visited))
            {
                visited.Add(adjacent);
                nextToVisit.Enqueue(adjacent);
            }
        }
        while(nextToVisit.Any());
    }

    /// <summary>
    /// Search a space of elements from start to goal element by iterating over _all_ direct neighbors
    /// of the current elements before moving on to search at theses neighbors.
    /// </summary>
    /// <param name="start">
    /// Element to start the search from
    /// </param>
    /// <param name="goal">
    /// Element to reach
    /// </param>
    /// <param name="adjacentNodes">
    /// Provides an enumeration of all direct neighbors of an element.
    /// </param>
    /// <returns>
    /// The elements on a path from <paramref name="start"/> to <paramref name="goal"/>.
    /// </returns>
    /// <remarks>
    /// Runs in O(n + m), where n is the number of elements in the search space and m is the number of neighbor relationships of these elements.
    /// </remarks>
    public static IEnumerable<TElem> BreadthFirstSearch<TElem>(TElem start,
                                                               TElem goal,
                                                               Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
        => BreadthFirstSearch(start, node => goal.Equals(node), adjacentNodes);

    /// <summary>
    /// Search a space of elements from start to goal element by iterating over _all_ direct neighbors
    /// of the current elements before moving on to search at theses neighbors.
    /// </summary>
    /// <param name="start">
    /// Element to start the search from
    /// </param>
    /// <param name="goalReached">
    /// Optional condition to indicate that at the specific element the goal of the search is reached.
    /// </param>
    /// <param name="adjacentNodes">
    /// Provides an enumeration of all direct neighbors of an element.
    /// </param>
    /// <returns>
    /// The elements on a path from <paramref name="start"/> until the goal is reached./>.
    /// </returns>
    /// <remarks>
    /// Runs in O(n + m), where n is the number of elements in the search space and m is the number of neighbor relationships of these elements.
    /// </remarks>
    public static IEnumerable<TElem> BreadthFirstSearch<TElem>(TElem start,
                                                               Func<TElem, bool> goalReached,
                                                               Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(goalReached);
        ArgumentNullException.ThrowIfNull(adjacentNodes);

        // the neighbors are stored in a fifo queue
        // this ensures _all_ elements of a specific depth are looked at first before any element of depth + 1 are searched
        // see also depth first search where a stack is used

        var nextToVisit = new Queue<TElem>();
        nextToVisit.Enqueue(start);

        var visited = new HashSet<TElem> { start };

        // here the parents of nodes are stored to rebuild the path back from goal to start (see BuildPath)

        var parents = new Dictionary<TElem, TElem>();

        do
        {
            var nextNode = nextToVisit.Dequeue();

            if(goalReached(nextNode))
            {
                return BuildPath(nextNode, parents);
            }

            foreach(var adjacent in adjacentNodes(nextNode).Except(visited))
            {
                visited.Add(adjacent);
                nextToVisit.Enqueue(adjacent);

                parents[adjacent] = nextNode;
            }
        }
        while(nextToVisit.Any());

        // enumerated the whole search space but goal was not reached or found

        return Array.Empty<TElem>();
    }

    // TODO: add max depth parameter for search and implement iterative depth search with it

    /// <summary>
    /// Enumerates a search space by moving to the first neighbor element before enumerating all other neighbor elements.
    /// </summary>
    /// <param name="start">
    /// Element to start the enumeration from
    /// </param>
    /// <param name="stopEnumeration">
    /// Optional condition to stop the enumeration. If it is <i>null</i> the whole graph will be iterated.
    /// </param>
    /// <param name="adjacentNodes">
    /// Provides an enumeration of all direct neighbors of an element.
    /// </param>
    /// <returns>
    /// Starting at <i>start</i> moves to the first neighbor before enumeration the other neighbors of start and so on.
    /// </returns>
    /// <remarks>
    /// Runs in O(n + m), where n is the number of elements in the search space and m is the number of neighbor relationships of these elements.
    /// </remarks>
    public static IEnumerable<TElem> DepthFirstEnumerate<TElem>(TElem start,
                                                                Func<TElem, bool> stopEnumeration,
                                                                Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(adjacentNodes);

        // the neighbors are stored in a stack
        // this ensures the only one neighbor of the current element is enumerated next (depth first) and
        // all other neighbors go to the end of the queue
        // see also breadth first enumeration where a fifo queue is used

        var nextToVisit = new Stack<TElem>();
        nextToVisit.Push(start);

        var visited = new HashSet<TElem> { start };

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
    /// Searches a space of elements by moving to the first neighbor element before searching
    /// all other neighbor elements (depth before breadth).
    /// </summary>
    /// <param name="start">
    /// Element to start the search from
    /// </param>
    /// <param name="goal">
    /// Element to reach
    /// </param>
    /// <param name="adjacentNodes">
    /// Provides an enumeration of all direct neighbors of an element.
    /// </param>
    /// <returns>
    /// The elements on a path from <paramref name="start"/> to <paramref name="goal"/>.
    /// </returns>
    /// <remarks>
    /// Runs in O(n + m), where n is the number of elements in the search space and m is the number of neighbor relationships of these elements.
    /// </remarks>
    public static IEnumerable<TElem> DepthFirstSearch<TElem>(TElem start,
                                                             TElem goal,
                                                             Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
        => DepthFirstSearch(start, node => goal.Equals(node), adjacentNodes);

    /// <summary>
    /// Searches a space of elements by moving to the first neighbor element before searching
    /// all other neighbor elements (depth before breadth).
    /// </summary>
    /// <param name="start">
    /// Element to start the search from
    /// </param>
    /// <param name="goalReached">
    /// Optional condition to indicate that at the specific element the goal of the search is reached.
    /// </param>
    /// <param name="adjacentNodes">
    /// Provides an enumeration of all direct neighbors of an element.
    /// </param>
    /// <returns>
    /// The elements on a path from <paramref name="start"/> until the goal is reached.
    /// </returns>
    /// <remarks>
    /// Runs in O(n + m), where n is the number of elements in the search space and m is the number of neighbor relationships of these elements.
    /// </remarks>
    public static IEnumerable<TElem> DepthFirstSearch<TElem>(TElem start,
                                                             Func<TElem, bool> goalReached,
                                                             Func<TElem, IEnumerable<TElem>> adjacentNodes) where TElem : notnull
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(goalReached);
        ArgumentNullException.ThrowIfNull(adjacentNodes);

        // the neighbors are stored in a stack
        // this ensures the only one neighbor of the current element is enumerated next (depth first) and
        // all other neighbors go to the end of the queue
        // see also breadth first enumeration where a fifo queue is used

        var nextToVisit = new Stack<TElem>();
        nextToVisit.Push(start);

        var visited = new HashSet<TElem> { start };

        // here the parents of nodes are stored to rebuild the path back from goal to start (see BuildPath)

        var parents = new Dictionary<TElem, TElem>();

        do
        {
            var nextNode = nextToVisit.Pop();

            if(goalReached(nextNode))
            {
                return BuildPath(nextNode, parents);
            }

            foreach(var adjacent in adjacentNodes(nextNode).Except(visited))
            {
                visited.Add(adjacent);
                nextToVisit.Push(adjacent);

                parents[adjacent] = nextNode;
            }
        }
        while(nextToVisit.Any());

        // enumerated the whole search space but goal was not reached or found

        return Array.Empty<TElem>();
    }

    /// <summary>
    /// A* algorithm to search the best path from <paramref name="start"/> to <paramref name="finish"/> condition by optimizing the search on <typeparamref name="TCost"/>.
    /// </summary>
    /// <param name="start">The start node for searching the best path.</param>
    /// <param name="finish">The node to reach.</param>
    /// <param name="Neighbors">This function has to return all neighbors of a single position in the graph.</param>
    /// <param name="CalcCosts">This function has to return the <i>actual</i> costs from one node to a neighbor.</param>
    /// <param name="EstimateToFinish">This function has to return the <i>estimated</i> costs from one node to another. The estimated cost must be equal or less than the actual costs!</param>
    /// <param name="maxCost">The maximum value of <typeparamref name="TCost"/> i.e. int.MaxValue.</param>
    /// <typeparam name="TPos">Type of nodes in the graph.</typeparam>
    /// <typeparam name="TCost">The type of the distance values.</typeparam>
    /// <returns>The nodes in the path from <paramref name="start"/> until <paramref name="finish"/> is reached</returns>
    public static IEnumerable<TPos> AStar<TPos, TCost>(
         TPos start,
         TPos finish,
         Func<TPos, IEnumerable<TPos>> Neighbors,
         Func<TPos, TPos, TCost> CalcCosts,
         Func<TPos, TCost> EstimateToFinish,
         TCost maxCost)
        where TPos : notnull
        where TCost : notnull, IComparisonOperators<TCost, TCost, bool>, IAdditionOperators<TCost, TCost, TCost>
    {
        return AStar<TPos, TCost, BinaryHeap<(TPos, TCost)>>(
            start,
            finish,
            Neighbors,
            CalcCosts,
            EstimateToFinish,
            maxCost,
            () => new BinaryHeap<(TPos, TCost)>(1000, new CostComparer<TPos, TCost>()));
    }

    /// <summary>
    /// A* algorithm to search the best path from <paramref name="start"/> to <paramref name="finish"/> condition by optimizing the search on <typeparamref name="TCost"/>.
    /// </summary>
    /// <param name="start">The start node for searching the best path.</param>
    /// <param name="finish">The node to reach.</param>
    /// <param name="Neighbors">This function has to return all neighbors of a single position in the graph.</param>
    /// <param name="CalcCosts">This function has to return the <i>actual</i> costs from one node to a neighbor.</param>
    /// <param name="EstimateToFinish">This function has to return the <i>estimated</i> costs from one node to another. The estimated cost must be equal or less than the actual costs!</param>
    /// <param name="maxCost">The maximum value of <typeparamref name="TCost"/> i.e. int.MaxValue.</param>
    /// <param name="CreateQueue">creates the priority queue for storing the open set in the A* algorithm</param>
    /// <typeparam name="TPos">Type of nodes in the graph.</typeparam>
    /// <typeparam name="TCost">The type of the distance values.</typeparam>
    /// <typeparam name="TPriorityQueue">Type of the priority queue for storing the open set of the A* algorithm</typeparam>
    /// <returns>The nodes in the path from <paramref name="start"/> until <paramref name="finish"/> is reached</returns>
    public static IEnumerable<TPos> AStar<TPos, TCost, TPriorityQueue>(
        TPos start,
        TPos finish,
        Func<TPos, IEnumerable<TPos>> Neighbors,
        Func<TPos, TPos, TCost> CalcCosts,
        Func<TPos, TCost> EstimateToFinish,
        TCost maxCost,
        Func<TPriorityQueue> CreateQueue)
        where TPos : notnull
        where TCost : notnull, IComparisonOperators<TCost, TCost, bool>, IAdditionOperators<TCost, TCost, TCost>
        where TPriorityQueue : IPriorityQueue<(TPos, TCost), Handle>
    {
        return AStar<TPos, TCost, TPriorityQueue>(
            start,
            node => object.Equals(node, finish),
            Neighbors,
            CalcCosts,
            EstimateToFinish,
            maxCost,
            CreateQueue);
    }

    /// <summary>
    /// A* algorithm to search the best path from <paramref name="start"/> to <paramref name="GoalReached"/> condition by optimizing the search on <typeparamref name="TCost"/>.
    /// </summary>
    /// <param name="start">The start node for searching the best path.</param>
    /// <param name="GoalReached">The function to determine if the goal is reached at a specific position.</param>
    /// <param name="Neighbors">This function has to return all neighbors of a single position in the graph.</param>
    /// <param name="CalcCosts">This function has to return the <i>actual</i> costs from one node to a neighbor.</param>
    /// <param name="EstimateToFinish">This function has to return the <i>estimated</i> costs from one node to another. The estimated cost must be equal or less than the actual costs!</param>
    /// <param name="maxCost">The maximum value of <typeparamref name="TCost"/> i.e. int.MaxValue.</param>
    /// <typeparam name="TPos">Type of nodes in the graph.</typeparam>
    /// <typeparam name="TCost">The type of the distance values.</typeparam>
    /// <returns>The nodes in the path from <paramref name="start"/> until the goal is reached</returns>
    public static IEnumerable<TPos> AStar<TPos, TCost>(
        TPos start,
        Func<TPos, bool> GoalReached,
        Func<TPos, IEnumerable<TPos>> Neighbors,
        Func<TPos, TPos, TCost> CalcCosts,
        Func<TPos, TCost> EstimateToFinish,
        TCost maxCost)
        where TPos : notnull
        where TCost : notnull, IComparisonOperators<TCost, TCost, bool>, IAdditionOperators<TCost, TCost, TCost>
    {
        return AStar<TPos, TCost, BinaryHeap<(TPos, TCost)>>(
            start,
            GoalReached,
            Neighbors,
            CalcCosts,
            EstimateToFinish,
            maxCost,
            () => new BinaryHeap<(TPos, TCost)>(1000, new CostComparer<TPos, TCost>()));
    }

    /// <summary>
    /// A* algorithm to search the best path from node <paramref name="start"/> to <paramref name="GoalReached"/> condition by
    /// optimizing the search on cost heuristic of <typeparamref name="TCost"/>.
    /// </summary>
    /// <param name="start">The start node for searching the best path.</param>
    /// <param name="GoalReached">The function to determine if the goal is reached at a specific position.</param>
    /// <param name="Neighbors">This function has to return all neighbors of a single position in the graph.</param>
    /// <param name="CalcCosts">This function has to return the <i>actual</i> costs from one node to a neighbor.</param>
    /// <param name="EstimateToFinish">This function has to return the <i>estimated</i> costs from one node to another. The estimated cost must be equal or less than the actual costs!</param>
    /// <param name="maxCost">The maximum value of <typeparamref name="TCost"/> i.e. int.MaxValue.</param>
    /// <param name="CreateQueue">creates the priority queue for storing the open set in the A* algorithm</param>
    /// <typeparam name="TPos">Type of nodes in the graph.</typeparam>
    /// <typeparam name="TCost">The type of the distance values.</typeparam>
    /// <typeparam name="TPriorityQueue">Type of the priority queue for storing the open set of the A* algorithm</typeparam>
    /// <returns>The nodes in the path from <paramref name="start"/> until the goal is reached</returns>
    public static IEnumerable<TPos> AStar<TPos, TCost, TPriorityQueue>(
        TPos start,
        Func<TPos, bool> GoalReached,
        Func<TPos, IEnumerable<TPos>> Neighbors,
        Func<TPos, TPos, TCost> CalcCosts,
        Func<TPos, TCost> EstimateToFinish,
        TCost maxCost,
        Func<TPriorityQueue> CreateQueue)
        where TPos  : notnull
        where TCost : notnull, IComparisonOperators<TCost, TCost, bool>, IAdditionOperators<TCost, TCost, TCost>
        where TPriorityQueue : IPriorityQueue<(TPos, TCost), Handle>
    {
        ArgumentNullException.ThrowIfNull(GoalReached);
        ArgumentNullException.ThrowIfNull(Neighbors);
        ArgumentNullException.ThrowIfNull(CalcCosts);
        ArgumentNullException.ThrowIfNull(EstimateToFinish);
        ArgumentNullException.ThrowIfNull(CreateQueue);

        // init maps for path and costs with the start node

        var openSet = CreateQueue();
        var startHandle = openSet.Insert((start, EstimateToFinish(start)));

        var openSetHandles = new Dictionary<TPos, Handle> {
            { start, startHandle }
        };

        var cameFrom = new Dictionary<TPos, TPos>();

        var minPathCosts = new Dictionary<TPos, TCost>
        {
            [start] = default!
        };

        // repeatedly take the nearest node from openSet and update the costs in minPathCosts und finishedPathCosts for all direct
        // neighbors of this node

        while(openSet.Count > 0)
        {
            (var current, _) = openSet.ExtractMin();
            openSetHandles.Remove(current);

            if(GoalReached(current))
            {
                return BuildPath(current, cameFrom);
            }
            else
            {
                foreach(var neighbor in Neighbors(current))
                {
                    var costToNeighbor = minPathCosts[current] + CalcCosts(current, neighbor);

                    if(costToNeighbor < minPathCosts.GetValueOrDefault(neighbor, maxCost))
                    {
                        // new best path from current to neighbor => update costs, openSet and the path taken (cameFrom)

                        cameFrom[neighbor]     = current;
                        minPathCosts[neighbor] = costToNeighbor;

                        var estimanedCostToNeighbor = costToNeighbor + EstimateToFinish(neighbor);

                        if(!openSetHandles.TryGetValue(neighbor, out var neighborPos))
                        {
                            openSetHandles[neighbor] = openSet.Insert((neighbor, estimanedCostToNeighbor));
                        }
                        else
                        {
                            openSet.DecreaseElement(neighborPos, (neighbor, estimanedCostToNeighbor));
                        }
                    }
                }
            }
        }

        // no path found
        return Array.Empty<TPos>();
    }

    /// <summary>
    /// Builds a path of visited nodes on the optimal path from goal (initial value of end) back to start by using
    /// the parent-child relationship for the path stored in cameFrom.
    /// </summary>
    /// <return>Path from start to end.</return>
    private static List<TPos> BuildPath<TPos>(TPos end, Dictionary<TPos, TPos> cameFrom) where TPos : notnull
    {
        var bestPath = new List<TPos>() { end };
        while(cameFrom.TryGetValue(end, out var parent))
        {
            // result should be path from start to end, so new nodes have to be inserted at the
            // start of the list to reverse it

            // TODO: check if this is really O(1)
            bestPath.Insert(0, parent);
            end = parent;
        }

        return bestPath;
    }

    /// <summary>
    /// Helper class for A* algorithm to store elements and their costs and order them only by cost values.
    /// </summary>
    private sealed class CostComparer<TElem, TCost> : IComparer<(TElem Elem, TCost Cost)>
        where TCost : notnull, IComparisonOperators<TCost, TCost, bool>
    {
        public int Compare((TElem Elem, TCost Cost) left, (TElem Elem, TCost Cost) right)
        {
            if(left.Cost == right.Cost)
            {
                return 0;
            }
            else
            {
                return left.Cost < right.Cost ? -1 : 1;
            }
        }
    }
}
