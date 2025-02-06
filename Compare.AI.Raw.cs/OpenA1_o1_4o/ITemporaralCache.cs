using System;
using System.Collections.Generic;

namespace OpenAI.o1.OpenAI4o.TemporalAsOfShowcase
{
    /// <summary>
    /// Represents a cache or store that can retrieve data "as of" specific times,
    /// as well as return deltas (changes) over a time range.
    /// </summary>
    public interface ITemporaralCache
    {
        /// <summary>
        /// Returns the record for the given <paramref name="id"/> as of the specified time.
        /// If none is found at or before that time, returns null (or default).
        /// </summary>
        DataRecord GetData(string id, DateTime asOf);

        /// <summary>
        /// Returns all records (changes) for the given <paramref name="id"/> in the 
        /// [<paramref name="start"/>, <paramref name="end"/>] time range.
        /// </summary>
        IEnumerable<DataRecord> GetDataDelta(string id, DateTime start, DateTime end);

        /// <summary>
        /// Returns, for each ID in <paramref name="ids"/>, the single record as of the specified time.
        /// If none is found for that ID at or before the time, it may be omitted or null,
        /// depending on the implementation.
        /// </summary>
        IEnumerable<DataRecord> GetDataList(IEnumerable<string> ids, DateTime asOf);

        /// <summary>
        /// For each ID in <paramref name="ids"/>, returns a tuple containing 
        /// the first and last records within [<paramref name="start"/>, <paramref name="end"/>].
        /// 
        /// (You could alter this method to return all changes, or another format, 
        /// depending on your needs.)
        /// </summary>
        IEnumerable<(DataRecord StartRecord, DataRecord EndRecord)> GetDataDeltaList(
            IEnumerable<string> ids,
            DateTime start,
            DateTime end
        );
    }


}
