using Calculator;

var filePath = @"C:\Users\stuar\OneDrive\Desktop\MyUsageData_28-09-2024.csv";

var prices = new PriceInfo(107.965m, 59.499m, 39.138m, 45.507m, 5m);
var calculator = new CostCalculator(prices);

await calculator.Initialise(filePath);

var billingInfo = calculator.GetBillingInfo(2024);
billingInfo.ForEach(bi => Console.WriteLine($"{bi}"));
