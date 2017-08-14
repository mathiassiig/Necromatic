using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char.Combat;
namespace Necromatic.Char.NPC
{
    public class UndeadNPC : CharacterNPC
    {
        void Awake()
        {
            Init();
        }

        protected override void HandleDeath()
        {
            enabled = false;
            Movement.M_Animator.SetTrigger("Death");
            Movement.M_Animator.SetBool("Dead", true);
        }
    }
}