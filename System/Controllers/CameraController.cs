using GameJAM_Devtober2021.System.Types;
using GameJAM_Devtober2021.System.Utils;
using Microsoft.Xna.Framework;
using System;

namespace GameJAM_Devtober2021.System.Controllers {
    public sealed class CameraController {

        public Matrix Matrix { get; private set; }
        public Vector2 Target { get; private set; }
        public Vector2 Offset { get; private set; }
        public float Scale { get; private set; }
        public float ScaleMin { get; set; } = 2.00f;
        public float ScaleMax { get; set; } = 4.00f;

        public CameraController( ) {
            Target = Vector2.Zero;
            Offset = Vector2.Zero;
            Scale = 2.0f;
        }

        public void SetOffset(float x, float y) {
            Offset = new Vector2(x, y);
        }

        public void SetScale(float scale) {
            Scale = scale <= 0 ? 1.0f : scale;
        }

        public void LookAt(float x, float y) {
            Target = new Vector2(x, y);
        }

        public void LookBy(float x, float y) {
            Target += new Vector2(-x, y) / Scale;
        }

        public void SlideTo(float x, float y, float duration, EaseType ease, Action complete) {
            AnimationHelper.Add(Target.X, x, duration, ease: ease, onUpdate: v => Target = new Vector2((float)v.Current, Target.Y)); 
            AnimationHelper.Add(Target.Y, y, duration, ease: ease, onUpdate: v => Target = new Vector2(Target.X, (float)v.Current), onComplete: _ => complete?.Invoke( ));
        }

        public void ZoomIn(float value) {
            Scale += value;

            if (Scale >= ScaleMax) Scale = ScaleMax;
        }

        public void ZoomOut(float value) {
            Scale -= value;

            if (Scale <= ScaleMin) Scale = ScaleMin;
            if (Scale <= 0) Scale = 1.0f;
        }

        public void Update( ) {
            Matrix = Matrix.CreateTranslation(new Vector3(-Target.X, Target.Y, 0)) *
                     Matrix.CreateScale(Scale) *
                     Matrix.CreateTranslation(new Vector3(Offset, 0));
        }

    }
}
