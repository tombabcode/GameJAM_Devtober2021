using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJAM_Devtober2021.System.Controllers {
    public class ContentController {

        private ContentManager _content;

        public SpriteBatch Canvas { get; private set; }
        public GraphicsDevice Device { get; private set; }
        
        public void Initialize(ContentManager content, SpriteBatch canvas, GraphicsDevice device) {
            _content = content;
            Canvas = canvas;
            Device = device;
        }

        public void LoadAssets( ) {

        }
    
    }
}
