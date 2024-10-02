namespace Calculator;

public class CostCalculator(PriceInfo prices)
{
    private List<ChargeInfo> _charges = [];
    
    public async Task Initialise(string filePath)
    {
        var fileContents = await File.ReadAllLinesAsync(filePath);

        _charges = fileContents.Skip(1).Select(line =>
        {
            var parts = line.Split(',');
            
            var rateType = Enum.Parse<RateType>(parts[0]);
            var startDate = DateTime.Parse(parts[1]);
            var endDate = DateTime.Parse(parts[2]);
            var amount = decimal.Parse(parts[3]);

            return new ChargeInfo(rateType, startDate, endDate, amount, 0);
        }).ToList();
        
        _charges = UpdateCharges();
    }
    
    public List<BillingInfo> GetMonthlyCosts(int year)
    {
        return Enumerable
                .Range(1, 12)
                .Select(month => GetMonthlyCosts(year, month))
                .Where(c => c.BoughtKWh > 0)
                .ToList();
    }

    public BillingInfo GetTotalCost(int year)
    {
        var monthlyCosts = GetMonthlyCosts(year);
        
        var boughtCost = monthlyCosts.Sum(c => c.BoughtCost);
        
        return new BillingInfo(year, 
            0, 
            monthlyCosts.Sum(c => c.BoughtCost), 
            monthlyCosts.Sum(c => c.BoughtKWh), 
            monthlyCosts.Sum(c => c.SoldCost), 
            monthlyCosts.Sum(c => c.SoldKWh), 
            monthlyCosts.Sum(c => c.StandingCost));
    }

    private BillingInfo GetMonthlyCosts(int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddSeconds(-1);
        
        var chargesInRange = _charges.Where(c => c.StartDate >= startDate && c.EndDate <= endDate).ToList();

        var bought = chargesInRange.Where(c => c.RateType == RateType.Usage).ToArray();
        var boughtCost = bought.Sum(c => c.Cost);
        var boughtKWh = bought.Sum(c => c.Usage);
        
        var sold = chargesInRange.Where(c => c.RateType == RateType.Solar).ToArray();
        var soldCost = sold.Sum(c => c.Cost);
        var soldKwh = sold.Sum(c => c.Usage);
        
        var standing = chargesInRange.Where(c => c.RateType == RateType.Standing).ToArray();
        var standingCost = standing.Sum(c => c.Cost);
        
        var billingInfo = new BillingInfo(
            year, 
            month, 
            boughtCost, 
            boughtKWh, 
            soldCost, 
            soldKwh, 
            standingCost);
        
        return billingInfo;
    }
    
    private List<ChargeInfo> UpdateCharges()
    {
        var standingCharges = GetStandingCharges(_charges);

        return _charges.Select(c =>
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
