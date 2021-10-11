using GameJAM_Devtober2021.System.Textures;
using GameJAM_Devtober2021.System.Types;

namespace GameJAM_Devtober2021.System.Logic.Objects {
    public sealed class ObjectData {

        public string ID { get; set; }
        public string Name { get; set; }

        public TextureData TextureData { get; set; }
        public TextureBase TextureBase { get; set; }

        public bool IsPickable { get; set; }
        public bool IsContainer { get; set; }
        public int MaxItemsCount { get; set; } = 1;
        
        public ItemCategory[] Categories { get; set; }

    }
}
