using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;

using DH = GameJAM_Devtober2021.System.Utils.DisplayHelper;

namespace GameJAM_Devtober2021.System.Scenes {
    public class SearchScene : SceneBase {

        private ConfigController _config;
        private ContentController _content;
        private InputController _input;
        private SceneController _scene;
        private CameraController _camera;

        private SpriteFont _fontRegular => _content.GetFont(FontType.Regular);
        private SpriteFont _fontConsole => _content.GetFont(FontType.Console);

        private Timer _timer;
        private int _timeLeft;
        private int _timeLeftSeconds;
        private int _timeLeftMinutes;

        private RenderTarget2D _sceneUI;
        private RenderTarget2D _sceneCore;

        public SearchScene(ConfigController config, ContentController content, InputController input, SceneController scene) : base("Search") {
            _config = config;
            _content = content;
            _input = input;
            _scene = scene;
        }

        public override void OnLoad( ) {
            base.OnLoad( );

            _sceneUI = new RenderTarget2D(_content.Device, _config.WindowWidth, _config.WindowHeight);
            _sceneCore = new RenderTarget2D(_content.Device, _config.WindowWidth, _config.WindowHeight);

            _camera = new CameraController( );
            _camera.SetOffset(_config.WindowWidth / 2, _config.WindowHeight / 2);
        }

        public override void OnShow( ) {
            base.OnShow( );

            // Start the timer
            _timeLeft = 5;
            _timeLeftMinutes = (int)Math.Floor(_timeLeft / 60f);
            _timeLeftSeconds = _timeLeft % 60;
            _timer = new Timer((object state) => {
                if (--_timeLeft < 0) {
                    OnTimerFinish( );
                    _timer.Dispose( );
                    return;
                }

                _timeLeftMinutes = (int)Math.Floor(_timeLeft / 60f);
                _timeLeftSeconds = _timeLeft % 60;
            }, null, Timeout.Infinite, 1000);
            // _timer.Change(0, 1000);
        }

        private void OnTimerFinish( ) {
            Logger.Info("Timer in SearchScene has finished");
        }

        public override void Update(GameTime time) {
            _input.Update( );
            _camera.Update( );

            if (_input.IsRMBPressed( )) {
                _camera.LookBy(_input.MouseDiffX, _input.MouseDiffY);
            }
        }

        private void RenderCoreScene(GameTime time) {
            DH.Texture(_content.TEXPixel, 0, 0, 50, 50, color: Color.Red, align: AlignType.CM);
        }

        private void RenderUIScene(GameTime time) {
            if (_config.IsDebugMode) {
                DH.Text(_fontConsole, $"Camera ({_camera.Target.X:0}, {_camera.Target.Y:0})", 10, 10);
            }

            DH.Text(_fontRegular, $"{_timeLeftMinutes:00}:{_timeLeftSeconds:00}", _config.WindowWidth / 2, 15, align: AlignType.CT);
        }

        public override void Display(GameTime time) {
            DH.Scene(_sceneCore, Color.Transparent, _camera.Matrix, ( ) => RenderCoreScene(time));
            DH.Scene(_sceneUI, Color.Transparent, null, ( ) => RenderUIScene(time));
            DH.Scene(null, Color.Black, null, ( ) => {
                DH.Texture(_sceneCore);
                DH.Texture(_sceneUI);
            });
        }

    }
}
