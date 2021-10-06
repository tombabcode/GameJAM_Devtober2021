using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Logic.Items;
using GameJAM_Devtober2021.System.Logic.Objects;
using GameJAM_Devtober2021.System.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameJAM_Devtober2021.System.Logic {
    public sealed class LevelController {

        private ContentController _content;

        public bool IsLoaded { get; private set; }
        public LevelModel LevelModel { get; private set; }
        public ObjectInstance[] Objects { get; private set; }

        public LevelController(ContentController content) {
            _content = content;
        }

        public void LoadLevel(CameraController camera, int id) {
            // Load path
            string path = Path.Combine("Data", $"level_{id}.json");

            // Check if level's file exists
            if (!File.Exists(path)) {
                Logger.Error($"Cannot load level [{path}]!");
                return;
            }

            // Load level data
            try {
                string data = File.ReadAllText(path);
                LevelModel = JsonConvert.DeserializeObject<LevelModel>(data);
                _content.LoadLevelAssets(LevelModel.LevelID);
            } catch(Exception e) {
                Logger.Error("Failed level loading on object deserialization!", e);
                return;
            }

            // Load objects
            try {
                List<ObjectInstance> result = new List<ObjectInstance>( );
                
                foreach (LevelObjectInstance obj in LevelModel.Objects) {
                    ObjectData data = _content.ObjectsData.FirstOrDefault(o => o.ID == obj.ID);
                    ObjectInstance instance = new ObjectInstance(data, obj.X, obj.Y);

                    // Generate items
                    instance.Items.Add(new ItemInstance(_content.ItemsData[0]));

                    result.Add(instance);
                }

                Objects = result.ToArray( );
            } catch(Exception e) {
                Logger.Error("Failed level loading on objects loading", e);
                return;
            }

            // camera.LookAt(LevelModel.LevelSpawnX, LevelModel.LevelSpawnY);
            IsLoaded = true;
        }

    }
}
