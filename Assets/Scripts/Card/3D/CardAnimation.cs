using Audio;
using DG.Tweening;
using UnityEngine;

namespace Card_3D
{
    public class CardAnimation : MonoBehaviour
    {
        private const float Duration = 0.3f;

        #region Basic

        public Tween LocalMove(Vector3 target, float duration = Duration)
        {
            return transform.DOLocalMove(target, duration);
        }

        public Tween LocalRotate(Vector3 target, float duration = Duration)
        {
            return transform.DOLocalRotate(target, duration);
        }
        
        public Tween Jump(Vector3 target, float duration = Duration, float jumpPower = 1f, int numJumps = 1)
        {
            return transform.DOJump(target, jumpPower, numJumps, duration);
        }

        public Tween Move(Vector3 target, float duration = Duration)
        {
            return transform.DOMove(target, duration);
        }

        public Tween Rotate(Vector3 target, float duration = Duration)
        {
            return transform.DORotate(target, duration);
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
    }
}