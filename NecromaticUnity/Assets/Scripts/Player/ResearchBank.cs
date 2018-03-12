using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character.Abilities;
using UniRx;

namespace Necromatic.Player
{
    public class ResearchBank
    {
        public List<Ability> Abilities { get; private set; }
        public readonly ReactiveProperty<bool> BankLoaded = new ReactiveProperty<bool>(false);

        public void LoadBank()
        {
            BankLoaded.Value = false;
            Abilities = new List<Ability>()
            {
				new RaiseCorpse(),
                new Sacrifice(),
            };
            BankLoaded.Value = true;
        }
    }
}