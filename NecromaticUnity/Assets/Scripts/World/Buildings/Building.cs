using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.AI;

namespace Necromatic.World.Buildings
{
    public class Building : MonoBehaviour, IBuilding
    {
        public int SizeX;
        public int SizeZ;
        private List<MeshRenderer> _renderers;
        private GameObject _clone;
        private List<MeshRenderer> _cloneRenderers;
        private Material _ghostMaterial;

        private List<MeshRenderer> _outlines;

        private bool _ghosting = false;
        private bool _snapped = false;

        public void StopBuilding()
        {
            Destroy(_clone);
            Destroy(gameObject);
        }

        public void Ghost()
        {
            _ghostMaterial = Resources.Load<Material>("Materials/World/Ghost");
            _ghosting = true;

            _clone = Instantiate(gameObject);
            var buildingScript = _clone.GetComponent<IBuilding>();
            Destroy(buildingScript as MonoBehaviour);

            _renderers = GetComponentsInChildren<MeshRenderer>().ToList();
            _renderers.ForEach(x => x.enabled = false);
            _cloneRenderers = _clone.GetComponentsInChildren<MeshRenderer>().ToList();
            _cloneRenderers.ForEach(x => x.material = _ghostMaterial);
        }

        private void SetGhostColor(Color c)
        {
            _cloneRenderers.ForEach(x => x.material.color = c);
        }

        [ContextMenu("Snap")]
        public bool Snap()
        {
            var available = CanTake();
            if (available)
            {
                _renderers.ForEach(x => x.enabled = true);
                gameObject.transform.position = _clone.transform.position;
                TakeSpot();
                var obstacle = GetComponent<NavMeshObstacle>();
                if(obstacle != null)
                {
                    obstacle.carving = true;
                }
                return true;
            }
            return false;
        }

        public void TakeSpot()
        {
            _ghosting = false;
            _snapped = true;
            Destroy(_clone);
            var pos = gameObject.transform.position;
            for (int x = 0; x < SizeX; x++)
            {
                for (int z = 0; z < SizeZ; z++)
                {
                    var nearestWorld = new Vector3(pos.x + x, pos.y, pos.z + z);
                    var nearest = BuildGrid.Instance.Grid.WorldToCell(nearestWorld);
                    BuildGrid.Instance.Consume(nearest, this);
                }
            }
        }

        void Update()
        {
            if (_ghosting)
            {
                var rounded = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
                var nearest = BuildGrid.Instance.Grid.WorldToCell(rounded);
                var nearestWorld = BuildGrid.Instance.Grid.CellToWorld(nearest);
                _clone.transform.position = nearestWorld;
                var available = CanTake();
                SetGhostColor(available ? Color.green : Color.red);
            }
        }

        bool CanTake()
        {
            var pos = _clone.transform.position;
            for (int x = 0; x < SizeX; x++)
            {
                for (int z = 0; z < SizeZ; z++)
                {
                    var nearestWorld = new Vector3(pos.x + x, pos.y, pos.z + z);
                    var nearest = BuildGrid.Instance.Grid.WorldToCell(nearestWorld);
                    if (!BuildGrid.Instance.IsFree(nearest))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        ICollection<Vector3Int> IBuilding.ReleaseSpot()
        {
            throw new System.NotImplementedException();
        }
    }
}