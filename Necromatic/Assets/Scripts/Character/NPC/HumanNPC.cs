using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.NPC
{
    public class HumanNPC : Character
    {
        protected override void HandleDeath()
        {
            var corpse = MasterPoolManager.Instance.GetCorpse(CharacterType.NPCHumanInfantry);
            corpse.transform.position = transform.position;
            base.HandleDeath();
        }
    }
}