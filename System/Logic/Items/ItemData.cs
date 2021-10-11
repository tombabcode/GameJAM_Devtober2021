﻿using GameJAM_Devtober2021.System.Textures;
using GameJAM_Devtober2021.System.Types;

namespace GameJAM_Devtober2021.System.Logic.Items {
    public sealed class ItemData {

        public string ID { get; set; }
        public string Name { get; set; }

        public TextureData TextureData { get; set; }
        public TextureBase TextureBase { get; set; }

        public ItemCategory[] Categories { get; set; }
        public int Level { get; set; }
        public ItemRarity Rarity { get; set; }
        public ItemStatisticsData Statistics { get; set; }
        public ItemMovesData[] Moves { get; set; }

    }

    public sealed class ItemStatisticsData {
        public int HPMin { get; set; }
        public int HPMax { get; set; }
        public int DamageMin { get; set; }
        public int DamageMax { get; set; }
        public float DodgeChanceMin { get; set; }
        public float DodgeChanceMax { get; set; }
        public float CriticalChanceMin { get; set; }
        public float CriticalChanceMax { get; set; }
    }

    public sealed class ItemMovesData {
        public string ID { get; set; }
        public MoveType Type { get; set; } = MoveType.Passive;
        public int RequiredIP { get; set; } = 0;

        public int GainIP { get; set; } = 0;
        public int GainHPMin { get; set; } = 0;
        public int GainHPMax { get; set; } = 0;
        public int GainDamageMin { get; set; } = 0;
        public int GainDamageMax { get; set; } = 0;
        public float GainDodgeChanceMin { get; set; } = 0;
        public float GainDodgeChanceMax { get; set; } = 0;
        public float GainCriticalChanceMin { get; set; } = 0;
        public float GainCriticalChanceMax { get; set; } = 0;
    }
}
