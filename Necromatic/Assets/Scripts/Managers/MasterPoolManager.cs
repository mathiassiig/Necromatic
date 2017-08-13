using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Necromatic
{
    public enum CharacterType
    {
        NPCUndeadInfantry,
        NPCHumanInfantry,
        Player
    }

    public class MasterPoolManager : Singleton<MasterPoolManager>
    {
        protected MasterPoolManager() { }

        private const string _prefabLocation = "Prefabs/";

        public Dictionary<CharacterType, string> Corpses = new Dictionary<CharacterType, string>
        {
            { CharacterType.NPCHumanInfantry,   $"{_prefabLocation}Corpses/Human/Corpse_Human_Infantry" }
        };

        public Dictionary<CharacterType, string> Characters = new Dictionary<CharacterType, string>
        {
            { CharacterType.NPCHumanInfantry,   $"{_prefabLocation}Characters/Human/NPC_Human_Infantry" },
            { CharacterType.NPCUndeadInfantry,  $"{_prefabLocation}Characters/Undead/NPC_Undead_Infantry" }
        };

        // todo: pooling

        public GameObject GetCorpse(CharacterType c)    =>      Instantiate(Resources.Load(Corpses[c]))     as GameObject;
        public GameObject GetCharacter(CharacterType c) =>      Instantiate(Resources.Load(Characters[c]))  as GameObject;
    }
}