using System.Collections.Generic;

namespace GameJAM_Devtober2021.System.Models {
    public class LevelModel {

        public string LevelID { get; set; }
        public string LevelName { get; set; }

        public int LevelSpawnX { get; set; }
        public int LevelSpawnY { get; set; }

        public List<ObjectDescriptionModel> Objects { get; set; }

    }
}
