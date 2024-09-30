namespace Calculator;

public class CostCalculator
{
    public static List<ChargeInfo> UpdateCost(List<ChargeInfo> charges)
    {
        const decimal supplyCharge = 107.965m;
        const decimal peak = 59.499m;
        const decimal shoulder = 39.138m;
        const decimal offPeak = 45.507m;
        const decimal feedIn = 5m;

        var standingCharges = GetStandingCharges(charges, supplyCharge);

        return charges.Select(c =>
        {
            if (c.RateType == RateType.Usage)
            {
                var tier = GetTier(TimeOnly.FromDateTime(c.EndDate));

                var cost = tier switch
                {
                    Tier.Peak => c.Amount * peak,
                    Tier.OffPeak => c.Amount * offPeak,
                    Tier.Shoulder => c.Amount * shoulder,
                    _ => throw new NotImplementedException(),
                };

                return c with { Cost = cost };
            }
            
            if (c.RateType == RateType.Solar)
            {
                return c with { Cost = c.Amount * -feedIn };
            }
            
            throw new NotImplementedException($"Unknown RateType: {c.RateType}");
        })
        .Concat(standingCharges)
        .OrderBy(c => c.StartDate)
        .ToList();
    }

    public static async Task<List<ChargeInfo>> ReadUsage(string filePath)
    {
        var fileContents = await File.ReadAllLinesAsync(filePath);

        return fileContents.Skip(1).Select(line =>
        {
            var parts = line.Split(',');
            
            var rateType = Enum.Parse<RateType>(parts[0]);
            var startDate = DateTime.Parse(parts[1]);
            var endDate = DateTime.Parse(parts[2]);
            var amount = decimal.Parse(parts[3]);

            return new ChargeInfo(rateType, startDate, endDate, amount, 0);
        }).ToList();
    }

    private static List<ChargeInfo> GetStandingCharges(List<ChargeInfo> charges, decimal supplyCharge)
    {
        var dates = charges.Select(c => c.EndDate.Date.Date).Distinct();

        return dates.Select(date => new ChargeInfo(RateType.Standing, date, date.Add(TimeSpan.FromMinutes(23 * 60 + 59)), supplyCharge, 0)).ToList();
    }

    private static Tier GetTier(TimeOnly time)
    {
        if (time >= new TimeOnly(10, 0) && time < new TimeOnly(15, 0))
        {
            return Tier.Shoulder;
        }

        if (time >= new TimeOnly(1, 0) && time > new TimeOnly(6, 0))
        {
            return Tier.OffPeak;
        }

        return Tier.Peak;
    }
}
