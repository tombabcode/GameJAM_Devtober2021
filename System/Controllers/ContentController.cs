﻿using GameJAM_Devtober2021.System.Logic;
using GameJAM_Devtober2021.System.Logic.Items;
using GameJAM_Devtober2021.System.Logic.Objects;
using GameJAM_Devtober2021.System.Textures;
using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

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

        public ItemData[] ItemsData { get; private set; }
        public ObjectData[] ObjectsData { get; private set; }

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

            string data;

            // TODO
            // Maybe merge somehow items and objects?

            // Load items
            data = File.ReadAllText(Path.Combine("Assets", "Items", "data.json"));
            ItemsData = JsonConvert.DeserializeObject<ItemData[]>(data);
            foreach (ItemData item in ItemsData) {
                Texture2D texture = _content.Load<Texture2D>(Path.Combine("Items", item.TextureData.Asset));

                switch (item.TextureData.Type) {
                    case "static": item.TextureBase = new TextureStatic(texture); break;
                    case "tileset": item.TextureBase = new TextureTileset(texture, item.TextureData.Columns, item.TextureData.Rows); break;
                }
            }

            // Load objects
            data = File.ReadAllText(Path.Combine("Assets", "Objects", "data.json"));
            ObjectsData = JsonConvert.DeserializeObject<ObjectData[]>(data);
            foreach (ObjectData obj in ObjectsData) {
                Texture2D texture = _content.Load<Texture2D>(Path.Combine("Objects", obj.TextureData.Asset));

                switch (obj.TextureData.Type) {
                    case "static": obj.TextureBase = new TextureStatic(texture); break;
                    case "tileset": obj.TextureBase = new TextureTileset(texture, obj.TextureData.Columns, obj.TextureData.Rows); break;
                }
            }

            Logger.Info("Content loaded");
        }

        public void LoadLevelAssets(string id) {
            TEXLevel = _content.Load<Texture2D>(Path.Combine("Levels", id, "level"));
        }

    }
}
