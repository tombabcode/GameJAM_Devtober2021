namespace GameJAM_Devtober2021.System.Models {
    public class ObjectDataModel {

        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsPickable { get; set; }
        public bool IsContainer { get; set; }

        public TextureModel Texture { get; set; }

    }
}
