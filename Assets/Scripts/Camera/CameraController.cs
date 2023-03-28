using System;
using DG.Tweening;
using UnityEngine;

public enum CameraMode
{
    Over = 0,
    Player = 1
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private static readonly Vector3 OverLook = new Vector3(0f, 5f, -3f);
    private const float OverLookAngle = 70f;
    private const float OverLookFOV = 25f;
    private static readonly Vector3 PlayerLook = new Vector3(0f, 2f, -4f);
    private const float PlayerLookAngle = 30f;
    private const float PlayerLookFOV = 60f;

    private Sequence overLookSequence;
    private Sequence playerLookSequence;
    private const float Duration = 0.25f;
    private const Ease EaseType = Ease.InOutQuad;
    private CameraMode _currentMode;

    private void Awake()
    {
        overLookSequence = DOTween.Sequence();
        var overMoveTween = transform.DOMove(OverLook, Duration);
        var overRotateTween = transform.DORotate(new Vector3(OverLookAngle, 0f, 0f), Duration);
        var overCameraTween = mainCamera.DOFieldOfView(OverLookFOV, Duration);

        overLookSequence
            .Append(overMoveTween)
            .Join(overRotateTween)
            .Join(overCameraTween)
            .SetAutoKill(false)
            .SetEase(EaseType)
            ;

        playerLookSequence = DOTween.Sequence();
        var playerMoveTween = transform.DOMove(PlayerLook, Duration);
        var playerRotateTween = transform.DORotate(new Vector3(PlayerLookAngle, 0f, 0f), Duration);
        var playerCameraTween = mainCamera.DOFieldOfView(PlayerLookFOV, Duration);

        playerLookSequence
            .Append(playerMoveTween)
            .Join(playerRotateTween)
            .Join(playerCameraTween)
            .SetAutoKill(false)
            .SetEase(EaseType)
            ;
    }

    private void Start()
    {
        ChangeMode(CameraMode.Over);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeMode(CameraMode.Player);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeMode(CameraMode.Over);
        }
    }

    private void ChangeMode(CameraMode mode)
    {
        if (_currentMode == mode) return;
        _currentMode = mode;
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
}