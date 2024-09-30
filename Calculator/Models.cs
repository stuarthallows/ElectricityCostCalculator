namespace Calculator;

public record PriceInfo(decimal Standing, decimal Peak, decimal Shoulder, decimal OffPeak, decimal FeedIn);

public enum Tier { Peak, OffPeak, Shoulder };

public enum RateType { Usage, Solar, Standing };

public record ChargeInfo(RateType RateType, DateTime StartDate, DateTime EndDate, decimal Usage, decimal Cost);

public record BillingInfo(
    int Year,
    int Month,
    decimal BoughtCost,
    decimal BoughtKWh,
    decimal SoldCost,
    decimal SoldKWh,
    decimal NetCost,
    decimal NetKWh,
    decimal StandingCost)
{
    public decimal TotalCost { get; } = NetCost + StandingCost;
    
    public override string ToString() => $"[{Year}-{Month:00}] - Bought: {BoughtCost / 100:C} ({BoughtKWh:N} kWh), Sold: {-SoldCost / 100:C} ({SoldKWh:N} kWh), Net: {NetCost / 100:C} ({NetKWh:N} kWh), Total cost: {TotalCost / 100:C}";
}