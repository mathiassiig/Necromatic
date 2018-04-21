using Necromatic.Character;
using Necromatic.Character.Inventory;
using Necromatic.Character.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Necromatic.Utility;

public class RangedInstance : MonoBehaviour, IWeaponInstance
{
    public IDisposable Attack(Weapon weaponData, IDamagable target, CharacterInstance attacker, Action onFinished = null, Action onHit = null)
    {
        var ranged = weaponData as RangedWeapon;
        var forward = (1 - weaponData.ForwardRetractRatio) / weaponData.Speed;
        var retract = weaponData.ForwardRetractRatio / weaponData.Speed;
        IDisposable attackingDisposabe;
        attacker.Representation.Animate(weaponData.UseAnimation);
        attacker.Representation.SetAttackSpeed(weaponData.Speed);
        weaponData.GameObjectInstance.transform.DOKill();
        weaponData.GameObjectInstance.transform.DOLocalRotate(ranged.FiringRotation, forward / 2f);
        weaponData.GameObjectInstance.transform.DOLocalMove(ranged.FiringPosition, forward / 2f);
        var projectile = Instantiate(ranged.ProjectilePrefab);
        projectile.gameObject.SetActive(false);
        Observable.Timer(TimeSpan.FromSeconds(ranged.SpawnProjectileTime / ranged.Speed)).TakeUntilDestroy(attacker).Subscribe(x =>
        {
              projectile.gameObject.SetActive(true);
        });
        attacker.Representation.PutInPosition(projectile.transform, ItemSlotLocation.Offhand);
        projectile.transform.localPosition = ranged.ProjectilePosition;
        projectile.transform.localRotation = Quaternion.Euler(ranged.ProjectileRotation);
        projectile.transform.localScale = ranged.ProjectileScale;
        attackingDisposabe = Observable.Timer(TimeSpan.FromSeconds(forward)).Subscribe(x =>
        {
            onHit?.Invoke();
            weaponData.GameObjectInstance.transform.DOKill();
            weaponData.GameObjectInstance.transform.DOLocalMove(ranged.Position, retract / 2f);
            weaponData.GameObjectInstance.transform.DOLocalRotate(ranged.Rotation, retract / 2f);
            var heart = target.gameObject.transform.position;
            heart.y = heart.y + 1f;
            var direction = MathUtils.Direction(projectile.transform.position, heart);
            projectile.Fire(direction, ranged, attacker);
            attackingDisposabe = Observable.Timer(TimeSpan.FromSeconds(retract)).Subscribe(y =>
            {
                onFinished?.Invoke();
            });
        });
        return attackingDisposabe;
    }

}
