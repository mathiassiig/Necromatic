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
        [SerializeField] protected Faction _faction;
        [SerializeField] protected Movement _movement;
        [SerializeField] protected Transform _weapon;
        [SerializeField] protected Death _death;
        [SerializeField] protected Representation _representation;
        [SerializeField] protected Combat _combat;
        [SerializeField] protected Stat _health;

        public Ability CurrentAbility;

        protected WeaponAnimator _animator = new WeaponAnimator();
        // accessors
        public Combat Combat => _combat;
        public Faction Faction => _faction;
        public Movement Movement => _movement;
        public Stat Health => _health;
        public Death Death => _death;
        public Representation Representation => _representation;

        void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            if(_death == null)
            {
                _death = new Death();
            }
            Movement.Init(_combat);
            Combat.CurrentState.Subscribe(state =>
            {
                _animator.FireAnimation(state, _weapon, Combat);
            });
            _health.Init();
            _health.Current.Subscribe(value =>
            {
                if (value <= 0)
                {
                    Death.Die(this);
                }
            });
        }

        public void AttackNearest()
        {
            Combat.TryAttackNearest(this);
        }

        public void DoAbility()
        {
            if (gameObject.tag == "Player")
            {
                CurrentAbility.PlayerFire();
            }
        }
    }
}