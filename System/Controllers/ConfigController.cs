using GameJAM_Devtober2021.System.Utils;
using System.IO;
using System.Text;

namespace GameJAM_Devtober2021.System.Controllers {
    public class ConfigController {

        public const int DEF_WindowWidth = 1280;
        public const int DEF_WindowHeight = 720;
        public const bool DEF_WindowFullscreen = false;
        public const float DEF_Volume = 0.5f;

        public const bool DEF_DebugMode = true;

        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }
        public bool WindowFullscreen { get; private set; }

        public float Volume { get; private set; } = DEF_Volume;

        public bool IsDebugMode { get; private set; }

        public void LoadConfig( ) {
            if (File.Exists("config.cfg")) {
                using (StreamReader sr = new StreamReader("config.cfg", Encoding.UTF8)) {
                    string line = string.Empty;
                    while ((line = sr.ReadLine( )) != null) {
                        string[] splitted = line.Split('=');

                        if (splitted.Length == 2) {
                            string key = splitted[0];
                            string value = splitted[1];

                            switch (key.ToLower( )) {
                                case "window_width": WindowWidth = int.TryParse(value, out int resWW) ? resWW : DEF_WindowWidth; break;
                                case "window_height": WindowHeight = int.TryParse(value, out int resWH) ? resWH : DEF_WindowHeight; break;
                                case "window_fullscreen": WindowFullscreen = bool.TryParse(value, out bool resF) ? resF : DEF_WindowFullscreen; break;
                                case "volume": Volume = float.TryParse(value, out float resV) ? resV : DEF_Volume; break;
                                case "debug": IsDebugMode = bool.TryParse(value, out bool resDM) ? resDM : DEF_DebugMode; break;
                            }
                        }
                    }
                    sr.Close( );
                }
            } else {
                WindowWidth = DEF_WindowWidth;
                WindowHeight = DEF_WindowHeight;
                WindowFullscreen = DEF_WindowFullscreen;
                IsDebugMode = DEF_DebugMode;

                SaveConfig( );
            }

            Logger.Info("Config loaded");
        }

        public void SaveConfig( ) {
            using (StreamWriter sw = new StreamWriter("config.cfg", false, Encoding.UTF8)) {
                sw.WriteLine("WINDOW_WIDTH=" + WindowWidth);
                sw.WriteLine("WINDOW_HEIGHT=" + WindowHeight);
                sw.WriteLine("WINDOW_FULLSCREEN=" + WindowFullscreen);
                sw.WriteLine("VOLUME=" + Volume);
                sw.WriteLine("DEBUG=" + IsDebugMode);
                sw.Close( );
            }
        }

    }
}
