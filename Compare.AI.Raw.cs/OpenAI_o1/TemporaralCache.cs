using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI.o1.TemporalAsOfShowcase
{
    /// <summary>
    /// An in-memory store of time-based data, keyed by ID.
    /// </summary>
    public class TemporaralCache
    {
        // Key = ID, Value = a list of DataRecords sorted by Timestamp ascending.
        private Dictionary<string, List<DataRecord>> _store 
            = new Dictionary<string, List<DataRecord>>();

        /// <summary>
        /// Add a new data record to the store, preserving chronological order.
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
        /// Returns null if no record is found in that time range or ID doesn't exist.
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
        /// If ID doesn't exist, returns empty.
        /// </summary>
        public IEnumerable<DataRecord> GetDataDelta(string id, DateTime start, DateTime end)
        {
            if (!_store.ContainsKey(id))
                return Enumerable.Empty<DataRecord>();

            var list = _store[id];
            return list.Where(r => r.Timestamp >= start && r.Timestamp <= end);
        }
    }
}
