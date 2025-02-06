using Compare.AI.Raw.cs;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenAI.o1.OpenAI4o.TemporalAsOfShowcase
{
    class Program_OpenAI_o1_4o
    {
        static Random _random = new Random();

        public static void Main_OpenAI_o1_4o(string[] args)
        {
            Console.WriteLine(Banner.Generate("OpenAI o1 o4"));
            // 1) Initialize our store and simulation parameters
            var store = new TemporaralCache();

            // Start "simulation time" at midnight (today)
            DateTime startTime = DateTime.Today;
            DateTime currentTime = startTime;

            // We want to simulate 5 minutes from midnight
            DateTime endTime = startTime.AddMinutes(5);

            // We'll keep separate ID pools for each data type
            // so we can re-use or create new IDs randomly.
            List<string> imageIDs = new List<string>();
            List<string> priceIDs = new List<string>();
            List<string> jsonIDs = new List<string>();

            // 2) Run the simulation loop until we exceed 5 minutes
            while (currentTime <= endTime)
            {
                // Move time forward by 5-10 seconds
                int incrementSeconds = _random.Next(5, 11); // [5..10]
                currentTime = currentTime.AddSeconds(incrementSeconds);

                if (currentTime > endTime)
                {
                    // If we surpass endTime, break out.
                    break;
                }

                // 3) Generate a random data record
                var record = GenerateRandomDataRecord(currentTime, imageIDs, priceIDs, jsonIDs);

                // 4) Store it
                store.AddData(record);

                Console.WriteLine($"[{currentTime:HH:mm:ss}] Added {record.DataType} record for ID='{record.ID}'");

                // 5) Occasionally, we demonstrate a random read (GetData or GetDataDelta)
                //    We'll do this, say, with a 30% chance each iteration
                if (_random.NextDouble() < 0.3)
                {
                    DoRandomReadDemo(store, currentTime, imageIDs, priceIDs, jsonIDs);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Simulation complete. Final read demos...");

            // Do a few final random reads
            for (int i = 0; i < 3; i++)
            {
                DoRandomReadDemo(store, endTime, imageIDs, priceIDs, jsonIDs);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Generates a random DataRecord at the given time:
        ///   - 1/3 chance of Image, 1/3 of Prices, 1/3 of Json
        ///   - 50/50 chance new ID or existing ID
        /// </summary>
        private static DataRecord GenerateRandomDataRecord(
            DateTime timestamp,
            List<string> imageIDs,
            List<string> priceIDs,
            List<string> jsonIDs)
        {
            // 1/3 chance for each data type
            var roll = _random.Next(3);
            DataType type = (DataType)roll;

            // Will we pick a new ID or re-use an existing one?
            bool pickExistingId = (_random.NextDouble() < 0.5);

            string chosenID;
            object dataValue;

            switch (type)
            {
                case DataType.Image:
                    chosenID = pickExistingId && imageIDs.Count > 0
                        ? imageIDs[_random.Next(imageIDs.Count)]
                        : "Image_" + Guid.NewGuid().ToString().Substring(0, 8);

                    // If it's new, remember it
                    if (!imageIDs.Contains(chosenID))
                        imageIDs.Add(chosenID);

                    dataValue = GenerateRandomImage();
                    break;

                case DataType.Prices:
                    chosenID = pickExistingId && priceIDs.Count > 0
                        ? priceIDs[_random.Next(priceIDs.Count)]
                        : "Prices_" + Guid.NewGuid().ToString().Substring(0, 8);

                    if (!priceIDs.Contains(chosenID))
                        priceIDs.Add(chosenID);

                    dataValue = GenerateRandomPrices();
                    break;

                default: // DataType.Json
                    chosenID = pickExistingId && jsonIDs.Count > 0
                        ? jsonIDs[_random.Next(jsonIDs.Count)]
                        : "Json_" + Guid.NewGuid().ToString().Substring(0, 8);

                    if (!jsonIDs.Contains(chosenID))
                        jsonIDs.Add(chosenID);

                    dataValue = GenerateRandomJson();
                    break;
            }

            return new DataRecord
            {
                Timestamp = timestamp,
                ID = chosenID,
                DataType = type,
                Value = dataValue
            };
        }

        /// <summary>
        /// Simulates random "image" bytes.
        /// </summary>
        private static byte[] GenerateRandomImage()
        {
            // For demonstration, just fill with random bytes.
            int size = _random.Next(5, 15); // smallish
            byte[] data = new byte[size];
            _random.NextBytes(data);
            return data;
        }

        /// <summary>
        /// Simulates random price dictionary (symbol -> price).
        /// </summary>
        private static Dictionary<string, double?> GenerateRandomPrices()
        {
            var dict = new Dictionary<string, double?>();
            // We'll pick a random set of "symbols"
            string[] possibleSymbols = { "AAPL", "GOOG", "TSLA", "MSFT" };
            int count = _random.Next(1, possibleSymbols.Length + 1);

            // We'll choose 'count' distinct symbols at random
            var chosenSymbols = possibleSymbols.OrderBy(x => _random.Next()).Take(count).ToList();

            foreach (var symbol in chosenSymbols)
            {
                // 10% chance we store null
                if (_random.NextDouble() < 0.1)
                {
                    dict[symbol] = null;
                }
                else
                {
                    // random price in [100..999]
                    dict[symbol] = Math.Round(100 + _random.NextDouble() * 900, 2);
                }
            }

            return dict;
        }

        /// <summary>
        /// Simulates random JSON text (just a dummy structure).
        /// </summary>
        private static string GenerateRandomJson()
        {
            // Build a simple JSON with random values
            // E.g.: { "id": "abc", "value": 123, "ok": true }
            string idValue = Guid.NewGuid().ToString().Substring(0, 4);
            double number = Math.Round(_random.NextDouble() * 100, 2);
            bool boolean = (_random.Next(2) == 0);

            // Very naive JSON:
            return $"{{\"id\":\"{idValue}\", \"value\":{number}, \"ok\":{boolean.ToString().ToLowerInvariant()}}}";
        }

        /// <summary>
        /// Demonstrates a random read: either GetData(asOf) or GetDataDelta(asOfStart, asOfEnd).
        /// Picks a random ID from existing ID lists, if available.
        /// </summary>
        private static void DoRandomReadDemo(
            TemporaralCache store,
            DateTime currentTime,
            List<string> imageIDs,
            List<string> priceIDs,
            List<string> jsonIDs)
        {
            // We must pick a random ID from any of the known IDs
            var allIDs = new List<string>();
            allIDs.AddRange(imageIDs);
            allIDs.AddRange(priceIDs);
            allIDs.AddRange(jsonIDs);

            if (allIDs.Count == 0)
            {
                // No IDs yet, skip
                return;
            }

            string randomID = allIDs[_random.Next(allIDs.Count)];

            // 50% chance we do GetData, 50% chance we do GetDataDelta
            bool doGetData = (_random.NextDouble() < 0.5);

            if (doGetData)
            {
                // We'll pick a random time between startTime and currentTime
                var randomAsOf = GetRandomTimeBetween(DateTime.Today, currentTime);

                var record = store.GetData(randomID, randomAsOf);
                if (record == null)
                {
                    Console.WriteLine($"[READ] GetData(ID='{randomID}', asOf={randomAsOf:HH:mm:ss}) -> No record found");
                }
                else
                {
                    Console.WriteLine($"[READ] GetData(ID='{randomID}', asOf={randomAsOf:HH:mm:ss}) -> Found {record.DataType}");
                }
            }
            else
            {
                // We'll pick random start/end between startTime and currentTime
                var t1 = GetRandomTimeBetween(DateTime.Today, currentTime);
                var t2 = GetRandomTimeBetween(DateTime.Today, currentTime);

                var start = (t1 < t2) ? t1 : t2;
                var end = (t1 < t2) ? t2 : t1;

                var records = store.GetDataDelta(randomID, start, end).ToList();
                Console.WriteLine($"[READ] GetDataDelta(ID='{randomID}', [{start:HH:mm:ss}, {end:HH:mm:ss}]) -> {records.Count} records");
            }
        }

        /// <summary>
        /// Returns a random time between 'start' and 'end' (inclusive).
        /// </summary>
        private static DateTime GetRandomTimeBetween(DateTime start, DateTime end)
        {
            if (start >= end) return start;

            var rangeSeconds = (end - start).TotalSeconds;
            var randomOffset = _random.NextDouble() * rangeSeconds;
            return start.AddSeconds(randomOffset);
        }
    }
}
