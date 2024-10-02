namespace Calculator;

public record PriceInfo(string CompanyName, decimal Standing, decimal Peak, decimal Shoulder, decimal OffPeak, decimal FeedIn);

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
    decimal StandingCost)
{
    public decimal NetCost => BoughtCost + SoldCost;
    
    public decimal NetKWh => BoughtKWh - SoldKWh;
        
    public decimal TotalCost { get; } = BoughtCost + SoldCost + StandingCost;
    
    public override string ToString() => $"[{Year}-{Month:00}] - " +
                                         $"Bought: {BoughtCost / 100:C} ({BoughtKWh:N} kWh), " +
                                         $"Sold: {-SoldCost / 100:C} ({SoldKWh:N} kWh), " +
                                         $"Net: {NetCost / 100:C} ({NetKWh:N} kWh), " +
                                         $"Standing: {StandingCost / 100:C}, " +
                                         $"Total cost: {TotalCost / 100:C}";
}