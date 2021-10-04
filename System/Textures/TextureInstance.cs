using Microsoft.Xna.Framework.Graphics;

namespace GameJAM_Devtober2021.System.Textures {
    public sealed class TextureInstance {

        public TextureBase TextureData { get; private set; }

        public TextureInstance(TextureBase data) {
            TextureData = data;
        }

        public Texture2D Get( ) => TextureData?.Texture;

    }
}
