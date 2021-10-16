using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Logic.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DH = GameJAM_Devtober2021.System.Utils.DisplayHelper;
using ANIM = GameJAM_Devtober2021.System.Utils.AnimationHelper;
using LANG = GameJAM_Devtober2021.System.Utils.LanguageHelper;
using GameJAM_Devtober2021.System.Types;
using System;
using System.Collections.Generic;
using GameJAM_Devtober2021.System.Logic.Items;
using System.Linq;

namespace GameJAM_Devtober2021.System.Logic {
    public sealed class OpenedObject {

        private ConfigController _config;
        private ContentController _content;
        private InputController _input;

        public ObjectInstance Target;

        public RenderTarget2D Scene { get; private set; }

        public bool IsVisible { get; private set; }
        public bool IsAnimating { get; private set; }

        public float Alpha { get; private set; }

        private List<float> _itemsRotation;
        private List<float> _itemsOffsetY;
        private float _itemsOffsetX;

        private ItemInstance _itemHover;

        public OpenedObject(ConfigController config, ContentController content, InputController input) {
            _config = config;
            _content = content;
            _input = input;

            Scene = new RenderTarget2D(_content.Device, _config.WindowWidth, _config.WindowHeight);
            _itemsRotation = new List<float>( );
            _itemsOffsetY = new List<float>( );
        }

        public void SetContainer(ObjectInstance obj) {
            if (obj.Data.IsContainer) {
                Target = obj;
            } else {
                Target = null;
            }
        }

        public void ShowContainer( ) {
            IsVisible = true;
            IsAnimating = true;
            ANIM.Add(0, 1, 600, onUpdate: v => Alpha = (float)v.Current );

            Random rand = new Random( );
            _itemsOffsetX = Target.Items.Sum(i => i.Texture.TextureData.GetSource(0).Width) / 2;

            for (int i = 0; i < Target.Items.Count; i++) {
                ItemInstance item = Target.Items[i];
                float rotation = (float)rand.NextDouble( ) * 60 - 30;
                _itemsRotation.Add(MathHelper.ToRadians(rotation));
                _itemsOffsetY.Add(_config.WindowHeight);

                int index = i;
                Console.WriteLine(index);
                ANIM.Add(_config.WindowHeight, _config.WindowHeight / 2, 600, (i + 1) * 300, ease: EaseType.CubicOut, onUpdate: v => _itemsOffsetY[index] = (float)v.Current);
            }
        }

        public void CloseContainer( ) {
            IsAnimating = true;
            ANIM.Add(1, 0, 600, onUpdate: v => Alpha = (float)v.Current, onComplete: _ => { 
                IsAnimating = false; 
                IsVisible = false;
                _itemsRotation.Clear( );
                _itemsOffsetY.Clear( );
                Target = null;
            });
        }

        public void Update(GameTime time) {
            if (IsVisible && !IsAnimating) {
                int localOffset = 0;
                for (int i = 0; i < Target.Items.Count; i++) {
                    ItemInstance item = Target.Items[i];
                    Rectangle source = item.Texture.TextureData.GetSource(0);
                    
                    if (_input.MouseX >= _config.WindowWidth / 2 - _itemsOffsetX + localOffset && _input.MouseX <= _config.WindowWidth / 2 - _itemsOffsetX + localOffset + source.Width && _input.MouseY >= _config.WindowHeight / 2 && _input.MouseY <= _config.WindowHeight / 2 + source.Height) {
                        _itemHover = item;
                        break;
                    }

                    localOffset += source.Width;
                }
            }
        }

        public void Display(GameTime time) {
            DH.Scene(Scene, Color.Transparent, null, ( ) => {
                DH.Box(0, 0, _config.WindowWidth, _config.WindowHeight, new Color(Color.Black, .7f));
                DH.Text(_content.GetFont(FontType.TextBoldB), LANG.Get(Target.Data.Name), _config.WindowWidth / 2, 100, Color.Gray, AlignType.CT);

                if (Target != null && Target.Items != null) {
                    int localOffset = 0;

                    for (int i = 0; i < Target.Items.Count; i++) {
                        ItemInstance item = Target.Items[i];
                        Rectangle source = item.Texture.TextureData.GetSource(0);

                        if (item == _itemHover) {
                            DH.TextureDictionary(_content.TEXUI, "selection_star",
                                _config.WindowWidth / 2 - _itemsOffsetX + localOffset + source.Width / 2,
                                _config.WindowHeight - _itemsOffsetY[i] + source.Height / 2,
                                128,
                                128,
                                0,
                                Vector2.Zero,
                                Color.White * 0.5f,
                                AlignType.CM
                            );
                        }

                        DH.Texture(item.Texture,
                            _config.WindowWidth / 2 - _itemsOffsetX + localOffset,
                            _config.WindowHeight - _itemsOffsetY[i],
                            rotation: _itemsRotation[i],
                            rotOrigin: new Vector2(.5f, .5f),
                            align: AlignType.LT
                        );

                        localOffset += source.Width;
                    }
                }
            });
        }

    }
}
