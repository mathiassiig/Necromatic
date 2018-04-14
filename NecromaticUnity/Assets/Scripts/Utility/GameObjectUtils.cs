using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Necromatic.Character;

namespace Necromatic.Utility
{
    public static class NecromaticLayers
    {
        public static string DEFAULT = "Default";
        public static string TREE = "Tree";
        public static string CHARACTER = "Character";
    }
    public static class GameObjectUtils
    {
        public static List<T> Detect<T>(float range, Vector3 position, LayerMask mask)
        {
            var colliders = Physics.OverlapSphere(position, range, mask);
            return colliders.Select(x => x.GetComponent<T>()).Where(x => x != null).ToList();
        }

        public static Transform Closest(List<Transform> targets, Transform sender)
        {
            var min = targets.Select(x => MathUtils.Distance(x.position, sender.position)).Min(); // todo: one-liner?
            return targets.FirstOrDefault(x => MathUtils.Distance(x.position, sender.position) == min);
        }

        public static T Closest<T>(List<T> targets, MonoBehaviour sender)
        {
            var min = targets.Select(x => MathUtils.Distance((x as MonoBehaviour).transform.position, sender.transform.position)).Min(); // todo: one-liner?
            return targets.FirstOrDefault(x => MathUtils.Distance((x as MonoBehaviour).transform.position, sender.transform.position) == min);
        }

        public static List<CharacterInstance> DetectEnemies(float range, Vector3 position, CharacterInstance sender)
        {
            return Detect<CharacterInstance>(range, position, LayerMask.GetMask("Character")).Where(x => x.Faction != sender.Faction).ToList();
        }

        public static T RayGetComponent<T>(Vector2 mousePos, LayerMask layer)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                var component = hit.collider.gameObject.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }
            return default(T);
        }

        public static Vector3? GetGroundPosition(Vector2 mousePos)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(NecromaticLayers.DEFAULT)))
            {
                return hit.point;
            }
            return null; // shouldn't happen
        }
    }
}