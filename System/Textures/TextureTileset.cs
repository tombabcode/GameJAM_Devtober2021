using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameJAM_Devtober2021.System.Textures {
    public sealed class TextureTileset : TextureBase {

        public int FramesX { get; set; }
        public int FramesY { get; set; }
        public int Frames => FramesX * FramesY;

        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }

        public TextureTileset(Texture2D texture, int framesX = 1, int framesY = 1) : base(texture) {
            FramesX = framesX < 1 || framesX >= texture.Width ? 1 : framesX;
            FramesY = framesY < 1 || framesY >= texture.Height ? 1 : framesY;
            FrameWidth = texture.Width / FramesX;
            FrameHeight = texture.Height / FramesY;
        }

        public override Rectangle GetSource(int id = 0) {
            if (id < 0 || id >= Frames) {
                return base.GetSource( );
            }

            int idx = id % FramesX;
            int idy = id / FramesX;

            return new Rectangle(
                idx * FrameWidth,
                idy * FrameHeight,
                FrameWidth,
                FrameHeight
            );
        }

    }
}
