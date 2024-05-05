using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfiding
{

    public class AStar
    {
        public static List<TileNode> FindPath(TileNode startNode, TileNode goalNode, TileNode[,] grid)
        {
            List<TileNode> openSet = new List<TileNode>();
            HashSet<TileNode> closedSet = new HashSet<TileNode>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                TileNode currentNode = openSet[0];

                // Encontrar o nó com o menor custo f
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || 
                        (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == goalNode)
                {
                    return RetracePath(startNode, goalNode);
                }

                foreach (TileNode neighbor in currentNode.NeighborsOnTile)
                {
                    if (neighbor == null || closedSet.Contains(neighbor) || neighbor.IsObstacle)
                        continue;

                    int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newMovementCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, goalNode);
                        neighbor.Parent = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            Debug.Log("null");
            return null; // Caminho não encontrado
        }

        private static List<TileNode> RetracePath(TileNode startNode, TileNode endNode)
        {
            List<TileNode> path = new List<TileNode>();
            TileNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }

        private static int GetDistance(TileNode nodeA, TileNode nodeB)
        {
            int dstX = Mathf.Abs((int)nodeA.GridPosition.x - (int)nodeB.GridPosition.x);
            int dstZ = Mathf.Abs((int)nodeA.GridPosition.y - (int)nodeB.GridPosition.y);

            return dstX + dstZ;
        }
    }
}
