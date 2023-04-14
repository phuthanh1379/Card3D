using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float _duration = 0.5f;
    private Sequence _sequence;

    private void Start()
    {
        _sequence = DOTween.Sequence();
        
        var jump = transform.DOJump(target.position, 1.5f, 1, _duration);
        
        var fxDuration = _duration * 2 / 3;
        var rotateNoise = transform
            .DORotate(new Vector3(0f, 90f, 0f), fxDuration / 2)
            ;
        
        var rotate  = transform
                .DORotate(target.rotation.eulerAngles, fxDuration / 2)
            ;
        
        var scale = transform
            .DOScale(Vector3.one * 2f, fxDuration)
            .SetDelay(fxDuration / 2)
            ;
        
        var s2 = DOTween.Sequence();
        s2
            .Append(rotateNoise)
            .Join(rotate)
            .SetAutoKill(false)
            .SetDelay(_duration / 3)
            ;

      _sequence
            .Append(jump)
            .Join(s2)
            .Join(scale)
            .SetAutoKill(false)
            .SetEase(Ease.OutBounce)
            ;

        _sequence.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _sequence.Restart();
    }
}
