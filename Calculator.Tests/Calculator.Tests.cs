using FluentAssertions;

namespace Calculator.Tests;

public class CostCalculatorTests
{
    // [Fact]
    // public void UpdateCost_HandlesOneHour()
    // {
    //     var startOfDay = new DateTime(2024, 9, 30);
    //     var prices = new PriceInfo(100m, 50m, 30m, 40m, 10m);
    //     var calculator = new CostCalculator(prices);
    //
    //     List<ChargeInfo> charges =
    //     [
    //         new (RateType.Usage, startOfDay, startOfDay.AddMinutes(29), 10, 0),
    //         new (RateType.Solar, startOfDay, startOfDay.AddMinutes(29), 2, 0)
    //     ];
    //
    //     var costed = calculator.UpdateCost(charges);
    //
    //     costed.Count.Should().Be(3);
    //     costed.Single(c => c.RateType == RateType.Usage).Cost.Should().Be(500);
    //     costed.Single(c => c.RateType == RateType.Solar).Cost.Should().Be(-20);
    //     costed.Single(c => c.RateType == RateType.Standing).Cost.Should().Be(100);
    // }
}
