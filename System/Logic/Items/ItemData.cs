using GameJAM_Devtober2021.System.Logic.Abstract;
using GameJAM_Devtober2021.System.Textures;
using GameJAM_Devtober2021.System.Types;

namespace GameJAM_Devtober2021.System.Logic.Items {
    public sealed class ItemData : DataBase {

        public ItemCategory[] Categories { get; set; }
        public int Level { get; set; }
        public ItemRarity Rarity { get; set; }

    }
}
