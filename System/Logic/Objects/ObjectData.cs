using GameJAM_Devtober2021.System.Logic.Abstract;
using GameJAM_Devtober2021.System.Types;

namespace GameJAM_Devtober2021.System.Logic.Objects {
    public sealed class ObjectData : DataBase {

        public bool IsPickable { get; set; }
        public bool IsContainer { get; set; }
        
        public ItemCategory[] Categories { get; set; }

    }
}
