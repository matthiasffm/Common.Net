using System.Collections;

namespace matthiasffm.Common.Collections;

/// <summary>
/// Knoten eines binären Baums.
/// </summary>
/// <typeparam name="T">Typ des im Knoten gespeicherten Wertes.</typeparam>
public class BinaryTreeNode<T>
{
    /// <summary>
    /// Referenz auf den Elternknoten.
    /// </summary>
    /// <remarks>Ist im Fall des Wurzelknotens <i>null</i>.</remarks>
    public BinaryTreeNode<T>? Parent { get; internal set; }

    /// <summary>
    /// Referenz auf den linken Kindknoten.
    /// </summary>
    /// <remarks>Ist im Fall eines Blattknotens <i>null</i>.</remarks>
    public BinaryTreeNode<T>? Left { get; internal set; }

    /// <summary>
    /// Referenz auf den rechten Kindknoten.
    /// </summary>
    /// <remarks>Ist im Fall eines Blattknotens <i>null</i>.</remarks>
    public BinaryTreeNode<T>? Right { get; internal set; }

    /// <summary>
    /// Im Knoten gespeicherter Wert.
    /// </summary>
    public T Item { get; set; }

    /// <summary>
    /// Ist der Knoten ein Blattknoten ohne Kinder?
    /// </summary>
    public bool IsLeaf { get => Left == null && Right == null; }


    /// <summary>
    /// Erstellt einen neuen Knoten mit item als Wert.
    /// </summary>
    public BinaryTreeNode(T item)
    {
        Item = item;
    }
}

/// <summary>
/// Datenstruktur binärer Baum.
/// </summary>
/// <typeparam name="T">Typ des in den Knoten gespeicherten Wertes.</typeparam>
public class BinaryTree<T> : IEnumerable<T>, ICloneable
{
    /// <summary>
    /// Wurzelknoten des Baumes.
    /// </summary>
    public BinaryTreeNode<T>? Root { get; protected set; }

    /// <summary>
    /// Erstellt einen leeren Binärbaum ohne Knoten.
    /// </summary>
    public BinaryTree()
    {
    }

    /// <summary>
    /// Erstellt einen Binärbaum mit einem Wurzelknoten vom Wert <i>root</i>.
    /// </summary>
    public BinaryTree(T root)
    {
        Root = new BinaryTreeNode<T>(root);
    }

    /// <summary>
    /// Erstellt einen neuen Wurzelknoten mit dem Wert <i>item</i>.
    /// </summary>
    /// <param name="item">der Wert des neuen Wurzelknotens</param>
    /// <returns>der neue Wurzelknoten</returns>
    /// <remarks>Alle vorherigen Informationen im Binärbaum gehen dabei verloren.</remarks>
    public BinaryTreeNode<T> AddRoot(T item)
    {
        Root = new BinaryTreeNode<T>(item);
        return Root;
    }

    /// <summary>
    /// Fügt unterhalt eines Elternknotens einen linken Kindknoten mit dem Wert <i>item</i> ein.
    /// </summary>
    /// <param name="parent">Referenz auf den Elternknoten</param>
    /// <param name="item">der Wert des neuen linken Kindknotens</param>
    /// <returns>der neue linke Kindknoten</returns>
    public BinaryTreeNode<T> AddLeft(BinaryTreeNode<T> parent, T item)
    {
        ArgumentNullException.ThrowIfNull(parent);

        var newNode = new BinaryTreeNode<T>(item);

        parent.Left = newNode;
        newNode.Parent = parent;

        return newNode;
    }

    /// <summary>
    /// Fügt unterhalt eines Elternknotens einen rechten Kindknoten mit dem Wert <i>item</i> ein.
    /// </summary>
    /// <param name="parent">Referenz auf den Elternknoten</param>
    /// <param name="item">der Wert des neuen rechten Kindknotens</param>
    /// <returns>der neue linke Kindknoten</returns>
    public BinaryTreeNode<T> AddRight(BinaryTreeNode<T> parent, T item)
    {
        ArgumentNullException.ThrowIfNull(parent);

        var newNode = new BinaryTreeNode<T>(item);

        parent.Right = newNode;
        newNode.Parent = parent;

        return newNode;
    }

    /// <summary>
    /// Ersetzt unterhalb eines Elternknotens einen linken Kindknoten mit dem Klon des anderen Teilbaums <i>leftSubtree</i>.
    /// </summary>
    /// <param name="parent">Referenz auf den Elternknoten</param>
    /// <param name="leftSubtree">der Teilbaum wird geklont links unterhalb des Elternknotens eingefügt</param>
    public void ReplaceLeftChild(BinaryTreeNode<T> parent, BinaryTreeNode<T> leftSubtree)
    {
        ArgumentNullException.ThrowIfNull(parent);
        ArgumentNullException.ThrowIfNull(leftSubtree);

        var clonedSubtree = parent.Left = CloneNode(leftSubtree);
        clonedSubtree.Parent = parent;
    }

