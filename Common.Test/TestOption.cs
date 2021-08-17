using FluentAssertions;
using NUnit.Framework;

namespace Common.Test;

public class TestOption
{
    [Test]
    public void TestReturn()
    {
        var retSome = Option<int>.Return(4);
        retSome.Should().Be(Option<int>.Some(4));
        retSome.Should().NotBe(Option<int>.None);

        var resNone = Option<int>.None;
        resNone.Should().Be(Option<int>.None);
        retSome.Should().NotBe(Option<int>.Some(1));
    }

    [Test]
    public void TestMatch()
    {
        var retSome = Option<int>.Return(4);

        var matchedSome = retSome.Match((i) => Option<string>.Some(i + "1"), () => Option<string>.None);

        matchedSome.Should().Be(Option<string>.Some("41"));
        matchedSome.Should().NotBe(Option<string>.None);

        var resNone = Option<int>.None;

        var matchedNone = resNone.Match((i) => Option<string>.Some(i + "1"), () => Option<string>.None);

        matchedNone.Should().Be(Option<string>.None);
        matchedNone.Should().NotBe(Option<string>.Some("41"));
    }
}
