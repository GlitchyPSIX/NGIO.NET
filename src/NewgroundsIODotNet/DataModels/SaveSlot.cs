using System;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    public struct SaveSlot {
        [JsonProperty("datetime")]
        public DateTime? DateTime { get; }

        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("size")]
        public int Size { get; }

        [JsonProperty("timestamp")]
        public uint Timestamp { get; }

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