using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.World
{
    public class DayNightManager : MonoBehaviour
    {
        [SerializeField] private Light _sunLight;
        [SerializeField] private float _maxDaylight = 1f;
        public float TwentyFourHours = 24f; // an entire game day in seconds
        private float _currentTime = 0;

        void Start()
        {
            _currentTime = TwentyFourHours / 2f;
        }

        void Update()
        {
            _currentTime = Mathf.Repeat(_currentTime + Time.deltaTime, TwentyFourHours);
            SetSunlight();

        }

        private void SetSunlight()
        {
            var t = _currentTime / TwentyFourHours;
            var pitch = Mathf.Repeat(t * 2 * Mathf.PI - Mathf.PI / 2, 2 * Mathf.PI);
            _sunLight.transform.localRotation = Quaternion.Euler(pitch * Mathf.Rad2Deg, 0, 0);
			_sunLight.intensity = CalculateLight(t);
        }

        private float CalculateLight(float t)
        {
            if (t < 0.25f)
            {
                return 0;
            }
            else if (t > 0.75f)
            {
                return 0;
            }
            else
            {
                if (t < 0.5)
                {
                    var i_t = (t - 0.25f) * 4;
					return CubicOut(i_t) * _maxDaylight;
                }
                else
                {
					var i_t = (t - 0.5f) * 4;
					return CubicIn(i_t) * _maxDaylight;
                }
            }
        }

        private float CubicOut(float x)
        {
            return Mathf.Pow(x, 3);
        }

		private float CubicIn(float x)
		{
			return - Mathf.Pow(x, 3) + 1;
		}

    }
}