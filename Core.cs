using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJAM_Devtober2021 {
    public sealed class Core : Game {

        private GraphicsDeviceManager _deviceMNG;
        private SpriteBatch _canvas;

        private ConfigController _config;
        private ContentController _content;
        private InputController _input;
        private SceneController _scene;

        public Core() {
            _config = new ConfigController( );
            _content = new ContentController( );
            _input = new InputController( );
            _scene = new SceneController( );

            _deviceMNG = new GraphicsDeviceManager(this);
        }

        protected override void Initialize( ) {
            base.Initialize( );

            IsMouseVisible = true;

            _deviceMNG.PreferredBackBufferWidth = 800;
            _deviceMNG.PreferredBackBufferHeight = 600;
            _deviceMNG.ApplyChanges( );

            _canvas = new SpriteBatch(GraphicsDevice);

            _scene.Initialize(_config, _content, _input);
            _content.Initialize(Content, _canvas, GraphicsDevice);

            _content.LoadAssets( );

            DisplayHelper.Canvas = _canvas;
            DisplayHelper.Device = GraphicsDevice;
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
            _scene.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            _scene.Display(gameTime);
        }

    }
}
