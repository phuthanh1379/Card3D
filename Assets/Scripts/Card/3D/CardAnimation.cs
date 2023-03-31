using DG.Tweening;
using UnityEngine;

namespace Card_3D
{
    public class CardAnimation : MonoBehaviour
    {
        private const float Duration = 0.3f;

        protected Tween LocalMove(Vector3 target)
        {
            return transform.DOLocalMove(target, Duration);
        }
        
        protected Tween Jump(Vector3 target, float jumpPower = 1f, int numJumps = 1)
        {
            return transform.DOJump(target, 1f, 1, Duration);
        }

        protected Tween Move(Vector3 target)
        {
            return transform.DOMove(target, Duration);
        }

        protected Tween Rotate(Vector3 target)
        {
            return transform.DORotate(target, Duration);
        }
    }
}