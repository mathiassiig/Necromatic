using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Managers
{
    public enum SoundEffect
    {
        StepGrassA,
        StepGrassB,
    }
    public class SoundManager : Singleton<SoundManager>
    {
        private const string _audioLocation = "Sounds/";
        public Dictionary<SoundEffect, string> SoundEffects = new Dictionary<SoundEffect, string>
        {
            { SoundEffect.StepGrassA,   $"{_audioLocation}World/step_grass_a" },
            { SoundEffect.StepGrassB,   $"{_audioLocation}World/step_grass_b" }
        };

        public AudioClip GetClip(SoundEffect effect) => Resources.Load<AudioClip>(SoundEffects[effect]);
    }
}