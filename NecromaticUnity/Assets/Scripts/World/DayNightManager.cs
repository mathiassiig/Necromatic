using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Necromatic.World
{
    public class DayNightManager : MonoBehaviour
    {
        [SerializeField] private Light _sunLight;
        [SerializeField] private float _maxDaylight = 1f;
        [SerializeField] private Gradient _dayCycleGradient;
        public float TwentyFourHours = 24f; // an entire game day in seconds
        private float _currentTime = 0;
        public ReactiveProperty<bool> IsDay = new ReactiveProperty<bool>();

        void Start()
        {
            _currentTime = TwentyFourHours / 2f;
        }

        void Update()
        {
            _currentTime = Mathf.Repeat(_currentTime + Time.deltaTime, TwentyFourHours);
            var t = _currentTime / TwentyFourHours;
            IsDay.Value = t > 0.25 && t <= 0.75;
            SetSunlight(t);
            RenderSettings.ambientLight = _dayCycleGradient.Evaluate(t);
        }

        private void SetSunlight(float t)
        {
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
                    return SunUp(i_t) * _maxDaylight;
                }
                else
                {
                    var i_t = (t - 0.5f) * 4;
                    return SunDown(i_t) * _maxDaylight;
                }
            }
        }

        private float SunUp(float x)
        {
            return Mathf.Cos(x * Mathf.PI + Mathf.PI) / 2 + 0.5f;
        }

        private float SunDown(float x)
        {
            return Mathf.Cos(x * Mathf.PI) / 2 + 0.5f;
        }

    }
}