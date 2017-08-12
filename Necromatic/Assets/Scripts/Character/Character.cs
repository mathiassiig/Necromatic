using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Necromatic.UI;
using Necromatic.Character.Combat;
namespace Necromatic.Character
{
    public class Character : MonoBehaviour
    {
        [Header("Stats")]
        public Stat Health;
        [SerializeField] private Vector3 _healthBarOffset = new Vector3(0, 2, 0);
        private StatBar _healthBar;
        public ReactiveProperty<bool> IsDead = new ReactiveProperty<bool>();

        [Header("Submodules")]
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private CharacterCombat _combat;

        // public get-accessors
        public CharacterMovement Movement => _movement;
        public CharacterCombat Combat => _combat;

        void Awake()
        {
            Init();
        }

        private void InitBar()
        {
            var prefab = Resources.Load<StatBar>("Prefabs/UI/StatBar");
            _healthBar = Instantiate(prefab);
            transform.ObserveEveryValueChanged(t=>t.position).Subscribe(pos =>
            {
                _healthBar.transform.position = pos + _healthBarOffset;
            });
            _healthBar.Init(Color.green, Color.red);
        }

        protected virtual void Init()
        {
            InitBar();
            Health.Init();
            IsDead.Subscribe(x =>
            {
                if (x) HandleDeath();
            });
            Health.Current.Subscribe(x =>
            {
                if (x <= 0) IsDead.Value = true;
                _healthBar.UpdateStatbar(Health.Max.Value, Health.Current.Value);
            });
        }

        protected virtual void HandleDeath()
        {
            Destroy(_healthBar.gameObject);
            Destroy(gameObject);
        }
    }
}