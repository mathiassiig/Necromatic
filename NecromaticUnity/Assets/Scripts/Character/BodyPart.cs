using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character
{
    public class BodyPart : MonoBehaviour
    {
        [SerializeField] private CharacterInstance _owner;
        public CharacterInstance Owner => _owner;
    }
}