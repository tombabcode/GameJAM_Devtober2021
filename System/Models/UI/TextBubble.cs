﻿using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using DH = GameJAM_Devtober2021.System.Utils.DisplayHelper;
using AH = GameJAM_Devtober2021.System.Utils.AlignHelper;

namespace GameJAM_Devtober2021.System.Models.UI {
    public sealed class TextBubble : UIElement {

        private const int SEGMENT_WIDTH = 8;
        private const int SEGMENT_HEIGHT = 16;

        private ContentController _content;

        private float _x;
        private float _y;
        private AlignType _align;

        // Properties
        public string Text { get; private set; }
        public float X { get { return _x; } set { _x = value; UpdateDisplay( ); } }
        public float Y { get { return _y; } set { _y = value; UpdateDisplay( ); } }
        public AlignType Align { get { return _align; } set { _align = value; UpdateDisplay( ); } }

        // Display helpers
        private float _textLength;
        private float _textLengthRounded;
        private float _textSegments;
        private float _textOffset;
        private Vector2 _displayPosition;

        private Texture2D _UITex;
        private Rectangle[] _UITexSources;

        public TextBubble(ContentController content, string text, float x, float y, AlignType align = AlignType.RB) {
            _content = content;

            _UITex = content.TEXUI.Texture;
            Align = align;
            Text = text;
            X = x;
            Y = y;

            _textLength = content.GetFont(FontType.Console).MeasureString(text).X;
            _textSegments = (float)Math.Ceiling(_textLength / SEGMENT_WIDTH);
            _textLengthRounded = _textSegments * SEGMENT_WIDTH;
            _textOffset = (_textLengthRounded - _textLength) / 2 + SEGMENT_WIDTH;

            UpdateDisplay( );
        }

        public void UpdateDisplay( ) {
            _UITexSources = new Rectangle[] {
                _content.TEXUI.GetSource("text_bubble_left"),
                _content.TEXUI.GetSource("text_bubble_middle"),
                _content.TEXUI.GetSource("text_bubble_right")
            };

            _displayPosition = _align != AlignType.LT ? AH.GetAlignedPosition(_align, X, Y, _textLengthRounded + SEGMENT_WIDTH * 2, SEGMENT_HEIGHT) : new Vector2(X, Y);
        }

        public void Display(float? customX = null, float? customY = null) {
            float x = customX ?? _displayPosition.X;
            float y = customY ?? _displayPosition.Y;

            // Draw left & right bubbles
            _content.Canvas.Draw(_UITex, new Vector2(x, y), _UITexSources[0], Color.White);
            _content.Canvas.Draw(_UITex, new Vector2(x + SEGMENT_WIDTH + _textLengthRounded, y), _UITexSources[2], Color.White);

            // Draw middle bubbles
            for (int i = 1; i <= _textSegments; i++)
                _content.Canvas.Draw(_UITex, new Vector2(x + i * SEGMENT_WIDTH, y), _UITexSources[1], Color.White);

            // Draw text
            _content.Canvas.DrawString(_content.GetFont(FontType.RegularS), Text, new Vector2(x + _textOffset, y), Color.Black);
        }

    }
}