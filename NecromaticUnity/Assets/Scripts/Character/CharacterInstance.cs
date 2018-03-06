using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character
{
    public class CharacterInstance : MonoBehaviour
    {
        [SerializeField] private Movement _movement;
        public Movement Movement => _movement;

        public Combat Combat { get; private set; } = new Combat();

        [SerializeField] private Stat _health;
        public Stat Health => _health;
    }
}