using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Models;
using GameJAM_Devtober2021.System.Textures;
using GameJAM_Devtober2021.System.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameJAM_Devtober2021.System.Logic {
    public sealed class LevelController {

        private ContentController _content;

        public LevelController(ContentController content) {
            _content = content;
        }

        public bool IsLoaded { get; private set; }
        public LevelModel LevelModel { get; private set; }
        public List<ObjectModel> Objects { get; private set; }

        public void LoadLevel(CameraController camera, int id) {
            string path = Path.Combine("Data", $"level_{id}.json");

            if (!File.Exists(path)) {
                Logger.Error($"Cannot load level [{path}]!");
                return;
            }

            try {
                string data = File.ReadAllText(path);
                LevelModel = JsonConvert.DeserializeObject<LevelModel>(data);
            } catch(Exception e) {
                Logger.Error("Failed level loading on object deserialization!", e);
                return;
            }

            _content.LoadLevelAssets(LevelModel.LevelID);

            try {
                Objects = new List<ObjectModel>( );
                LevelModel.Objects.ForEach(obj => {
                    TextureBase texture = _content.LoadLevelAsset(obj.ID, out ObjectDataModel model);

                    if (texture == null || model == null) {
                        Logger.Error($"Failed level loading on asset '{obj.ID}'!");
                        return;
                    }

                    ObjectModel result = new ObjectModel(texture.GetInstance( ), model, obj.X, obj.Y);
                    Objects.Add(result);
                });
            } catch(Exception e) {
                Logger.Error("Failed level loading on objects loading", e);
                return;
            }

            // camera.LookAt(LevelModel.LevelSpawnX, LevelModel.LevelSpawnY);
            IsLoaded = true;
        }

    }
}
