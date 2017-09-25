using Necromatic.Char.Combat;
using Necromatic.Char.NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Char
{
    public class HumanDeath : MonoBehaviour, IDeath
    {

        public void HandleDeath()
        {
            var charScript = GetComponent<CharacterNPC>() as Character;
            var corpse = MasterPoolManager.Instance.GetCorpse(CharacterType.NPCHumanInfantry);
            corpse.transform.position = transform.position;
            var corpseScript = corpse.GetComponent<Corpse>();
            corpseScript.Init(charScript);
        }
    }
}