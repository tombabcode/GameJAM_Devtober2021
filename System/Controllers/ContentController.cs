using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace GameJAM_Devtober2021.System.Controllers {
    public class ContentController {

        private ContentManager _content;

        public SpriteBatch Canvas { get; private set; }
        public GraphicsDevice Device { get; private set; }

        private SpriteFont _fontConsole;
        private SpriteFont _fontRegular;

        public Texture2D TEXPixel { get; private set; }
        
        public void Initialize(ContentManager content, SpriteBatch canvas, GraphicsDevice device) {
            _content = content;
            Canvas = canvas;
            Device = device;

            Logger.Info("Content initialized");
        }

        public SpriteFont GetFont(FontType type) {
            return type switch {
                FontType.Regular => _fontRegular,
                FontType.Console => _fontConsole,
                _ => _fontRegular,
            };
        }

        public void LoadAssets( ) {
            _fontRegular = _content.Load<SpriteFont>(Path.Combine("Fonts", "default"));
            _fontConsole = _content.Load<SpriteFont>(Path.Combine("Fonts", "console"));

            TEXPixel = new Texture2D(Device, 1, 1);
            TEXPixel.SetData(new Color[] { Color.White });

            Logger.Info("Content loaded");
        }

    }
}
