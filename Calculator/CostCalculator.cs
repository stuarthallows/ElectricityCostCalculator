namespace Calculator;

public class CostCalculator(PriceInfo prices)
{
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
    
    public List<ChargeInfo> UpdateCost(List<ChargeInfo> charges)
    {
        var standingCharges = GetStandingCharges(charges);

        return charges.Select(c =>
        {
            if (c.RateType == RateType.Usage)
            {
                var tier = GetTier(TimeOnly.FromDateTime(c.EndDate));

                var cost = tier switch
                {
                    Tier.Peak => c.Usage * prices.Peak,
                    Tier.OffPeak => c.Usage * prices.OffPeak,
                    Tier.Shoulder => c.Usage * prices.Shoulder,
                    _ => throw new NotImplementedException(),
                };

                return c with { Cost = cost };
            }
            
            if (c.RateType == RateType.Solar)
            {
                return c with { Cost = c.Usage * -prices.FeedIn };
            }
            
            throw new NotImplementedException($"Unknown RateType: {c.RateType}");
        })
        .Concat(standingCharges)
        .OrderBy(c => c.StartDate)
        .ToList();
    }

    private List<ChargeInfo> GetStandingCharges(List<ChargeInfo> charges)
    {
        var dates = charges.Select(c => c.EndDate.Date.Date).Distinct();

        return dates.Select(date => new ChargeInfo(RateType.Standing, date, date.Add(TimeSpan.FromMinutes(23 * 60 + 59)), 0, prices.Standing)).ToList();
    }

    private static Tier GetTier(TimeOnly time)
    {
        if (time >= new TimeOnly(10, 0) && time < new TimeOnly(15, 0))
        {
            return Tier.Shoulder;
        }

        if (time >= new TimeOnly(1, 0) && time < new TimeOnly(6, 0))
        {
            return Tier.OffPeak;
        }

        return Tier.Peak;
    }
}
