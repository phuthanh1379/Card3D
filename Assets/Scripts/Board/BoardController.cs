using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public Vector3 currentMousePosition;
    private Camera mainCamera;

    public static BoardController Instance;
    
    [SerializeField] private List<Transform> slots = new();

    private void OnDrawGizmos()
    {
        if (slots.Count == 0) return;
        foreach (var slot in slots)
        {
            Gizmos.DrawWireCube(slot.position, new Vector3(1f, 1f, 1f));
        }
    }
    
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
