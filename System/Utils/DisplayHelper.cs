using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Logic.Objects;
using GameJAM_Devtober2021.System.Models;
using GameJAM_Devtober2021.System.Textures;
using GameJAM_Devtober2021.System.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using AH = GameJAM_Devtober2021.System.Utils.AlignHelper;

namespace GameJAM_Devtober2021.System.Utils {
    public static class DisplayHelper {

        public static ContentController Content { private get; set; }
        private static SpriteBatch _canvas => Content.Canvas;
        private static GraphicsDevice _device => Content.Device;

        // Display scene
        public static void Scene(RenderTarget2D scene, Color? color, Matrix? camera, Action logic, Effect effect = null) {
            _device.SetRenderTarget(scene);
            _device.Clear(color ?? Color.Black);
            _canvas.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, effect, camera);
                logic?.Invoke( );
            _canvas.End( );
        }

        public static void DisplayScene(RenderTarget2D scene, Color? color = null) {
            _canvas.Draw(scene, new Rectangle(0, 0, scene.Width, scene.Height), color ?? Color.White);
        }

        // Display raw texture
        public static void Raw(Texture2D texture, float x = 0, float y = 0, float width = -1, float height = -1, float rotation = 0, Vector2? rotOrigin = null, Color? color = null, AlignType align = AlignType.LT) {
            Vector2 size = new Vector2(width <= 0 ? texture.Width : width, height <= 0 ? texture.Height : height);
            Vector2 position = align != AlignType.LT ? AH.GetAlignedPosition(align, x, y, size.X, size.Y) : new Vector2(x, y);
            Vector2 scale = new Vector2(size.X / texture.Width, size.Y / texture.Height);

            _canvas.Draw(texture, position, null, color ?? Color.White, rotation, rotOrigin ?? Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        // Display box
        public static void Box(float x = 0, float y = 0, float width = 1, float height = 1, Color? color = null, AlignType align = AlignType.LT) {
            Vector2 position = align != AlignType.LT ? AH.GetAlignedPosition(align, x, y, width, height) : new Vector2(x, y);
            Vector2 scale = new Vector2(width, height);

            _canvas.Draw(Content.TEXPixel, position, null, color ?? Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        // Display texture
        public static void Texture(TextureBase texture, float x = 0, float y = 0, float width = -1, float height = -1, float rotation = 0, Vector2? rotOrigin = null, Color? color = null, AlignType align = AlignType.LT)
            => Raw(texture.Texture, x, y, width, height, rotation, rotOrigin, color, align);

        // Display texture
        public static void Texture(TextureInstance texture, float x = 0, float y = 0, float width = -1, float height = -1, float rotation = 0, Vector2? rotOrigin = null, Color? color = null, AlignType align = AlignType.LT)
            => Raw(texture.TextureData.Texture, x, y, width, height, rotation, rotOrigin, color, align);

        // Display text
        public static void Text(SpriteFont font, string text, float x, float y, Color? color = null, AlignType align = AlignType.LT) {
            Vector2 size = font.MeasureString(text);
            Vector2 position = align != AlignType.LT ? AH.GetAlignedPosition(align, x, y, size.X, size.Y) : new Vector2(x, y);

            _canvas.DrawString(font, text, position, color ?? Color.White);
        }

        // Display custom elements
        public static void LevelObject(ObjectInstance instance) => Raw(instance.Data.TextureBase.Texture, instance.X, -instance.Y);

        public static void TextureDictionary(TextureDictionary texture, string key, float x = 0f, float y = 0f, float width = -1f, float height = -1f, float rotation = 0f, Vector2? rotationOrigin = null, Color? color = null, AlignType align = AlignType.LT) {
            Texture2D tex = texture.Texture;
            Rectangle source = texture.GetSource(key);
            Vector2 size = new Vector2(width <= 0 ? source.Width : width, height <= 0 ? source.Height : height);
            Vector2 position = align != AlignType.LT ? AH.GetAlignedPosition(align, x, y, size.X, size.Y) : new Vector2(x, y);
            Vector2 scale = new Vector2(size.X / source.Width, size.Y / source.Height);

            _canvas.Draw(tex, position, source, color ?? Color.White, rotation, rotationOrigin ?? Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public static void Tileset(TextureTileset texture, int id = 0, float x = 0f, float y = 0f, float width = -1f, float height = -1f, float rotation = 0f, Vector2? rotationOrigin = null, Color? color = null, AlignType align = AlignType.LT) {
            Texture2D tex = texture.Texture;
            Rectangle source = texture.GetSource(id);
            Vector2 size = new Vector2(width <= 0 ? source.Width : width, height <= 0 ? source.Height : height);
            Vector2 position = align != AlignType.LT ? AH.GetAlignedPosition(align, x, y, size.X, size.Y) : new Vector2(x, y);
            Vector2 scale = new Vector2(size.X / source.Width, size.Y / source.Height);

            _canvas.Draw(tex, position, source, color ?? Color.White, rotation, rotationOrigin ?? Vector2.Zero, scale, SpriteEffects.None, 0);
        }

    }
}
