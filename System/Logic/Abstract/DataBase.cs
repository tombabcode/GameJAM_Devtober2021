using GameJAM_Devtober2021.System.Textures;

namespace GameJAM_Devtober2021.System.Logic.Abstract {
    public abstract class DataBase {

        public string ID { get; set; }
        public string Name { get; set; }

        public TextureData TextureData { get; set; }
        public TextureBase TextureBase { get; set; }

    }
}
