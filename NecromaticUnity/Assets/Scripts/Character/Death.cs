using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace Necromatic.Character
{
    public class Death
    {
        public ReactiveProperty<bool> Dead;

        public Death()
        {
            Dead = new ReactiveProperty<bool>(false);
        }

        public virtual void Die(CharacterInstance c)
        {
            Dead.Value = true;
            Object.Destroy(c.gameObject);
        }
    }
}