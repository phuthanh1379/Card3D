using Audio;
using DG.Tweening;
using UnityEngine;

namespace Card_3D
{
    public class CardAnimation : MonoBehaviour
    {
        private const float Duration = 0.3f;

        #region Basic

        public Tween LocalMove(Vector3 target)
        {
            return transform.DOLocalMove(target, Duration);
        }

        public Tween LocalRotate(Vector3 target)
        {
            return transform.DOLocalRotate(target, Duration);
        }
        
        public Tween Jump(Vector3 target, float jumpPower = 1f, int numJumps = 1)
        {
            return transform.DOJump(target, jumpPower, numJumps, Duration);
        }

        public Tween Move(Vector3 target)
        {
            return transform.DOMove(target, Duration);
        }

        public Tween Rotate(Vector3 target)
        {
            return transform.DORotate(target, Duration);
        }

        public Tween ShakePosition()
        {
            return transform.DOShakePosition(Duration, 1f, 10);
        }

        public Tween ShakeRotation()
        {
            return transform.DOShakeRotation(Duration, 90f, 10);
        }

        #endregion

        #region Advanced

        public Sequence LocalMoveSequence(Vector3 target, float noiseStrength = 0f)
        {
            var sequence = DOTween.Sequence();
            if (noiseStrength == 0f) 
                return sequence.Append(transform.DOLocalMove(target, Duration));
            
            var noiseTarget = target + Vector3.one * noiseStrength;
            
            var move = transform.DOLocalMove(target, Duration / 2);
            var noise = transform.DOLocalMove(noiseTarget, Duration);

            sequence
                .Append(noise)
                .Append(move)
                ;
            
            return sequence;
        }

        // public Tween LocalRotate(Vector3 target)
        // {
        //     return transform.DOLocalRotate(target, Duration);
        // }
        
        public Sequence JumpSequence(Vector3 target, float jumpPower = 1f, int numJumps = 1, float noiseStrength = 0f)
        {
            if (noiseStrength == 0f) 
                return transform.DOJump(target, jumpPower, numJumps, Duration);
            
            var sequence = DOTween.Sequence();
            var noiseTarget = target + Vector3.one * noiseStrength;
            
            var move = transform.DOMove(target, Duration / 2);
            var noise = transform.DOJump(noiseTarget, jumpPower, numJumps, Duration);

            sequence
                .Append(noise)
                .Append(move)
                ;
            
            return sequence;

        }
        
        public Sequence MoveSequence(Vector3 target, float noiseStrength = 0f)
        {
            var sequence = DOTween.Sequence();
            if (noiseStrength == 0f) 
                return sequence.Append(transform.DOMove(target, Duration));
            
            var noiseTarget = target + Vector3.one * noiseStrength;
            
            var move = transform.DOMove(target, Duration / 2);
            var noise = transform.DOMove(noiseTarget, Duration);

            sequence
                .Append(noise)
                .Append(move)
                ;
            
            return sequence;
        }

        // public Tween Rotate(Vector3 target)
        // {
        //     return transform.DORotate(target, Duration);
        // }

        #endregion
    }
}