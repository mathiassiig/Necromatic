using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Necromatic.UI.Research
{
    public class AbilityUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _button;

        public void Init(ResearchData data)
        {
            _name.text = data.Name;
            _cost.text = data.Cost.ToString();
            _iconImage.sprite = Resources.Load<Sprite>(data.IconPath);
            Material mat = Instantiate(_iconImage.material);
            _iconImage.material = mat;
            _iconImage.material.SetFloat("_GrayscaleAmount", 1);
            _button.onClick.AddListener(() =>
            {
                Purchase();
                data.OnPurchase();
            });
        }

        public void Purchase()
        {
            _cost.gameObject.SetActive(false);
            _iconImage.material.SetFloat("_GrayscaleAmount", 0);
        }
    }
}