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
    private static readonly Vector3 PlayerLook = new Vector3(0f, 2f, -4f);
    private const float PlayerLookAngle = 30f;

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
        // var orthoTween
        overLookSequence
            .Append(overMoveTween)
            .Join(overRotateTween)
            .SetAutoKill(false)
            .SetEase(EaseType)
            ;

        playerLookSequence = DOTween.Sequence();
        var playerMoveTween = transform.DOMove(PlayerLook, Duration);
        var playerRotateTween = transform.DORotate(new Vector3(PlayerLookAngle, 0f, 0f), Duration);
        // var orthoTween
        playerLookSequence
            .Append(playerMoveTween)
            .Join(playerRotateTween)
            .SetAutoKill(false)
            .SetEase(EaseType)
            ;
        
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