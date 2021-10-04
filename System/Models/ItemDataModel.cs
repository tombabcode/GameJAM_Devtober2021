using GameJAM_Devtober2021.System.Types;

namespace GameJAM_Devtober2021.System.Models {
    public class ItemDataModel {

        public string ID { get; set; }
        public string Name { get; set; }
        public ItemCategoryType[] Categories { get; set; } 

        public TextureModel Texture { get; set; }

    }
}
