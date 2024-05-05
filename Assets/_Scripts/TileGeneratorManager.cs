using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfiding
{
    public class TileGeneratorManager : MonoBehaviour
    {
        public static TileGeneratorManager Singleton; 
        
        [Header("Tileset Settings")]
        
        [SerializeField] private Transform _tileStartGenerationPoint; 
        [SerializeField] private Transform _tileHierarchyGroup;  
        
        [Space]
        
        [SerializeField] private int _xSize; 
        [SerializeField] private int _zSize;

        [Space]

        [SerializeField] private int _tilesSpacing; 

        [Header("Odds")]

        [Range(0, 1)]
        [SerializeField] private float _changeToGenerateObstacle; 

        [Header("Prefabs")]

        [SerializeField] private GameObject _walkableTilePrefab; 
        [SerializeField] private GameObject _obstacleTilePrefab; 

        public TileNode[,] TilemapMatriz { get; private set; }
        
        private void Start()
        {
            GenereteSingleton(); 
            
            InitMatriz(); 
            GenerateTileMap(); 
            SetTilemapNeighbors(); 
        }

        private void GenereteSingleton()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Debug.LogError($"More then one {gameObject.name} singleton in scene!");
            }
        }

        private void GenerateTileMap()
        {
            for (int x = 0; x < _xSize; x++)
            {
                for (int z = 0; z < _zSize; z++)
                {
                    var prefab = CanotSpawnObstacleTile(x, z)
                    ? _walkableTilePrefab
                    : GetRandomTile(); 
                    
                    var xPos = _tileStartGenerationPoint.position.x + (x * _tilesSpacing); 
                    var zPos = _tileStartGenerationPoint.position.z + (z * _tilesSpacing);
                    
                    var worldPositon = new Vector3(xPos, 0f, zPos); 
                    var tilePosition = new Vector2(x, z); 

                    var tileObj = Instantiate(prefab, worldPositon, Quaternion.identity); 
                    var tile = tileObj.GetComponent<TileNode>(); 

                    tile.InitNode(tilePosition); 
                    tileObj.transform.SetParent(_tileHierarchyGroup); 

                    TilemapMatriz[x, z] = tile; 

                    if (IsThePlayerSpawnTile(x, z))
                        PlayerManager.Singleton.InitPlayer(tile);
                }
            }

            bool IsThePlayerSpawnTile(int xTilePos, int zTilePos) => xTilePos == 0 && zTilePos == 0;  
            bool CanotSpawnObstacleTile(int xTilePos, int zTilePos) => 
            xTilePos == 0 && zTilePos == 0 
            ||
            xTilePos == 1 && zTilePos == 0 
            ||
            xTilePos == 0 && zTilePos == 1 ;  
        }

        private void InitMatriz() => TilemapMatriz = new TileNode[_xSize, _zSize]; 

        private void SetTilemapNeighbors()
        {
            for (int x = 0; x < _xSize; x++)
            {
                for (int z = 0; z < _zSize; z++)
                {
                    var tile = TilemapMatriz[x, z];

                    var tempNeighbors = new List<TileNode>();

                    if (HaveInFront(z))
                        tempNeighbors.Add(TilemapMatriz[x, z + 1]);

                    if (HaveInBehind(z))
                        tempNeighbors.Add(TilemapMatriz[x, z - 1]);

                    if (HaveInRight(x))
                        tempNeighbors.Add(TilemapMatriz[x + 1, z]);

                    if (HaveInLeft(x))
                        tempNeighbors.Add(TilemapMatriz[x - 1, z]);

                    tile.SetNeighbors(tempNeighbors);
                }
            }

            bool HaveInFront(int z) => z + 1 < _zSize; 
            bool HaveInBehind(int z) => z - 1 >= 0; 
            bool HaveInRight(int x) => x + 1 < _xSize; 
            bool HaveInLeft(int x) => x - 1 >= 0; 
        }

        private GameObject GetRandomTile()
        {
            var random = Random.value; 

            var tile = random <= _changeToGenerateObstacle 
            ? _obstacleTilePrefab
            : _walkableTilePrefab; 

            return tile; 
        }
    }
}
