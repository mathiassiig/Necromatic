using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Managers
{
    public enum SoundEffect
    {
        Step_Grass_A,
        Step_Grass_B,
        Weapon_Sword_Slash_A
    }
    public class SoundManager : Singleton<SoundManager>
    {
        private const string _audioLocation = "Sounds/";
        public Dictionary<SoundEffect, string> SoundEffects = new Dictionary<SoundEffect, string>
        {
            { SoundEffect.Step_Grass_A,   $"{_audioLocation}World/step_grass_a" },
            { SoundEffect.Step_Grass_B,   $"{_audioLocation}World/step_grass_b" },
            { SoundEffect.Weapon_Sword_Slash_A,   $"{_audioLocation}Weapons/weapon_sword_slash_a" }
        };

        public AudioClip GetClip(SoundEffect effect) => Resources.Load<AudioClip>(SoundEffects[effect]);
    }
}