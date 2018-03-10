using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Necromatic.Utility;
using System.Linq;

namespace Necromatic.Character
{
    [System.Serializable]
    public enum Faction
    {
        Undead,
        Human
    }

    public class CharacterInstance : MonoBehaviour
    {
        [SerializeField] private Faction _faction;
        [SerializeField] private Movement _movement;
        [SerializeField] private Transform _weapon;
        [SerializeField] private Death _death;
        [SerializeField] private Representation _representation;
        [SerializeField] private Combat _combat;
        [SerializeField] private Stat _health;

        public Ability CurrentAbility;

        private WeaponAnimator _animator = new WeaponAnimator();
        // accessors
        public Combat Combat => _combat;
        public Faction Faction => _faction;
        public Movement Movement => _movement;
        public Stat Health => _health;
        public Death Death => _death;
        public Representation Representation => _representation;

        void Start()
        {
            Movement.Init(_combat);
            Combat.CurrentState.Subscribe(state =>
            {
                _animator.FireAnimation(state, _weapon, Combat);
            });
            _health.Init();
            _health.Current.Subscribe(value => 
            {
                if(value <= 0)
                {
                    Death.Die();
                }
            });
        }

        public void AttackNearest()
        {
            Combat.TryAttackNearest(this);
        }

        public void DoAbility()
        {
            if(gameObject.tag == "Player")
            {
                CurrentAbility.PlayerFire();
            }
        }
    }
}