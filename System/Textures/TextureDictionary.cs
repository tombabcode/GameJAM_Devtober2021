using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameJAM_Devtober2021.System.Textures {
    public sealed class TextureDictionary : TextureBase {

        public Dictionary<string, Rectangle> Sources { get; private set; }

        public TextureDictionary(Texture2D texture, Dictionary<string, Rectangle> sources) : base(texture) {
            Sources = sources;
        }

        public Rectangle GetSource(string key) {
            return Sources.ContainsKey(key) ? Sources[key] : Rectangle.Empty;
        }

    }
}
