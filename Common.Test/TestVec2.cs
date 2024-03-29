﻿using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Math;

namespace matthiasffm.Common.Test;

internal class TestVec2
{
    [Test]
    public void TestAdd()
    {
        // arrange
        var vec1 = new Vec2<int>(1, 2);
        var vec2 = new Vec2<int>(0, 2);
        var vec3 = new Vec2<int>(-4, -7);
        var vec4 = new Vec2<int>(0, 0);

        // act
        var add1 = vec1 + vec2;
        var add2 = vec3 + vec1;
        var add3 = vec1 + vec4;
        var add4 = vec4 + vec1;

        // assert
        add1.Should().Be(new Vec2<int>(1, 4));
        add2.Should().Be(new Vec2<int>(-3, -5));
        add3.Should().Be(new Vec2<int>(1, 2));
        add4.Should().Be(new Vec2<int>(1, 2));
    }

    [Test]
    public void TestSubtract()
    {
        // arrange
        var vec1 = new Vec2<int>(1, 2);
        var vec2 = new Vec2<int>(0, 2);
        var vec3 = new Vec2<int>(-4, -7);
        var vec4 = new Vec2<int>(0, 0);

        // act
        var sub1 = vec1 - vec2;
        var sub2 = vec1 - vec3;
        var sub3 = vec3 - vec1;
        var sub4 = vec1 - vec4;

        // assert
        sub1.Should().Be(new Vec2<int>(1, 0));
        sub2.Should().Be(new Vec2<int>(5, 9));
        sub3.Should().Be(new Vec2<int>(-5, -9));
        sub4.Should().Be(new Vec2<int>(1, 2));
    }

    [Test]
    public void TestHadamardProduct()
    {
        // arrange
        var vec1 = new Vec2<int>(1, 2);
        var vec2 = new Vec2<int>(0, 2);
        var vec3 = new Vec2<int>(-4, -7);
        var vec4 = new Vec2<int>(0, 0);

        // act
        var prod1 = vec1 * vec2;
        var prod2 = vec3 * vec1;
        var prod3 = vec1 * vec3;
        var prod4 = vec4 * vec1;

        // assert
        prod1.Should().Be(new Vec2<int>(0, 4));
        prod2.Should().Be(new Vec2<int>(-4, -14));
        prod3.Should().Be(new Vec2<int>(-4, -14));
        prod4.Should().Be(new Vec2<int>(0, 0));
    }

    [Test]
    public void TestHadamardDivision()
    {
        // arrange
        var vec1 = new Vec2<int>(4, 9);
        var vec2 = new Vec2<int>(1, 1);
        var vec3 = new Vec2<int>(2, -3);
        var vec4 = new Vec2<int>(0, 0);

        // act
        var div1 = vec1 / vec2;
        var div2 = vec3 / vec1;
        var div3 = vec1 / vec3;
        var div4 = vec4 / vec1;
        var div5 = () => vec1 / vec4;

        // assert
        div1.Should().Be(new Vec2<int>(4, 9));
        div2.Should().Be(new Vec2<int>(1 / 2, -1 / 3));
        div3.Should().Be(new Vec2<int>(2, -3));
        div4.Should().Be(new Vec2<int>(0, 0));
        div5.Should().Throw<DivideByZeroException>();
    }

    [Test]
    public void TestScalarProduct()
    {
        // arrange
        var vec1 = new Vec2<int>(1, 2);
        var vec2 = new Vec2<int>(0, 2);
        var vec3 = new Vec2<int>(-4, -7);

        // act
        var prod1 = vec1 * 5;
        var prod2 = 2 * vec2;
        var prod3 = -1 * vec3;
        var prod4 = vec3 * 0;

        // assert
        prod1.Should().Be(new Vec2<int>(5, 10));
        prod2.Should().Be(new Vec2<int>(0, 4));
        prod3.Should().Be(new Vec2<int>(4, 7));
        prod4.Should().Be(new Vec2<int>(0, 0));
    }

