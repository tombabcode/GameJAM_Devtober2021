using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Logic;
using GameJAM_Devtober2021.System.Logic.Objects;
using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DH = GameJAM_Devtober2021.System.Utils.DisplayHelper;
using LANG = GameJAM_Devtober2021.System.Utils.LanguageHelper;
using ANIM = GameJAM_Devtober2021.System.Utils.AnimationHelper;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using GameJAM_Devtober2021.System.Models;
using GameJAM_Devtober2021.System.Logic.Items;

namespace GameJAM_Devtober2021.System.Scenes {
    public class GameIntro : SceneBase {

        private ConfigController _config;
        private ContentController _content;
        private InputController _input;
        private SceneController _scene;
        private CameraController _camera;
        private LevelController _level;

        private OpenedObject _openedObject;

        // Render targets
        private RenderTarget2D _sceneUI;
        private RenderTarget2D _sceneCore;

        // Alpha
        private float _sceneAlpha = 0;
        private float _uiTextAlpha = 0;
        private float _levelAlpha = 0;
        private float _objectAlpha = 0;
        private float _continueTextAlpha = 0;

        // States
        private int _currentStage = 0;
        private bool _isAnimating = false;
        private bool _lockCamera = true;

        private bool _stage5Completed = false;
        private bool _stage8Completed = false;

        private bool _canSkip = false;

        // Hover
        ObjectInstance _levelObject = null;
        private bool _isObjectHovered = false;

        public GameIntro(ConfigController config, ContentController content, InputController input, SceneController scene) : base("GameIntro") {
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

            _openedObject = new OpenedObject(_config, _content, _input);
        }

        public override void OnShow( ) {
            base.OnShow( );

            _isAnimating = true;
            _camera.LookAt(_level.LevelModel.LevelSpawnX, _level.LevelModel.LevelSpawnY);
            _lockCamera = true;

            // Animate fade-in
            ANIM.Add(0, 1, 600, onUpdate: v => _sceneAlpha = (float)v.Current, onComplete: model => {
                _isAnimating = false;
                ChangeState( );
            });

            // Play audio
            AudioHelper.StopAll( );
            AudioHelper.PlayOnce(_content.MUSICIntro, "intro", true, 0);
            ANIM.Add(0, 1, 600, ease: EaseType.CubicIn, onUpdate: v => AudioHelper.SetVolume(_config.Volume * (float)v.Current));
        }

        public override void Update(GameTime time) {
            _input.Update( );
            _camera.Update( );

            if (!_lockCamera && _input.IsRMBPressed( )) 
                _camera.LookBy(_input.MouseDiffX, _input.MouseDiffY);

            // Skip
            if (_canSkip && _input.IsKeyPressedOnce(Keys.Space)) {
                HideText(ChangeState);

                if (_continueTextAlpha != 0)
                    ANIM.Add(1, 0, 600, ease: EaseType.CubicInOut, onUpdate: v => _continueTextAlpha = (float)v.Current, onComplete: _ => _continueTextAlpha = 0);
            }

            // Stage 5 to 6 (You can move camera around)
            if (_currentStage == 5 && !_stage5Completed && _input.IsRMBPressed( )) {
                _stage5Completed = true;
                HideText(( ) => {
                    _lockCamera = true;
                    ItemData itemData = _content.ItemsData.FirstOrDefault(o => o.ID == "hammer");
                    ItemInstance itemInstance = new ItemInstance(itemData);
                    ObjectData objData = _content.ObjectsData.FirstOrDefault(o => o.ID == "toolbox");
                    _levelObject = new ObjectInstance(objData, 97, 34);
                    _levelObject.Items.Add(itemInstance);
                    ANIM.Add(0, 1, 600, onUpdate: v => _objectAlpha = (float)v.Current);
                    _camera.SlideTo(97 + objData.TextureBase.GetSource(0).Width / 2, 0, 1500, EaseType.CubicInOut, ( ) => ChangeState( ));
                }, 250);
            }

            // Stage 8 (You need to select object)
            if (_currentStage == 8 && !_stage8Completed) {
                // Update mouse position
                int _mouseX = (int)((_input.MouseX - _camera.Offset.X) / _camera.Scale + _camera.Target.X);
                int _mouseY = (int)((_input.MouseY - _camera.Offset.Y) / _camera.Scale - _camera.Target.Y);

                Rectangle source = _levelObject.Texture.TextureData.GetSource(_levelObject.SkinID);
                if (_input.MouseX >= 0 && _input.MouseX <= _config.WindowWidth && _input.MouseY >= 0 && _input.MouseY <= _config.WindowHeight &&
                    _mouseX >= _levelObject.X && _mouseX <= _levelObject.X + source.Width && _mouseY >= -_levelObject.Y && _mouseY <= -_levelObject.Y + source.Height) {
                        _isObjectHovered = true;

                    if (_input.IsLMBPressedOnce( ) && !_openedObject.IsVisible) {
                        _stage8Completed = true;
                        _openedObject.SetContainer(_levelObject);
                        _openedObject.ShowContainer( );
                        HideText(ChangeState);
                    }
                } else {
                    _isObjectHovered = false;
                }
            }

            if (_openedObject.IsVisible) {
                _openedObject.Update(time);
            }
        }

