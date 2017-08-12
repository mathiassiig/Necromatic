using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gibber : MonoBehaviour
{
    [SerializeField] private Sprite[] _bloodSprites;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<ParticleSystem> _particles;

    void Awake()
    {
        _spriteRenderer.gameObject.SetActive(false);
    }


    public void Gib()
    {
        _spriteRenderer.gameObject.SetActive(true);
        _spriteRenderer.sprite = RandomSprite();
        foreach (var ps in _particles)
        {
            ps.Play();
        }
    }

    private Sprite RandomSprite()
    {
        int max = _bloodSprites.Length;
        int random = Random.Range(0, max);
        return _bloodSprites[random];
    }
}
