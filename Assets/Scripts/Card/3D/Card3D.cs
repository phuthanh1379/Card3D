using UnityEngine;

namespace Card_3D
{
    public class Card3D : CardAnimation
    {
        private CardInfo info;
        [SerializeField] private MeshRenderer cardRenderer;

        public int Index { get; set; }
        public Vector3 DeckPosition { get; set; }
        public Vector3 ShowTrumpPosition { get; set; }
        private float _delay;

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

            _delay = (Index + 1) * GameConstants.CardAnimDuration;

            InitShowTrumpSequence(ShowTrumpPosition);
            InitIntroSequence(DeckPosition);
            PlayIntroSequence(_delay);
        }
        
        public void PlayEffect()
        {
            PlayShowTrumpSequence(_delay);
        }

        public void PlayEffectReverse()
        {
            PlayShowTrumpSequenceReverse(_delay);
        }
    }
}