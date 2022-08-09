using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Math;

namespace matthiasffm.Common.Test;

internal class TestVec2
{
    [Test]
    public void TestCompareToObj()
    {
        // arrange
        var vec1 = new Vec2<int>(1, 2);
        string compare1 = null;
        string compare2 = "test";
        Vec2<int> compare3 = null;
        Vec2<float> compare4 = null;

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
    public void TestCompareToVec2()
    {
        // arrange
        var vec1 = new Vec2<int>(1, 2);
        var vec2 = new Vec2<int>(0, 2);
        var vec3 = new Vec2<int>(1, 2);
        var vec4 = new Vec2<int>(2, 2);
        var vec5 = new Vec2<int>(4, 3);
        var vec6 = new Vec2<int>(0, 0);

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
        var vec1 = new Vec2<int>(1, 2);
        var vec2 = new Vec2<int>(0, 2);
        var vec3 = new Vec2<int>(-4, -7);
        var vec4 = new Vec2<int>(0, 0);
        Vec2<int> vec5 = null;

        // act
        var add1 = vec1 + vec2;
        var add2 = vec3 + vec1;
        var add3 = vec1 + vec4;
        var add4 = vec4 + vec1;
        var add5 = () => vec1 + vec5;

        // assert
        add1.Should().Be(new Vec2<int>(1, 4));
        add2.Should().Be(new Vec2<int>(-3, -5));
        add3.Should().Be(new Vec2<int>(1, 2));
        add4.Should().Be(new Vec2<int>(1, 2));
        add5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestIncrement()
    {
        // arrange
        var vec1 = new Vec2<int>(1, 2);
        Vec2<int> vec2 = null;

        // act
        var inc1 = vec1++;
        var inc2 = vec1++;
        var inc3 = ++vec1;
        var inc4 = ++vec1;
        var inc5 = () => vec2++;

        // assert
        vec1.Should().Be(new Vec2<int>(1 + 4, 2 + 4));
        inc1.Should().Be(new Vec2<int>(1, 2));
        inc2.Should().Be(new Vec2<int>(2, 3));
        inc3.Should().Be(new Vec2<int>(4, 5));
        inc4.Should().Be(new Vec2<int>(5, 6));
        inc5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestSubtract()
    {
        // arrange
        var vec1 = new Vec2<int>(1, 2);
        var vec2 = new Vec2<int>(0, 2);
        var vec3 = new Vec2<int>(-4, -7);
        var vec4 = new Vec2<int>(0, 0);
        Vec2<int> vec5 = null;

        // act
        var sub1 = vec1 - vec2;
        var sub2 = vec1 - vec3;
        var sub3 = vec3 - vec1;
        var sub4 = vec1 - vec4;
        var sub5 = () => vec1 - vec5;

        // assert
        sub1.Should().Be(new Vec2<int>(1, 0));
        sub2.Should().Be(new Vec2<int>(5, 9));
        sub3.Should().Be(new Vec2<int>(-5, -9));
        sub4.Should().Be(new Vec2<int>(1, 2));
        sub5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestDecrement()
    {
        // arrange
        var vec1 = new Vec2<int>(1, 2);
        Vec2<int> vec2 = null;

        // act
        var dec1 = vec1--;
        var dec2 = vec1--;
        var dec3 = --vec1;
        var dec4 = --vec1;
        var dec5 = () => vec2--;

        // assert
        vec1.Should().Be(new Vec2<int>(1 - 4, 2 - 4));
        dec1.Should().Be(new Vec2<int>(1, 2));
        dec2.Should().Be(new Vec2<int>(0, 1));
        dec3.Should().Be(new Vec2<int>(-2, -1));
        dec4.Should().Be(new Vec2<int>(-3, -2));
        dec5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestHadamardProduct()
    {
        // arrange
        var vec1 = new Vec2<int>(1, 2);
        var vec2 = new Vec2<int>(0, 2);
        var vec3 = new Vec2<int>(-4, -7);
        var vec4 = new Vec2<int>(0, 0);
        Vec2<int> vec5 = null;

        // act
        var prod1 = vec1 * vec2;
        var prod2 = vec3 * vec1;
        var prod3 = vec1 * vec3;
        var prod4 = vec4 * vec1;
        var prod5 = () => vec1 * vec5;

        // assert
        prod1.Should().Be(new Vec2<int>(0, 4));
        prod2.Should().Be(new Vec2<int>(-4, -14));
        prod3.Should().Be(new Vec2<int>(-4, -14));
        prod4.Should().Be(new Vec2<int>(0, 0));
        prod5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestHadamardDivision()
    {
        // arrange
        var vec1 = new Vec2<int>(4, 9);
        var vec2 = new Vec2<int>(1, 1);
        var vec3 = new Vec2<int>(2, -3);
        var vec4 = new Vec2<int>(0, 0);
        Vec2<int> vec5 = null;

        // act
        var div1 = vec1 / vec2;
        var div2 = vec3 / vec1;
        var div3 = vec1 / vec3;
        var div4 = vec4 / vec1;
        var div5 = () => vec1 / vec4;
        var div6 = () => vec1 / vec5;

        // assert
        div1.Should().Be(new Vec2<int>(4, 9));
        div2.Should().Be(new Vec2<int>(1 / 2, -1 / 3));
        div3.Should().Be(new Vec2<int>(2, -3));
        div4.Should().Be(new Vec2<int>(0, 0));
        div5.Should().Throw<DivideByZeroException>();
        div6.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestScalarProduct()
    {
        // arrange
        var vec1 = new Vec2<int>(1, 2);
        var vec2 = new Vec2<int>(0, 2);
        var vec3 = new Vec2<int>(-4, -7);
        Vec2<int> vec4 = null;

        // act
        var prod1 = vec1 * 5;
        var prod2 = 2 * vec2;
        var prod3 = -1 * vec3;
        var prod4 = vec3 * 0;
        var prod5 = () => 5 * vec4;
        var prod6 = () => vec4 * 2;

        // assert
        prod1.Should().Be(new Vec2<int>(5, 10));
        prod2.Should().Be(new Vec2<int>(0, 4));
        prod3.Should().Be(new Vec2<int>(4, 7));
        prod4.Should().Be(new Vec2<int>(0, 0));
        prod5.Should().Throw<ArgumentNullException>();
        prod6.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestScalarDivision()
    {
        // arrange
        var vec1 = new Vec2<int>(4, 9);
        var vec2 = new Vec2<int>(1, 3);
        var vec3 = new Vec2<int>(2, -3);
        Vec2<int> vec5 = null;

        // act
        var div1 = vec1 / 4;
        var div2 = vec2 / 1;
        var div3 = vec3 / -2;
        var div4 = () => vec1 / 0;
        var div5 = () => vec5 / 2;

        // assert
        div1.Should().Be(new Vec2<int>(1, 9 / 4));
        div2.Should().Be(new Vec2<int>(1, 3));
        div3.Should().Be(new Vec2<int>(-1, 3 / 2));
        div4.Should().Throw<DivideByZeroException>();
        div5.Should().Throw<ArgumentNullException>();
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
        Vec2<int> vec5 = null;

        // act
        var dot1 = vec1.Dot(vec1);
        var dot2 = vec1.Dot(vec2);
        var dot3 = vec1.Dot(vec3);
        var dot4 = vec1.Dot(vec4);
        var dot5 = () => vec1.Dot(vec5);

        // assert
        dot1.Should().Be(97);
        dot2.Should().Be(-31);
        dot3.Should().Be(-19);
        dot4.Should().Be(0);
        dot5.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TestIn()
    {
        // arrange
        var topLeft     = new Vec2<int>(-4, 9);
        var bottomRight = new Vec2<int>(7, -3);
        Vec2<int> vecN = null;

        // act
        var in1 = new Vec2<int>(-3, -3).In(topLeft, bottomRight);
        var in2 = new Vec2<int>(-5, -3).In(topLeft, bottomRight);
        var in3 = new Vec2<int>(-4, -4).In(topLeft, bottomRight);
        var in4 = new Vec2<int>(2, 7).In(topLeft, bottomRight);
        var in5 = new Vec2<int>(2, 7).In(bottomRight, topLeft);
        var inN1 = () => new Vec2<int>(-3, -3).In(topLeft, vecN);
        var inN2 = () => new Vec2<int>(-3, -3).In(vecN, bottomRight);

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
        Vec2<float> vecN = null;

        // act
        var lerp1 = Vec2<float>.Lerp(vecMin, vecMax, 0);
        var lerp2 = Vec2<float>.Lerp(vecMin, vecMax, 1.0f);
        var lerp3 = Vec2<float>.Lerp(vecMin, vecMax, 0.4f);
        var lerp4 = () => Vec2<float>.Lerp(vecN, vecMax, 0.3f);
        var lerp5 = () => Vec2<float>.Lerp(vecMin, vecN, 0.3f);

        // assert
        lerp1.Should().Be(vecMin);
        lerp2.Should().Be(vecMax);
        lerp3.Should().Be(new Vec2<float>(1, -1));
        lerp4.Should().Throw<ArgumentNullException>();
        lerp5.Should().Throw<ArgumentNullException>();
    }
}
