using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Char.Combat;
namespace Necromatic.Char.NPC
{
    public class HumanNPC : Character
    {
        protected override void HandleDeath()
        {
            var corpse = MasterPoolManager.Instance.GetCorpse(CharacterType.NPCHumanInfantry);
            corpse.transform.position = transform.position;
            var corpseScript = corpse.GetComponent<Corpse>();
            corpseScript.Init(this);
        }
    }
}