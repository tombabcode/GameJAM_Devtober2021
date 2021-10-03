using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJAM_Devtober2021.System.Textures {
    public abstract class TextureBase {

        public Texture2D Texture { get; set; }

        public TextureBase(Texture2D texture) {
            Texture = texture;
        }

        public virtual TextureInstance GetInstance( ) {
            return new TextureInstance(this);
        }

        public virtual Rectangle GetSource(int id = 0) { return new Rectangle(0, 0, Texture.Width, Texture.Height); }

    }
}
