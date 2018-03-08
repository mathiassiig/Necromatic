using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;

namespace Necromatic.Utility
{
    public class MotherPool : MonoBehaviour
    {
        [Header("Characters")]
        [SerializeField]
        private CharacterInstance _skeleton;
        [SerializeField] private CharacterInstance _human;

        public CharacterInstance GetCharacterPrefab(CharacterType t)
        {
            switch (t)
            {
				case CharacterType.Human:
					return _human;
				case CharacterType.Skeleton:
					return _skeleton;
            }
			throw new System.Exception("$Error, character of type {nameOf(t)} not found");
        }
    }
}