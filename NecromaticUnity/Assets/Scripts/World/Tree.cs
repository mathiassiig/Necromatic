using System.Collections;
using System.Collections.Generic;
using Necromatic.Character;
using UnityEngine;
using UniRx;
using Necromatic.Utility;
using DG.Tweening;

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

        [SerializeField] private Transform _logMesh;
        [SerializeField] private Transform _logRoot;
        private float _logRadius = 0.5f;

        void Awake()
        {
            _combat = new Combat(this);
            _representation = gameObject.AddComponent<Representation>();
            Health._initial = 200;
            Health.Init(this);
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
            var attackerDir = MathUtils.PlaneDirection(_logRoot, Combat.LastAttacker.Value.transform);
            var angle = Mathf.Atan2(attackerDir.x, attackerDir.y);
            var pos = new Vector2(_logRoot.localPosition.x, _logRoot.localPosition.z);
            var newPosition = MathUtils.CirclePoint(pos, _logRadius, angle);
            //Debug.Log(newPosition);
            _logRoot.localPosition = new Vector3(newPosition.x, _logRoot.localPosition.y, newPosition.y);
            _logRoot.localRotation = Quaternion.Euler(0, angle*Mathf.Rad2Deg, 0);
            _logMesh.SetParent(_logRoot);
            _logRoot.DOLocalRotate(new Vector3(-99, _logRoot.localEulerAngles.y, 0), 0.6f);
            //Destroy(_logRoot.gameObject);
        }
    }
}