using System.Collections;

namespace matthiasffm.Common.Collections;

/// <summary>
/// Single node of a binary tree.
/// </summary>
/// <typeparam name="T">Type of the value at the node. <see cref="Item"/></typeparam>
public class BinaryTreeNode<T>
{
    /// <summary>
    /// Reference to parent node.
    /// </summary>
    /// <remarks>Is <i>null</i> if the node is the root node of the tree.</remarks>
    public BinaryTreeNode<T>? Parent { get; internal set; }

    /// <summary>
    /// Reference to left child node.
    /// </summary>
    /// <remarks>Is <i>null</i> if the node has no left child.</remarks>
    public BinaryTreeNode<T>? Left { get; internal set; }

    /// <summary>
    /// Reference to right child node.
    /// </summary>
    /// <remarks>Is <i>null</i> if the node has no right child.</remarks>
    public BinaryTreeNode<T>? Right { get; internal set; }

    /// <summary>
    /// Value stored at the node.
    /// </summary>
    public T Item { get; set; }

    /// <summary>
    /// Is <i>true</i> if the node has no left child and no right child nodes.
    /// </summary>
    public bool IsLeaf { get => Left is null && Right is null; }

    /// <summary>
    /// Creates a new node and stores the value <paramref name="item"/> there.
    /// </summary>
    public BinaryTreeNode(T item)
    {
        Item = item;
    }
}

/// <summary>
/// Data structure for a binary tree where every node in the tree has 0 to 1 parent node and 0 to 2 child nodes.
/// </summary>
/// <typeparam name="T">Type of the value at the node. <see cref="BinaryTreeNode{T}.Item"/></typeparam>
public class BinaryTree<T> : IEnumerable<T>, ICloneable
{
    /// <summary>
    /// Root node of the tree.
    /// </summary>
    /// <remarks>Is <i>null</i> if the tree is empty.</remarks>
    public BinaryTreeNode<T>? Root { get; protected set; }

    /// <summary>
    /// Creates a new empty binary tree.
    /// </summary>
    public BinaryTree()
    {
    }

    /// <summary>
    /// Creates a new binary tree with one new node as its root and <paramref name="root"/> as its value.
    /// </summary>
    /// <param name="root">The value for the root node.</param>
    public BinaryTree(T root)
    {
        Root = new BinaryTreeNode<T>(root);
    }

    /// <summary>
    /// Creates a new root node for the binary tree.
    /// </summary>
    /// <param name="item">The value of the new root node.</param>
    /// <returns>New root node data structure</returns>
    /// <remarks>All previous informations of the binary tree are lost after this operation!</remarks>
    public BinaryTreeNode<T> AddRoot(T item)
    {
        Root = new BinaryTreeNode<T>(item);
        return Root;
    }

    /// <summary>
    /// Creates a new child node and adds it as a left child to a parent node.
    /// </summary>
    /// <param name="parent">Reference to parent node.</param>
    /// <param name="item">The value of the new left child node.</param>
    /// <returns>New left child node data structure</returns>
    /// <remarks>All previous informations of a left child node for <paramref name="parent"/> are lost after this operation!</remarks>
    public BinaryTreeNode<T> AddLeft(BinaryTreeNode<T> parent, T item)
    {
        ArgumentNullException.ThrowIfNull(parent);

        if(parent.Left != null)
        {
            parent.Left.Parent = null;
        }

        var newNode = new BinaryTreeNode<T>(item);

        parent.Left = newNode;
        newNode.Parent = parent;

        return newNode;
    }

    /// <summary>
    /// Creates a new child node and adds it as a right child to a parent node.
    /// </summary>
    /// <param name="parent">Reference to parent node.</param>
    /// <param name="item">The value of the new right child node.</param>
    /// <returns>New right child node data structure</returns>
    /// <remarks>All previous informations of a right child node for <paramref name="parent"/> are lost after this operation!</remarks>
    public BinaryTreeNode<T> AddRight(BinaryTreeNode<T> parent, T item)
    {
        ArgumentNullException.ThrowIfNull(parent);

        if(parent.Right != null)
        {
            parent.Right.Parent = null;
        }

        var newNode = new BinaryTreeNode<T>(item);

        parent.Right = newNode;
        newNode.Parent = parent;

        return newNode;
    }

    /// <summary>
    /// Clones a complete subtree beginning from its root and adds it as a left child to a parent node.
    /// </summary>
    /// <param name="parent">Reference to parent node.</param>
    /// <param name="rootOfSubtreeToClone">the root of a subtree to clone and add</param>
    /// <returns>New left child node data structure == the root of the cloned subtree.</returns>
    /// <remarks>All previous informations of a left child node for <paramref name="parent"/> are lost after this operation!</remarks>
    public BinaryTreeNode<T> AddLeftSubtree(BinaryTreeNode<T> parent, BinaryTreeNode<T> rootOfSubtreeToClone)
    {
        ArgumentNullException.ThrowIfNull(parent);
        ArgumentNullException.ThrowIfNull(rootOfSubtreeToClone);

        if(parent.Left != null)
        {
            parent.Left.Parent = null;
        }

        var clonedSubtree = parent.Left = CloneNode(rootOfSubtreeToClone);
        clonedSubtree.Parent = parent;

        return clonedSubtree;
    }

