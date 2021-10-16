using GameJAM_Devtober2021.System.Controllers;
using GameJAM_Devtober2021.System.Logic.Items;
using GameJAM_Devtober2021.System.Logic.Objects;
using GameJAM_Devtober2021.System.Types;
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
        public List<ObjectInstance> Objects { get; private set; }

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

            Random rand = new Random( );

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

                Objects = result;
            } catch(Exception e) {
                Logger.Error("Failed level loading on objects loading", e);
                return;
            }

            // Calculate items count
            int itemsCountMin = LevelModel.Level < 5 ? LevelModel.Level : 5;
            int itemsCountMax = LevelModel.Level < 12 ? LevelModel.Level : 12;
            int itemsCount = (int)(rand.NextDouble( ) * (itemsCountMax - itemsCountMin) + itemsCountMin);

            // Spawn items
            for (int i = 0; i < itemsCount; i++) {
                ObjectInstance[] available = Objects.Where(c => c.Data.IsContainer && c.Items.Count < c.Data.MaxItemsCount).ToArray( );

                if (available.Length == 0) {
                    break;
                }

                // Do as long as item will be picked. Some containers may don't allow specific items, so container will have no items in this level.
                // That's why "while" is used.
                while (available.Length != 0) {

                    // Selected container
                    ObjectInstance container = available[(int)(rand.NextDouble( ) * available.Length)];

                    // Select available items
                    ItemData[] items = _content.ItemsData.Where(item => {
                        if (item.Level > LevelModel.Level)
                            return false;

                        foreach (ItemCategory category in item.Categories)
                            if (!container.Data.Categories.Contains(category))
                                return false;

                        return true;
                    }).ToArray( );

                    // Check if any item was selected. If so - create instance
                    if (items.Length > 0) {
                        ItemData data = items[(int)(rand.NextDouble( ) * items.Length)];
                        ItemInstance instance = new ItemInstance(data);

                        container.Items.Add(instance);
                        break;
                    }
                }
            }

            // camera.LookAt(LevelModel.LevelSpawnX, LevelModel.LevelSpawnY);
            IsLoaded = true;
        }

    }
}
