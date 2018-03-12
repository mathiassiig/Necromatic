using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Utility;
using Necromatic.Character;
using UniRx;
using Necromatic.Player;

namespace Necromatic
{
    public class GameManager : MonoBehaviour
    {
        public ResearchBank ResearchBank { get; private set; } = new ResearchBank();

        void Start()
        {
            ResearchBank.LoadBank();
        }
    }
}