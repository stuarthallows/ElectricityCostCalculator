using Calculator;

const string filePath = @"C:\Users\stuar\OneDrive\Desktop\MyUsageData_28-09-2024.csv";

PriceInfo[] prices = 
    [
        new ("AGL", 107.965m, 59.499m, 39.138m, 45.507m, 5m),
        new ("AGL Solar Savers", 111.52m, 50.06m, 30.10m, 34.87m, 4m),
        new ("Energy Locals", 85m, 39.50m, 23.00m, 27.50m, 2m),
        new ("Lumo", 112.200m, 53.130m, 23.078m, 29.282m, 0m),
        
        // https://www.energymadeeasy.gov.au/plan?id=KOG756059MRE3&postcode=5075
        new ("Kogan Single Rate", 102.77m, 36.80m, 36.80m, 36.80m, 1.4m),

        new ("Glo Bird", 127.600m, 48.4m, 36.19m, 36.19m, 1.5m)
    ];

const int year = 2024;
foreach (var price in prices)
{
    var calculator = new CostCalculator(price);

    await calculator.Initialise(filePath);

    // var monthlyCosts = calculator.GetMonthlyCosts(2023);
    // monthlyCosts.ForEach(bi => Console.WriteLine($"{bi}"));

    var totalCost = calculator.GetTotalCost(year);
    Console.WriteLine($"{price.CompanyName} - Total cost for {year}: {totalCost}");
}

