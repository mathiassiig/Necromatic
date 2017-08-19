using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;
namespace Necromatic.Char.Combat
{
    public enum Faction
    {
        Human,
        Undead
    }

    public class CharacterCombat : MonoBehaviour
    {
        [SerializeField]
        private WeaponBase _weapon;
        [SerializeField]
        private float _timeBeforeHit = 0.4f;
        [SerializeField]
        private Animator _animator;

        public Character CharacterScript
        {
            get
            {
                if (_characterScript == null)
                {
                    _characterScript = GetComponentInParent<Character>();
                }
                return _characterScript;
            }
        }

        private Character _characterScript;

        public Hurtable CurrentTarget { get; private set; }
        public Faction _characterFaction;

        public ReactiveProperty<bool> Attacking = new ReactiveProperty<bool>();
        public WeaponBase Weapon => _weapon;

        public Character GetEnemy(float range)
        {
            var colliders = Physics.OverlapSphere(transform.position, range);
            var enemies = new List<Character>();
            foreach (var collider in colliders)
            {
                var combatScript = collider.gameObject.GetComponentInChildren<CharacterCombat>();
                if (combatScript != null && combatScript._characterFaction != _characterFaction)
                {
                    enemies.Add(collider.gameObject.GetComponent<Character>());
                }
            }
            if (enemies.Count > 0)
            {
                var minDistance = float.MaxValue;
                Character toReturn = null;
                foreach (var enemy in enemies)
                {
                    if (!enemy.IsDead.Value)
                    {
                        var dis = (enemy.transform.position - transform.position).magnitude;
                        if (dis < minDistance)
                        {
                            minDistance = dis;
                            toReturn = enemy;
                        }
                    }
                }
                return toReturn;
            }
            return null;
        }

        public void DoAttack()
        {
            _weapon.Attack(CurrentTarget, CharacterScript);
        }

        public UniRx.IObservable<long> AttackAnimation(float time)
        {
            // attack animation is 24 frames, fetch it automagically later
            Attacking.Value = true;
            var baseTime = 0.24f;
            var multiplier = baseTime / time;
            _animator.SetFloat("AttackSpeed", multiplier);
            _animator.SetBool("Attack", true);
            var obs = Observable.Timer(TimeSpan.FromSeconds(time)).TakeUntilDestroy(this);
            obs.Subscribe(_ =>
            {
                Attacking.Value = false;
                _animator.SetBool("Attack", false);
            });
            return obs;
        }

        // start the attacking, triggering the animator
        public void InitAttack(Hurtable target)
        {
            CurrentTarget = target;
            AttackAnimation(_weapon.Cooldown);
        }

        // can we attack, then initialize the attack
        public void TryAttack() // for user
        {
            var enemy = CurrentTarget ?? GetEnemy(_weapon.Range); // already have a target? otherwise fetch another
            if (enemy != null && _weapon.CanAttack.Value)
            {
                InitAttack(enemy);
            }
        }
    }
}