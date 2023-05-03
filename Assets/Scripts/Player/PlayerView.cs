using UnityEngine;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Renderer _characterRenderer;
        [SerializeField] private Material _alteredMaterial;
        private Material[] _originMaterials;
        
        private void Awake() =>
            _originMaterials = _characterRenderer.sharedMaterials;

        public void ToAlteredState()
        {
            Material[] materials = _characterRenderer.materials;

            for (var i = 0; i < materials.Length; i++)
            {
                materials[i] = _alteredMaterial;
            }

            _characterRenderer.materials = materials;
        }

        public void ToNormalState() =>
            _characterRenderer.materials = _originMaterials;
    }
}