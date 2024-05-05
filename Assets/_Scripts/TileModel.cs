using UnityEngine;

namespace Pathfiding
{
    public class TileModel : MonoBehaviour
    {
        [SerializeField] private TileNode _currentNode; 

        [Header("Materials")]

        [SerializeField] private Material _deafaultMaterial; 
        [SerializeField] private Material _highlightedMaterial; 

        private Renderer _renderer; 

        private void Start()
        {
            _renderer = GetComponent<Renderer>(); 
        }

        private void OnMouseDown()
        {
            _currentNode.OnClick(); 
        }

        public void Hightlight()
        {
            _renderer.material = _highlightedMaterial; 
        }

        public void DisableHightlight()
        {
            _renderer.material = _deafaultMaterial; 
        }
    }
}
