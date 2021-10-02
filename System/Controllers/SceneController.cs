using GameJAM_Devtober2021.System.Scenes;
using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameJAM_Devtober2021.System.Controllers {
    public class SceneController {

        private Dictionary<SceneType, SceneBase> _scenes;
        public SceneType CurrentScene { get; private set; }

        public void Initialize(ConfigController config, ContentController content, InputController input) {
            _scenes = new Dictionary<SceneType, SceneBase>( ) {
                { SceneType.Gameplay, new GameplayScene(config, content, input, this) },
                { SceneType.Combat, new CombatScene(config, content, input, this) },
                { SceneType.Search, new SearchScene(config, content, input, this) }
            };

            Logger.Info("Scene controller initialized");
            ChangeScene(SceneType.Search);
        }

        public void ChangeScene(SceneType type) {
            if (CurrentScene == type) {
                return;
            }

            // Hide event
            GetCurrentScene( ).OnHide( );

            // Change current scene
            CurrentScene = type;
            SceneBase scene = GetCurrentScene( );
            
            // On load
            if (!scene.IsLoaded) {
                scene.OnLoad( );
            }

            // On show
            scene.OnShow( );
        }

        public SceneBase GetCurrentScene( ) {
            return _scenes.TryGetValue(CurrentScene, out SceneBase res) ? res : null;
        }

        public void Update(GameTime time) {
            GetCurrentScene( )?.Update(time);
        }

        public void Display(GameTime time) {
            GetCurrentScene( )?.Display(time);
        }

    }
}
