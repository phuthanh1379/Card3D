using Audio;
using Common;
using DG.Tweening;
using UnityEngine;

namespace Card_3D
{
    public class Card3D : CardAnimation
    {
        private CardInfo info;
        [SerializeField] private MeshRenderer cardRenderer;

        private int _index;
        public int Index
        {
            set
            {
                _index = value;
                _delay = (_index + 1) * GameConstants.CardAnimDuration;
                _yPos = (_index + 1) * GameConstants.CardWidth;
            } 
        }

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

            // Intro sequence
            var sequence = DOTween.Sequence();
            sequence
                .Append(Move(deck.position + new Vector3(0f, _yPos, 0f)))
                .Join(Rotate(deck.rotation.eulerAngles))
                .SetDelay(_delay)
                .Play();
        }

        public void JumpRotateCard(Transform target, bool isDelayed = true, bool setParent = false)
        {
            var pos = target.position + new Vector3(0f, _yPos, 0f);

            var x = new System.Random().Next(2);
            var y = new System.Random().Next(2);
            var z = new System.Random().Next(2);

            var posNoise = pos + new Vector3(x, y, z) * 0.15f;
            
            
            var rot = target.rotation.eulerAngles;

            var delay = isDelayed ? _delay : 0;

            var sequence = DOTween.Sequence();
            sequence
                .Append(Jump(posNoise))
                .Join(Rotate(rot))
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    Move(pos).Play();
                    if (setParent)
                        transform.SetParent(target);
                })
                .Play();
        }

        public Sequence JumpRotateSequence(Transform target, bool isDelayed = true, bool setParent = false)
        {
            var pos = target.position + new Vector3(0f, _yPos, 0f);
            var rot = target.rotation.eulerAngles;

            var delay = isDelayed ? _delay : 0;

            var sequence = DOTween.Sequence();
            return sequence
                    .Append(Jump(pos))
                    .Join(Rotate(rot))
                    .SetDelay(delay)
                    .OnComplete(() =>
                    {
                        if (setParent)
                            transform.SetParent(target);
                    })
                ;
        }
    }
}