    /// <summary>
    /// Clones a complete subtree beginning from its root and adds it as a right child to a parent node.
    /// </summary>
    /// <param name="parent">Reference to parent node.</param>
    /// <param name="rootOfSubtreeToClone">the root of a subtree to clone and add</param>
    /// <returns>New right child node data structure == the root of the cloned subtree.</returns>
    /// <remarks>All previous informations of a right child node for <paramref name="parent"/> are lost after this operation!</remarks>
    public BinaryTreeNode<T> AddRightSubtree(BinaryTreeNode<T> parent, BinaryTreeNode<T> rootOfSubtreeToClone)
    {
        ArgumentNullException.ThrowIfNull(parent);
        ArgumentNullException.ThrowIfNull(rootOfSubtreeToClone);

        if(parent.Right != null)
        {
            parent.Right.Parent = null;
        }

        var clonedSubtree = parent.Right = CloneNode(rootOfSubtreeToClone);
        clonedSubtree.Parent = parent;

        return clonedSubtree;
    }

    /// <summary>
    /// Removes the left child tree beneath a parent node.
    /// </summary>
    /// <param name="parent">the parent node</param>
    /// <remarks>All previous informations of a left child node for <paramref name="parent"/> are lost after this operation!</remarks>
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
    /// Removes the right child tree beneath a parent node.
    /// </summary>
    /// <param name="parent">the parent node</param>
    /// <remarks>All previous informations of a right child node for <paramref name="parent"/> are lost after this operation!</remarks>
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
    /// Counts the depth of a node in the tree (root is at depth 1).
    /// </summary>
    /// <remarks>runs in O(log n)</remarks>
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
    /// Searches for the nearest leaf node left (rightmost leaf) to <paramref name="node"/>.
    /// </summary>
    /// <param name="node">start of search</param>
    /// <returns>Returns the rightmost leaf left from <paramref name="node"/> or <i>null</i> if no leaf was found.</returns>
    /// <remarks>runs in O(log n)</remarks>
    public BinaryTreeNode<T>? NextLeafToLeft(BinaryTreeNode<T>? node)
    {
        if(node is null)
        {
            return null;
        }

        // move to a node with a left child

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

        // now move to the rightmost child

        do
        {
            node = node!.Left;

            while(node!.Right is not null)
            {
                node = node.Right;
            }
        }
        while(!node.IsLeaf);

        return node;
    }

    /// <summary>
    /// Searches for the nearest leaf node right (leftmost leaf) to <paramref name="node"/>.
    /// </summary>
    /// <param name="node">start of search</param>
    /// <returns>Returns the leftmost leaf right from <paramref name="node"/> or <i>null</i> if no leaf was found.</returns>
    /// <remarks>runs in O(log n)</remarks>
    public BinaryTreeNode<T>? NextLeafToRight(BinaryTreeNode<T>? node)
    {
        if(node is null)
        {
            return null;
        }

        // move to a node with a right child

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

        // now move to the leftmost child

        do
        {
            node = node!.Right;

            while(node!.Left is not null)
            {
                node = node.Left;
            }
        }
        while (!node.IsLeaf);

        return node;
    }

    #region Iterators

    /// <summary>
    /// Traverses all nodes in a subtree in preorder sequence starting at <i>node</i>.
    /// </summary>
    /// <param name="node">Starting node of the iteration</param>
    /// <returns>Preorder iterator</returns>
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
    /// Traverses all nodes in a subtree in inorder sequence starting at <i>node</i>.
    /// </summary>
    /// <param name="node">Starting node of the iteration</param>
    /// <returns>Inorder iterator</returns>
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
    /// Traverses all nodes in a subtree in postorder sequence starting at <i>node</i>.
    /// </summary>
    /// <param name="node">Starting node of the iteration</param>
    /// <returns>Postorder iterator</returns>
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
    /// Traverses all nodes in a subtree in level order sequence starting at <i>node</i>.
    /// </summary>
    /// <param name="node">Starting node of the iteration</param>
    /// <returns>Level order iterator</returns>
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
    /// Returns typed iterator to traverse all nodes in the tree in preorder sequence.
    /// </summary>
    /// <returns>Preorder iterator</returns>
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
    /// Returns iterator to traverse all nodes in the tree in preorder sequence.
    /// </summary>
    /// <returns>Preorder iterator</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    #region ICloneable

    /// <summary>
    /// Clones the tree data structure (flat clone, values in nodes are not cloned).
    /// </summary>
    /// <returns>Cloned binary tree</returns>
    public object Clone()
    {
        var tree = new BinaryTree<T>();
        if(Root is not null)
        {
            tree.Root = CloneNode(Root);
        }
        return tree;
    }

    private static BinaryTreeNode<T> CloneNode(BinaryTreeNode<T> toClone)
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
