using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Card_3D
{
    public class CardAnimation : MonoBehaviour
    {
        private Sequence showTrumpSequence;
        private Sequence introSequence;
        private Sequence dealSequence;

        private const float Duration = 0.3f;

        #region Show Trump (Move + Jump + Rotate)

        protected void InitShowTrumpSequence(Vector3 target)
        {
            showTrumpSequence = DOTween.Sequence();
            var jump = transform.DOJump(
                target, 1f, 1, Duration);
            var rotate = transform.DORotate(Vector3.zero, Duration);

            showTrumpSequence
                .Append(jump)
                .Join(rotate)
                .SetAutoKill(false);
        }

        protected void PlayShowTrumpSequence(float delay)
        {
            showTrumpSequence
                .SetDelay(delay)
                .Restart();
        }

        protected void PlayShowTrumpSequenceReverse(float delay)
        {
            showTrumpSequence
                // .SetDelay(delay)
                .PlayBackwards();
        }

        #endregion

        #region Intro (Fall to Target)

        protected void InitIntroSequence(Vector3 target)
        {
            introSequence = DOTween.Sequence();
            var move = transform.DOLocalMove(target, Duration);
            var rotate = transform.DORotate(
                new Vector3(0f, 0f, 180f), Duration);

            introSequence
                .Append(move)
                .Join(rotate)
                .SetAutoKill(false);
        }

        protected void PlayIntroSequence(float delay)
        {
            introSequence
                .SetDelay(delay)
                .Restart();
        }

        #endregion

        #region Deal (Move (and Rotate) to Target)

        protected void InitDealSequence(Transform target)
        {
            dealSequence = DOTween.Sequence();
            var move = transform.DOMove(
                transform.position, Duration);
            var rotate = transform.DORotate(
                transform.rotation.eulerAngles, Duration);

            dealSequence
                .Append(move)
                .Join(rotate)
                .SetAutoKill(false);
        }

        protected void PlayDealSequence(float delay)
        {
            dealSequence
                .SetDelay(delay)
                .Restart();
        }

        #endregion
    }
}