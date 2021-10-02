using GameJAM_Devtober2021.System.Controllers;
using Microsoft.Xna.Framework;

using DH = GameJAM_Devtober2021.System.Utils.DisplayHelper;

namespace GameJAM_Devtober2021.System.Scenes {
    public class CombatScene : SceneBase {

        private ConfigController _config;
        private ContentController _content;
        private InputController _input;
        private SceneController _scene;

        public CombatScene(ConfigController config, ContentController content, InputController input, SceneController scene) : base("Combat") {
            _config = config;
            _content = content;
            _input = input;
            _scene = scene;
        }

        public override void Update(GameTime time) {
            _input.Update( );
        }

        public override void Display(GameTime time) {
            DH.Scene(null, Color.Blue, null, ( ) => {

            });
        }

    }
}
