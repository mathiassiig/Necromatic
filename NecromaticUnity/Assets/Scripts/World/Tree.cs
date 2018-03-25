using System.Collections;
using System.Collections.Generic;
using Necromatic.Character;
using UnityEngine;
using UniRx;

namespace Necromatic.World
{
    public class Tree : MonoBehaviour, IDamagable
    {
        public Stat Health { get; private set; } = new Stat();

        private Combat _combat;
        public Combat Combat => _combat;

        private Death _death = new Death();
        public Death Death => _death;
        public bool Cut { get; private set; }

        private Representation _representation;
        public Representation Representation => _representation;

        [SerializeField] private Transform _tree;
        void Awake()
        {
            _combat = new Combat(this);
            _representation = gameObject.AddComponent<Representation>();
            Health._initial = 100;
            Health.Init();
            Health.Current.Subscribe(x =>
            {
                if (x <= 0)
                {
                    Fall();
                    Cut = true;
                }
            });

        }

        void Fall()
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            Destroy(_tree.gameObject);
        }
    }
}