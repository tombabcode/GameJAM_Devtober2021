using Microsoft.Xna.Framework;

namespace GameJAM_Devtober2021.System.Controllers {
    public sealed class CameraController {

        public Matrix Matrix { get; private set; }
        public Vector2 Target { get; private set; }
        public Vector2 Offset { get; private set; }
        public float Scale { get; private set; }

        public CameraController( ) {
            Target = Vector2.Zero;
            Offset = Vector2.Zero;
            Scale = 1.0f;
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
            Target += new Vector2(x, y);
        }

        public void ZoomIn(float value) {
            Scale += value;
        }

        public void ZoomOut(float value) {
            Scale -= value;
            if (Scale <= 0) {
                Scale = 0.25f;
            }
        }

        public void Update( ) {
            Matrix = Matrix.CreateScale(Scale) *
                     Matrix.CreateTranslation(new Vector3(Offset, 0)) *
                     Matrix.CreateTranslation(new Vector3(Target, 0));
        }

    }
}
