using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Necromatic.Utility;
using System.Linq;
using Necromatic.Character.Abilities;

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

        private List<Buff> _buffs = new List<Buff>();
        public List<Buff> Buffs => _buffs;

        public void AddBuff(Buff buff)
        {
            _buffs.Add(buff);
        }

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
            Combat.Init(this);
            Combat.CurrentState.Subscribe(state =>
            {
                _animator.FireAnimation(state, _weapon, Combat);
            });
            Movement.Init(_combat);
            _health.Init();
            _health.Current.Subscribe(value =>
            {
                if (value <= 0)
                {
                    Death.Die(this);
                }
            });
            FindObjectOfType<MotherPool>().AddBarToCharacter(this);
        }

        public void AttackNearest()
        {
            Combat.TryAttackNearest(this);
        }

        public void DoAbility()
        {
            if (gameObject.tag == "Player")
            {
                CurrentAbility.PlayerFire(this);
            }
        }
    }
}