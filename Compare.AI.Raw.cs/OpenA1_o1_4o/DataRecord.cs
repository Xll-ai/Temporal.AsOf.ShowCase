

using System;
using System.ComponentModel.DataAnnotations;

namespace OpenAI.o1.OpenAI4o.TemporalAsOfShowcase
{
    /// <summary>
    /// A single data record with timestamp, ID, type, and the data payload (object).
    /// </summary>
    public class DataRecord
    {
        public DateTime Timestamp { get; set; }
        public string ID { get; set; }
        public DataType DataType { get; set; }
        public object Value { get; set; }
    }
}
