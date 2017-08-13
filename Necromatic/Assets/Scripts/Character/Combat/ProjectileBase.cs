using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char;
using System;
using UniRx;
public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private float _velocity = 1f; // unity metres per second
    [SerializeField] private bool _lookTowards; // have projectile face target
    [SerializeField] private float _impactDistance = 0.1f; // distance from target before 'hitting'

    private float _damage;
    private Character _target;

    public void Init(Character target, float damage, Action impactAction)
    {
        _target = target;
        _damage = damage;

        var observable = Observable.EveryUpdate().TakeUntilDestroy(this).Subscribe(x =>
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
                Impact(impactAction);
			}
        });
    }

    public void Impact(Action impactAction)
    {
        impactAction();
        Destroy(gameObject);
    }
}
