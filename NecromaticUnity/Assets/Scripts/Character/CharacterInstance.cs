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
        public Faction Faction => _faction;

        [SerializeField] private Movement _movement;
        public Movement Movement => _movement;

        public Combat Combat { get; private set; } = new Combat();

        [SerializeField] private Stat _health;
        public Stat Health => _health;

        [SerializeField] private Transform _weapon;
        private WeaponAnimator _animator = new WeaponAnimator();

        public readonly ReactiveProperty<bool> Dead = new ReactiveProperty<bool>(false);

        void Start()
        {
            Movement.Init(Combat);
            Combat.CurrentState.Subscribe(state =>
            {
                _animator.FireAnimation(state, _weapon, Combat);
            });
            _health.Init();
            _health.Current.Subscribe(value => 
            {
                if(value <= 0)
                {
                    Die();
                }
            });
        }

        public void AttackNearest()
        {
            var enemies = GameObjectUtils.DetectEnemies(5, transform.position, this); // todo: expose range
            if (enemies != null && enemies.Count != 0)
            {
                var nearest = enemies.FirstOrDefault(e => e.transform == GameObjectUtils.Closest(enemies.Select(x => x.transform).ToList(), transform));
                Combat.TryAttack(nearest);
            }
        }
        
        protected virtual void Die()
        {
            Dead.Value = true;
            Destroy(gameObject);
        }
    }
}