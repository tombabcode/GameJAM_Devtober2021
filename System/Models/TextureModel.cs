namespace GameJAM_Devtober2021.System.Models {
    public sealed class TextureModel {

        public string Asset { get; set; }
        public string Type { get; set; } = "static";
        public int Columns { get; set; } = 1;
        public int Rows { get; set; } = 1;

    }
}
