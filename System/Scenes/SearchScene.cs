using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Logic;
using GameJAM_Devtober2021.System.Logic.Items;
using GameJAM_Devtober2021.System.Logic.Objects;
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
        private ObjectInstance _objectHovered;
        private ObjectInstance _objectSelected;

        // Main item that will fight
        private ItemInstance _primaryItem;

        // Animations
        private bool _isAnimating = true;
        private float _sceneAlpha = 0;

        private Random _rand;

        public SearchScene(ConfigController config, ContentController content, InputController input, SceneController scene) : base("Search") {
            _config = config;
            _content = content;
            _input = input;
            _scene = scene;

            _rand = new Random( );
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

            _isAnimating = true;

            // Start the timer
            _timeLeft = 180;
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
            _timer.Change(0, 1000);

            _camera.LookAt(_level.LevelModel.LevelSpawnX, _level.LevelModel.LevelSpawnY);

            // Animate fade-in
            AnimationHelper.Add(0, 1, 600, onUpdate: v => _sceneAlpha = (float)v.Current, onComplete: _ => _isAnimating = false);
        }

        private void OnTimerFinish( ) {
            Logger.Info("Timer in SearchScene has finished");
        }

        public override void Update(GameTime time) {
            _input.Update( );
            _camera.Update( );

            if (_input.IsRMBPressed( )) _camera.LookBy(_input.MouseDiffX, _input.MouseDiffY);
            if (_input.HasScrolledUp( )) _camera.ZoomIn(0.25f);
            if (_input.HasScrolledDown( )) _camera.ZoomOut(0.25f);

            // Update mouse position
            _mouseX = (int)((_input.MouseX - _camera.Offset.X) / _camera.Scale + _camera.Target.X);
            _mouseY = (int)((_input.MouseY - _camera.Offset.Y) / _camera.Scale - _camera.Target.Y);

            // Update object selection
            ObjectInstance hover = null;
            foreach (ObjectInstance obj in _level.Objects) {
                Rectangle source = obj.Texture.TextureData.GetSource(obj.SkinID);

                if (_input.MouseX >= 0 && _input.MouseX <= _config.WindowWidth && _input.MouseY >= 0 && _input.MouseY <= _config.WindowHeight &&
                    _mouseX >= obj.X && _mouseX <= obj.X + source.Width && _mouseY >= -obj.Y && _mouseY <= -obj.Y + source.Height) 
                {
                    hover = obj;
                    break;
                }
            }

            // Set hovered object
            if (hover != null && hover != _objectHovered) {
                _objectHovered = hover;
            } else if (hover == null) {
                _objectHovered = null;
            }

            // Set selected object
            if (_objectHovered != null && _input.IsLMBPressedOnce( )  && _objectHovered.Items.Count > 0) {
                _objectSelected = _objectHovered;
                _scene.ChangeScene(SceneType.Combat);
                ((CombatScene)_scene.GetCurrentScene( )).SetActor(_objectSelected.Items[0]);
            }
        }

        private void RenderCoreScene(GameTime time) {
            if (!_level.IsLoaded) {
                return;
            }

            // Display level
            DH.Raw(_content.TEXLevel, 0, 0, align: AlignType.LB);

            // Display objects
            foreach (ObjectInstance obj in _level.Objects) {
                DH.Texture(obj.Texture, obj.X, -obj.Y);
            }

            // Display text bubble with selected object's name
            if (_objectHovered != null) {
                DH.Text(
                    _content.GetFont(FontType.RegularS), 
                    LANG.Get(_objectHovered.Data.Name), 
                    _objectHovered.X + _objectHovered.Width / 2,
                    5,
                    Color.White * 0.5f,
                    AlignType.CT
                );
            }
        }

        private void RenderUIScene(GameTime time) {
            // Show open container
            if (_objectSelected != null) {
                DH.Box(0, 0, _config.WindowWidth, _config.WindowHeight, new Color(0, 0, 0, 200));
                DH.Text(_content.GetFont(FontType.Regular), LANG.Get("ui_inventory"), _config.WindowWidth / 2, 45, align: AlignType.CT);

                int offset = (_objectSelected.Items.Count - 1) * 64 / 2;
                for (int i = 0; i < _objectSelected.Items.Count; i++) {
                    ItemInstance item = _objectSelected.Items[i];

                    DH.Raw(item.Texture.Get( ), _config.WindowWidth / 2 - offset, _config.WindowHeight / 2, align: AlignType.CM);
                }
            }

            if (_config.IsDebugMode) {
                string objectHovered = _objectHovered == null ? "---" : LANG.Get(_objectHovered.Data.Name);
                string objectSelected = _objectSelected == null ? "---" : LANG.Get(_objectSelected.Data.Name);

                DH.Text(_fontConsole, $"Camera Position ({_camera.Target.X:0}, {_camera.Target.Y:0})", 10, 10);
                DH.Text(_fontConsole, $"Camera Zoom ({_camera.Scale:0.00}x)", 10, 25);
                DH.Text(_fontConsole, $"Mouse Postiion ({_mouseX}, {-_mouseY})", 10, 40);
                DH.Text(_fontConsole, $"Object (hover) ({objectHovered})", 10, 55);
                DH.Text(_fontConsole, $"       (selection) ({objectSelected})", 10, 70);
            }

            DH.Text(_fontRegular, $"{_timeLeftMinutes:00}:{_timeLeftSeconds:00}", _config.WindowWidth / 2, 15, align: AlignType.CT);

        }

        public override void Display(GameTime time) {
            DH.Scene(_sceneCore, Color.Transparent, _camera.Matrix, ( ) => RenderCoreScene(time), _content.FXFilmGrain);
            DH.Scene(_sceneUI, Color.Transparent, null, ( ) => RenderUIScene(time));
            DH.Scene(null, Color.Black, null, ( ) => {
                DH.Raw(_sceneCore, color: Color.White * _sceneAlpha);
                DH.Raw(_sceneUI, color: Color.White * _sceneAlpha);
            });
        }

    }
}
