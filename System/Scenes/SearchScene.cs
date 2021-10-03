using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Logic;
using GameJAM_Devtober2021.System.Models;
using GameJAM_Devtober2021.System.Models.UI;
using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;

using DH = GameJAM_Devtober2021.System.Utils.DisplayHelper;
using LANG = GameJAM_Devtober2021.System.Utils.LanguageHelper;

namespace GameJAM_Devtober2021.System.Scenes {
    public class SearchScene : SceneBase {

        private ConfigController _config;
        private ContentController _content;
        private InputController _input;
        private SceneController _scene;
        private CameraController _camera;
        private LevelController _level;

        private SpriteFont _fontRegular => _content.GetFont(FontType.Regular);
        private SpriteFont _fontConsole => _content.GetFont(FontType.Console);

        // Time
        private Timer _timer;
        private int _timeLeft;
        private int _timeLeftSeconds;
        private int _timeLeftMinutes;

        // Selection
        private int _mouseX;
        private int _mouseY;

        // Render targets
        private RenderTarget2D _sceneUI;
        private RenderTarget2D _sceneCore;

        // Selected object
        private ObjectModel _selectedObject;
        private TextBubble _UISelectedObjectBubble;

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
            _camera.SetOffset(0, 0);
            _camera.SetOffset(_config.WindowWidth / 2, _config.WindowHeight / 2);

            _level = new LevelController(_content);
            _level.LoadLevel(_camera, 0);
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
            _selectedObject = null;

            _input.Update( );
            _camera.Update( );

            if (_input.IsRMBPressed( )) _camera.LookBy(_input.MouseDiffX, _input.MouseDiffY);
            if (_input.HasScrolledUp( )) _camera.ZoomIn(0.25f);
            if (_input.HasScrolledDown( )) _camera.ZoomOut(0.25f);

            // Update mouse position
            _mouseX = (int)((_input.MouseX - _camera.Offset.X) / _camera.Scale + _camera.Target.X);
            _mouseY = (int)((_input.MouseY - _camera.Offset.Y) / _camera.Scale - _camera.Target.Y);

            // Update object selection
            ObjectModel selection = null;
            _level.Objects.ForEach(obj => {
                Rectangle source = obj.Texture.TextureData.GetSource(obj.SkinID);

                if (_mouseX >= obj.X && _mouseX <= obj.X + source.Width &&
                    _mouseY >= -obj.Y && _mouseY <= -obj.Y + source.Height) {
                    selection = obj;
                    return;
                }
            });

            if (selection != null && selection != _selectedObject) {
                _selectedObject = selection;
                _UISelectedObjectBubble = new TextBubble(_content, LANG.Get(selection.DataModel.Name), selection.X + 5, -selection.Y + 5);
            } else if (selection == null) {
                _selectedObject = null;
                _UISelectedObjectBubble = null;
            }
        }

        private void RenderCoreScene(GameTime time) {
            if (!_level.IsLoaded) {
                return;
            }

            // Display level
            DH.Raw(_content.TEXLevel, 0, 0, align: AlignType.LB);

            // Display objects
            _level.Objects.ForEach(obj => {
                DH.LevelObject(obj);

                if (_selectedObject == obj) {
                    DH.LevelObject(obj, 1);
                }
            });
        }

        private void RenderUIScene(GameTime time) {
            if (_config.IsDebugMode) {
                string objectHovered = _selectedObject == null ? "---" : LanguageHelper.Get(_selectedObject.DataModel.Name);

                DH.Text(_fontConsole, $"Camera Position ({_camera.Target.X:0}, {_camera.Target.Y:0})", 10, 10);
                DH.Text(_fontConsole, $"Camera Zoom ({_camera.Scale:0.00}x)", 10, 25);
                DH.Text(_fontConsole, $"Mouse Postiion ({_mouseX}, {-_mouseY})", 10, 40);
                DH.Text(_fontConsole, $"Object hover ({objectHovered})", 10, 55);
            }

            DH.Text(_fontRegular, $"{_timeLeftMinutes:00}:{_timeLeftSeconds:00}", _config.WindowWidth / 2, 15, align: AlignType.CT);

            // Display text bubble with selected object's name
            // TODO FIX POSITIONING
            _UISelectedObjectBubble?.Display( );
        }

        public override void Display(GameTime time) {
            DH.Scene(_sceneCore, Color.Transparent, _camera.Matrix, ( ) => RenderCoreScene(time));
            DH.Scene(_sceneUI, Color.Transparent, null, ( ) => RenderUIScene(time));
            DH.Scene(null, Color.Black, null, ( ) => {
                DH.Raw(_sceneCore);
                DH.Raw(_sceneUI);
            });
        }

    }
}
