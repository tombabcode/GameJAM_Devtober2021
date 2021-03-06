using GameJAM_Devtober2021.System.Models;
using GameJAM_Devtober2021.System.Types;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using STATE = GameJAM_Devtober2021.System.Types.AnimationState;

namespace GameJAM_Devtober2021.System.Utils {
    public static class AnimationHelper {

        public static List<AnimationModel> Animations { get; set; } = new List<AnimationModel>( );

        public static void Update(GameTime time) {
            for (int i = 0; i < Animations.Count; i++) {
                AnimationModel model = Animations[i];

                // Animation is paused
                if (model.State == STATE.Paused)
                    continue;

                // Animation is stopped (remove)
                if (model.State == STATE.Stopped) {
                    model.OnComplete?.Invoke(model);
                    Animations.RemoveAt(i);
                    i++;
                    continue;
                }

                // Get elapsed time
                double elapsed = time.ElapsedGameTime.TotalMilliseconds;

                // If animation is not started
                if (model.State == STATE.NotStarted) {
                    double delay = model.Delay - elapsed;

                    // Delay not matched
                    if (delay > 0) {
                        model.Delay = delay;
                        continue;
                    }

                    // Update elapsed and start animation
                    elapsed = delay == 0 ? 0 : Math.Abs(delay);
                    model.State = STATE.PlayingForward;
                    model.OnStart?.Invoke(model);
                }

                // Calculate passed time
                double passed = model.TimePassed + elapsed;

                // If passed time is greater than duration (animation finished)
                if (passed >= model.Duration) {
                    // If animations is looped, continue calculating, otherwise reset
                    model.TimePassed = model.IsLooped ? passed % model.Duration : 0;

                    // If there is no loop, finish animation
                    if (!model.IsLooped) {
                        model.OnComplete?.Invoke(model);
                        model.State = STATE.Stopped;
                        Animations.RemoveAt(i);
                        i++;
                        continue;
                    }

                    // On loop
                    model.OnLoop?.Invoke(model);

                    // Reverse if yoyo
                    if (model.IsYoyo)
                        model.State = model.State == STATE.PlayingForward ? STATE.PlayingReversed : STATE.PlayingForward;

                // Animation is going on, simple pass time
                } else {
                    model.TimePassed = passed;
                }

                // Update position
                model.Current = Ease(model.Ease, model.Source, model.Target, model.State == STATE.PlayingForward ? model.Progress : 1 - model.Progress);
                // model.Current = model.Progress * (model.Target - model.Source) + model.Source;
                // model.Current = (1 - model.Progress) * (model.Target - model.Source) + model.Source;

                // On update
                model.OnUpdate?.Invoke(model);
            }
        }

        public static AnimationModel Add(double source, double target, double duration, double delay = 0, bool loop = false, bool yoyo = false, EaseType ease = EaseType.Linear, Action<AnimationModel> onUpdate = null, Action<AnimationModel> onStart = null, Action<AnimationModel> onComplete = null, Action<AnimationModel> onLoop = null) {
            AnimationModel model = new AnimationModel( ) {
                ID = Guid.NewGuid( ).ToString( ),
                Source = source,
                Target = target,
                Current = source,
                Duration = duration,
                Delay = delay,
                IsLooped = loop,
                IsYoyo = yoyo,
                Ease = ease,
                OnUpdate = onUpdate,
                OnStart = onStart,
                OnComplete = onComplete,
                OnLoop = onLoop
            };

            Animations.Add(model);

            return model;
        }

        public static void Pause(AnimationModel model) {
            model.State = STATE.Paused;
        }

        public static void Pause(string id) {
            AnimationModel model = Animations.FirstOrDefault(anim => anim.ID == id);
            if (model != null)
                model.State = STATE.Paused;
        }

        public static void Stop(AnimationModel model) {
            model.State = STATE.Stopped;
        }

        public static void Stop(string id) {
            AnimationModel model = Animations.FirstOrDefault(anim => anim.ID == id);
            if (model != null)
                model.State = STATE.Stopped;
        }

        public static double Ease(EaseType ease, double min, double max, double time) {
            double value = 0;

            switch (ease) {
                case EaseType.Linear: value = time; break;
                case EaseType.CubicIn: value = Math.Pow(time, 3); break;
                case EaseType.CubicOut: value = 1 - Math.Pow(1 - time, 3); break;
                case EaseType.CubicInOut: value = time < .5f ? 4 * Math.Pow(time, 3) : 1 - Math.Pow(-2 * time + 2, 3) / 2; break;
            }

            return (max - min) * value + min;
        }

    }
}
