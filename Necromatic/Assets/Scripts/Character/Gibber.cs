using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using System;
public class Gibber : MonoBehaviour
{
    [SerializeField] private Sprite[] _bloodSprites;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<ParticleSystem> _particles;
    private const float _autoDeleteTime = 120f;

    void Awake()
    {
        _spriteRenderer.gameObject.SetActive(false);
    }


    public void Gib()
    {
        transform.SetParent(null);
        _spriteRenderer.gameObject.transform.localScale = Vector3.zero;
        _spriteRenderer.gameObject.transform.DOScale(1, 0.15f).SetEase(Ease.OutExpo);
        _spriteRenderer.gameObject.SetActive(true);
        _spriteRenderer.sprite = RandomSprite();
        foreach (var ps in _particles)
        {
            ps.Play();
        }
        Observable.Timer(TimeSpan.FromSeconds(_autoDeleteTime)).First().Subscribe(_ =>
        {
            _spriteRenderer.DOFade(0, 2f).OnComplete(() => Destroy(gameObject));
        });
    }

    private Sprite RandomSprite()
    {
        int max = _bloodSprites.Length;
        int random = UnityEngine.Random.Range(0, max);
        return _bloodSprites[random];
    }
}
