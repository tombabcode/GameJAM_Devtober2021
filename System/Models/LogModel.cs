using GameJAM_Devtober2021.System.Types;
using System;

namespace GameJAM_Devtober2021.System.Models {
    public sealed class LogModel {

        public DateTime Time { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public LogType Type { get; set; }

    }
}
