using Calculator;

var filePath = @"C:\Users\stuar\OneDrive\Desktop\small.csv";

var prices = new PriceInfo(107.965m, 59.499m, 39.138m, 45.507m, 5m);
var calculator = new CostCalculator(prices);

var charges = await CostCalculator.ReadUsage(filePath);
var costed = calculator.UpdateCost(charges);

