namespace Calculator;

public class CostCalculator
{
    public List<ChargeInfo> UpdateCost(List<ChargeInfo> charges)
    {
        var peak = 59.499m;
        var shoulder = 39.138m;
        var offPeak = 45.507m;
        var supplyCharge = 107.965m;
        var feedIn = 5m;

        return charges.Select(c =>
        {
            var tier = GetTier(TimeOnly.FromDateTime(c.EndDate));

            // If Usage
            var cost = tier switch
            {
                Tier.Peak => c.Amount * peak,
                Tier.OffPeak => c.Amount * offPeak,
                Tier.Shoulder => c.Amount * shoulder,
            };

            return c with { Cost = cost };
        })
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
