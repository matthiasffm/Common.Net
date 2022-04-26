using FluentAssertions;
using NUnit.Framework;

using matthiasffm.Common.Functional;

namespace matthiasffm.Common.Test;

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

    [Test]
    public void TestMonadSome()
    {
        var resultSome = Option<int>.Return(42 * 7)
                                    .Bind(v => DivideBy(v, 6))
                                    .Bind(v => Substract(v, 24))
                                    .Bind(v => Sqrt(v));

        resultSome.Should().Be(Option<int>.Return(5));
    }

    [Test]
    public void TestMonadNone()
    {
        var resultSome = Option<int>.Return(42 * 7)
                                    .Bind(v => DivideBy(v, 6))
                                    .Bind(v => Substract(v, 50))
                                    .Bind(v => Sqrt(v));

        resultSome.Should().Be(Option<int>.None);
    }

    private static Option<int> DivideBy(int a, int b) => b != 0 ? Option<int>.Some(a / b) : Option<int>.None;
    private static Option<int> Substract(int a, int b) => Option<int>.Return(a - b);
    private static Option<int> Sqrt(int a) => a > 0 ? Option<int>.Some((int)System.Math.Sqrt(a)) : Option<int>.None;
}

/*
public string RefillBalance(int customerId, decimal moneyAmount)
{
    var moneyToCharge = MoneyAmount.Create(moneyAmount);
    var customer = customerRepositoryAfterAfter.GetById(customerId).ToResult("Customer can't be found");
    return Result.Combine(moneyToCharge, customer)
        .OnSuccess(() => customer.Value!.AddBalance(moneyToCharge.Value))
        .OnSuccess(() => paymentServiceAfter.ChargePayment(customer.Value!.BillingInfo, moneyToCharge.Value))
        .OnSuccess(
            () => customerRepositoryAfterAfter.Save(customer.Value!)
                .OnFailure(()=> paymentServiceAfter.RollbackLastTransaction()))
        .OnBoth(result => Log(result))
        .OnBoth(result => result.IsSuccess ? "OK" : result.Error);


}

.Case(
		   right: (data) => ProcessData(data),
		   left: (error) => PrintError(error));

 
 
private Option<int> SubWithError(int i)
{
    if(i == 0)
        return Option<int>.None; // return Err(0);
    else
        return Option<int>.Return(i + 1); // return Ok(i + 1);
}

private int Main(int args)
{
    var res = SubWithError(args);
    switch(res)
    {
        when Ok(x)  => return 1
        when Err(x) => return 0;
    }
}
 */
