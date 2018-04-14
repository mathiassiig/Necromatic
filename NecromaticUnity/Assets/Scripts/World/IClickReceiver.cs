using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necromatic.Character.NPC.Strategies.Results;
using Necromatic.Character;
namespace Necromatic.World
{
    public interface IClickReceiver
    {
        void Click(List<ISelectable> senders);
    }
}