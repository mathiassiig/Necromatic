using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Char.User
{
    public class Player : Character
    {
        protected override void Init()
        {
            base.Init();
            _healthBar.gameObject.SetActive(false);
        }
    }
}