using DG.Tweening;
using UnityEngine;

namespace Card_3D
{
    public class Card3D : CardAnimation
    {
        private CardInfo info;
        [SerializeField] private MeshRenderer cardRenderer;

        public int Index { get; set; }

        private float _yPos;
        private float _delay;

        public void RenderInfo(CardInfo cardInfo, Material backMaterial, Material sideMaterial, Transform deck)
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

            _delay = (Index + 1) * GameConstants.CardAnimDuration;
            _yPos = (Index + 1) * GameConstants.CardWidth;
            
            // Intro sequence
            var sequence = DOTween.Sequence();
            sequence
                .Append(Move(deck.position + new Vector3(0f, _yPos, 0f)))
                .Join(Rotate(deck.rotation.eulerAngles))
                .SetDelay(_delay)
                .Play();
        }

        public void JumpRotateCard(Transform target, bool isDelayed = true)
        {
            var pos = target.position + new Vector3(0f, _yPos, 0f);
            var rot = target.rotation.eulerAngles;

            var delay = isDelayed ? _delay : 0;

            var sequence = DOTween.Sequence();
            sequence
                .Append(Jump(pos))
                .Join(Rotate(rot))
                .SetDelay(delay)
                .Play();
        }
        
    }
}