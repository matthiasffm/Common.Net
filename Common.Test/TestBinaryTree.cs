using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Collections;

namespace matthiasffm.Common.Test;

internal class TestBinaryTree
{
    [Test]
    public void TestCount()
    {
        // arrange
        var testTree = CreateTreeForIteratorTests();

        // act
        var count = testTree.Count();

        // assert
        count.Should().Be(7);
    }

    [Test]
    public void TestEmptyCount()
    {
        // arrange
        var testTree = new BinaryTree<int>();

        // act
        var count = testTree.Count();

        // assert
        count.Should().Be(0);
    }

    [Test]
    public void TestContains()
    {
        // arrange
        var testTree = CreateTreeForIteratorTests();

        // act
        var contains1 = testTree.Contains(1);
        var contains3 = testTree.Contains(3);
        var contains8 = testTree.Contains(8);

        // assert
        contains1.Should().BeTrue();
        contains3.Should().BeTrue();
        contains8.Should().BeFalse();
    }

    [Test]
    public void TestContainsOnEmptyTree()
    {
        // arrange
        var testTree = new BinaryTree<int>();

        // act
        var contains1 = testTree.Contains(1);
        var contains3 = testTree.Contains(3);

        // assert
        contains1.Should().BeFalse();
        contains3.Should().BeFalse();
    }

    [Test]
    public void TestDepth()
    {
        // arrange
        var testTree = CreateTreeForIteratorTests();

        // act
        var depthRoot = testTree.Depth(testTree.Root);
        var depth2    = testTree.Depth(testTree.Root.Left);
        var depth5    = testTree.Depth(testTree.Root.Right.Left);

        // assert
        depthRoot.Should().Be(1);
        depth2.Should().Be(2);
        depth5.Should().Be(3);
    }

    [Test]
    public void TestDepthOnEmptyTree()
    {
        // arrange
        var testTree = new BinaryTree<int>();

        // act
        var rootDepth = testTree.Depth(testTree.Root);

        // assert
        rootDepth.Should().Be(0);
    }

    [Test]
    public void TestPreOrder()
    {
        // arrange
        var testTree = CreateTreeForIteratorTests();

        // act
        var preOrder = testTree.IteratePreOrder(testTree.Root)
                               .Select(n => n.Item);

        // assert
        preOrder.Should().BeEquivalentTo(new[] { 1, 2, 4, 3, 5, 7, 6 });
    }

    [Test]
    public void TestInOrder()
    {
        // arrange
        var testTree = CreateTreeForIteratorTests();

        // act
        var inOrder = testTree.IterateInOrder(testTree.Root)
                              .Select(n => n.Item);

        // assert
        inOrder.Should().BeEquivalentTo(new[] { 4, 2, 1, 5, 7, 3, 6 });
    }

    [Test]
    public void TestPostOrder()
    {
        // arrange
        var testTree = CreateTreeForIteratorTests();

        // act
        var postOrder = testTree.IteratePostOrder(testTree.Root)
                                .Select(n => n.Item);

        // assert
        postOrder.Should().BeEquivalentTo(new[] { 4, 2, 7, 5, 6, 3, 1 });
    }

    [Test]
    public void TestLevelOrder()
    {
        // arrange
        var testTree = CreateTreeForIteratorTests();

        // act
        var levelOrder = testTree.IterateLevelOrder(testTree.Root)
                                 .Select(n => n.Item);

        // assert
        levelOrder.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5, 6, 7 });
    }

    [Test]
    public void TestNextLeafToLeft()
    {
        // arrange
        var testTree = CreateTreeForIteratorTests();

        // act
        var leftOf2    = testTree.NextLeafToLeft(testTree.Root.Left);
        var leftOf3    = testTree.NextLeafToLeft(testTree.Root.Right);
        var leftOf4    = testTree.NextLeafToLeft(testTree.Root.Left.Left);
        var leftOf5    = testTree.NextLeafToLeft(testTree.Root.Right.Left);
        var leftOf6    = testTree.NextLeafToLeft(testTree.Root.Right.Right);
        var leftOf7    = testTree.NextLeafToLeft(testTree.Root.Right.Left.Right);
        var leftOfRoot = testTree.NextLeafToLeft(testTree.Root);

        // assert
        leftOf2.Item.Should().Be(4);
        leftOf3.Item.Should().Be(7);
        leftOf4.Should().BeNull();
        leftOf5.Item.Should().Be(4);
        leftOf6.Item.Should().Be(7);
        leftOf7.Item.Should().Be(4);
        leftOfRoot.Item.Should().Be(4);
    }

    [Test]
    public void TestNextLeafToRight()
    {
        // arrange
        var testTree = CreateTreeForIteratorTests();

        // act
        var rightOf2 = testTree.NextLeafToRight(testTree.Root.Left);
        var rightOf3 = testTree.NextLeafToRight(testTree.Root.Right);
        var rightOf4 = testTree.NextLeafToRight(testTree.Root.Left.Left);
        var rightOf5 = testTree.NextLeafToRight(testTree.Root.Right.Left);
        var rightOf6 = testTree.NextLeafToRight(testTree.Root.Right.Right);
        var rightOf7 = testTree.NextLeafToRight(testTree.Root.Right.Left.Right);
        var rightOfRoot = testTree.NextLeafToRight(testTree.Root);

        // assert
        rightOf2.Item.Should().Be(7);
        rightOf3.Item.Should().Be(6);
        rightOf4.Item.Should().Be(7);
        rightOf5.Item.Should().Be(7);
        rightOf6.Should().BeNull();
        rightOf7.Item.Should().Be(6);
        rightOfRoot.Item.Should().Be(7);
    }

    [Test]
    public void TestDeleteNodes()
    {
        // arrange
        var testTree = CreateTreeForIteratorTests();

        // act
        testTree.DeleteLeftChild(testTree.Root.Left);
        testTree.DeleteLeftChild(testTree.Root.Right.Right);
        testTree.DeleteRightChild(testTree.Root.Right.Left);

        // assert
        testTree.Root.Left.Left.Should().BeNull();
        testTree.Root.Right.Right.Left.Should().BeNull();
        testTree.Root.Right.Left.Right.Should().BeNull();
    }

    // create tree for iterator tests
    // 
    //      1
    //     / \
    //    2   3
    //   /   / \
    //  4   5   6
    //       \
    //        7
    //
    private static BinaryTree<int> CreateTreeForIteratorTests()
    {
        var tree = new BinaryTree<int>();

        var root = tree.AddRoot(1);
        var node2 = tree.AddLeft(root, 2);
        tree.AddLeft(node2, 4);
        var node3 = tree.AddRight(root, 3);
        var node5 = tree.AddLeft(node3, 5);
        tree.AddRight(node3, 6);
        tree.AddRight(node5, 7);

        return tree;
    }
}
