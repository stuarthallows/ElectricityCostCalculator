namespace Calculator;

public record PriceInfo(decimal Standing, decimal Peak, decimal Shoulder, decimal OffPeak, decimal FeedIn);

public enum Tier { Peak, OffPeak, Shoulder };

public enum RateType { Usage, Solar, Standing };

public record ChargeInfo(RateType RateType, DateTime StartDate, DateTime EndDate, decimal Usage, decimal Cost);
