using Compare.AI.Raw.cs;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Llama.TemporalAsOfShowcase
{ 
    public class DataStore
    {
        private readonly ConcurrentDictionary<string, List<(DateTime, object)>> dataHistory = new ConcurrentDictionary<string, List<(DateTime, object)>>();
        private readonly ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();

        public void AddData(string id, object value, DateTime timestamp)
        {
            lockSlim.EnterWriteLock();
            try
            {
                if (!dataHistory.TryGetValue(id, out List<(DateTime, object)> history))
                {
                    history = new List<(DateTime, object)>();
                    dataHistory[id] = history;
                }

                history.Add((timestamp, value));
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        public object GetData(DateTime asOf)
        {
            lockSlim.EnterReadLock();
            try
            {
                foreach (var (id, history) in dataHistory)
                {
                    var latest = history.LastOrDefault(x => x.Item1 <= asOf);
                    if (latest != default)
                    {
                        return latest.Item2;
                    }
                }

                throw new KeyNotFoundException($"No data found for {asOf}");
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        public (object, object) GetDataDelta(DateTime asOfStart, DateTime asOfEnd)
        {
            lockSlim.EnterReadLock();
            try
            {
                foreach (var (id, history) in dataHistory)
                {
                    var start = history.LastOrDefault(x => x.Item1 <= asOfStart);
                    var end = history.LastOrDefault(x => x.Item1 <= asOfEnd);

                    if (start != default && end != default)
                    {
                        return (start.Item2, end.Item2);
                    }
                }

                throw new KeyNotFoundException($"No data found for {asOfStart} or {asOfEnd}");
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        public string GetDataDescription(object data)
        {
            if (data is byte[])
            {
                return "Images/Video";
            }
            else if (data is Dictionary<string, double?>)
            {
                return "Financial Price data";
            }
            else if (data is string)
            {
                return "Text/JSON";
            }
            else
            {
                return "Unknown data type";
            }
        }
    }

    public class Program_Llama
    {
        private static Random random = new Random();

        public static void Main_Llama(string[] args)
        {
            Console.WriteLine(Banner.Generate("Llama 3.1 70B"));

            var dataStore = new DataStore();
            var clock = new DateTime(2022, 1, 1, 0, 0, 0);

            // Simulate 5 minutes of data
            Parallel.For(0, 300, i =>
            {
                // Randomly select a data type
                var dataType = random.Next(3);

                // Randomly select an ID
                var id = random.Next(2) == 0 ? "ID-Image" : random.Next(2) == 0 ? "ID-Prices" : "ID-JSON";

                // Create a new data item
                object data;
                switch (dataType)
                {
                    case 0:
                        data = new byte[] { 1, 2, 3, 4, 5 };
                        break;
                    case 1:
                        data = new Dictionary<string, double?> { { "Price1", 10.99 }, { "Price2", 5.99 } };
                        break;
                    default:
                        data = "{\"key\":\"value\"}";
                        break;
                }

                // Add the data to the store
                dataStore.AddData(id, data, clock.AddSeconds(random.Next(5, 11)));
            });

            // Randomly access the data
            Parallel.For(0, 100, i =>
            {
                var asOf = clock.AddSeconds(random.Next(-300, 300));
                try
                {
                    var data = dataStore.GetData(asOf);
                    var description = dataStore.GetDataDescription(data);
                    Console.WriteLine($"GetData({asOf}): {description} - {data}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"GetData({asOf}): {ex.Message}");
                }

                var asOfStart = clock.AddSeconds(random.Next(-300, 300));
                var asOfEnd = clock.AddSeconds(random.Next(-300, 300));
                try
                {
                    var (start, end) = dataStore.GetDataDelta(asOfStart, asOfEnd);
                    var startDescription = dataStore.GetDataDescription(start);
                    var endDescription = dataStore.GetDataDescription(end);
                    Console.WriteLine($"GetDataDelta({asOfStart}, {asOfEnd}): Start={startDescription} - {start}, End={endDescription} - {end}");
                    if ((start is Dictionary<string, double?> startDict) && (end is Dictionary<string, double?> endDict))
                    {
                        foreach (var key in startDict.Keys)
                        {
                            if (endDict.ContainsKey(key))
                            {
                                Console.WriteLine($"Delta for {key}: {startDict[key]} -> {endDict[key]}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"GetDataDelta({asOfStart}, {asOfEnd}): {ex.Message}");
                }
            });
        }
    }
}