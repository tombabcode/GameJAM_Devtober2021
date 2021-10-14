using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Types;
using Microsoft.Xna.Framework;
using System;

using DH = GameJAM_Devtober2021.System.Utils.DisplayHelper;
using AH = GameJAM_Devtober2021.System.Utils.AlignHelper;
using Microsoft.Xna.Framework.Graphics;

namespace GameJAM_Devtober2021.System.Models.UI {
    public sealed class Button : UIElement {

        // Refs
        private ContentController _content;
        private InputController _input;
        private ConfigController _config;

        // Helper values
        private SpriteFont _fontValue;
        private Vector2 _size;
        private Vector2 _position;
        private bool _isHovered;

        // Customs
        public string Text { get; set; }
        public Color? TextColor { get; set; } = null;
        public AlignType TextAlign { get; set; } = AlignType.CM;
        public FontType Font { get; set; }
        public float PaddingLeft { get; set; } = 0;
        public float PaddingRight { get; set; } = 0;
        public float PaddingTop { get; set; } = 0;
        public float PaddingBottom { get; set; } = 0;

        // Setters
        public float Padding { set { SetPadding(value); } }
        public float PaddingX { set { SetPaddingHorizontally(value); } }
        public float PaddingY { set { SetPaddingVeritcally(value); } }

        // Events
        public Action<Button> OnHover { get; set; }
        public Action<Button> OnUnhover { get; set; }
        public Action<Button> OnClick { get; set; }

        // Conditions
        public Func<bool> CheckHoverCondition { get; set; }
        public Func<bool> CheckClickCondition { get; set; }

        public Button(ConfigController config, InputController input, ContentController content) {
            _config = config;
            _input = input;
            _content = content;
        }

        public override void Update(GameTime time) {
            // Hover
            if (CheckHoverCondition == null || CheckHoverCondition.Invoke( )) {
                if (_input.MouseX >= _position.X && _input.MouseX <= _position.X + _size.X &&
                    _input.MouseY >= _position.Y && _input.MouseY <= _position.Y + _size.Y
                ) {
                    if (!_isHovered) {
                        OnHover(this);
                        _isHovered = true;
                    }
                } else {
                    if (_isHovered) {
                        OnUnhover(this);
                        _isHovered = false;
                    }
                }
            }

            // Click
            if (CheckClickCondition == null || CheckClickCondition.Invoke( )) {
                if (_isHovered && _input.IsLMBPressedOnce( ))
                    OnClick(this);
            }
        }

        public override void Display(GameTime time) {
            DH.Text(_content.GetFont(Font), Text, X, Y, TextColor, TextAlign);
        }

        public override void Refresh( ) {
            _fontValue = _content.GetFont(Font);

            if (string.IsNullOrEmpty(Text)) {
                _size = Vector2.Zero;
                _position = new Vector2(X, Y);
            } else {
                Vector2 size = _fontValue.MeasureString(Text);
                _size = new Vector2(size.X + PaddingLeft + PaddingRight, size.Y + PaddingTop + PaddingBottom);
                _position = AH.GetAlignedPosition(Align, X, Y, _size.X, _size.Y);
            }
        }

        private void SetPadding(float padding) {
            PaddingLeft = padding;
            PaddingRight = padding;
            PaddingTop = padding;
            PaddingBottom = padding;
        }

        private void SetPaddingHorizontally(float padding) {
            PaddingLeft = padding;
            PaddingRight = padding;
        }

        private void SetPaddingVeritcally(float padding) {
            PaddingTop = padding;
            PaddingBottom = padding;
        }

    }
}
