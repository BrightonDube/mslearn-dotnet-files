// See https://aka.ms/new-console-template for more information
using System.Text.Json;

using System.IO;
using System.Collections.Generic;
using System.Text.Json.Serialization;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");
var salesTotalDirectory = Path.Combine(currentDirectory, "salesTotalDirectory");

Directory.CreateDirectory(salesTotalDirectory);

// Console.WriteLine(Path.Combine("stores", "201", "Donkeys"));
// string fileName = $"stores{Path.DirectorySeparatorChar}201{Path.DirectorySeparatorChar}sales{Path.DirectorySeparatorChar}sales.json";

// FileInfo info = new FileInfo(fileName);

// Console.WriteLine($"Full Name: {info.FullName}{Environment.NewLine}Directory: {info.Directory}{Environment.NewLine}Extension: {info.Extension}{Environment.NewLine}Create Date: {info.CreationTime}"); // And many more
var salesFiles = FindFiles(storesDirectory);
// foreach (var file in salesFiles)
// {
//     Console.WriteLine(file);
// }

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();
    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);
    foreach (var file in foundFiles)
    {
        if (Path.GetExtension(file) == ".json")
        {
            salesFiles.Add(file);
        }
    }
    return salesFiles;
}
double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // READ FILES LOOP
    foreach (var file in salesFiles)
    {
        //Read contents of the file
        var salesJson = File.ReadAllText(file);

        //Parse the contents as JSON
        SalesData? data = JsonSerializer.Deserialize<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}
var salesTotal = CalculateSalesTotal(salesFiles); 

File.AppendAllText(Path.Combine(salesTotalDirectory, "totals.txt"), $"{salesTotal:F2}{Environment.NewLine}");


GenerateSalesSummaryReport(salesFiles, salesTotalDirectory);

void GenerateSalesSummaryReport(IEnumerable<string> salesFilesList, string outputDirectory)
{
    var details = new List<(string FileName, double Amount)>();
    double grandTotal = 0;

    foreach (var file in salesFilesList)
    {
        var json = File.ReadAllText(file);
        SalesData? data = JsonSerializer.Deserialize<SalesData?>(json);
        double amount = data?.Total ?? 0;
        grandTotal += amount;
        details.Add((Path.GetFileName(file), amount));
    }

    var reportPath = Path.Combine(outputDirectory, "sales_summary.txt");
    using var writer = new StreamWriter(reportPath, false);
    writer.WriteLine("Sales Summary");
    writer.WriteLine("***********************************");
    writer.WriteLine($" Total Sales: {grandTotal.ToString("C2")}{Environment.NewLine}");
    writer.WriteLine(" Details:");
    foreach (var d in details)
    {
        writer.WriteLine($"  {d.FileName}: {d.Amount.ToString("C2")}");
    }
}

record SalesData(double Total);


