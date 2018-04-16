using System.Collections;
using System.Collections.Generic;
using Necromatic.Character;
using UnityEngine;
using UniRx;
using Necromatic.Utility;
using DG.Tweening;
using Necromatic.Character.NPC.Strategies.Results;
using Necromatic.Character.NPC.Strategies;

namespace Necromatic.World
{
    public class Tree : MonoBehaviour, IDamagable, IClickReceiver
    {
        public Stat Health { get; private set; } = new Stat();

        private Combat _combat;
        public Combat Combat => _combat;

        private Death _death = new Death();
        public Death Death => _death;
        public bool Cut { get; private set; }
        public bool Fallen { get; private set; }

        private Representation _representation;
        public Representation Representation => _representation;

        [SerializeField] private Transform _logMesh;
        [SerializeField] private Transform _logRoot;
        [SerializeField] private Transform _foilage;
        [SerializeField] private Transform _logLower;
        [SerializeField] private Transform _logUpper;

        public List<Transform> Logs = new List<Transform>();

        private float _logRadius = 0.5f;

        void Awake()
        {
            _combat = new Combat(this);
            _representation = gameObject.AddComponent<Representation>();
            Health._initial = 40;
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

        private void Fall()
        {
            var attackerDir = MathUtils.PlaneDirection(_logRoot, Combat.LastAttacker.Value.transform);
            var angle = Mathf.Atan2(attackerDir.x, attackerDir.y);
            var pos = new Vector2(_logRoot.localPosition.x, _logRoot.localPosition.z);
            var newPosition = MathUtils.CirclePoint(pos, _logRadius, angle);
            _logRoot.localPosition = new Vector3(newPosition.x, _logRoot.localPosition.y, newPosition.y);
            _logRoot.localRotation = Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0);
            _logMesh.SetParent(_logRoot);
            var time = 0.6f;
            var collider = GetComponent<Collider>();
            Logs.Add(_logLower);
            Logs.Add(_logUpper);
            _logRoot.DOLocalRotate(new Vector3(-90, _logRoot.localEulerAngles.y, 0), time)
                .SetEase(Ease.InExpo)
                .OnComplete(() =>
                {
                    Destroy(_foilage.gameObject);
                    AddPhysics(_logLower.gameObject, collider, attackerDir);
                    AddPhysics(_logUpper.gameObject, collider, attackerDir);
                    Fallen = true;
                });
        }

        private void AddPhysics(GameObject g, Collider treeCollider, Vector3 falldir)
        {
            g.transform.SetParent(null);
            var rb = g.AddComponent<Rigidbody>();
            rb.mass = 40;
            var mc = g.AddComponent<MeshCollider>();
            Physics.IgnoreCollision(mc, treeCollider, true);
            mc.convex = true;
            mc.sharedMesh = g.GetComponent<MeshFilter>().mesh;
            rb.AddTorque(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f));
            rb.AddForce(Vector3.up * 150, ForceMode.Impulse);

        }

        public void Click(List<ISelectable> senders)
        {
            foreach (var sender in senders)
            {
                if (sender.Inventory.Has(Character.Inventory.SpecialType.Axe))
                {
                    sender.Inventory.EquipSpecial(Character.Inventory.SpecialType.Axe, ItemSlotLocation.Weapon);
                    sender.AI.SetPrimaryStrategy(new SearchForTrees());
                    var cutThis = new TreeSpottedResult(this);
                    sender.AI.AddTask(cutThis);
                }
                else
                {
                    Debug.Log("You need an axe"); // todo
                }
            }
        }
    }
}