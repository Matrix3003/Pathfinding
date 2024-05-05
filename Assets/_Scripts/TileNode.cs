using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfiding
{
    public class TileNode : MonoBehaviour
    {
        [Header("Node Settings")]
        
        [SerializeField] private float _costToCrossTile = 1f; 
        [SerializeField] private bool _isObstacle = false; 

        [Space]

        [SerializeField] private List<TileNode> _neighborsOnTile = new (); 

        public Vector2 GridPosition { get; private set; }

        private GameObject _currentTile;

        public void InitNode(Vector3 gridPosition)
        {
            GridPosition = gridPosition; 
        }
    }
}
