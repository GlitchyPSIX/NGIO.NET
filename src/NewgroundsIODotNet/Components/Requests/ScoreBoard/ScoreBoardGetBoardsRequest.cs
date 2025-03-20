using NewgroundsIODotNet.Components.Interfaces;
using System.Collections.Generic;

namespace NewgroundsIODotNet.Components.Requests.ScoreBoard {
    public class ScoreBoardGetBoardsRequest : INgioComponentRequest {
        public string Component => "ScoreBoard.getBoards";
        public Dictionary<string, object> Parameters => null;
        public object Echo { get; set; }
        public bool RequiresSecureCall => false;
    }
}