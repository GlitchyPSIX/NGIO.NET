using NewgroundsIODotNet.Components.Interfaces;
using System.Collections.Generic;

namespace NewgroundsIODotNet.Components.Requests.ScoreBoard {
    /// <summary>
    /// Request to obtain the ScoreBoards of the current game.
    /// </summary>
    /// <remarks>This component has no parameters.</remarks>
    public class ScoreBoardGetBoardsRequest : INgioComponentRequest {
        public string Component => "ScoreBoard.getBoards";
        public Dictionary<string, object> Parameters => null;
        public object Echo { get; set; }
        public bool RequiresSecureCall => false;
    }
}