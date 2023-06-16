using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float duration;
    private Sequence _sequence;

    private void Start()
    {
        var move = transform.DOMove(target.position, duration);
        var stretch = transform.DOScale(new Vector3(1f, 1f, 1.5f), duration * 2 / 3).SetDelay(duration / 3);
        
        // var jump = transform.DOJump(target.position, 1.5f, 1, _duration);
        var rotate = transform.DORotate(target.rotation.eulerAngles, duration);
        var elevate = transform.DOMoveY(1f, duration);
        var stretch2 = transform.DOScale(Vector3.one * 2.5f, duration * 2 / 3).SetDelay(duration / 3);

        var returnMove = transform.DOMoveY(0.0005f, duration);
        var scale = transform.DOScale(Vector3.one * 2f, duration * 2 / 3).SetDelay(duration / 3);

        var s1 = DOTween.Sequence();
        s1
            .Append(move)
            .Join(stretch)
            .SetEase(Ease.InOutBounce)
            .SetAutoKill(false)
            ;

        var s2 = DOTween.Sequence();
        s2
            .Append(elevate)
            .Join(rotate)
            .Join(stretch2)
            .SetAutoKill(false)
            ;

        _sequence = DOTween.Sequence();
        _sequence
            .Append(s1)
            .Append(s2)
            .Append(returnMove)
            .Join(scale)
            .SetAutoKill(false)
            ;

        _sequence.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _sequence.Restart();
    }
}
