using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace Necromatic.Character
{
    public class Death : MonoBehaviour
    {
        public ReactiveProperty<bool> Dead = new ReactiveProperty<bool>(false);

        public virtual void Die()
        {
            Dead.Value = true;
            Destroy(gameObject);
        }
    }
}