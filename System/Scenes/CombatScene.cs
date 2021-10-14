using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Logic.Items;
using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using DH = GameJAM_Devtober2021.System.Utils.DisplayHelper;
using LANG = GameJAM_Devtober2021.System.Utils.LanguageHelper;

namespace GameJAM_Devtober2021.System.Scenes {
    public class CombatScene : SceneBase {

        private ConfigController _config;
        private ContentController _content;
        private InputController _input;
        private SceneController _scene;
        private CameraController _camera;

        private ItemInstance _playerItem;
        private ItemInstance _enemyItem;

        // Render targets
        private RenderTarget2D _sceneUI;
        private RenderTarget2D _sceneCore;

        private int _playerIP;
        private int _enemyIP;

        private int _playerHP = 0;
        private float _playerDodge = 0;
        private float _playerCritical = 0;
        private int _playerDamageMin = 0;
        private int _playerDamageMax = 0;

        private int _enemyHP = 0;
        private float _enemyDodge = 0;
        private float _enemyCritical = 0;
        private int _enemyDamageMin = 0;
        private int _enemyDamageMax = 0;

        private Color _playerHPColor = Color.White;
        private Color _playerDamageColor = Color.White;
        private Color _playerDodgeColor = Color.White;
        private Color _playerCriticalColor = Color.White;

        private ItemMovesData _selectedMove;

        public CombatScene(ConfigController config, ContentController content, InputController input, SceneController scene) : base("Combat") {
            _config = config;
            _content = content;
            _input = input;
            _scene = scene;
        }

        public void SetActor(ItemInstance item) {
            _playerItem = item;

            Random rand = new Random( );
            ItemData[] enemies = _content.ItemsData.Where(i => i.Level == item.Data.Level).ToArray();

            if (enemies.Length > 0) {
                int id = (int)(rand.NextDouble( ) * enemies.Length);

                _enemyItem = new ItemInstance(enemies[id]);
            }

            _playerIP = 0;
            _enemyIP = 0;

            Logger.Info($"Combat initialized. Player actor is [{_playerItem.Data.ID}] and enemy is [{_playerItem.Data.ID}]");
        }

        public override void OnLoad( ) {
            base.OnLoad( );

            _sceneUI = new RenderTarget2D(_content.Device, _config.WindowWidth, _config.WindowHeight);
            _sceneCore = new RenderTarget2D(_content.Device, _config.WindowWidth, _config.WindowHeight);

            _camera = new CameraController( );
            _camera.SetOffset(0, 0);
            _camera.SetOffset(_config.WindowWidth / 2, _config.WindowHeight / 2);
            _camera.SetScale(2);
        }

        private void RenderCoreScene(GameTime time) {
            DH.Texture(_playerItem.Texture, -32, align: AlignType.RM);
            DH.Texture(_enemyItem.Texture, 32, align: AlignType.LM);
        }

        private void RenderUIScene(GameTime time) {
            // Calculate positions
            int px = 5;
            int py = _config.WindowHeight / 2;
            int ex = _config.WindowWidth - 5;
            int ey = _config.WindowHeight / 2;

            // Set text
            string pDamage = _playerDamageMin == _playerDamageMax ? _playerDamageMin.ToString( ) : $"{_playerDamageMin} - {_playerDamageMax}";
            string eDamage = _enemyDamageMin == _enemyDamageMax ? _enemyDamageMin.ToString( ) : $"{_enemyDamageMin} - {_enemyDamageMax}";

            // Display info
            DH.Text(_content.GetFont(FontType.TextBoldM), $"{LANG.Get(_playerItem.Data.Name)}", px, py, Color.White, AlignType.LM);
            DH.Text(_content.GetFont(FontType.TextBoldM), $"{LANG.Get(_enemyItem.Data.Name)}", ex, ey, Color.White, AlignType.RM);
            DH.Text(_content.GetFont(FontType.TextRegularS), $"{LANG.Get("ui_health")}: {_playerHP}", px, py + 20, _playerHPColor, AlignType.LM);
            DH.Text(_content.GetFont(FontType.TextRegularS), $"{_enemyHP} :{LANG.Get("ui_health")}", ex, ey + 20, Color.White, AlignType.RM);
            DH.Text(_content.GetFont(FontType.TextRegularS), $"{LANG.Get("ui_damage")}: {pDamage}", px, py + 40, _playerDamageColor, AlignType.LM);
            DH.Text(_content.GetFont(FontType.TextRegularS), $"{eDamage} :{LANG.Get("ui_damage")}", ex, ey + 40, Color.White, AlignType.RM);
            DH.Text(_content.GetFont(FontType.TextRegularS), $"{LANG.Get("ui_dodge_chance")}: {_playerDodge * 100:0.0}%", px, py + 60, _playerDodgeColor, AlignType.LM);
            DH.Text(_content.GetFont(FontType.TextRegularS), $"{_enemyDodge * 100:0.0}% :{LANG.Get("ui_dodge_chance")}", ex, ey + 60, Color.White, AlignType.RM);
            DH.Text(_content.GetFont(FontType.TextRegularS), $"{LANG.Get("ui_critical_chance")}: {_playerCritical * 100:0.0}%", px, py + 80, _playerCriticalColor, AlignType.LM);
            DH.Text(_content.GetFont(FontType.TextRegularS), $"{_enemyCritical * 100:0.0}% :{LANG.Get("ui_critical_chance")}", ex, ey + 80, Color.White, AlignType.RM);

            // Selection info
            if (_selectedMove != null) {
                if (_selectedMove.RequiredIP > 0) {
                    DH.Text(_content.GetFont(FontType.TextRegularM), $"{LANG.Get(_selectedMove.ID)}", _config.WindowWidth / 2, _config.WindowHeight - 94, align: AlignType.CB);
                    DH.Text(_content.GetFont(FontType.TextRegularS), $"{LANG.Get("ui_require")} {_selectedMove.RequiredIP} IP", _config.WindowWidth / 2, _config.WindowHeight - 76, align: AlignType.CB);
                } else {
                    DH.Text(_content.GetFont(FontType.TextRegularM), $"{LANG.Get(_selectedMove.ID)}", _config.WindowWidth / 2, _config.WindowHeight - 76, align: AlignType.CB);
                }
            }

            // Display moves
            int offset = _playerItem.Data.Moves.Length * 56 / 2;
            for (int i = 0; i < _playerItem.Data.Moves.Length; i++) {
                ItemMovesData data = _playerItem.Data.Moves[i];
                Color color = data == _selectedMove ? Color.White : Color.Gray;
                DH.Raw(_content.TEXMoves[data.ID], _config.WindowWidth / 2 - offset + i * 56, _config.WindowHeight - 8, color: color, align: AlignType.LB);
            }
        }

