using Calculator;

var filePath = @"C:\Users\stuar\OneDrive\Desktop\MyUsageData_28-09-2024.csv";

var calculator = new CostCalculator();

var charges = await CostCalculator.ReadUsage(filePath);
var costed = calculator.UpdateCost(charges);
