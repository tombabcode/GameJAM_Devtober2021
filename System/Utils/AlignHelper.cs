using GameJAM_Devtober2021.System.Types;
using Microsoft.Xna.Framework;

namespace GameJAM_Devtober2021.System.Utils {
    public static class AlignHelper {

        public static Vector2 GetOffset(AlignType type) {
            return type switch {
                AlignType.LT => new Vector2(0.0f, 0.0f),
                AlignType.LM => new Vector2(0.0f, 0.5f),
                AlignType.LB => new Vector2(0.0f, 1.0f),
                AlignType.CT => new Vector2(0.5f, 0.0f),
                AlignType.CM => new Vector2(0.5f, 0.5f),
                AlignType.CB => new Vector2(0.5f, 1.0f),
                AlignType.RT => new Vector2(1.0f, 0.0f),
                AlignType.RM => new Vector2(1.0f, 0.5f),
                AlignType.RB => new Vector2(1.0f, 1.0f),
                _ => new Vector2(0, 0),
            };
        }

        public static Vector2 GetAlignedPosition(float offsetX, float offsetY, float x, float y, float width, float height) {
            return new Vector2(
                x - width * offsetX,
                y - height * offsetY
            );
        }

        public static Vector2 GetAlignedPosition(AlignType type, float x, float y, float width, float height) {
            Vector2 offset = GetOffset(type);
            return GetAlignedPosition(offset.X, offset.Y, x, y, width, height);
        }

    }
}
