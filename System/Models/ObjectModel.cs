using GameJAM_Devtober2021.System.Textures;

namespace GameJAM_Devtober2021.System.Models {
    public sealed class ObjectModel {

        public TextureInstance Texture { get; private set; }
        public ObjectDataModel DataModel { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int SkinID { get; private set; }
        
        public ObjectModel(TextureInstance texture, ObjectDataModel dataModel, int x, int y, int skinID = 0) {
            Texture = texture;
            DataModel = dataModel;

            X = x;
            Y = y;
            SkinID = 0;
        }

    }
}
