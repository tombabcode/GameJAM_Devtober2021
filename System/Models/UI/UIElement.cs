using GameJAM_Devtober2021.System.Types;
using Microsoft.Xna.Framework;

namespace GameJAM_Devtober2021.System.Models.UI {
    public abstract class UIElement {

        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public AlignType Align { get; set; } = AlignType.LT;

        public virtual void Update(GameTime time) { }
        public virtual void Display(GameTime time) { }
        public virtual void Refresh( ) { }

    }
}
