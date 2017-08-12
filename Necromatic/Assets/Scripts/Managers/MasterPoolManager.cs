using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Necromatic
{
    public enum CharacterType
    {
        NPCUndeadInfantry,
        NPCHumanInfantry
    }

    public enum CorpseType
    {
        CorpseHumanInfantry
    }

    public class MasterPoolManager : Singleton<MasterPoolManager>
    {
        protected MasterPoolManager() { }

        private const string _prefabLocation = "Prefabs/";

        public Dictionary<CorpseType, string> Corpses = new Dictionary<CorpseType, string>
        {
            { CorpseType.CorpseHumanInfantry,   $"{_prefabLocation}/Corpses/Human/Corpse_Human_Infantry" }
        };

        public Dictionary<CharacterType, string> Characters = new Dictionary<CharacterType, string>
        {
            { CharacterType.NPCHumanInfantry,   $"{_prefabLocation}/Characters/Human/NPC_Human_Infantry" },
            { CharacterType.NPCUndeadInfantry,  $"{_prefabLocation}/Characters/Undead/NPC_Undead_Infantry" }
        };

        // todo: pooling

        public GameObject GetCorpse(CorpseType c)
        {
            return null;
        }

        public GameObject GetCharacter(CharacterType c)
        {
            var location = Characters[c];
            var prefab = Resources.Load(location);
            return Instantiate(prefab) as GameObject;
        }
    }
}