using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorPickerUnity;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace Necromatic.UI
{

    public class StatBar : MonoBehaviour
    {
        [SerializeField] private Image _foreground;
		private bool _initialized = false;
        private Color _colorMax;
        private Color _colorMin;

		void Start()
		{
			Init(Color.green, Color.red);
		}

        public void Init(Color colorMax, Color colorMin)
        {
            _foreground.color = colorMax;
            _colorMax = colorMax;
            _colorMin = colorMin;
        }

        public void UpdateStatbar(float max, float current)
        {
			if(!_initialized)
			{
				Init(Color.green, Color.red);
			}
            float t = Mathf.Clamp(current / max, 0, 1);
            var colorMax = ColorConverter.ConvertColor(_colorMax);
            var colorMin = ColorConverter.ConvertColor(_colorMin);
            float hLerped = Mathf.Lerp(colorMin.H, colorMax.H, t);
            _foreground.color = ColorConverter.ConvertRGBHSL(new RGBHSL(hLerped, colorMax.S, colorMax.L));
			_foreground.fillAmount = t;
            bool zero = current == 0;
            _foreground.gameObject.SetActive(!zero);

        }
    }

}