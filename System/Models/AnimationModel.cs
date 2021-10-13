using GameJAM_Devtober2021.System.Types;
using System;

namespace GameJAM_Devtober2021.System.Models {
    public sealed class AnimationModel {

        public string ID { get; set; }

        // Values
        public double Source { get; set; }
        public double Target { get; set; }
        public double Current { get; set; }

        // Time
        public double Duration { get; set; }
        public double Delay { get; set; }
        public double Progress => TimePassed / Duration;

        // States
        public bool IsLooped { get; set; } = false;
        public bool IsYoyo { get; set; } = false;
        public AnimationState State { get; set; } = AnimationState.NotStarted;

        public double TimePassed { get; set; }

        public Action<AnimationModel> OnUpdate { get; set; }
        public Action<AnimationModel> OnStart { get; set; }
        public Action<AnimationModel> OnComplete { get; set; }
        public Action<AnimationModel> OnLoop { get; set; }

    }
}
