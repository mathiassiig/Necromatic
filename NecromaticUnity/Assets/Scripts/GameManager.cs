using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
using Necromatic.Character;
using Necromatic.Player;
using Necromatic.World;
using UniRx;

namespace Necromatic
{
    public class GameManager : Singleton<GameManager>
    {
        public ResearchBank ResearchBank { get; private set; }

        protected GameManager()
        {
            ResearchBank = new ResearchBank();
        }

        void Start()
        {
            ResearchBank.LoadBank();
        }

    }
}