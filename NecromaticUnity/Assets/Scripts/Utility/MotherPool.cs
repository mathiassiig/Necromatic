using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character;
using Necromatic.UI;
using UniRx;
using Necromatic.Character.Inventory;

namespace Necromatic.Utility
{
    public class MotherPool : MonoBehaviour
    {
        [Header("Characters")]
        [SerializeField]
        private CharacterInstance _skeleton;
        [SerializeField] private CharacterInstance _human;

        [Header("Utilities")]
        [SerializeField]
        private StatBar _statBar;

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

        public void AddBarToCharacter(CharacterInstance c)
        {
            var bar = Instantiate(_statBar, Vector3.zero, Quaternion.identity);
            bar.transform.SetParent(c.transform);
            bar.transform.localPosition = new Vector3(0, 2.5f, 0);
            c.Health.Current.Subscribe(hp =>
            {
                bar.UpdateStatbar(c.Health.Max.Value, hp);
            });
        }
    }
}