        private void RenderCoreScene(GameTime time) {
            if (!_level.IsLoaded) {
                return;
            }

            // Display level
            DH.Raw(_content.TEXLevel, 0, 0, color: Color.White * _levelAlpha, align: AlignType.LB);

            // Display objects
            if (_levelObject != null)
                DH.Texture(_levelObject.Texture, _levelObject.X, -_levelObject.Y, color: Color.White * _objectAlpha);

            // Display text bubble with selected object's name
            if (_isObjectHovered && !_openedObject.IsVisible) {
                DH.Text(
                    _content.GetFont(FontType.RegularS),
                    LANG.Get(_levelObject.Data.Name),
                    _levelObject.X + _levelObject.Width / 2,
                    5,
                    Color.White * 0.5f,
                    AlignType.CT
                );
            }
        }

        private void RenderUIScene(GameTime time) {
            float textY = (_currentStage < 4 ? .5f : .75f) * _config.WindowHeight;

            // Intro text
            DH.Text(
                _content.GetFont(FontType.TextRegularB), 
                LANG.Get($"intro_{_currentStage}"), 
                _config.WindowWidth / 2, 
                textY,
                Color.Gray * _uiTextAlpha,
                AlignType.CM
            );

            // Press spacebar to skip
            if (_continueTextAlpha != 0)
                DH.Text(_content.GetFont(FontType.TextBoldS), LANG.Get("intro_continue"), _config.WindowWidth / 2, _config.WindowHeight - 64, Color.DimGray * _continueTextAlpha, AlignType.CM);
        }

        public override void Display(GameTime time) {
            // Display opened object
            if (_openedObject.IsVisible) {
                _openedObject.Display(time);
            }

            DH.Scene(_sceneCore, Color.Transparent, _camera.Matrix, ( ) => RenderCoreScene(time), _content.FXFilmGrain);
            DH.Scene(_sceneUI, Color.Transparent, null, ( ) => RenderUIScene(time));
            DH.Scene(null, Color.Black, null, ( ) => {
                DH.Raw(_sceneCore, color: Color.White * _sceneAlpha);
                if (_openedObject.IsVisible)
                    DH.Raw(_openedObject.Scene, color: Color.White * _openedObject.Alpha);

                DH.Raw(_sceneUI, color: Color.White * _sceneAlpha);
            });
        }

        private void ChangeState( ) {
            _currentStage++;

            Logger.Info("Intro state #" + _currentStage);

            switch (_currentStage) {
                case 1:
                    ANIM.Add(0, 1, 600, ease: EaseType.CubicInOut, onUpdate: v => _continueTextAlpha = (float)v.Current);
                    ShowText(( ) => _canSkip = true); 
                    break;
                case 2: case 3: ShowText(( ) => _canSkip = true); break;
                case 4:
                    ANIM.Add(0, 1, 600, ease: EaseType.CubicOut, onUpdate: v => _levelAlpha = (float)v.Current);
                    ShowText(( ) => _canSkip = true); 
                    break;
                case 5: ShowText(( ) => _lockCamera = false); break; 
                case 6: case 7: ShowText(( ) => _canSkip = true); break;
                case 8: ShowText( ); break;
            }
        }

        private void ShowText(Action complete = null, float delay = 0) {
            ANIM.Add(0, 1, 600, delay, ease: EaseType.CubicInOut,
                onStart: _ => _isAnimating = true,
                onUpdate: v => _uiTextAlpha = (float)v.Current,
                onComplete: _ => {
                    complete?.Invoke( );
                    _isAnimating = false;
                }
            );
        }

        private void HideText(Action complete = null, float delay = 0) {
            ANIM.Add(1, 0, 600, delay, ease: EaseType.CubicInOut, 
                onStart: _ => { _isAnimating = true; _canSkip = false; },
                onUpdate: v => _uiTextAlpha = (float)v.Current,
                onComplete: _ => {
                    complete?.Invoke( );
                    _isAnimating = false;
                }
            );
        }

    }
}
