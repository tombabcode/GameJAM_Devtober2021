using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Models.UI;
using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

        private bool _isAnimating;
        private float _sceneAlpha = 0;

        public MainMenuScene(ConfigController config, ContentController content, InputController input, SceneController scene) : base("MainMenu") {
            _config = config;
            _content = content;
            _input = input;
            _scene = scene;
        }

        public override void OnLoad( ) {
            base.OnLoad( );

            _sceneCore = new RenderTarget2D(_content.Device, _config.WindowWidth, _config.WindowHeight);

            // TODO
            // - Maybe ID or something to easy select objects, instead of e.g. _ui[29]
            // - Change Button to TextButton and base Button.
            // - Add possibility to set buttons' width and height
            _ui = new UIElement[] {
                new Button(_config, _input, _content) {
                    X = _config.WindowWidth / 2,
                    Y = _config.WindowHeight / 2,
                    Align = AlignType.CM,
                    Font = FontType.TextRegularB,
                    Text = LANG.Get("btn_play"),
                    TextColor = new Color(Color.Gray, 0),
                    OnHover = btn => OnButtonHover(btn),
                    OnUnhover = btn => OnButtonUnhover(btn),
                    OnClick = btn => OnButtonAction(btn, "play"),
                    PaddingX = 50,
                    PaddingY = 5
                },
                new Button(_config, _input, _content) {
                    X = _config.WindowWidth / 2,
                    Y = _config.WindowHeight / 2 + 50,
                    Align = AlignType.CM,
                    Font = FontType.TextRegularB,
                    Text = LANG.Get("btn_settings"),
                    TextColor = new Color(Color.Gray, 0),
                    OnHover = btn => OnButtonHover(btn),
                    OnUnhover = btn => OnButtonUnhover(btn),
                    PaddingX = 50,
                    PaddingY = 5
                },
                new Button(_config, _input, _content) {
                    X = _config.WindowWidth / 2,
                    Y = _config.WindowHeight / 2 + 100,
                    Align = AlignType.CM,
                    Font = FontType.TextRegularB,
                    Text = LANG.Get("btn_exit"),
                    TextColor = new Color(Color.Gray, 0),
                    OnHover = btn => OnButtonHover(btn),
                    OnUnhover = btn => OnButtonUnhover(btn),
                    OnClick = btn => Core.OnExit( ),
                    PaddingX = 50,
                    PaddingY = 5
                }
            };

            foreach (UIElement ui in _ui)
                ui.Refresh( );

            AudioHelper.PlayOnce(_content.MUSICMenu, "main_menu_music", true);
            AudioHelper.SetVolume(0);
            _ = AnimationHelper.Add(0, 1, 600, ease: EaseType.CubicIn, onUpdate: v => AudioHelper.SetVolume(_config.Volume * (float)v.Current));
        }

        public override void OnShow( ) {
            base.OnShow( );

            _isAnimating = true;

            _ = AnimationHelper.Add(0, 1, 600,
                onUpdate: v => _sceneAlpha = (float)v.Current,
                onComplete: _ => _isAnimating = false
            );
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
                DH.DisplayScene(_sceneCore, Color.White * _sceneAlpha);
            });
        }

        private void OnButtonUnhover(Button button) {
            button.TextColor = Color.Gray;
        }

        private void OnButtonHover (Button button) {
            if (!_isAnimating)
                AudioHelper.PlayMultiple(_content.SOUNDMouseHover, "mouse_hover_effect");
            button.TextColor = Color.White;
        }

        private void OnButtonAction(Button button, string id) {
            if (_isAnimating) {
                return;
            }

            _isAnimating = true;
            _ = AnimationHelper.Add(1, 0, 600,
                onUpdate: v => _sceneAlpha = (float)v.Current,
                onComplete: model => _scene.ChangeScene(SceneType.GameIntro)
            );
            _ = AnimationHelper.Add(1, 0, 600, ease: EaseType.CubicOut, onUpdate: v => AudioHelper.SetVolume(_config.Volume * (float)v.Current));
        }

    }
}
