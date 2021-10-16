using GameJAM_Devtober2021.System.Controllers;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace GameJAM_Devtober2021.System.Utils {
    public static class AudioHelper {

        public static ConfigController Config { private get; set; }
        public static Dictionary<string, SoundEffectInstance> Instances { get; private set; } = new Dictionary<string, SoundEffectInstance>( );

        public static void PlayOnce(SoundEffect audio, string id, bool loop = false, float? volume = null) {
            SoundEffectInstance instance = audio.CreateInstance( );
            instance.Volume = volume ?? Config.Volume;
            instance.IsLooped = loop;
            instance.Play( );

            Instances.Add(id, instance);
        }

        public static void PlayMultiple(SoundEffect audio, string id, bool loop = false, float? volume = null) {
            SoundEffectInstance instance = audio.CreateInstance( );
            instance.Volume = volume ?? Config.Volume;
            instance.IsLooped = loop;
            instance.Play( );

            int i = 0;
            string result = id;
            while (i < 64) {
                if (!Instances.ContainsKey(id + ++i)) {
                    result += i;
                    break;
                }
            }

            Instances.Add(result, instance);
        }

        public static void Pause(string id) {
            if (Instances.TryGetValue(id, out SoundEffectInstance instance)) {
                instance.Pause( );
            }
        }

        public static void Stop(string id) {
            if (Instances.TryGetValue(id, out SoundEffectInstance instance)) {
                instance.Stop( );
                instance.Dispose( );
            }
        }

        public static void StopAll( ) {
            foreach (KeyValuePair<string, SoundEffectInstance> kv in Instances) {
                kv.Value.Stop( );
                kv.Value.Dispose( );
            }
        }

        public static void Update( ) {
            List<string> toRemove = new List<string>( );
            foreach (KeyValuePair<string, SoundEffectInstance> kv in Instances) {
                SoundEffectInstance instance = kv.Value;
                if (instance == null || instance.IsDisposed == true || instance.State == SoundState.Stopped) {
                    toRemove.Add(kv.Key);
                }
            }

            foreach (string key in toRemove) {
                Instances.Remove(key);
            }
        }

        public static void SetVolume(float value) {
            foreach (KeyValuePair<string, SoundEffectInstance> kv in Instances) {
                SoundEffectInstance instance = kv.Value;
                if (instance != null)
                    instance.Volume = value;
            }
        }

    }
}
