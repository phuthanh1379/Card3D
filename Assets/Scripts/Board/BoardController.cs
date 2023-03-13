using System;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public Vector3 currentMousePosition;
    [SerializeField] private Camera mainCamera;

    public static BoardController Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Table")) continue;
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red);

            currentMousePosition = hit.point;
            break;
        }
    }
}
