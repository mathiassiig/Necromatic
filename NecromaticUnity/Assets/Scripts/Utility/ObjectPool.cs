using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Utility
{
    [ExecuteInEditMode]
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _object;
        [SerializeField] private int _amount;
        void OnEnable()
        {
            if (transform.childCount < _amount)
            {
				var toInstantiate = _amount - transform.childCount;
                for (int i = 0; i < toInstantiate; i++)
                {
                    var child = Instantiate(_object, transform.position, Quaternion.identity);
                    child.transform.SetParent(transform);
                }
            }
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public GameObject Take()
        {
            if (transform.childCount == 0)
            {
                return null;
            }
            var child = transform.GetChild(0).gameObject;
            child.SetActive(true);
            return child;
        }

        public void Return(GameObject g)
        {
            g.SetActive(false);
            g.transform.SetParent(transform);
        }
    }
}