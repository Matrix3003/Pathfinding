using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfiding
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Singleton; 

        [Header("Player Settings")]

        [SerializeField] private float _timeToCrossTile; 
        [SerializeField] private Ease _movementEase; 

        [Space]

        [SerializeField] private float _timeToStayInTileBetweenMovements = .5f; 

        private TileNode _atualNodePositon; 

        private bool _isMoving; 

        private Coroutine _movementCoroutine; 

        private void Awake()
        {
            GenereteSingleton();
        }

        public void InitPlayer(TileNode currentNode)
        {
            _atualNodePositon = currentNode; 
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

        public void GoTo(TileNode destination)
        {
            if(destination.IsObstacle) return; 
            if(_atualNodePositon == destination) return; 

            var grid = TileGeneratorManager.Singleton.TilemapMatriz; 

            var path = AStar.FindPath(_atualNodePositon, destination, grid); 

            if (path == null)
            {
                var camera  = Camera.main.transform; 
                camera.DOShakePosition(.5f, 1); 

                return; 
            }

            if (_isMoving)
            {
                DisableAllHighlight(); 
                
                transform.DOKill(); 
                StopCoroutine(_movementCoroutine); 
            }

            HighlightPath(path); 
            _movementCoroutine = StartCoroutine(MovementCoroutine(path)); 
        }

        private void HighlightPath(List<TileNode> path)
        {
            foreach (var tile in path)
            {
                tile.Model.Hightlight(); 
            }
        }

        private void DisableAllHighlight()
        {
            foreach (var tile in TileGeneratorManager.Singleton.TilemapMatriz)
            {
                tile.Model.DisableHightlight(); 
            }
        }

        private IEnumerator MovementCoroutine(List<TileNode> path)
        {
            _isMoving = true;

            for (int i = 0; i < path.Count; i++)
            {
                var tile = path[i];
                _atualNodePositon = path[i]; 
                
                transform.DOMove(tile.MovementPosition.position, _timeToCrossTile).SetEase(_movementEase); 
                transform.DOLookAt(tile.MovementPosition.position, .2f, AxisConstraint.Y); 

                yield return new WaitForSeconds(_timeToCrossTile + _timeToStayInTileBetweenMovements); 

                tile.Model.DisableHightlight(); 
            }

            _isMoving = false; 
        }
    }
}
