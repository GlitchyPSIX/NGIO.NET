using System;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    /// <summary>
    /// Represents a Cloud Save slot.
    /// </summary>
    public struct SaveSlot {
        /// <summary>
        /// Last time this Slot was modified
        /// </summary>
        [JsonProperty("datetime")]
        public DateTime? DateTime { get; }

        /// <summary>
        /// ID of this Cloud Save slot
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; }

        /// <summary>
        /// Size in bytes of the slot
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; }

        /// <summary>
        /// Timestamp of last modification
        /// </summary>
        [JsonProperty("timestamp")]
        public uint Timestamp { get; }

        /// <summary>
        /// URL to fetch the Slot data from
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; }

        [JsonConstructor]
        public SaveSlot(DateTime? dateTime, int id, int size, uint timestamp, string url) {
            DateTime = dateTime;
            Id = id;
            Size = size;
            Timestamp = timestamp;
            Url = url;
        }

        public override string ToString() {
            return $"< Save Slot # {Id} [{Size} bytes] - last saved {DateTime} >";
        }
    }
}