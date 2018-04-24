using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.World.Buildings
{
    public class Wall : MonoBehaviour, IBuilding
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private MeshRenderer _outlineRenderer;
        private Vector3Int _cellPosition;

        private bool _ghosting = false;
        private bool _snapped = false;

        void Awake()
        {
            Ghost();
        }

        public void Ghost()
        {
            _outlineRenderer.gameObject.SetActive(true);
            _renderer.enabled = true;
            _ghosting = true;
        }

        public Vector3Int ReleaseSpot()
        {
            BuildGrid.Instance.Free(_cellPosition);
            Destroy(gameObject);
            return _cellPosition;
        }

        [ContextMenu("Snap")]
        public bool Snap()
        {
            var nearest = BuildGrid.Instance.Grid.WorldToCell(transform.position);
            var available = BuildGrid.Instance.IsFree(nearest);
            if (available)
            {
                gameObject.transform.position = _outlineRenderer.gameObject.transform.position;
                TakeSpot(nearest);
                return true;
            }
            return false;
        }

        public void TakeSpot(Vector3Int cellPosition)
        {
            _cellPosition = cellPosition;
            _ghosting = false;
            _snapped = true;
            _outlineRenderer.gameObject.SetActive(false);
            BuildGrid.Instance.Consume(cellPosition, this);
        }

        void Update()
        {
            if (_ghosting)
            {
                var nearest = BuildGrid.Instance.Grid.WorldToCell(transform.position);
                var nearestCenterWorld = BuildGrid.Instance.Grid.GetCellCenterWorld(nearest);
                _outlineRenderer.gameObject.transform.position = nearestCenterWorld;
                var available = BuildGrid.Instance.IsFree(nearest);
                _outlineRenderer.material.SetColor("_Color", available ? Color.green : Color.red);
            }
        }
    }
}