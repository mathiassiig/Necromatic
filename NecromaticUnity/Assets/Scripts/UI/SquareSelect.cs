using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using Necromatic.Character;

namespace Necromatic.UI
{
    public class SquareSelect
    {
        private Image _selectionImage;
        private Canvas _selectionCanvasPrefab;

        private bool _selecting = false;
        private Vector2 _selectionStartPosition;
        private Vector2 _selectionEndPosition;
        private RectTransform _mainCanvas;

        private Bounds _selectionBounds = new Bounds();
        private Vector3 _selectionOffset = new Vector3(0.2f, 0.2f, 0.2f);
        private Vector3 _worldSelectionStart;
        private Vector3 _worldSelectionEnd;

        public ReactiveProperty<List<ISelectable>> SelectedUnits = new ReactiveProperty<List<ISelectable>>();
        private System.IDisposable _selectionProcess;

        public void Init(Image selectionImage, Canvas selectionCanvasPrefab)
        {
            _selectionImage = selectionImage;
            SelectedUnits.Value = new List<ISelectable>();
            _mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<RectTransform>();
        }

        public void Select(Vector2 mouseInput)
        {
            foreach (var s in SelectedUnits.Value)
            {
                s.Deselect();
            }
            if (!_selecting)
            {
                SelectedUnits.Value.Clear();
                _selectionImage.gameObject.SetActive(true);
                _selecting = true;
                _selectionStartPosition = mouseInput;
                _worldSelectionStart = GetGroundPosition(mouseInput);
                _selectionProcess = Observable.EveryUpdate().TakeWhile((x) => _selecting).Subscribe(_ =>
                {
                    SelectUnits(_selectionEndPosition);
                });

            }
            MoveImage(mouseInput);
            _selectionEndPosition = mouseInput;
        }

        private Vector3 GetGroundPosition(Vector2 mousePos)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Default")))
            {
                return hit.point;
            }
            return Vector3.zero; // shouldn't happen
        }

        private void SelectUnits(Vector2 mouseInput)
        {
            _worldSelectionEnd = GetGroundPosition(mouseInput);
            _selectionBounds.min = _worldSelectionStart;
            _selectionBounds.max = _worldSelectionEnd + Vector3.up * 5f;
            RescaleBox();
            var characters = Physics.OverlapBox(_selectionBounds.center, _selectionBounds.size / 2, Quaternion.identity, LayerMask.GetMask("Character"));
            var selectables = characters.Where(x => x.GetComponent<ISelectable>() != null).Select(x => x.GetComponent<ISelectable>()).ToList();
            if (selectables != null && selectables.Count > 0)
            {
                SelectedUnits.Value = selectables;
                foreach (var s in SelectedUnits.Value)
                {
                    s.Select();
                }
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

        private void MoveImage(Vector2 mouseInput)
        {
            var scale_x = _mainCanvas.sizeDelta.x / Screen.width;
            var scale_y = _mainCanvas.sizeDelta.y / Screen.height;
            _selectionImage.rectTransform.anchoredPosition = new Vector2(_selectionStartPosition.x * scale_x, _selectionStartPosition.y * scale_y);
            _selectionImage.rectTransform.sizeDelta = (mouseInput - _selectionStartPosition);
            var pivotX = _selectionImage.rectTransform.sizeDelta.x > 0 ? 0 : 1;
            var pivotY = _selectionImage.rectTransform.sizeDelta.y > 0 ? 0 : 1;
            _selectionImage.rectTransform.pivot = new Vector2(pivotX, pivotY);
            var sd = _selectionImage.rectTransform.sizeDelta;
            var sdX = Mathf.Sign(sd.x) * sd.x;
            var sdY = Mathf.Sign(sd.y) * sd.y;
            _selectionImage.rectTransform.sizeDelta = new Vector2(scale_x * sdX, scale_y * sdY);
        }

        public void SelectionDone()
        {
            _selecting = false;
            _selectionImage.gameObject.SetActive(false);
        }
    }
}