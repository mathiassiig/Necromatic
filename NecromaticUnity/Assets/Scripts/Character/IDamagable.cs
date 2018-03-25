using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromatic.Character
{
    public interface IDamagable
    {
        Combat Combat { get; }
        GameObject gameObject { get; }
        Death Death { get; }
        Stat Health { get; }
        Representation Representation { get; }
    }
}