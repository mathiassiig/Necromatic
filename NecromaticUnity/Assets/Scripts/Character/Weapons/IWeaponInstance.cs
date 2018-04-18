using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Necromatic.Character.Inventory;

namespace Necromatic.Character.Weapons
{
    public interface IWeaponInstance
    {
        IDisposable Attack(Weapon weaponData, IDamagable target, CharacterInstance attacker, Action onFinished = null, Action onHit = null);        
    }
}