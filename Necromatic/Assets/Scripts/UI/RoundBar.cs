using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Necromatic.Char.User;
using UniRx;
using DG.Tweening;
using System;

namespace Necromatic.UI
{
    public class RoundBar : MonoBehaviour
    {
        [SerializeField] private Image _foreground;
        [SerializeField] private Image _background;
        private Player _player;
        private const float _animationTime = 0.2f;
        private const int _updateEveryFrames = 5;
        private bool _updateNow = true;


        void Awake()
        {
            _player = FindObjectOfType<Player>();
            Observable.NextFrame().First().Subscribe(_ =>
            {
                _player.Health.Current.Subscribe(x =>
                {
                    if (_updateNow)
                    {
                        _updateNow = false;
                        Observable.TimerFrame(_updateEveryFrames)
                        .First()
                        .Subscribe(u =>
                        {
                            UpdateBar(_player.Health.Max.Value, _player.Health.Current.Value);
                            _updateNow = true;
                        });
                    }
                });
            });
        }


        private void UpdateBar(float max, float current)
        {
            var height = _background.rectTransform.rect.height;
            var t = current / max;
            _foreground.rectTransform.DOSizeDelta(new Vector2(_foreground.rectTransform.sizeDelta.x, t * height), _animationTime)
                .SetEase(Ease.OutExpo);
        }
    }
}