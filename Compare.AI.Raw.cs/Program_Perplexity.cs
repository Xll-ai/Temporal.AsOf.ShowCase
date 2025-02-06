using Compare.AI.Raw.cs;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Perplexity.TemporalAsOfShowcase
{
    public class DataItem
    {
        public string ID { get; set; }
        public object Data { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ImageData
    {
        public byte[] ImageBytes { get; set; }
        public string Format { get; set; }
    }

    public class FinancialData
    {
        public Dictionary<string, decimal> Prices { get; set; }
    }

    public class TextData
    {
        public string Content { get; set; }
    }

    public class DataDelta
    {
        public string ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public object StartValue { get; set; }
        public object EndValue { get; set; }
    }

    public class AsOfDataManager
    {
        private readonly ConcurrentDictionary<DateTime, DataItem> dataPoints = new ConcurrentDictionary<DateTime, DataItem>();

        public void AddDataPoint(DateTime timestamp, DataItem data)
        {
            dataPoints[timestamp] = data;
        }

        public DataItem GetData(DateTime asOf)
        {
            if (dataPoints.IsEmpty)
                throw new InvalidOperationException("No data points available.");

            var keys = dataPoints.Keys.ToList();
            keys.Sort();
            var key = keys.Where(k => k <= asOf).DefaultIfEmpty(keys.First()).Max();
            return dataPoints[key];
        }

        public (int ChangeCount, IEnumerable<DataDelta> Deltas) GetDataDelta(DateTime asOfStart, DateTime asOfEnd)
        {
            var relevantData = dataPoints
                .Where(kvp => kvp.Key >= asOfStart && kvp.Key <= asOfEnd)
                .OrderBy(kvp => kvp.Key)
                .ToList();

            var deltas = new List<DataDelta>();
            var groupedByID = relevantData.GroupBy(d => d.Value.ID);

            foreach (var group in groupedByID)
            {
                var orderedGroup = group.OrderBy(g => g.Key).ToList();
                if (orderedGroup.Count > 1)
                {
                    deltas.Add(new DataDelta
                    {
                        ID = group.Key,
                        StartTime = orderedGroup.First().Key,
                        EndTime = orderedGroup.Last().Key,
                        StartValue = orderedGroup.First().Value.Data,
                        EndValue = orderedGroup.Last().Value.Data
                    });
                }
            }

            return (deltas.Count, deltas);
        }
    }

    public class Program_Perplexity
    {
        private static readonly Random random = new Random();
        private static readonly ConcurrentBag<string> existingIds = new ConcurrentBag<string>();
        private static readonly AsOfDataManager dataManager = new AsOfDataManager();

        public static void Main_Perplexity()
        {
            Console.WriteLine(Banner.Generate("Perplexity"));

            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            DateTime startTime = DateTime.Now.Date;
            DateTime endTime = startTime.AddMinutes(5);

            await GenerateDataAsync(startTime, endTime);
            DemonstrateDataRetrieval(startTime, endTime);
        }

        private static async Task GenerateDataAsync(DateTime startTime, DateTime endTime)
        {
            DateTime currentTime = startTime;

            while (currentTime < endTime)
            {
                GenerateRandomData(currentTime);
                int delay = random.Next(5000, 10001);
                currentTime = currentTime.AddMilliseconds(delay);
                await Task.Delay(delay / 100); // Slow down simulation for demonstration
            }
        }

        private static void GenerateRandomData(DateTime timestamp)
        {
            int dataType = random.Next(3);
            string id = existingIds.Any() && random.Next(2) == 0
                ? existingIds.ElementAt(random.Next(existingIds.Count))
                : Guid.NewGuid().ToString();

            existingIds.Add(id);

            DataItem dataItem = new DataItem { ID = id, Timestamp = timestamp };

            switch (dataType)
            {
                case 0: // Image
                    dataItem.Data = new ImageData { ImageBytes = new byte[100], Format = "PNG" };
                    break;
                case 1: // Financial
                    dataItem.Data = new FinancialData
                    {
                        Prices = new Dictionary<string, decimal>
                        {
                            { "AAPL", (decimal)(random.NextDouble() * 100 + 100) },
                            { "GOOGL", (decimal)(random.NextDouble() * 100 + 1000) }
                        }
                    };
                    break;
                case 2: // Text/JSON
                    var jsonObject = new { Name = "Sample", Value = random.Next(100) };
                    dataItem.Data = new TextData { Content = JsonSerializer.Serialize(jsonObject) };
                    break;
            }

            dataManager.AddDataPoint(timestamp, dataItem);
            Console.WriteLine($"Added data: {dataItem.ID} at {timestamp}");
        }

        private static void DemonstrateDataRetrieval(DateTime startTime, DateTime endTime)
        {
            // Get data at a random time
            DateTime randomTime = startTime.AddSeconds(random.Next((int)(endTime - startTime).TotalSeconds));
            var dataAtTime = dataManager.GetData(randomTime);
            Console.WriteLine($"\nData at {randomTime}:");
            PrintDataItem(dataAtTime);

            // Get data delta for the entire time range
            var (changeCount, deltas) = dataManager.GetDataDelta(startTime, endTime);
            Console.WriteLine($"\nData delta from {startTime} to {endTime}:");
            Console.WriteLine($"Number of changes: {changeCount}");
            foreach (var delta in deltas)
            {
                Console.WriteLine($"ID: {delta.ID}");
                Console.WriteLine($"Start Time: {delta.StartTime}, End Time: {delta.EndTime}");
                Console.WriteLine("Start Value:");
                PrintDataValue(delta.StartValue);
                Console.WriteLine("End Value:");
                PrintDataValue(delta.EndValue);
                Console.WriteLine();
            }
        }

        private static void PrintDataItem(DataItem item)
        {
            Console.WriteLine($"ID: {item.ID}, Timestamp: {item.Timestamp}");
            PrintDataValue(item.Data);
        }

        private static void PrintDataValue(object data)
        {
            switch (data)
            {
                case ImageData imageData:
                    Console.WriteLine($"Type: Image, Size: {imageData.ImageBytes.Length} bytes, Format: {imageData.Format}");
                    break;
                case FinancialData financialData:
                    Console.WriteLine("Type: Financial Data");
                    foreach (var price in financialData.Prices)
                    {
                        Console.WriteLine($"  {price.Key}: {price.Value:C}");
                    }
                    break;
                case TextData textData:
                    Console.WriteLine($"Type: Text/JSON, Content: {textData.Content}");
                    break;
                default:
                    Console.WriteLine($"Unknown data type: {data.GetType().Name}");
                    break;
            }
        }
    }
}
