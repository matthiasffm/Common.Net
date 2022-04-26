using System.Collections;

namespace matthiasffm.Common.Collections;

public class BinaryTreeNode<T>
{
    public BinaryTreeNode<T>? Parent { get; internal set; }
    public BinaryTreeNode<T>? Left { get; internal set; }
    public BinaryTreeNode<T>? Right { get; internal set; }

    public T Item { get; internal set; }

    public bool IsLeaf { get => Left == null && Right == null; }

    public BinaryTreeNode(T item)
    {
        Item = item;
    }
}

public class BinaryTree<T> : IEnumerable<T>, ICloneable
{
    public BinaryTreeNode<T>? Root { get; protected set; }

    public BinaryTree()
    {
    }

    public BinaryTree(T root)
    {
        Root = new BinaryTreeNode<T>(root);
    }

    public BinaryTreeNode<T> AddRoot(T item)
    {
        Root = new BinaryTreeNode<T>(item);
        return Root;
    }

    public BinaryTreeNode<T> AddLeft(BinaryTreeNode<T> parent, T item)
    {
        ArgumentNullException.ThrowIfNull(parent);

        var newNode = new BinaryTreeNode<T>(item);

        parent.Left = newNode;
        newNode.Parent = parent;

        return newNode;
    }

    public BinaryTreeNode<T> AddRight(BinaryTreeNode<T> parent, T item)
    {
        ArgumentNullException.ThrowIfNull(parent);

        var newNode = new BinaryTreeNode<T>(item);

        parent.Right = newNode;
        newNode.Parent = parent;

        return newNode;
    }

    public void ReplaceLeftChild(BinaryTreeNode<T> parent, BinaryTreeNode<T> newLeftChild)
    {
        ArgumentNullException.ThrowIfNull(parent);
        ArgumentNullException.ThrowIfNull(newLeftChild);

        var clonedSubtree = parent.Left = CloneNode(newLeftChild);
        clonedSubtree.Parent = parent;
    }

    public void ReplaceRightChild(BinaryTreeNode<T> parent, BinaryTreeNode<T> newRightChild)
    {
        ArgumentNullException.ThrowIfNull(parent);
        ArgumentNullException.ThrowIfNull(newRightChild);

        var clonedSubtree = parent.Right = CloneNode(newRightChild);
        clonedSubtree.Parent = parent;
    }

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

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    #region ICloneable

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


// Test

// WriteLine("Right(3)    = " + tree.NextLeafToRight(node3)?.Item);
// WriteLine("Right(2)    = " + tree.NextLeafToRight(node2)?.Item);
// WriteLine("Right(1)    = " + tree.NextLeafToRight(node1)?.Item);
// WriteLine("Right(8)    = " + tree.NextLeafToRight(node8)?.Item);
// WriteLine("Right(9)    = " + tree.NextLeafToRight(node9)?.Item);
// WriteLine("Right(root) = " + tree.NextLeafToRight(root)?.Item);

