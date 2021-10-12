using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Models.UI;
using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DH = GameJAM_Devtober2021.System.Utils.DisplayHelper;
using LANG = GameJAM_Devtober2021.System.Utils.LanguageHelper;

namespace GameJAM_Devtober2021.System.Scenes {
    public sealed class MainMenuScene : SceneBase {

        private ConfigController _config;
        private ContentController _content;
        private InputController _input;
        private SceneController _scene;

        private RenderTarget2D _sceneCore;

        private UIElement[] _ui = new UIElement[0];

        public MainMenuScene(ConfigController config, ContentController content, InputController input, SceneController scene) : base("MainMenu") {
            _config = config;
            _content = content;
            _input = input;
            _scene = scene;
        }

        public override void OnLoad( ) {
            base.OnLoad( );

            _sceneCore = new RenderTarget2D(_content.Device, _config.ViewWidth, _config.ViewHeight);

            // TODO
            // - Maybe ID or something to easy select objects, instead of e.g. _ui[29]
            // - Change Button to TextButton and base Button.
            // - Add possibility to set buttons' width and height
            _ui = new UIElement[] {
                new Button(_config, _input, _content) {
                    X = _config.ViewWidth / 2,
                    Y = _config.ViewHeight / 2,
                    Align = AlignType.CM,
                    Font = FontType.Regular,
                    Text = LANG.Get("btn_newgame"),
                    TextColor = Color.Gray,
                    OnHover = btn => OnButtonHover(btn),
                    OnUnhover = btn => btn.TextColor = Color.Gray,
                    OnClick = btn => _scene.ChangeScene(SceneType.Gameplay),
                    PaddingX = 50,
                    PaddingY = 5
                },
                new Button(_config, _input, _content) {
                    X = _config.ViewWidth / 2,
                    Y = _config.ViewHeight / 2 + 45f,
                    Align = AlignType.CM,
                    Font = FontType.Regular,
                    Text = LANG.Get("btn_settings"),
                    TextColor = Color.Gray,
                    OnHover = btn => OnButtonHover(btn),
                    OnUnhover = btn => btn.TextColor = Color.Gray,
                    PaddingX = 50,
                    PaddingY = 5
                },
                new Button(_config, _input, _content) {
                    X = _config.ViewWidth / 2,
                    Y = _config.ViewHeight / 2 + 90,
                    Align = AlignType.CM,
                    Font = FontType.Regular,
                    Text = LANG.Get("btn_exit"),
                    TextColor = Color.Gray,
                    OnHover = btn => OnButtonHover(btn),
                    OnUnhover = btn => btn.TextColor = Color.Gray,
                    OnClick = btn => Core.OnExit( ),
                    PaddingX = 50,
                    PaddingY = 5
                }
            };

            foreach (UIElement ui in _ui)
                ui.Refresh( );

            AudioHelper.PlayOnce(_content.MUSICMenu, "main_menu_music", true);
        }

        public override void Update(GameTime time) {
            _input.Update( );

            foreach (UIElement ui in _ui)
                ui.Update(time);
        }

        private void RenderCoreScene (GameTime time) {
            foreach (UIElement ui in _ui)
                ui.Display(time);
        }

        public override void Display(GameTime time) {
            DH.Scene(_sceneCore, Color.Transparent, null, ( ) => RenderCoreScene(time));
            DH.Scene(null, Color.Black, null, ( ) => {
                DH.DisplayScene(_sceneCore, _config);
            });
        }

        private void OnButtonHover (Button button) {
            AudioHelper.PlayMultiple(_content.SOUNDMouseHover, "mouse_hover_effect");
            button.TextColor = Color.White;
        }

    }
}
