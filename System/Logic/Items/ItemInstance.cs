using GameJAM_Devtober2021.System.Textures;
using System;

namespace GameJAM_Devtober2021.System.Logic.Items {
    public sealed class ItemInstance {

        public int Health { get; private set; }
        public float DodgeChance { get; private set; }
        public float CriticalChance { get; private set; }

        public ItemData Data { get; private set; }
        public TextureInstance Texture { get; private set; }

        public ItemInstance(ItemData data) {
            Data = data;
            Texture = data.TextureBase.GetInstance( );

            Random rand = new Random( );
            ItemStatisticsData stats = data.Statistics;

            // Set stats
            Health = (int)(rand.NextDouble( ) * (stats.HPMax - stats.HPMin) + stats.HPMin);
            DodgeChance = (float)(rand.NextDouble( ) * (stats.DodgeChanceMax - stats.DodgeChanceMin) + stats.DodgeChanceMin);
            CriticalChance = (float)(rand.NextDouble( ) * (stats.CriticalChanceMax - stats.CriticalChanceMin) + stats.CriticalChanceMin);
        }

    }
}
