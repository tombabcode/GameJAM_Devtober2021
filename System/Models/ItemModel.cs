using GameJAM_Devtober2021.System.Textures;

namespace GameJAM_Devtober2021.System.Models {
    public sealed class ItemModel {

        public TextureInstance Texture { get; private set; }
        public ItemDataModel DataModel { get; private set; }

        public int SkinID { get; private set; }

        public ItemModel(TextureInstance texture, ItemDataModel dataModel) {
            Texture = texture;
            DataModel = dataModel;
        }

    }
}
