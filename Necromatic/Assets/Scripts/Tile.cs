using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;

    public void Init(Color color)
    {
        _renderer.material.color = color;
    }
}
