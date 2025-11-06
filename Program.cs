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
var salesTotal = CalculateSalesTotal(salesFiles); // Add this line of code

File.AppendAllText(Path.Combine(salesTotalDirectory, "totals.txt"), $"{salesTotal:F2}{Environment.NewLine}");
record SalesData(double Total);


