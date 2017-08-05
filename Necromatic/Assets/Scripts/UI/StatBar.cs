using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.UI
{

    public class StatBar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _foreground;
        [SerializeField] private float _barWidth = 10;
        private Color _colorMax;
        private Color _colorMin;



        public void Init(Color colorMax, Color colorMin)
        {
            _foreground.color = colorMax;
            _colorMax = colorMax;
            _colorMin = colorMin;
        }

        public void UpdateStatbar(float max, float current)
        {
            float t = Mathf.Clamp(current / max, 0, 1);
            float width = Mathf.Lerp(0, _barWidth, t);
            Color c = Color.Lerp(_colorMin, _colorMax, t);
            _foreground.color = c;
            _foreground.size = new Vector2(width, _foreground.size.y);

        }
    }

}