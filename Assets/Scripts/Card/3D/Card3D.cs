using UnityEngine;

namespace Card_3D
{
    public class Card3D : MonoBehaviour
    {
        private CardInfo info;
        [SerializeField] private MeshRenderer cardRenderer;

        public void RenderInfo(CardInfo cardInfo, Material backMaterial, Material sideMaterial)
        {
            info = cardInfo;
            name = cardInfo.name;
        
            var material = cardRenderer.materials[2];
            material.mainTexture = cardInfo.cardTexture;
            cardRenderer.materials = new[]
            {
                sideMaterial,
                backMaterial,
                material
            };
        }
    }
}