    /// <summary>
    /// Ersetzt unterhalb eines Elternknotens einen rechten Kindknoten mit dem Klon des anderen Teilbaums <i>rightSubtree</i>.
    /// </summary>
    /// <param name="parent">Referenz auf den Elternknoten</param>
    /// <param name="rightSubtree">der Teilbaum wird geklont rechts unterhalb des Elternknotens eingefügt</param>
    public void ReplaceRightChild(BinaryTreeNode<T> parent, BinaryTreeNode<T> rightSubtree)
    {
        ArgumentNullException.ThrowIfNull(parent);
        ArgumentNullException.ThrowIfNull(rightSubtree);

        var clonedSubtree = parent.Right = CloneNode(rightSubtree);
        clonedSubtree.Parent = parent;
    }

    /// <summary>
    /// Löscht den linken Kind-Teilbaum unterhalb des Elternknotens.
    /// </summary>
    /// <param name="parent">Referenz auf den Elternknoten</param>
    public void DeleteLeftChild(BinaryTreeNode<T> parent)
    {
        ArgumentNullException.ThrowIfNull(parent);

        if(parent.Left != null)
        {
            parent.Left.Parent = null;
        }

        parent.Left = null;
    }

    /// <summary>
    /// Löscht den rechten Kind-Teilbaum unterhalb des Elternknotens.
    /// </summary>
    /// <param name="parent">Referenz auf den Elternknoten</param>
    public void DeleteRightChild(BinaryTreeNode<T> parent)
    {
        ArgumentNullException.ThrowIfNull(parent);

        if(parent.Right != null)
        {
            parent.Right.Parent = null;
        }

        parent.Right = null;
    }

    /// <summary>
    /// Ermittelt die Tiefe einen Knotens innerhalb des Binärbaums.
    /// </summary>
    /// <param name="node">der Knoten, dessen Tiefe ermittelt werden soll.</param>
    /// <returns>Die Tiefe von <i>node</i>, dabei hat der Wurzelknoten die Tiefe 1</returns>
    /// <remarks>O(logn)</remarks>
    public int Depth(BinaryTreeNode<T>? node)
    {
        int depth = 0;

        while(node is not null)
        {
            depth++;
            node = node.Parent;
        }

        return depth;
    }

    /// <summary>
    /// Ermittelt den nächsten Blattknoten links von <i>node</i>.
    /// </summary>
    /// <param name="node">von diesem Knoten aus wird links der nächste Blattknoten gesucht</param>
    /// <returns>den nächsten Blattknoten links oder <i>null</i>, wenn keiner gefunden wird</returns>
    /// <remarks>O(logn)</remarks>
    public BinaryTreeNode<T>? NextLeafToLeft(BinaryTreeNode<T>? node)
    {
        if(node is null)
        {
            return null;
        }

        var prev = node;
        while(node.Left is null || node.Left == prev)
        {
            if(node.Parent is null)
            {
                return null;
            }
            prev = node;
            node = node.Parent;
        }

        do
        {
            node = node!.Left;

            while(!node!.IsLeaf && node.Right is not null)
            {
                node = node.Right;
            }
        }
        while(!node.IsLeaf);

        return node;
    }

    /// <summary>
    /// Ermittelt den nächsten Blattknoten rechts von <i>node</i>.
    /// </summary>
    /// <param name="node">von diesem Knoten aus wird rechts der nächste Blattknoten gesucht</param>
    /// <returns>den nächsten Blattknoten rechts oder <i>null</i>, wenn keiner gefunden wird</returns>
    /// <remarks>O(logn)</remarks>
    public BinaryTreeNode<T>? NextLeafToRight(BinaryTreeNode<T>? node)
    {
        if(node is null)
        {
            return null;
        }

        var prev = node;
        while(node.Right is null || node.Right == prev)
        {
            if(node.Parent is null)
            {
                return null;
            }
            prev = node;
            node = node.Parent;
        }

        do
        {
            node = node!.Right;

            while(!node!.IsLeaf && node.Left is not null)
            {
                node = node.Left;
            }
        }
        while (!node.IsLeaf);

        return node;
    }

    #region Iterators

