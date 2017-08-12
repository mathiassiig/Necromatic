using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Necromatic
{
    public class SquareSelect : MonoBehaviour
    {
        [SerializeField]
        private Image _selectionImage;
        private bool _selectStarted = false;
        private Vector2 _selectionStartPosition;
        public void Select(Vector2 mouseInput)
        {
            _selectionImage.gameObject.SetActive(true);
            if (!_selectStarted)
            {
                _selectStarted = true;
                _selectionStartPosition = mouseInput;
            }
            _selectionImage.rectTransform.anchoredPosition = _selectionStartPosition;
            _selectionImage.rectTransform.sizeDelta = (mouseInput-_selectionStartPosition);
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