using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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

    }
}
