using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Timers;

namespace GameJAM_Devtober2021 {
    public sealed class Core : Game {

        public const string GAME_TITLE = "A lot of things!";
        public const string GAME_VERSION = "1.0";

        private GraphicsDeviceManager _deviceMNG;
        private SpriteBatch _canvas;

        private ConfigController _config;
        private ContentController _content;
        private InputController _input;
        private SceneController _scene;

        public static Action OnExit { get; private set; }

        public Core() {
            Logger.Initialize( );
            LanguageHelper.LoadLanguage("en");

            OnExit = () => Exit( );

            _config = new ConfigController( );
            _content = new ContentController( );
            _input = new InputController( );
            _scene = new SceneController( );

            _config.LoadConfig( );

            _deviceMNG = new GraphicsDeviceManager(this);
        }

        protected override void Initialize( ) {
            base.Initialize( );

            AudioHelper.Config = _config;

            IsMouseVisible = true;
            Content.RootDirectory = "Assets";

            _deviceMNG.PreferredBackBufferWidth = _config.WindowWidth;
            _deviceMNG.PreferredBackBufferHeight = _config.WindowHeight;
            _deviceMNG.IsFullScreen = _config.WindowFullscreen;
            _deviceMNG.ApplyChanges( );

            Logger.Info("Config applied");

            _canvas = new SpriteBatch(GraphicsDevice);

            _content.Initialize(Content, _canvas, GraphicsDevice);
            _content.LoadAssets( );

            _scene.Initialize(_config, _content, _input);

            DisplayHelper.Content = _content;

            Random rand = new Random( );
            Timer timer = new Timer( );
            timer.Elapsed += new ElapsedEventHandler((_, e) => _content.FXFilmGrain.Parameters["InputRandomValue"].SetValue((float)rand.NextDouble( ) * 125f + 43758.5453123f));
            timer.Interval = 50;
            timer.Enabled = true;
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
            _scene.Update(gameTime);
            AudioHelper.Update( );
            AnimationHelper.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            _scene.Display(gameTime);
        }

    }
}
