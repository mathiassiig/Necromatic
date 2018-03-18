using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// based on https://github.com/sharpaccent/Astar-for-Unity/blob/master/Assets/Scripts/Pathfinder.cs
namespace Necromatic.World
{
    public class Pathfinder
    {
        public NavigationMesh NavMesh;
        private Node _end;
        public List<Node> FindPath(Node start, Node end)
        {
            _end = end;
            if(NavMesh == null)
            {
                NavMesh = GameManager.Instance.NavMesh;
            }
            return FindPathActual(start, end);
        }

        public List<Node> FindPathActual(Node start, Node target)
        {
            List<Node> foundPath = new List<Node>();

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];

                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost ||
                        (openSet[i].FCost == currentNode.FCost &&
                        openSet[i].HCost < currentNode.HCost))
                    {
                        if (!currentNode.Equals(openSet[i]))
                        {
                            currentNode = openSet[i];
                        }
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode.Equals(target))
                {
                    foundPath = RetracePath(start, currentNode);
                    break;
                }

                foreach (Node neighbour in GetNeighbours(currentNode))
                {
                    if (!closedSet.Contains(neighbour))
                    {
                        float newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);

                        if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                        {
                            neighbour.GCost = newMovementCostToNeighbour;
                            neighbour.HCost = GetDistance(neighbour, target);
                            neighbour.ParentNode = currentNode;
                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                        }
                    }
                }
            }
            return foundPath;
        }

        public List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.ParentNode;
            }

            path.Reverse();

            return path;
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> retList = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {

                    if (x == 0 && y == 0)
                    {

                    }
                    else
                    {
                        Vector2Int searchPos = new Vector2Int();

                        searchPos.x = node.GridPos.x + x;
                        searchPos.y = node.GridPos.y + y;
                        var neighbor = GetNode(searchPos);

                        if (neighbor != null && (neighbor.Taken == false || neighbor == _end))
                        {
                            retList.Add(neighbor);
                        }
                    }
                }
            }

            return retList;

        }

        public Node GetNeighbourNode(Vector2Int adjPos, bool searchTopDown, Node currentNodePos)
        {

            Node retVal = null;

            Node node = GetNode(adjPos);
            if (node != null && node.Taken)
            {
                retVal = node;
            }
            else if (searchTopDown)
            {
                adjPos.y -= 1;
                Node bottomBlock = GetNode(adjPos);

                if (bottomBlock != null && bottomBlock.Taken)
                {
                    retVal = bottomBlock;
                }
                else
                {
                    adjPos.y += 2;
                    Node topBlock = GetNode(adjPos);
                    if (topBlock != null && topBlock.Taken)
                    {
                        retVal = topBlock;
                    }
                }
            }
            int originalX = adjPos.x - currentNodePos.GridPos.x;
            int originalY = adjPos.y - currentNodePos.GridPos.y;

            if (Mathf.Abs(originalX) == 1 && Mathf.Abs(originalY) == 1)
            {
                Node neighbour1 = GetNode(new Vector2Int(currentNodePos.GridPos.x + originalX, currentNodePos.GridPos.y));
                if (neighbour1 == null || !neighbour1.Taken)
                {
                    retVal = null;
                }

                Node neighbour2 = GetNode(new Vector2Int(currentNodePos.GridPos.x, currentNodePos.GridPos.y + originalY));
                if (neighbour2 == null || !neighbour2.Taken)
                {
                    retVal = null;
                }
            }

            if (retVal != null)
            {

            }

            return retVal;
        }

        private Node GetNode(Vector2Int navPos)
        {
            Node n = null;

            lock (NavMesh)
            {
                n = NavMesh.GetNode(navPos);
            }
            return n;
        }

        private float GetDistance(Node nodeA, Node nodeB)
        {
            var posA = (Vector2)nodeA.GridPos;
            var posB = (Vector2)nodeB.GridPos;
            return (posA - posB).magnitude;
        }

    }
}
