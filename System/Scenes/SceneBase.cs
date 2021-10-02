using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using System;

namespace GameJAM_Devtober2021.System.Scenes {
    public abstract class SceneBase {

        public string SceneName { get; private set; }
        public bool IsLoaded { get; private set; }

        public SceneBase(string name) {
            SceneName = name;
        }

        public abstract void Update(GameTime time);
        public abstract void Display(GameTime time);

        public virtual void OnLoad( ) { 
            if (IsLoaded) {
                return;
            }

            IsLoaded = true;
            Logger.Info($"{SceneName}Scene on load");
        }

        public virtual void OnShow( ) {
            Logger.Info($"{SceneName}Scene on show");
        }

        public virtual void OnHide( ) {
            Logger.Info($"{SceneName}Scene on hide");
        }

    }
}
