using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
using System.Linq;
namespace Necromatic.Char.NPC
{
    public class UndeadWorkerNPC : UndeadNPC
    {
        [SerializeField] private GameObject _resourceWood;
        [SerializeField] private GameObject _resourceBag;
        [SerializeField] private LayerMask _trees;

        private float _treeSearchRadius = 10;
        private bool _hasTree => _currentTree != null;
        private ResourceTree _currentTree;

        void Awake()
        {
            Init();
        }

        protected override void Think()
        {
            if (!_hasTree)
            {
                FindTree(); // look for trees
                base.Think(); // and enemies
            }
            // if you have a tree, just focus on that until told otherwise
        }

        protected override void NPCUpdate()
        {
            if (_hasTree && Vector3Utils.XZDistanceGreater(transform.position, _currentTree.transform.position, 1f))
            {
                _npcMovement.NavigateTo(_currentTree.transform.position);
            }
            else if(_hasTree)
            {
                CutTree();
            }
            else
            {
                base.NPCUpdate();
            }
        }

        private void CutTree()
        {
            var force = (_currentTree.transform.position - transform.position).normalized;
            _currentTree.Timber(force);
            _currentTree = null;
        }

        private void FindTree()
        {
            var transforms = Physics.OverlapSphere(transform.position, _treeSearchRadius, _trees).Select(x => x.transform).ToArray();
            if(transforms == null || transforms.Length == 0)
            {
                return;
            }
            var closestTransform = Vector3Utils.GetClosestTransform(transforms, transform);
            var closestTree = transforms.FirstOrDefault(x => x == closestTransform).GetComponent<ResourceTree>();
            if(closestTree != null)
            {
                _currentTree = closestTree;
            }
        }
    }
}