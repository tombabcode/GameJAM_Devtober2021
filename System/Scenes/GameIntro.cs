using GameJAM_Devtober2021.System.Controllers;
using Microsoft.Xna.Framework;

using DH = GameJAM_Devtober2021.System.Utils.DisplayHelper;

namespace GameJAM_Devtober2021.System.Scenes {
    public class GameIntro : SceneBase {

        private ConfigController _config;
        private ContentController _content;
        private InputController _input;
        private SceneController _scene;

        public GameIntro(ConfigController config, ContentController content, InputController input, SceneController scene) : base("GameIntro") {
            _config = config;
            _content = content;
            _input = input;
            _scene = scene;
        }

        public override void OnLoad( ) {
            base.OnLoad( );

            _scene.ChangeScene(Types.SceneType.Search);
        }

        public override void Update(GameTime time) {
            _input.Update( );
        }

        public override void Display(GameTime time) {
            DH.Scene(null, Color.Black, null, ( ) => {
                    
            });
        }

    }
}
