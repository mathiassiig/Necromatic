﻿using System.Collections;
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
        [SerializeField] private Stat _health;
        [SerializeField] private Transform _weapon;
        [SerializeField] private Death _death;
        private WeaponAnimator _animator = new WeaponAnimator();
        public Combat Combat { get; private set; } = new Combat();
        
        // accessors
        public Faction Faction => _faction;
        public Movement Movement => _movement;
        public Stat Health => _health;
        public Death Death => _death;

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
                    Death.Die();
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
    }
}