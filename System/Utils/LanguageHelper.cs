using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameJAM_Devtober2021.System.Utils {
    public static class LanguageHelper {

        public static string CurrentLanguage { get; private set; }

        public static Dictionary<string, string> Translations { get; private set; }

        public static string Get(string key) {
            return Translations.TryGetValue(key, out string res) ? res : "???";
        }

        public static void LoadLanguage(string lang) {
            string path = Path.Combine("Language", $"lang_{lang}.json");

            if (!Directory.Exists("Language") || !File.Exists(path)) {
                CurrentLanguage = string.Empty;
                Logger.Error($"Failed to load language with tag [{lang}]");
                return;
            }

            CurrentLanguage = lang;
            string data = File.ReadAllText(path);
            Dictionary<string, string> translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

            Translations = translations;
            Logger.Info($"Loaded [{lang}] translations");
        }

    }
}
