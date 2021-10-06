using GameJAM_Devtober2021.System.Logic.Abstract;
using GameJAM_Devtober2021.System.Logic.Items;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameJAM_Devtober2021.System.Logic.Objects {
    public sealed class ObjectInstance : InstanceBase {

        private int _skinID;

        public int X { get; private set; }
        public int Y { get; private set; }
        public int SkinID { get { return _skinID; } private set { _skinID = value; UpdateSource( ); } }
        public Rectangle TEXSource { get; private set; }

        public List<ItemInstance> Items { get; private set; }

        public int Width => TEXSource.Width;
        public int Height => TEXSource.Height;

        public ObjectInstance(ObjectData data, int x, int y, int skinID = 0) : base(data) {
            X = x;
            Y = y;
            SkinID = skinID;

            Items = new List<ItemInstance>( ); 
        }

        public void UpdateSource( ) {
            TEXSource = Texture.TextureData.GetSource(SkinID);
        }

    }
}
