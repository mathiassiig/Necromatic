using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Necromatic.Character;

namespace Necromatic.Utility
{
    public static class GameObjectUtils
    {
        public static List<T> Detect<T>(float range, Vector3 position, LayerMask mask)
        {
            var colliders = Physics.OverlapSphere(position, range, mask);
            return colliders.Select(x => x.GetComponent<T>()).Where(x => x != null).ToList();
        }

        public static Transform Closest(List<Transform> targets, Transform sender)
        {
            var min = targets.Select(x => Distance(x.position, sender.position)).Min(); // todo: one-liner?
            return targets.FirstOrDefault( x => Distance(x.position, sender.position) == min);
        }

        public static T Closest<T>(List<T> targets, MonoBehaviour sender)
        {
            var min = targets.Select(x => Distance((x as MonoBehaviour).transform.position, sender.transform.position)).Min(); // todo: one-liner?
            return targets.FirstOrDefault(x => Distance((x as MonoBehaviour).transform.position, sender.transform.position) == min); 
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            return (a-b).sqrMagnitude;
        }

        public static List<CharacterInstance> DetectEnemies(float range, Vector3 position, CharacterInstance sender)
        {
            return Detect<CharacterInstance>(range, position, LayerMask.GetMask("Character")).Where(x => x.Faction != sender.Faction).ToList();
        }
    }
}