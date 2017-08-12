using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic
{
    public static class UndeathConverter
    {
        public static Dictionary<CharacterType, CharacterType> LivingToDead = new Dictionary<CharacterType, CharacterType>
        {
            {CharacterType.NPCHumanInfantry, CharacterType.NPCUndeadInfantry}
        };
    }
}