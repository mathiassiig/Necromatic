using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [Tooltip("Where to put the pooled objects. If this isn't set, the objects will be directly under this gameobject")]
    [SerializeField] private Transform _activeCellContainer;
    [SerializeField] private int _initialActiveCellsSize;
    [SerializeField] private int _recycledCellsSize; // size of disabled objects

    private Transform _recycledObjectRoot;
    private Queue<GameObject> _recycledObjects = new Queue<GameObject>();
    
    void Awake()
    {
        InitRecycleTransform();
    }

    void InitRecycleTransform()
    {
        var recycler = new GameObject();
        recycler.transform.SetParent(transform);
        _recycledObjectRoot = recycler.transform;
    }

    public void CacheObject(GameObject obj)
    {
        if (_recycledObjects.Count < _recycledCellsSize)
        {
            _recycledObjects.Enqueue(obj);
            obj.transform.SetParent(_recycledObjectRoot);
            obj.SetActive(false);
        }
        else
        {
            Destroy(obj);
        }
    }

    public GameObject PoolObject()
    {
        if(_recycledObjects.Count > 0)
        {
            return _recycledObjects.Dequeue();
        }
        else
        {
            return Instantiate(_prefab, _activeCellContainer ?? transform);
        }
    }
}