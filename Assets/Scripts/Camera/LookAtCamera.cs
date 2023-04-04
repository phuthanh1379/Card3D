using System;
using DG.Tweening;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Sequence overLookSequence;
    private Sequence playerLookSequence;

    private const Ease EaseType = Ease.InOutQuad;
    private const float Duration = 0.1f;

    private void Awake()
    {
        overLookSequence = DOTween.Sequence();
        var move1 = transform.DOMove(new Vector3(0f, 0f, -3f), Duration);
        var rotate1 = transform.DORotate(new Vector3(-20f, 0f, 0f), Duration);
        var scale1 = transform.DOScale(Vector3.one * 1.5f, Duration);
        overLookSequence
            .Append(move1)
            .Join(rotate1)
            .Join(scale1)
            .SetAutoKill(false)
            .SetEase(EaseType)
            ;

        playerLookSequence = DOTween.Sequence();
        var move2 = transform.DOMove(new Vector3(0f, 0.75f, -3f), Duration);
        var rotate2 = transform.DORotate(new Vector3(-60f, 0f, 0f), Duration);
        var scale2 = transform.DOScale(Vector3.one * 0.75f, Duration);
        playerLookSequence
            .Append(move2)
            .Join(rotate2)
            .Join(scale2)
            .SetAutoKill(false)
            .SetEase(EaseType)
            ;
    }

    private void OnCameraChanges(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.Over:
                overLookSequence.Restart();
                break;
            case CameraMode.Player:
                playerLookSequence.Restart();
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        CameraController.CameraChanges += OnCameraChanges;
    }

    private void OnDisable()
    {
        CameraController.CameraChanges -= OnCameraChanges;
    }
}
