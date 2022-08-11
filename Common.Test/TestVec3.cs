using FluentAssertions;

using matthiasffm.Common.Math;

using NUnit.Framework;

namespace matthiasffm.Common.Test;

internal class TestVec3
{
    [Test]
    public void TestCompareToObj()
    {
        // arrange
        var vec1 = new Vec3<int>(1, 2, 3);
        string compare1 = null;
        string compare2 = "test";
        Vec3<int> compare3 = null;
        Vec3<float> compare4 = null;

        // act
        int compRes1 = vec1.CompareTo(compare1);
        int compRes2 = vec1.CompareTo(compare2);
        int compRes3 = vec1.CompareTo(compare3);
        int compRes4 = vec1.CompareTo(compare4);

        // assert
        compRes1.Should().Be(1);
        compRes2.Should().Be(1);
        compRes3.Should().Be(1);
        compRes4.Should().Be(1);
    }

    [Test]
    public void TestCompareToVec3()
    {
        // arrange
        var vec1 = new Vec3<int>(1, 2, 3);
        var vec2 = new Vec3<int>(0, 2, 3);
        var vec3 = new Vec3<int>(1, 2, 3);
        var vec4 = new Vec3<int>(2, 2, 5);
        var vec5 = new Vec3<int>(4, 3, 7);
        var vec6 = new Vec3<int>(0, 0, 0);

        // act
        int comp1 = vec1.CompareTo(vec2);
        int comp2 = vec1.CompareTo(vec3);
        int comp3 = vec1.CompareTo(vec4);
        int comp4 = vec1.CompareTo(vec5);
        int comp5 = vec1.CompareTo(vec6);

        // assert
        comp1.Should().Be(1);
        comp2.Should().Be(0);
        comp3.Should().Be(-1);
        comp4.Should().Be(-1);
        comp5.Should().Be(1);
    }