    [Test]
    public void TestScalarDivision()
    {
        // arrange
        var vec1 = new Vec2<int>(4, 9);
        var vec2 = new Vec2<int>(1, 3);
        var vec3 = new Vec2<int>(2, -3);

        // act
        var div1 = vec1 / 4;
        var div2 = vec2 / 1;
        var div3 = vec3 / -2;
        var div4 = () => vec1 / 0;

        // assert
        div1.Should().Be(new Vec2<int>(1, 9 / 4));
        div2.Should().Be(new Vec2<int>(1, 3));
        div3.Should().Be(new Vec2<int>(-1, 3 / 2));
        div4.Should().Throw<DivideByZeroException>();
    }

    [Test]
    public void TestAbs()
    {
        // arrange
        var vec1 = new Vec2<int>(4, 9);
        var vec2 = new Vec2<int>(-1, -3);
        var vec3 = new Vec2<int>(2, -3);

        // act
        var abs1 = vec1.Abs();
        var abs2 = vec2.Abs();
        var abs3 = vec3.Abs();

        // assert
        abs1.Should().Be(new Vec2<int>(4, 9));
        abs2.Should().Be(new Vec2<int>(1, 3));
        abs3.Should().Be(new Vec2<int>(2, 3));
    }

    [Test]
    public void TestDot()
    {
        // arrange
        var vec1 = new Vec2<int>(4, 9);
        var vec2 = new Vec2<int>(-1, -3);
        var vec3 = new Vec2<int>(2, -3);
        var vec4 = new Vec2<int>(0, 0);

        // act
        var dot1 = vec1.Dot(vec1);
        var dot2 = vec1.Dot(vec2);
        var dot3 = vec1.Dot(vec3);
        var dot4 = vec1.Dot(vec4);

        // assert
        dot1.Should().Be(97);
        dot2.Should().Be(-31);
        dot3.Should().Be(-19);
        dot4.Should().Be(0);
    }

    [Test]
    public void TestIn()
    {
        // arrange
        var bottomLeft = new Vec2<int>(-4, -3);
        var topRight   = new Vec2<int>(7, 9);

        // act
        var in1 = new Vec2<int>(-3, -3).In(bottomLeft, topRight);
        var in2 = new Vec2<int>(-5, -3).In(bottomLeft, topRight);
        var in3 = new Vec2<int>(-4, -4).In(bottomLeft, topRight);
        var in4 = new Vec2<int>(2, 7).In(bottomLeft, topRight);
        var in5 = new Vec2<int>(2, 7).In(topRight, bottomLeft);

        // assert
        in1.Should().BeTrue();
        in2.Should().BeFalse();
        in3.Should().BeFalse();
        in4.Should().BeTrue();
        in5.Should().BeFalse();
    }

    [Test]
    public void TestLength()
    {
        // arrange
        var vec1 = new Vec2<int>(4, 9);
        var vec2 = new Vec2<int>(-1, -3);
        var vec3 = new Vec2<int>(0, 0);

        // act
        var length1 = vec1.LengthSquared;
        var length2 = vec2.LengthSquared;
        var length3 = vec3.LengthSquared;

        // assert
        length1.Should().Be(97);
        length2.Should().Be(10);
        length3.Should().Be(0);
    }

    [Test]
    public void TestLerp()
    {
        // arrange
        var vecMin  = new Vec2<float>(-1, -7);
        var vecMax  = new Vec2<float>(4, 8);

        // act
        var lerp1 = Vec2<float>.Lerp(vecMin, vecMax, 0);
        var lerp2 = Vec2<float>.Lerp(vecMin, vecMax, 1.0f);
        var lerp3 = Vec2<float>.Lerp(vecMin, vecMax, 0.4f);

        // assert
        lerp1.Should().Be(vecMin);
        lerp2.Should().Be(vecMax);
        lerp3.Should().Be(new Vec2<float>(1, -1));
    }