        public override void Update(GameTime time) {
            _input.Update( );
            _camera.Update( );

            SetDataFromPlayer( );
            SetDataFromEnemy( );

            _selectedMove = null;

            int offset = _playerItem.Data.Moves.Length * 56 / 2;
            for (int i = 0; i < _playerItem.Data.Moves.Length; i++) {
                ItemMovesData data = _playerItem.Data.Moves[i];

                int x = _config.WindowWidth / 2 - offset + i * 56;
                int y = _config.WindowHeight - 8 - 64;

                if (_input.MouseX >= x && _input.MouseX <= x + 48 && _input.MouseY >= y && _input.MouseY <= y + 64) {
                    _selectedMove = data;

                    _playerDamageMin += data.GainDamage;
                    _playerDamageMax += data.GainDamage;
                    _playerCritical += data.GainCriticalChance;
                    _playerDodge += data.GainDodgeChance;
                    _playerHP += data.GainHP;

                    Color colorImproved = Color.Green;
                    Color colorReduced = Color.Red;

                    if (data.GainDamage != 0) _playerDamageColor = data.GainDamage > 0 ? colorImproved : colorReduced;
                    if (data.GainHP != 0) _playerHPColor = data.GainHP > 0 ? colorImproved : colorReduced;
                    if (data.GainDodgeChance != 0) _playerDodgeColor = data.GainDodgeChance > 0 ? colorImproved : colorReduced;
                    if (data.GainCriticalChance != 0) _playerCriticalColor = data.GainCriticalChance > 0 ? colorImproved : colorReduced;

                    if (_input.IsLMBPressedOnce( ))
                        ExecuteMove(data);

                    break;
                }
            }
        }

        private void ExecuteMove(ItemMovesData data) {
        }

        public override void Display(GameTime time) {
            DH.Scene(_sceneCore, Color.Transparent, _camera.Matrix, ( ) => RenderCoreScene(time));
            DH.Scene(_sceneUI, Color.Transparent, null, ( ) => RenderUIScene(time));
            DH.Scene(null, Color.Black, null, ( ) => {
                DH.Raw(_sceneCore);
                DH.Raw(_sceneUI);
            });
        }

        private void SetDataFromPlayer( ) {
            _playerDamageMin = _playerItem.Data.Statistics.DamageMin;
            _playerDamageMax = _playerItem.Data.Statistics.DamageMax;
            _playerHP = _playerItem.Health;
            _playerDodge = _playerItem.DodgeChance;
            _playerCritical = _playerItem.CriticalChance;

            _playerHPColor = Color.White;
            _playerDodgeColor = Color.White;
            _playerDamageColor = Color.White;
            _playerCriticalColor = Color.White;
        }

        private void SetDataFromEnemy( ) {
            _enemyDamageMin = _enemyItem.Data.Statistics.DamageMin;
            _enemyDamageMax = _enemyItem.Data.Statistics.DamageMax;
            _enemyHP = _enemyItem.Health;
            _enemyDodge = _enemyItem.DodgeChance;
            _enemyCritical = _enemyItem.CriticalChance;
        }

    }
}
