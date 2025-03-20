using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Interfaces {
    public interface INgioSessionResponse {
        [JsonIgnore]
        Session? Session { get; }
    }
}