    /// <summary>
    /// Iteriert über die Binärbaum-Knoten in Preorder-Reihenfolge ausgehend von <i>node</i>.
    /// </summary>
    /// <param name="node">Ausgangspunkt der Iteration</param>
    /// <returns>Preorder-Iterator</returns>
    public IEnumerable<BinaryTreeNode<T>> IteratePreOrder(BinaryTreeNode<T>? node)
    {
        if(node is not null)
        {
            var stack = new Stack<BinaryTreeNode<T>>();
            stack.Push(node);

            while(stack.TryPop(out var nextNode))
            {
                yield return nextNode;

                if(nextNode.Right is not null)
                {
                    stack.Push(nextNode.Right);
                }

                if(nextNode.Left is not null)
                {
                    stack.Push(nextNode.Left);
                }
            }
        }
    }

    /// <summary>
    /// Iteriert über die Binärbaum-Knoten in Inorder-Reihenfolge ausgehend von <i>node</i>.
    /// </summary>
    /// <param name="node">Ausgangspunkt der Iteration</param>
    /// <returns>Inorder-Iterator</returns>
    public IEnumerable<BinaryTreeNode<T>> IterateInOrder(BinaryTreeNode<T>? node)
    {
        var stack = new Stack<BinaryTreeNode<T>>();

        while(stack.Any() || node is not null)
        {
            if(node is not null)
            {
                stack.Push(node);
                node = node.Left;
            }
            else
            {
                node = stack.Pop();
                yield return node;
                node = node.Right;
            }
        }
    }

    /// <summary>
    /// Iteriert über die Binärbaum-Knoten in Postorder-Reihenfolge ausgehend von <i>node</i>.
    /// </summary>
    /// <param name="node">Ausgangspunkt der Iteration</param>
    /// <returns>Postorder-Iterator</returns>
    public IEnumerable<BinaryTreeNode<T>> IteratePostOrder(BinaryTreeNode<T>? node)
    {
        var stack = new Stack<BinaryTreeNode<T>>();
        BinaryTreeNode<T>? lastVisited = null;

        while(stack.Any() || node is not null)
        {
            if(node is not null)
            {
                stack.Push(node);
                node = node.Left;
            }
            else
            {
                var peekNode = stack.Peek();

                if(peekNode.Right is not null && lastVisited != peekNode.Right)
                {
                    node = peekNode.Right;
                }
                else
                {
                    yield return peekNode;
                    lastVisited = stack.Pop();
                }
            }
        }
    }

    /// <summary>
    /// Iteriert über die Binärbaum-Knoten in Levelorder-Reihenfolge ausgehend von <i>node</i>.
    /// </summary>
    /// <param name="node">Ausgangspunkt der Iteration</param>
    /// <returns>Levelorder-Iterator</returns>
    public IEnumerable<BinaryTreeNode<T>> IterateLevelOrder(BinaryTreeNode<T> node)
    {
        var nodeQueue = new Queue<BinaryTreeNode<T>>();
        nodeQueue.Enqueue(node);

        while(nodeQueue.TryDequeue(out var nextNode))
        {
            yield return nextNode;

            if(nextNode.Left is not null)
            {
                nodeQueue.Enqueue(nextNode.Left);
            }

            if(nextNode.Right is not null)
            {
                nodeQueue.Enqueue(nextNode.Right);
            }
        }
    }

    #endregion

    #region IEnumerable<T>

    /// <summary>
    /// Liefert den typisierten Preorder-Iterator für den gesamten Binärbaum.
    /// </summary>
    /// <returns>Preorder-Iterator</returns>
    public IEnumerator<T> GetEnumerator()
    {
        if(Root is not null)
        {
            foreach(var node in IteratePreOrder(Root))
            {
                yield return node.Item;
            }
        }
    }

    /// <summary>
    /// Liefert den Preorder-Iterator für den gesamten Binärbaum.
    /// </summary>
    /// <returns>Preorder-Iterator</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    #region ICloneable

    /// <summary>
    /// Klont die Struktur des kompletten Binärbaums (flach, kein Klon der Werte im Baum).
    /// </summary>
    /// <returns>geklonter Binärbaum</returns>
    /// <remarks>Dabei wird die Datenstruktur des Baums geklont, die Werte in den Knoten aber nicht.</remarks>
    public object Clone()
    {
        var tree = new BinaryTree<T>();
        if(Root is not null)
        {
            tree.Root = CloneNode(Root);
        }
        return tree;
    }

    private BinaryTreeNode<T> CloneNode(BinaryTreeNode<T> toClone)
    {
        var clonedNode = new BinaryTreeNode<T>(toClone.Item);

        if(toClone.Left is not null)
        {
            clonedNode.Left = CloneNode(toClone.Left);
            clonedNode.Left.Parent = clonedNode;
        }

        if(toClone.Right is not null)
        {
            clonedNode.Right = CloneNode(toClone.Right);
            clonedNode.Right.Parent = clonedNode;
        }

        return clonedNode;
    }

    #endregion
}
