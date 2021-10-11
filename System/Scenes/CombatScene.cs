using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Logic.Items;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using DH = GameJAM_Devtober2021.System.Utils.DisplayHelper;

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

        public CombatScene(ConfigController config, ContentController content, InputController input, SceneController scene) : base("Combat") {
            _config = config;
            _content = content;
            _input = input;
            _scene = scene;
        }

        public void SetActor(ItemInstance item) {
            _playerItem = item;

            ItemData[] enemies = _content.ItemsData.Where(i => i.Level == ((ItemData)item.Data).Level).ToArray();

            if (enemies.Length > 0) {
                Random rand = new Random( );
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
            DH.Texture(_playerItem.Texture);
            DH.Texture(_enemyItem.Texture, 64);
        }

        private void RenderUIScene(GameTime time) {
        }

        public override void Update(GameTime time) {
            _input.Update( );
            _camera.Update( );
        }

        public override void Display(GameTime time) {
            DH.Scene(_sceneCore, Color.Transparent, _camera.Matrix, ( ) => RenderCoreScene(time));
            DH.Scene(_sceneUI, Color.Transparent, null, ( ) => RenderUIScene(time));
            DH.Scene(null, Color.Red, null, ( ) => {
                DH.Raw(_sceneCore);
                DH.Raw(_sceneUI);
            });
        }

    }
}
