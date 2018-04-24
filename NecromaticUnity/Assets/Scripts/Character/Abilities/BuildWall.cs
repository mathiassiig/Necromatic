using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Necromatic.World;

namespace Necromatic.Character.Abilities
{
    public class BuildWall : ClickAbility
    {
        private Vector3? _firstPoint;
        private Vector3? _secondPoint;
        private WallOld _wallScript;

        protected override void HandleHitObject(RaycastHit objectHit)
        {
            var pos = objectHit.point;
            if (!IsBuildable(pos))
            {
                return;
            }
            var nearestGridPosWorldPos = GameManager.Instance.BuildGrid.GetWorldPos(GameManager.Instance.BuildGrid.GetGridPos(pos));
            if (_firstPoint == null)
            {
                _firstPoint = nearestGridPosWorldPos;

                var wall = new GameObject();
                _wallScript = wall.AddComponent<WallOld>();
                var filter = wall.AddComponent<MeshFilter>();
                _wallScript.Filter = filter;
                var meshRenderer = wall.AddComponent<MeshRenderer>();
                meshRenderer.material = new Material(Shader.Find("Diffuse"));
            }
            else if (_secondPoint == null)
            {
                _secondPoint = nearestGridPosWorldPos;
            }
            if (_firstPoint != null && _secondPoint != null)
            {
                _wallScript.Construct();
                _firstPoint = null;
                _secondPoint = null;
            }
        }

        protected override void HandleHoverObject(RaycastHit objectHit)
        {
            if (_firstPoint != null && _secondPoint == null)
            {
                var nearestGridPosWorldPos = GameManager.Instance.BuildGrid.GetWorldPos(GameManager.Instance.BuildGrid.GetGridPos(objectHit.point));
                _wallScript.UpdateWallCorners(_firstPoint.Value, nearestGridPosWorldPos);
                _wallScript.ReMesh();
            }
        }

        protected override string GetLayer()
        {
            return "Default";
        }

        private bool IsBuildable(Vector3 pos)
        {
            var node = GameManager.Instance.BuildGrid.GetNode(pos);
            return !node.Taken;
        }
    }
}