namespace GameJAM_Devtober2021.System.Logic {
    public sealed class LevelModel {
        public string LevelID { get; set; }
        public string LevelName { get; set; }

        public int LevelSpawnX { get; set; }
        public int LevelSpawnY { get; set; }

        public LevelObjectInstance[] Objects { get; set; }
    }

    public sealed class LevelObjectInstance {
        public string ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
