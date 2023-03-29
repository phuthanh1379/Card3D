using DG.Tweening;
using UnityEngine;

namespace Card_3D
{
    public class Card3D : MonoBehaviour
    {
        private CardInfo info;
        [SerializeField] private MeshRenderer cardRenderer;

        private Sequence sequence;

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

        private void InitEffect()
        {
            sequence = DOTween.Sequence();
            var scale1 = transform.DOScale(Vector3.one * 1.2f, 0.2f);
            var scale2 = transform.DOScale(Vector3.one, 0.2f);

            sequence.Append(scale1)
                .Append(scale2)
                .SetAutoKill(false);
        }

        public void PlayEffect(float t)
        {
            Debug.Log($"Play: Delay={t}");
            sequence
                .SetDelay(t)
                .Play();
        }
    }
}