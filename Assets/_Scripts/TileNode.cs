using System.Collections.Generic;
using UnityEngine;

namespace Pathfiding
{
    public class TileNode : MonoBehaviour
    {
        [field:Header("Node Settings")]
        
        [field:SerializeField] public float Cost = 1f; 
        [field:SerializeField] public bool IsObstacle { get; private set; }  

        public Vector2 GridPosition { get; private set; }

        [field:Header("Referebnces")]

        [field:SerializeField] public Transform MovementPosition { get; private set; }
        [field:SerializeField] public TileModel Model { get; private set; } 

        [field:Header("Debug")]

        [field:SerializeField] public List<TileNode> NeighborsOnTile { get; private set; } = new(); 

        // A* 
        public int GCost { get; set; } 
        public int HCost { get; set; } 
        public int FCost { get { return GCost + HCost; } } 

        public TileNode Parent { get; set; } 


        public void InitNode(Vector2 gridPosition)
        {
            GridPosition = gridPosition; 
        }

        public void SetNeighbors(List<TileNode> neighbors)
        {
            NeighborsOnTile = new(neighbors); 
        }

        public void OnClick()
        {
            PlayerManager.Singleton.GoTo(this); 
        }
    }
}
