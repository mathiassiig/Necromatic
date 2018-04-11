using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic.Character
{
    public interface ISelectable
    {
        void Select();
        void Deselect();
    }
}