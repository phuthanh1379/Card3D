using Audio;
using DG.Tweening;
using UnityEngine;

namespace Card_3D
{
    public class CardAnimation : MonoBehaviour
    {
        private const float Duration = 0.3f;

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
            return transform.DOJump(target, 1f, 1, Duration);
        }

        public Tween Move(Vector3 target)
        {
            return transform.DOMove(target, Duration);
        }

        public Tween Rotate(Vector3 target)
        {
            return transform.DORotate(target, Duration);
        }
    }
}