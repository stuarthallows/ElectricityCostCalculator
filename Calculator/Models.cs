namespace Calculator;

public enum Tier { Peak, OffPeak, Shoulder };

public enum RateType { Usage, Solar };

public record ChargeInfo(RateType RateType, DateTime StartDate, DateTime EndDate, decimal Amount, decimal Cost);