    [Test]
    public void TestTryFormat()
    {
        // arrange

        var vec = new Vec2<float>(1234567, -7654321);
        var mem = new char[40];
        Span<char> span = mem;

        // act

        var spanLonger = span.Slice(0, 30);
        var successLonger = vec.TryFormat(spanLonger, out int writtenLonger, null, null);

        var spanExact = span.Slice(0, 19);
        var successExact = vec.TryFormat(spanExact, out int writtenExact, null, null);

        var spanShort1 = span.Slice(0, 18);
        var successShort1 = vec.TryFormat(spanShort1, out int writtenShort1, null, null);
        var spanShort2 = span.Slice(0, 12);
        var successShort2 = vec.TryFormat(spanShort2, out int writtenShort2, null, null);
        var spanShort3 = span.Slice(0, 11);
        var successShort3 = vec.TryFormat(spanShort3, out int writtenShort3, null, null);
        var spanShort4 = span.Slice(0, 10);
        var successShort4 = vec.TryFormat(spanShort4, out int writtenShort4, null, null);
        var spanShort5 = span.Slice(0, 9);
        var successShort5 = vec.TryFormat(spanShort5, out int writtenShort5, null, null);
        var spanShort6 = span.Slice(0, 1);
        var successShort6 = vec.TryFormat(spanShort6, out int writtenShort6, null, null);

        var spanEmpty = span.Slice(0, 0);
        var successEmpty = vec.TryFormat(spanEmpty, out int writtenEmpty, null, null);

        // assert

        successLonger.Should().BeTrue();
        writtenLonger.Should().Be(19);
        spanLonger.Slice(0, writtenLonger).ToString().Should().Be("(1234567, -7654321)");

        successExact.Should().BeTrue();
        writtenExact.Should().Be(19);
        spanExact.Slice(0, writtenExact).ToString().Should().Be("(1234567, -7654321)");

        successShort1.Should().BeFalse();
        writtenShort1.Should().Be(18);
        spanShort1.Slice(0, writtenShort1).ToString().Should().Be("(1234567, -7654321");
        successShort2.Should().BeFalse();
        writtenShort2.Should().Be(10);
        spanShort2.Slice(0, writtenShort2).ToString().Should().Be("(1234567, ");
        successShort3.Should().BeFalse();
        writtenShort3.Should().Be(10);
        spanShort3.Slice(0, writtenShort3).ToString().Should().Be("(1234567, ");
        successShort4.Should().BeFalse();
        writtenShort4.Should().Be(10);
        spanShort4.Slice(0, writtenShort4).ToString().Should().Be("(1234567, ");
        successShort5.Should().BeFalse();
        writtenShort5.Should().Be(8);
        spanShort5.Slice(0, writtenShort5).ToString().Should().Be("(1234567");
        successShort6.Should().BeFalse();
        writtenShort6.Should().Be(0);
        spanShort6.Slice(0, writtenShort6).ToString().Should().BeEmpty();

        successEmpty.Should().BeFalse();
        writtenEmpty.Should().Be(0);
        spanEmpty.Slice(0, writtenEmpty).ToString().Should().BeEmpty();
    }

    [Test]
    public void TestSquaredDistance()
    {
        // arrange
        var vec1 = new Vec2<float>(-1, -7);
        var vec2 = new Vec2<float>(4, 8);

        // act
        var distSquared12 = vec1.SquaredDistance(vec2);
        var distSquared21 = vec2.SquaredDistance(vec1);
        var distSquared11 = vec1.SquaredDistance(vec1);

        // assert
        distSquared12.Should().Be(distSquared21);
        distSquared12.Should().Be(5 * 5 + 15 * 15);
        distSquared11.Should().Be(0);
    }

    [Test]
    public void TestManhattanDistance()
    {
        // arrange
        var vec1 = new Vec2<float>(-1, -7);
        var vec2 = new Vec2<float>(4, 8);

        // act
        var distManhattan12 = vec1.ManhattanDistance(vec2);
        var distManhattan21 = vec2.ManhattanDistance(vec1);
        var distManhattan11 = vec1.ManhattanDistance(vec1);

        // assert
        distManhattan12.Should().Be(distManhattan21);
        distManhattan12.Should().Be(5 + 15);
        distManhattan11.Should().Be(0);
    }
}
