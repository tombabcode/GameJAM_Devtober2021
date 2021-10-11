using GameJAM_Devtober2021.System.Logic.Items;
using GameJAM_Devtober2021.System.Textures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameJAM_Devtober2021.System.Logic.Objects {
    public sealed class ObjectInstance {

        private int _skinID;

        public int X { get; private set; }
        public int Y { get; private set; }
        public int SkinID { get { return _skinID; } private set { _skinID = value; UpdateSource( ); } }
        public Rectangle TEXSource { get; private set; }

        public List<ItemInstance> Items { get; private set; }

        public int Width => TEXSource.Width;
        public int Height => TEXSource.Height;

        public ObjectData Data { get; private set; }
        public TextureInstance Texture { get; private set; }

        public ObjectInstance(ObjectData data, int x, int y, int skinID = 0) {
            Data = data;
            Texture = data.TextureBase.GetInstance( );

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
