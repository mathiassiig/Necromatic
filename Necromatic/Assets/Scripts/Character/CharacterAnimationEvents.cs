using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace Necromatic.Char
{
    public class CharacterAnimationEvents : MonoBehaviour
    {
        public readonly BehaviorSubject<bool> Attacking = new BehaviorSubject<bool>(false);
        public void Attack()
        {
            // unity fires the animation event twice for whatever reason.
            // so far, I have not figured out why, so I'm doing this little hack
            // truthfully, I just want this to be a trigger instead of a boolean anyway
            // maybe everything I'm doing here is wrong.
            // Update 2: Still haven't figured this out yet, this fires randomly
            Attacking.OnNext(true);
        }
    }
}