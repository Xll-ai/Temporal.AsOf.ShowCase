using Compare.AI.Raw.cs;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI4o.TemporalAsOfShowcase
{
    public class TimeSeriesStore<T>
    {
        private readonly ConcurrentDictionary<DateTime, ConcurrentDictionary<string, T>> _data = new();
        private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// Adds a data point at a specific timestamp.
        /// </summary>
        public void AddData(DateTime timestamp, string id, T value)
        {
            _lock.EnterWriteLock();
            try
            {
                if (!_data.ContainsKey(timestamp))
                {
                    _data[timestamp] = new ConcurrentDictionary<string, T>();
                }
                _data[timestamp][id] = value;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the most recent data available as of a specific time.
        /// </summary>
        public T GetData(DateTime asOf, string id)
        {
            _lock.EnterReadLock();
            try
            {
                foreach (var entry in _data.Keys.OrderByDescending(k => k))
                {
                    if (entry <= asOf && _data[entry].ContainsKey(id))
                    {
                        return _data[entry][id];
                    }
                }
                return default;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets the first and last values within a given time range.
        /// </summary>
        public List<(string ID, DateTime FirstTimestamp, T FirstValue, DateTime LastTimestamp, T LastValue)> GetDataDelta(DateTime asOfStart, DateTime asOfEnd)
        {
            _lock.EnterReadLock();
            try
            {
                var groupedData = _data
                    .Where(kvp => kvp.Key >= asOfStart && kvp.Key <= asOfEnd)
                    .SelectMany(kvp => kvp.Value.Select(item => (kvp.Key, item.Key, item.Value)))
                    .GroupBy(entry => entry.Item2) // Group by ID
                    .Select(group =>
                    {
                        var orderedEntries = group.OrderBy(entry => entry.Item1).ToList();
                        return (
                            ID: group.Key,
                            FirstTimestamp: orderedEntries.First().Item1,
                            FirstValue: orderedEntries.First().Item3,
                            LastTimestamp: orderedEntries.Last().Item1,
                            LastValue: orderedEntries.Last().Item3
                        );
                    })
                    .ToList();

                return groupedData;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    public class Program_OpenAI_4o
    {
        private static readonly Random _random = new Random();
        private static readonly string[] IdPool = { "A", "B", "C", "D", "E" };

        private static TimeSeriesStore<byte[]> _imageStore = new TimeSeriesStore<byte[]>();
        private static TimeSeriesStore<Dictionary<string, double?>> _priceStore = new TimeSeriesStore<Dictionary<string, double?>>();
        private static TimeSeriesStore<string> _jsonStore = new TimeSeriesStore<string>();

        public static void Main_OpenAI_4o()
        {
            Console.WriteLine(Banner.Generate("OpenAI 4o"));

            DateTime currentTime = new DateTime(2024, 2, 1, 0, 0, 0);
            DateTime endTime = currentTime.AddMinutes(5);

            Console.WriteLine("Simulating Concurrent Data Arrival...\n");

            Task.Run(() => SimulateDataArrival(currentTime, endTime));
            Task.Run(() => SimulateRandomQueries(currentTime, endTime));

            Console.ReadLine(); // Keep the program running
        }

        private static void SimulateDataArrival(DateTime startTime, DateTime endTime)
        {
            DateTime currentTime = startTime;

            while (currentTime < endTime)
            {
                int sleepTime = _random.Next(5, 11); // Random 5-10 sec interval
                Thread.Sleep(sleepTime * 100); // Simulate real-time processing (reduced for testing)

                AddRandomData(currentTime);
                currentTime = currentTime.AddSeconds(sleepTime);
            }
        }

        private static void SimulateRandomQueries(DateTime startTime, DateTime endTime)
        {
            while (true)
            {
                Thread.Sleep(_random.Next(300, 1000)); // Query every 300ms - 1s

                DateTime queryTime = startTime.AddSeconds(_random.Next((int)(endTime - startTime).TotalSeconds));
                string randomID = IdPool[_random.Next(IdPool.Length)];

                var priceData = _priceStore.GetData(queryTime, randomID);
                Console.WriteLine($"\n🔍 Query at {queryTime} for ID[{randomID}]");
                if (priceData != null) Console.WriteLine($"- 💰 Price[{randomID}]: {string.Join(", ", priceData.Select(kv => $"{kv.Key}: {kv.Value}"))}");

                if (_random.Next(10) < 3) // 30% chance to fetch deltas
                {
                    DateTime startRange = startTime.AddSeconds(_random.Next(30));
                    DateTime endRange = startTime.AddSeconds(_random.Next(60, 150));

                    var priceDeltas = _priceStore.GetDataDelta(startRange, endRange);
                    Console.WriteLine($"\n📊 GetDataDelta from {startRange} to {endRange}: {priceDeltas.Count} changes.");
                    foreach (var delta in priceDeltas)
                    {
                        Console.WriteLine($"- ID[{delta.ID}]: From {delta.FirstValue} at {delta.FirstTimestamp} → To {delta.LastValue} at {delta.LastTimestamp}");
                    }
                }
            }
        }

        private static void AddRandomData(DateTime timestamp)
        {
            int dataType = _random.Next(3);
            string id = IdPool[_random.Next(IdPool.Length)];

            if (dataType == 0)
            {
                byte[] imageData = new byte[10];
                _random.NextBytes(imageData);
                _imageStore.AddData(timestamp, id, imageData);
                Console.WriteLine($"[{timestamp}] ➕ Added 📸 Image[{id}]");
            }
            else if (dataType == 1)
            {
                var prices = new Dictionary<string, double?>
                {
                    { "StockA", _random.NextDouble() * 100 },
                    { "StockB", _random.NextDouble() * 200 },
                    { "StockC", _random.NextDouble() * 150 }
                };
                _priceStore.AddData(timestamp, id, prices);
                Console.WriteLine($"[{timestamp}] ➕ Added 💰 Prices[{id}]");
            }
            else
            {
                string jsonData = $"{{ \"value\": {_random.Next(1, 100)} }}";
                _jsonStore.AddData(timestamp, id, jsonData);
                Console.WriteLine($"[{timestamp}] ➕ Added 📜 JSON[{id}]");
            }
        }
    }
}