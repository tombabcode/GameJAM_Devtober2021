using GameJAM_Devtober2021.System.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using AH = GameJAM_Devtober2021.System.Utils.AlignHelper;

namespace GameJAM_Devtober2021.System.Utils {
    public static class DisplayHelper {

        public static SpriteBatch Canvas { private get; set; }
        public static GraphicsDevice Device { private get; set; }

        public static void Scene(RenderTarget2D scene, Color? color, Matrix? camera, Action logic) {
            Device.SetRenderTarget(scene);
            Device.Clear(color ?? Color.Black);
            Canvas.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera);
                logic?.Invoke( );
            Canvas.End( );
        }

        public static void Texture(Texture2D texture, float x = 0f, float y = 0f, float width = -1f, float height = -1f, float rotation = 0f, Vector2? rotationOrigin = null, Color? color = null, AlignType align = AlignType.LT) {
            Vector2 size = new Vector2(width <= 0 ? texture.Width : width, height <= 0 ? texture.Height : height);
            Vector2 position = align != AlignType.LT ? AH.GetAlignedPosition(align, x, y, size.X, size.Y) : new Vector2(x, y);
            Vector2 scale = new Vector2(size.X / texture.Width, size.Y / texture.Height);

            Canvas.Draw(texture, position, null, color ?? Color.White, rotation, rotationOrigin ?? Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public static void Text(SpriteFont font, string text, float x, float y, Color? color = null, AlignType align = AlignType.LT) {
            Vector2 size = font.MeasureString(text);
            Vector2 position = align != AlignType.LT ? AH.GetAlignedPosition(align, x, y, size.X, size.Y) : new Vector2(x, y);

            Canvas.DrawString(font, text, position, color ?? Color.White);
        }

    }
}
