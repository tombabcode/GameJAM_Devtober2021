using GameJAM_Devtober2021.System.Utils;

namespace GameJAM_Devtober2021.System.Controllers {
    public class ConfigController {

        public const int DEF_WindowWidth = 800;
        public const int DEF_WindowHeight = 600;
        public const bool DEF_WindowFullscreen = false;

        public const bool DEF_DebugMode = true;

        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }
        public bool WindowFullscreen { get; private set; }

        public bool IsDebugMode { get; private set; }

        public void LoadConfig( ) {
            WindowWidth = DEF_WindowWidth;
            WindowHeight = DEF_WindowHeight;
            WindowFullscreen = DEF_WindowFullscreen;

            IsDebugMode = DEF_DebugMode;

            Logger.Info("Config loaded");
        }

    }
}