    [Test]
    public void TestAdd()
    {
        // arrange
        var vec1 = new Vec3<int>(1, 2, 3);
        var vec2 = new Vec3<int>(0, 2, 1);
        var vec3 = new Vec3<int>(-4, -7, -5);
        var vec4 = new Vec3<int>(0, 0, 0);
        Vec3<int> vec5 = null;

        // act
        var add1 = vec1 + vec2;
        var add2 = vec3 + vec1;
        var add3 = vec1 + vec4;
        var add4 = vec4 + vec1;
        var add5 = () => vec1 + vec5;

        // assert
        add1.Should().Be(new Vec3<int>(1, 4, 4));
        add2.Should().Be(new Vec3<int>(-3, -5, -2));
        add3.Should().Be(new Vec3<int>(1, 2, 3));
        add4.Should().Be(new Vec3<int>(1, 2, 3));
        add5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestIncrement()
    {
        // arrange
        var vec1 = new Vec3<int>(1, 2, 3);
        Vec3<int> vec2 = null;

        // act
        var inc1 = vec1++;
        var inc2 = vec1++;
        var inc3 = ++vec1;
        var inc4 = ++vec1;
        var inc5 = () => vec2++;

        // assert
        vec1.Should().Be(new Vec3<int>(1 + 4, 2 + 4, 3 + 4));
        inc1.Should().Be(new Vec3<int>(1, 2, 3));
        inc2.Should().Be(new Vec3<int>(2, 3, 4));
        inc3.Should().Be(new Vec3<int>(4, 5, 6));
        inc4.Should().Be(new Vec3<int>(5, 6, 7));
        inc5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestSubtract()
    {
        // arrange
        var vec1 = new Vec3<int>(1, 2, 3);
        var vec2 = new Vec3<int>(0, 2, 4);
        var vec3 = new Vec3<int>(-4, -7, -2);
        var vec4 = new Vec3<int>(0, 0, 0);
        Vec3<int> vec5 = null;

        // act
        var sub1 = vec1 - vec2;
        var sub2 = vec1 - vec3;
        var sub3 = vec3 - vec1;
        var sub4 = vec1 - vec4;
        var sub5 = () => vec1 - vec5;

        // assert
        sub1.Should().Be(new Vec3<int>(1, 0, -1));
        sub2.Should().Be(new Vec3<int>(5, 9, 5));
        sub3.Should().Be(new Vec3<int>(-5, -9, -5));
        sub4.Should().Be(new Vec3<int>(1, 2, 3));
        sub5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestDecrement()
    {
        // arrange
        var vec1 = new Vec3<int>(1, 2, 3);
        Vec3<int> vec2 = null;

        // act
        var dec1 = vec1--;
        var dec2 = vec1--;
        var dec3 = --vec1;
        var dec4 = --vec1;
        var dec5 = () => vec2--;

        // assert
        vec1.Should().Be(new Vec3<int>(1 - 4, 2 - 4, 3 - 4));
        dec1.Should().Be(new Vec3<int>(1, 2, 3));
        dec2.Should().Be(new Vec3<int>(0, 1, 2));
        dec3.Should().Be(new Vec3<int>(-2, -1, 0));
        dec4.Should().Be(new Vec3<int>(-3, -2, -1));
        dec5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestHadamardProduct()
    {
        // arrange
        var vec1 = new Vec3<int>(1, 2, 3);
        var vec2 = new Vec3<int>(0, 2, 4);
        var vec3 = new Vec3<int>(-4, -7, -2);
        var vec4 = new Vec3<int>(0, 0, 0);
        Vec3<int> vec5 = null;

        // act
        var prod1 = vec1 * vec2;
        var prod2 = vec3 * vec1;
        var prod3 = vec1 * vec3;
        var prod4 = vec4 * vec1;
        var prod5 = () => vec1 * vec5;

        // assert
        prod1.Should().Be(new Vec3<int>(0, 4, 12));
        prod2.Should().Be(new Vec3<int>(-4, -14, -6));
        prod3.Should().Be(new Vec3<int>(-4, -14, -6));
        prod4.Should().Be(new Vec3<int>(0, 0, 0));
        prod5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestHadamardDivision()
    {
        // arrange
        var vec1 = new Vec3<int>(4, 9, 8);
        var vec2 = new Vec3<int>(1, 1, 1);
        var vec3 = new Vec3<int>(2, -3, 4);
        var vec4 = new Vec3<int>(0, 0, 0);
        Vec3<int> vec5 = null;

        // act
        var div1 = vec1 / vec2;
        var div2 = vec3 / vec1;
        var div3 = vec1 / vec3;
        var div4 = vec4 / vec1;
        var div5 = () => vec1 / vec4;
        var div6 = () => vec1 / vec5;

        // assert
        div1.Should().Be(new Vec3<int>(4, 9, 8));
        div2.Should().Be(new Vec3<int>(1 / 2, -1 / 3, 1 / 2));
        div3.Should().Be(new Vec3<int>(2, -3, 2));
        div4.Should().Be(new Vec3<int>(0, 0, 0));
        div5.Should().Throw<DivideByZeroException>();
        div6.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestScalarProduct()
    {
        // arrange
        var vec1 = new Vec3<int>(1, 2, 3);
        var vec2 = new Vec3<int>(0, 2, 4);
        var vec3 = new Vec3<int>(-4, -7, -3);
        Vec3<int> vec4 = null;

        // act
        var prod1 = vec1 * 5;
        var prod2 = 2 * vec2;
        var prod3 = -1 * vec3;
        var prod4 = vec3 * 0;
        var prod5 = () => 5 * vec4;
        var prod6 = () => vec4 * 2;

        // assert
        prod1.Should().Be(new Vec3<int>(5, 10, 15));
        prod2.Should().Be(new Vec3<int>(0, 4, 8));
        prod3.Should().Be(new Vec3<int>(4, 7, 3));
        prod4.Should().Be(new Vec3<int>(0, 0, 0));
        prod5.Should().Throw<ArgumentNullException>();
        prod6.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestScalarDivision()
    {
        // arrange
        var vec1 = new Vec3<int>(4, 9, 5);
        var vec2 = new Vec3<int>(1, 3, 6);
        var vec3 = new Vec3<int>(2, -3, 7);
        Vec3<int> vec5 = null;

        // act
        var div1 = vec1 / 4;
        var div2 = vec2 / 1;
        var div3 = vec3 / -2;
        var div4 = () => vec1 / 0;
        var div5 = () => vec5 / 2;

        // assert
        div1.Should().Be(new Vec3<int>(1, 9 / 4, 5 / 4));
        div2.Should().Be(new Vec3<int>(1, 3, 6));
        div3.Should().Be(new Vec3<int>(-1, 3 / 2, -7 / 2));
        div4.Should().Throw<DivideByZeroException>();
        div5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestAbs()
    {
        // arrange
        var vec1 = new Vec3<int>(4, 9, 5);
        var vec2 = new Vec3<int>(-1, -3, -6);
        var vec3 = new Vec3<int>(2, -3, 7);

        // act
        var abs1 = vec1.Abs();
        var abs2 = vec2.Abs();
        var abs3 = vec3.Abs();

        // assert
        abs1.Should().Be(new Vec3<int>(4, 9, 5));
        abs2.Should().Be(new Vec3<int>(1, 3, 6));
        abs3.Should().Be(new Vec3<int>(2, 3, 7));
    }

    [Test]
    public void TestDot()
    {
        // arrange
        var vec1 = new Vec3<int>(4, 9, 5);
        var vec2 = new Vec3<int>(-1, -3, -3);
        var vec3 = new Vec3<int>(2, -3, 1);
        var vec4 = new Vec3<int>(0, 0, 0);
        Vec3<int> vec5 = null;

        // act
        var dot1 = vec1.Dot(vec1);
        var dot2 = vec1.Dot(vec2);
        var dot3 = vec1.Dot(vec3);
        var dot4 = vec1.Dot(vec4);
        var dot5 = () => vec1.Dot(vec5);

        // assert
        dot1.Should().Be(122);
        dot2.Should().Be(-46);
        dot3.Should().Be(-14);
        dot4.Should().Be(0);
        dot5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestIn()
    {
        // arrange
        var topLeft = new Vec3<int>(-4, -3, 2);
        var bottomRight = new Vec3<int>(7, 9, 5);
        Vec3<int> vecN = null;

        // act
        var in1 = new Vec3<int>(-3, -3, 3).In(topLeft, bottomRight);
        var in2 = new Vec3<int>(-5, -3, 3).In(topLeft, bottomRight);
        var in3 = new Vec3<int>(-4, -4, 1).In(topLeft, bottomRight);
        var in4 = new Vec3<int>(2, 7, 4).In(topLeft, bottomRight);
        var in5 = new Vec3<int>(2, 7, 3).In(bottomRight, topLeft);
        var inN1 = () => new Vec3<int>(-3, -3, -3).In(topLeft, vecN);
        var inN2 = () => new Vec3<int>(-3, -3, -3).In(vecN, bottomRight);

        // assert
        in1.Should().BeTrue();
        in2.Should().BeFalse();
        in3.Should().BeFalse();
        in4.Should().BeTrue();
        in5.Should().BeFalse();
        inN1.Should().Throw<ArgumentNullException>();
        inN2.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestLength()
    {
        // arrange
        var vec1 = new Vec3<int>(4, 9, 5);
        var vec2 = new Vec3<int>(-1, -3, -7);
        var vec3 = new Vec3<int>(0, 0, 0);

        // act
        var length1 = vec1.LengthSquared;
        var length2 = vec2.LengthSquared;
        var length3 = vec3.LengthSquared;

        // assert
        length1.Should().Be(122);
        length2.Should().Be(59);
        length3.Should().Be(0);
    }

    [Test]
    public void TestLerp()
    {
        // arrange
        var vecMin = new Vec3<float>(-1, -7, -3);
        var vecMax = new Vec3<float>(4, 8, 2);
        Vec3<float> vecN = null;

        // act
        var lerp1 = Vec3<float>.Lerp(vecMin, vecMax, 0);
        var lerp2 = Vec3<float>.Lerp(vecMin, vecMax, 1.0f);
        var lerp3 = Vec3<float>.Lerp(vecMin, vecMax, 0.4f);
        var lerp4 = () => Vec3<float>.Lerp(vecN, vecMax, 0.3f);
        var lerp5 = () => Vec3<float>.Lerp(vecMin, vecN, 0.3f);

        // assert
        lerp1.Should().Be(vecMin);
        lerp2.Should().Be(vecMax);
        lerp3.Should().Be(new Vec3<float>(1, -1, -1));
        lerp4.Should().Throw<ArgumentNullException>();
        lerp5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestTryFormat()
    {
        // arrange

        var vec = new Vec3<float>(1234567, -7654321, 1122233);
        var mem = new char[50];
        Span<char> span = mem;

        // act

        var spanLonger = span.Slice(0, 40);
        var successLonger = vec.TryFormat(spanLonger, out int writtenLonger, null, null);

        var spanExact = span.Slice(0, 28);
        var successExact = vec.TryFormat(spanExact, out int writtenExact, null, null);

        var spanShort1 = span.Slice(0, 27);
        var successShort1 = vec.TryFormat(spanShort1, out int writtenShort1, null, null);
        var spanShort2 = span.Slice(0, 26);
        var successShort2 = vec.TryFormat(spanShort2, out int writtenShort2, null, null);
        var spanShort3 = span.Slice(0, 19);
        var successShort3 = vec.TryFormat(spanShort3, out int writtenShort3, null, null);
        var spanShort4 = span.Slice(0, 18);
        var successShort4 = vec.TryFormat(spanShort4, out int writtenShort4, null, null);
        var spanShort5 = span.Slice(0, 9);
        var successShort5 = vec.TryFormat(spanShort5, out int writtenShort5, null, null);
        var spanShort6 = span.Slice(0, 1);
        var successShort6 = vec.TryFormat(spanShort6, out int writtenShort6, null, null);

        var spanEmpty = span.Slice(0, 0);
        var successEmpty = vec.TryFormat(spanEmpty, out int writtenEmpty, null, null);

        // assert

        successLonger.Should().BeTrue();
        writtenLonger.Should().Be(28);
        spanLonger.Slice(0, writtenLonger).ToString().Should().Be("(1234567, -7654321, 1122233)");

        successExact.Should().BeTrue();
        writtenExact.Should().Be(28);
        spanExact.Slice(0, writtenExact).ToString().Should().Be("(1234567, -7654321, 1122233)");

        successShort1.Should().BeFalse();
        writtenShort1.Should().Be(27);
        spanShort1.Slice(0, writtenShort1).ToString().Should().Be("(1234567, -7654321, 1122233");
        successShort2.Should().BeFalse();
        writtenShort2.Should().Be(20);
        spanShort2.Slice(0, writtenShort2).ToString().Should().Be("(1234567, -7654321, ");
        successShort3.Should().BeFalse();
        writtenShort3.Should().Be(18);
        spanShort3.Slice(0, writtenShort3).ToString().Should().Be("(1234567, -7654321");
        successShort4.Should().BeFalse();
        writtenShort4.Should().Be(18);
        spanShort4.Slice(0, writtenShort4).ToString().Should().Be("(1234567, -7654321");
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
        var vec1 = new Vec3<float>(-1, -7, 4);
        var vec2 = new Vec3<float>(4, 8, 7);
        Vec3<float> vecN = null;

        // act
        var distSquared12 = vec1.SquaredDistance(vec2);
        var distSquared21 = vec2.SquaredDistance(vec1);
        var distSquared11 = vec1.SquaredDistance(vec1);
        var distSquared1N = () => vec1.SquaredDistance(vecN);

        // assert
        distSquared12.Should().Be(distSquared21);
        distSquared12.Should().Be(5 * 5 + 15 * 15 + 3 * 3);
        distSquared11.Should().Be(0);
        distSquared1N.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestManhattanDistance()
    {
        // arrange
        var vec1 = new Vec3<float>(-1, -7, 6);
        var vec2 = new Vec3<float>(4, 8, 4);
        Vec3<float> vecN = null;

        // act
        var distManhattan12 = vec1.ManhattanDistance(vec2);
        var distManhattan21 = vec2.ManhattanDistance(vec1);
        var distManhattan11 = vec1.ManhattanDistance(vec1);
        var distManhattan1N = () => vec1.ManhattanDistance(vecN);

        // assert
        distManhattan12.Should().Be(distManhattan21);
        distManhattan12.Should().Be(5 + 15 + 2);
        distManhattan11.Should().Be(0);
        distManhattan1N.Should().Throw<ArgumentNullException>();
    }
}
