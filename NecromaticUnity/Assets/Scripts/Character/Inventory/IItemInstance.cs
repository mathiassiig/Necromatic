using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character.Inventory
{
    public interface IItemInstance
    {
        void Init(CharacterInstance owner);
    }
}