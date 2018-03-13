using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Necromatic.World;
namespace Necromatic.Testing
{
    [TestFixture]
    public class NavTests
    {
        [Test]
        public void GetGridPosTest_FirstQuadrant()
        {
            var navMesh = new NavigationMesh();
            var result = navMesh.GetGridPos(new Vector3(0.75f, 0, 0.75f));
            Assert.AreEqual(new Vector2Int(1, 1), result);
        }

        [Test]
        public void GetGridPosTest_SecondQuadrant()
        {
            var navMesh = new NavigationMesh();
            var result = navMesh.GetGridPos(new Vector3(-0.75f, 0, 0.75f));
            Assert.AreEqual(new Vector2Int(-2, 1), result);
        }

        [Test]
        public void GetGridPosTest_ThirdQuadrant()
        {
            var navMesh = new NavigationMesh();
            var result = navMesh.GetGridPos(new Vector3(-0.75f, 0, -0.75f));
            Assert.AreEqual(new Vector2Int(-2, -2), result);
        }

        [Test]
        public void GetGridPosTest_FourthQuadrant()
        {
            var navMesh = new NavigationMesh();
            var result = navMesh.GetGridPos(new Vector3(0.75f, 0, -0.75f));
            Assert.AreEqual(new Vector2Int(1, -2), result);
        }
    }
}