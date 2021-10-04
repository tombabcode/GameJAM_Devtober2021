using GameJAM_Devtober2021.System.Models;
using GameJAM_Devtober2021.System.Textures;
using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameJAM_Devtober2021.System.Controllers {
    public class ContentController {

        private ContentManager _content;

        public SpriteBatch Canvas { get; private set; }
        public GraphicsDevice Device { get; private set; }

        private SpriteFont _fontConsole;
        private SpriteFont _fontRegular;
        private SpriteFont _fontRegularSmall;

        public Texture2D TEXPixel { get; private set; }
        public TextureDictionary TEXUI { get; private set; }

        // Level
        public LevelModel LevelModel { get; private set; }
        public Texture2D TEXLevel { get; private set; }

        // Items
        public List<ObjectDataModel> ObjectsData{ get; private set; }
        public List<ItemDataModel> ItemsData { get; private set; }

        public void Initialize(ContentManager content, SpriteBatch canvas, GraphicsDevice device) {
            _content = content;
            Canvas = canvas;
            Device = device;

            Logger.Info("Content initialized");
        }

        public SpriteFont GetFont(FontType type) {
            return type switch {
                FontType.Regular => _fontRegular,
                FontType.RegularS => _fontRegularSmall,
                FontType.Console => _fontConsole,
                _ => _fontRegular,
            };
        }

        public void LoadAssets( ) {
            _fontRegular = _content.Load<SpriteFont>(Path.Combine("Fonts", "default"));
            _fontRegularSmall = _content.Load<SpriteFont>(Path.Combine("Fonts", "default.small"));
            _fontConsole = _content.Load<SpriteFont>(Path.Combine("Fonts", "console"));

            TEXPixel = new Texture2D(Device, 1, 1);
            TEXPixel.SetData(new Color[] { Color.White });

            TEXUI = new TextureDictionary(_content.Load<Texture2D>("ui"), new Dictionary<string, Rectangle>( ) {
                { "text_bubble_left", new Rectangle(0, 0, 8, 16) },
                { "text_bubble_right", new Rectangle(8, 0, 8, 16) },
                { "text_bubble_middle", new Rectangle(16, 0, 8, 16) }
            });

            // Load object data
            string dataObj = File.ReadAllText(Path.Combine("Assets", "Objects", "data.json"));
            ObjectsData = JsonConvert.DeserializeObject<List<ObjectDataModel>>(dataObj);

            // Load items data
            string dataItems = File.ReadAllText(Path.Combine("Assets", "Items", "data.json"));
            ItemsData = JsonConvert.DeserializeObject<List<ItemDataModel>>(dataItems);

            Logger.Info("Content loaded");
        }

        public void LoadLevelAssets(string id) {
            TEXLevel = _content.Load<Texture2D>(Path.Combine("Levels", id, "level"));
        }

        public TextureBase LoadLevelObject(string id, out ObjectDataModel model) {
            // Try to find model
            model = ObjectsData.FirstOrDefault(obj => obj.ID == id);

            // If model was not found, skip
            if (model == null) {
                Logger.Error($"Level asset loading failure. Couldn't find object '{id}' model!");
                return null;
            }

            string path = Path.Combine("Objects", model.Texture.Asset);

            switch (model.Texture.Type.ToLower( )) {
                case "static": return new TextureStatic(_content.Load<Texture2D>(path));
                case "tileset": return new TextureTileset(_content.Load<Texture2D>(path), model.Texture.Columns, model.Texture.Rows);
                default: return null;
            }
        }

        public TextureBase LoadLevelItem(string id, out ItemDataModel model) {
            // Try to find model
            model = ItemsData.FirstOrDefault(item => item.ID == id);

            // If model was not found, skip
            if (model == null) {
                Logger.Error($"Level asset loading failure. Couldn't find item '{id}' model!");
                return null;
            }

            string path = Path.Combine("Items", model.Texture.Asset);

            switch (model.Texture.Type.ToLower( )) {
                case "static": return new TextureStatic(_content.Load<Texture2D>(path));
                case "tileset": return new TextureTileset(_content.Load<Texture2D>(path), model.Texture.Columns, model.Texture.Rows);
                default: return null;
            }
        }

    }
}
