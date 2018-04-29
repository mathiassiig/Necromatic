using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.AI;
using System;

namespace Necromatic.World.Buildings
{
    public enum Rotation
    {
        None,
        Quarter,
        TwoQuarter,
        ThreeQuarter
    }
    public class Building : MonoBehaviour, IBuilding
    {
        public int SizeX;
        public int SizeZ;
        private Vector3 _offset;
        public Vector3 Offset => _offset;
        private NavMeshObstacle _obstacle;
        private List<MeshRenderer> _renderers;
        private GameObject _clone;
        private List<MeshRenderer> _cloneRenderers;
        private Material _ghostMaterial;

        private List<MeshRenderer> _outlines;

        private bool _ghosting = false;
        private bool _snapped = false;

        private List<Vector3> _cellPositionsLocal;

        private void RegenerateOffset()
        {
            var rot = GetRotation();
            switch (rot)
            {
                case Rotation.None:
                    _offset = new Vector3(-SizeX / 2f, 0, -SizeZ / 2f);
                    break;
                case Rotation.Quarter:
                    _offset = new Vector3(-SizeZ / 2f, 0, SizeX / 2f);
                    break;
                case Rotation.TwoQuarter:
                    _offset = new Vector3(SizeX / 2f, 0, SizeZ / 2f);
                    break;
                case Rotation.ThreeQuarter:
                    _offset = new Vector3(SizeZ / 2f, 0, -SizeX / 2f);
                    break;
            }
        }

        private void RegenerateCells()
        {
            _cellPositionsLocal = new List<Vector3>();
            var rot = GetRotation();
            switch(rot)
            {
                case Rotation.None:
                    for (int x = 0; x < SizeX; x++)
                    {
                        for (int z = 0; z < SizeZ; z++)
                        {
                            var local = new Vector3(x, 0, z);
                            _cellPositionsLocal.Add(local);
                        }
                    }
                    break;
                case Rotation.Quarter:
                    for (int x = 0; x < SizeZ; x++)
                    {
                        for (int z = 0; z < SizeX; z++)
                        {
                            var local = new Vector3(x, 0, -z - 1);
                            _cellPositionsLocal.Add(local);
                        }
                    }
                    break;
                case Rotation.TwoQuarter:
                    for (int x = 0; x < SizeX; x++)
                    {
                        for (int z = 0; z < SizeZ; z++)
                        {
                            var local = new Vector3(-x - 1, 0, -z -1);
                            _cellPositionsLocal.Add(local);
                        }
                    }
                    break;
                case Rotation.ThreeQuarter:
                    for (int x = 0; x < SizeZ; x++)
                    {
                        for (int z = 0; z < SizeX; z++)
                        {
                            var local = new Vector3(-x - 1, 0, z);
                            _cellPositionsLocal.Add(local);
                        }
                    }
                    break;
            }
        }

        private Rotation GetRotation()
        {
            var rot = _clone.transform.eulerAngles.y;
            rot = Mathf.Repeat(rot, 360f);
            if (Mathf.Approximately(rot, 0))
            {
                return Rotation.None;
            }
            else if (Mathf.Approximately(rot, 90))
            {
                return Rotation.Quarter;
            }
            else if (Mathf.Approximately(rot, 180))
            {
                return Rotation.TwoQuarter;
            }
            else if (Mathf.Approximately(rot, 270))
            {
                return Rotation.ThreeQuarter;
            }
            Debug.LogWarning("Building rotated in non-perpendicular fashion: " + rot);
            return Rotation.None;
        }

        public void StopBuilding()
        {
            Destroy(_clone);
            Destroy(gameObject);
        }

        void OnDrawGizmos()
        {
            if (_cellPositionsLocal != null && _clone != null)
            {
                Gizmos.color = new Color(1, 1, 0, 0.88f);
                foreach (var b in _cellPositionsLocal)
                {
                    Gizmos.DrawCube(b + _clone.transform.position + new Vector3(0.5f, 0, 0.5f), new Vector3(1, 0.1f, 1));
                }
                Gizmos.color = new Color(0, 1, 1, 0.5f);
                Gizmos.DrawCube(_clone.transform.position - _offset + new Vector3(0, _obstacle.size.y/2f, 0), 
                    new Vector3(Mathf.Abs(_offset.x)*2, _obstacle.size.y, Mathf.Abs(_offset.z)*2));
            }
        }

        public void Ghost()
        {
            _ghostMaterial = Resources.Load<Material>("Materials/World/Ghost");
            _ghosting = true;
            _obstacle = GetComponent<NavMeshObstacle>();
            if(_obstacle != null)
            {
                _obstacle.enabled = false;
            }

            _clone = Instantiate(gameObject);
            var buildingScript = _clone.GetComponent<IBuilding>();
            Destroy(buildingScript as MonoBehaviour);

            _renderers = GetComponentsInChildren<MeshRenderer>().ToList();
            _renderers.ForEach(x => x.enabled = false);
            _cloneRenderers = _clone.GetComponentsInChildren<MeshRenderer>().ToList();
            _cloneRenderers.ForEach(x => x.material = _ghostMaterial);
            RegenerateCells();
            RegenerateOffset();
        }

        private void SetGhostColor(Color c)
        {
            _cloneRenderers.ForEach(x => x.material.color = c);
        }

        public void Rotate()
        {
            _clone.transform.Rotate(Vector3.up * 90);
            RegenerateCells();
            RegenerateOffset();
        }

        [ContextMenu("Snap")]
        public bool Snap()
        {
            var available = CanTake();
            if (available)
            {
                _renderers.ForEach(x => x.enabled = true);
                gameObject.transform.position = _clone.transform.position;
                gameObject.transform.rotation = _clone.transform.rotation;
                TakeSpot();
                if (_obstacle != null)
                {
                    _obstacle.enabled = true;
                    _obstacle.carving = true;
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
            _cellPositionsLocal.ForEach(x => BuildGrid.Instance.Consume(BuildGrid.Instance.Grid.WorldToCell(x + _clone.transform.position), this));
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
            if(_obstacle != null)
            {
                var colliders = Physics.OverlapBox(_clone.transform.position - _offset + new Vector3(0, _obstacle.size.y / 2f + 0.1f, 0),
                    new Vector3(Mathf.Abs(_offset.x), _obstacle.size.y/2f, Mathf.Abs(_offset.z)));
                if(colliders.Length > 0)
                {
                    return false;
                }
            }
            foreach (var p in _cellPositionsLocal)
            {
                var nearestWorld = p + _clone.transform.position;
                var nearest = BuildGrid.Instance.Grid.WorldToCell(nearestWorld);
                if (!BuildGrid.Instance.IsFree(nearest))
                {
                    return false;
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