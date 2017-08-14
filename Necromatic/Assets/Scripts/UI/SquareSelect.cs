using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using Necromatic.Char;
using Necromatic.Char.Combat;
using Necromatic.Char.NPC;

namespace Necromatic
{
    public class SquareSelect : MonoBehaviour
    {
        [SerializeField] private Image _selectionImage;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _characterLayer;
        private bool _selectStarted = false;
        private Vector2 _selectionStartPosition;
        private Vector2 _selectionEndPosition;

        private Bounds _selectionBounds = new Bounds();
        private Vector3 _selectionOffset = new Vector3(0.2f, 0.2f, 0.2f);
        private Vector3 _worldSelectionStart;
        private Vector3 _worldSelectionEnd;

        public ReactiveProperty<List<CharacterNPC>> SelectedUnits = new ReactiveProperty<List<CharacterNPC>>();

        public void Select(Vector2 mouseInput)
        {
            if (!_selectStarted)
            {
                _selectionImage.gameObject.SetActive(true);
                _selectStarted = true;
                _selectionStartPosition = mouseInput;
                _worldSelectionStart = GetGroundPosition(mouseInput);
            }
            MoveImage(mouseInput);
            _selectionEndPosition = mouseInput;
        }


        private Vector3 GetGroundPosition(Vector2 mousePos)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
            {
                return hit.point;
            }
            return Vector3.zero; // shouldn't happen
        }

        void FixedUpdate()
        {
            if (_selectStarted)
            {
                SelectUnits(_selectionEndPosition);
            }
        }

        private void SelectUnits(Vector2 mouseInput)
        {
            _worldSelectionEnd = GetGroundPosition(mouseInput);
            _selectionBounds.min = _worldSelectionStart;
            _selectionBounds.max = _worldSelectionEnd + Vector3.up * 5f;
            RescaleBox();
            var characters = Physics.OverlapBox(_selectionBounds.center, _selectionBounds.size/2, Quaternion.identity, _characterLayer);
            if (characters != null)
            {
                SelectedUnits.Value = characters
                    .Select(x => x.GetComponent<CharacterNPC>())
                    .Where(x => x.Combat._characterFaction == Faction.Undead)
                    .ToList();
            }
        }

        private void RescaleBox()
        {
            var min = _selectionBounds.min;
            var max = _selectionBounds.max;
            var pivot = _selectionImage.rectTransform.pivot;
            _selectionBounds.min = new Vector3(min.x + (2 * pivot.x - 1) * _selectionOffset.x, min.y, min.z + (2 * pivot.y - 1) * _selectionOffset.z);
            _selectionBounds.max = new Vector3(max.x - (2 * pivot.x - 1) * _selectionOffset.x, max.y, max.z - (2 * pivot.y - 1) * _selectionOffset.z);

            var size = _selectionBounds.size;
            _selectionBounds.size = new Vector3(size.x * Mathf.Sign(size.x), size.y * Mathf.Sign(size.y), size.z * Mathf.Sign(size.z));
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(_selectionBounds.center, _selectionBounds.size);
        }

        private void MoveImage(Vector2 mouseInput)
        {
            _selectionImage.rectTransform.anchoredPosition = _selectionStartPosition;
            _selectionImage.rectTransform.sizeDelta = (mouseInput - _selectionStartPosition);
            var pivotX = _selectionImage.rectTransform.sizeDelta.x > 0 ? 0 : 1;
            var pivotY = _selectionImage.rectTransform.sizeDelta.y > 0 ? 0 : 1;
            _selectionImage.rectTransform.pivot = new Vector2(pivotX, pivotY);
            var sd = _selectionImage.rectTransform.sizeDelta;
            var sdX = Mathf.Sign(sd.x) * sd.x;
            var sdY = Mathf.Sign(sd.y) * sd.y;
            _selectionImage.rectTransform.sizeDelta = new Vector2(sdX, sdY);
        }

        public void SelectionDone()
        {
            _selectStarted = false;
            _selectionImage.gameObject.SetActive(false);
        }
    }
}