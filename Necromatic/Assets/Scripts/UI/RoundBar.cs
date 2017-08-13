using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Necromatic.Char.User;
using UniRx;
namespace Necromatic.UI
{
    public class RoundBar : MonoBehaviour
    {
        [SerializeField] private Image _foreground;
        [SerializeField] private Image _background;
        private Player _player;
        private bool Health = true; // if false, mana

        void Awake()
        {
            _player = FindObjectOfType<Player>();
            Observable.NextFrame().First().Subscribe(_ =>
            {
                _player.Health.Current.Subscribe(x =>
                {
                    UpdateBar(_player.Health.Max.Value, _player.Health.Current.Value);
                });
            });
        }

        private void UpdateBar(float max, float current)
        {
            var height = _background.rectTransform.rect.height;
            var t = current / max;
            var yOffset = -height * t + height;
            _foreground.rectTransform.offsetMax = new Vector2(0, -yOffset);
        }
    }
}