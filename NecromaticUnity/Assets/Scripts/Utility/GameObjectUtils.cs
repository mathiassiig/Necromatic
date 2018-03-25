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

        public static Vector2 CirclePoint(Vector2 center, float radius, float angle)
        {
            var x = center.x + radius * Mathf.Cos(angle);
            var y = center.y + radius * Mathf.Sin(angle);
            return new Vector2(x, y);
        }

        public static float Distance(Vector3 a, Vector3 b) => (a-b).sqrMagnitude;

        public static Vector2 PlaneDirection(Transform from, Transform to)
        {
            var fromV = new Vector2(from.position.x, from.position.z);
            var toV = new Vector2(to.position.x, to.position.z);
            return (toV - fromV).normalized;
        }

        public static List<CharacterInstance> DetectEnemies(float range, Vector3 position, CharacterInstance sender)
        {
            return Detect<CharacterInstance>(range, position, LayerMask.GetMask("Character")).Where(x => x.Faction != sender.Faction).ToList();
        }
    }
}