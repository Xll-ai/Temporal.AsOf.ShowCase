using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI.o1.OpenAI4o.TemporalAsOfShowcase
{


    /// <summary>

    /// <summary>
    /// An in-memory store of time-based data, keyed by ID.
    /// </summary>
    public class TemporaralCache : ITemporaralCache
    {
        // Key = ID, Value = a list of DataRecords sorted by Timestamp ascending.
        private readonly Dictionary<string, List<DataRecord>> _store
            = new Dictionary<string, List<DataRecord>>();

        /// <summary>
        /// Adds a new data record to the store, preserving chronological order.
        /// </summary>
        public void AddData(DataRecord record)
        {
            if (!_store.ContainsKey(record.ID))
            {
                _store[record.ID] = new List<DataRecord>();
            }

            var list = _store[record.ID];
            list.Add(record);

            // Keep the list sorted by time, ascending.
            list.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
        }

        /// <summary>
        /// Gets the most recent DataRecord for the given ID at or before 'asOf'.
        /// Returns null if no record is found or ID doesn't exist.
        /// </summary>
        public DataRecord GetData(string id, DateTime asOf)
        {
            if (!_store.ContainsKey(id))
                return null;

            var list = _store[id];
            // We want the largest Timestamp <= asOf
            var candidate = list
                .Where(r => r.Timestamp <= asOf)
                .OrderByDescending(r => r.Timestamp)
                .FirstOrDefault();

            return candidate;
        }

        /// <summary>
        /// Returns all data records for the given ID in [start, end] time range.
        /// If ID doesn't exist, returns an empty enumeration.
        /// </summary>
        public IEnumerable<DataRecord> GetDataDelta(string id, DateTime start, DateTime end)
        {
            if (!_store.ContainsKey(id))
                return Enumerable.Empty<DataRecord>();

            var list = _store[id];
            return list.Where(r => r.Timestamp >= start && r.Timestamp <= end);
        }

        /// <summary>
        /// Returns, for each ID in the list, the DataRecord "as of" the specified time.
        /// If no record is found for that ID at or before that time, it is omitted.
        /// </summary>
        public IEnumerable<DataRecord> GetDataList(IEnumerable<string> ids, DateTime asOf)
        {
            var results = new List<DataRecord>();

            foreach (var id in ids)
            {
                var record = GetData(id, asOf);
                if (record != null)
                {
                    results.Add(record);
                }
            }

            return results;
        }

        /// <summary>
        /// For each ID, returns a tuple of (StartRecord, EndRecord) representing
        /// the first and last records within the [start, end] range.
        /// If no records are found in that range for an ID, returns (null, null).
        /// </summary>
        public IEnumerable<(DataRecord StartRecord, DataRecord EndRecord)> GetDataDeltaList(
            IEnumerable<string> ids,
            DateTime start,
            DateTime end)
        {
            var results = new List<(DataRecord, DataRecord)>();

            foreach (var id in ids)
            {
                // Get all records in [start, end] for this ID, sorted ascending
                var deltas = GetDataDelta(id, start, end)
                    .OrderBy(r => r.Timestamp)
                    .ToList();

                if (deltas.Count == 0)
                {
                    // No data in that range -> (null, null)
                    results.Add((null, null));
                    continue;
                }

                var first = deltas.First();
                var last = deltas.Last();
                results.Add((first, last));
            }

            return results;
        }
    }
}
