using GameJAM_Devtober2021.System.Logic;
using GameJAM_Devtober2021.System.Logic.Items;
using GameJAM_Devtober2021.System.Logic.Objects;
using GameJAM_Devtober2021.System.Textures;
using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameJAM_Devtober2021.System.Controllers {
    public class ContentController {

        // Credits
        // Music
        // Music by <a href="/users/vjgalaxy-8110364/?tab=audio&amp;utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=audio&amp;utm_content=3587">vjgalaxy</a> from <a href="https://pixabay.com/music/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=3587">Pixabay</a>
        // menu - Music by<a href= "/users/timtaj-16489647/?tab=audio&amp;utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=audio&amp;utm_content=9066" > TimTaj </ a > from < a href= "https://pixabay.com/music/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=9066" > Pixabay </ a >
        // effect click - https://freesound.org/people/GameAudio/sounds/220183/
        // game intro music - Music by <a href="/users/zen_man-4257870/?tab=audio&amp;utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=audio&amp;utm_content=2691">Zen_Man</a> from <a href="https://pixabay.com/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=2691">Pixabay</a>

        private ContentManager _content;

        public SpriteBatch Canvas { get; private set; }
        public GraphicsDevice Device { get; private set; }

        private SpriteFont _fontConsole;
        private SpriteFont _fontRegular;
        private SpriteFont _fontRegularSmall;
        private SpriteFont _fontTextRegularS;
        private SpriteFont _fontTextRegularM;
        private SpriteFont _fontTextRegularB;
        private SpriteFont _fontTextBoldS;
        private SpriteFont _fontTextBoldM;
        private SpriteFont _fontTextBoldB;

        public Texture2D TEXPixel { get; private set; }
        public TextureDictionary TEXUI { get; private set; }

        // Audio
        public SoundEffect MUSICMenu { get; private set; }
        public SoundEffect MUSICIntro { get; private set; }
        public SoundEffect SOUNDMouseHover { get; private set; }

        // Level
        public LevelModel LevelModel { get; private set; }
        public Texture2D TEXLevel { get; private set; }

        public ItemData[] ItemsData { get; private set; }
        public ObjectData[] ObjectsData { get; private set; }

        public Effect FXFilmGrain { get; private set; }

        public Dictionary<string, Texture2D> TEXMoves { get; private set; }

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
                FontType.TextRegularS => _fontTextRegularS,
                FontType.TextRegularM => _fontTextRegularM,
                FontType.TextRegularB => _fontTextRegularB,
                FontType.TextBoldS => _fontTextBoldS,
                FontType.TextBoldM => _fontTextBoldM,
                FontType.TextBoldB => _fontTextBoldB,
                _ => _fontRegular,
            };
        }

        public void LoadAssets( ) {
            _fontRegular = _content.Load<SpriteFont>(Path.Combine("Fonts", "default"));
            _fontRegularSmall = _content.Load<SpriteFont>(Path.Combine("Fonts", "default.small"));
            _fontConsole = _content.Load<SpriteFont>(Path.Combine("Fonts", "console"));

            _fontTextRegularS = _content.Load<SpriteFont>(Path.Combine("Fonts", "TextRegularS"));
            _fontTextRegularM = _content.Load<SpriteFont>(Path.Combine("Fonts", "TextRegularM"));
            _fontTextRegularB = _content.Load<SpriteFont>(Path.Combine("Fonts", "TextRegularB"));
            _fontTextBoldS = _content.Load<SpriteFont>(Path.Combine("Fonts", "TextBoldS"));
            _fontTextBoldM = _content.Load<SpriteFont>(Path.Combine("Fonts", "TextBoldM"));
            _fontTextBoldB = _content.Load<SpriteFont>(Path.Combine("Fonts", "TextBoldB"));

            TEXPixel = new Texture2D(Device, 1, 1);
            TEXPixel.SetData(new Color[] { Color.White });

            TEXUI = new TextureDictionary(_content.Load<Texture2D>("ui"), new Dictionary<string, Rectangle>( ) {
                { "text_bubble_left", new Rectangle(0, 0, 8, 16) },
                { "text_bubble_right", new Rectangle(8, 0, 8, 16) },
                { "text_bubble_middle", new Rectangle(16, 0, 8, 16) },
                { "selection_star", new Rectangle(0, 16, 128, 128) }
            });

            TEXMoves = new Dictionary<string, Texture2D>( ) {
                { "move_hit", _content.Load<Texture2D>(Path.Combine("Moves", "move_hit")) },
                { "move_prepare", _content.Load<Texture2D>(Path.Combine("Moves", "move_prepare")) },
                { "move_stab", _content.Load<Texture2D>(Path.Combine("Moves", "move_stab")) },
                { "move_swing", _content.Load<Texture2D>(Path.Combine("Moves", "move_swing")) }
            };

            MUSICMenu = _content.Load<SoundEffect>(Path.Combine("Audio", "menu"));
            MUSICIntro = _content.Load<SoundEffect>(Path.Combine("Audio", "intro"));
            SOUNDMouseHover = _content.Load<SoundEffect>(Path.Combine("Audio", "mouse_hover"));
            FXFilmGrain = _content.Load<Effect>(Path.Combine("Effects", "film_grain"));

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
