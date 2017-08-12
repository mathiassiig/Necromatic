using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private float _velocity = 1f; // unity metres per second
    [SerializeField] private bool _lookTowards; // have projectile face target
    [SerializeField] private float _impactDistance = 0.1f; // distance from target before 'hitting'

    private float _damage;
    private bool _move;
    private Character _target;

    public void Init(Character target, float damage)
    {
        _target = target;
        _damage = damage;
        _move = true;
    }

    void Update()
    {
        if(_move)
        {
            if (_lookTowards)
            {
                transform.LookAt(_target.transform);
            }
            var distance = (_target.transform.position - transform.position);
            var direction = distance.normalized;
            Vector3 toMove = distance.magnitude <= _impactDistance ? distance : direction * _velocity * Time.deltaTime;
            transform.position = transform.position + toMove;
            if (distance.magnitude <= _impactDistance)
            {
                Impact();
                _move = false;
            }
        }
    }

    protected void Impact()
    {
        _target.Health.Add(-_damage);
        Destroy(gameObject);
    }
}
