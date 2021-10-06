using GameJAM_Devtober2021.System.Textures;

namespace GameJAM_Devtober2021.System.Logic.Abstract {
    public abstract class InstanceBase {

        public DataBase Data { get; private set; }
        public TextureInstance Texture { get; private set; }

        public InstanceBase(DataBase data) {
            Data = data;
            Texture = data.TextureBase.GetInstance( );
        }

    }
}
