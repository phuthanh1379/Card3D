using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public Vector3 currentMousePosition;
    private Camera mainCamera;

    public static BoardController Instance;
    
    [SerializeField] private List<Transform> slots = new();
    [SerializeField] private List<Transform> hands = new();

    private void OnDrawGizmos()
    {
        if (slots.Count == 0) return;
        foreach (var slot in slots)
        {
            Gizmos.DrawWireCube(slot.position + new Vector3(0f, 0.25f, 0f), new Vector3(0.85f, 0.5f, 0.85f));
        }
        
        if (hands.Count == 0) return;
        foreach (var hand in hands)
        {
            Gizmos.DrawWireCube(hand.position + new Vector3(0f, 0.25f, 0f), new Vector3(0.85f, 0.5f, 0.85f));
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
        // var hits = new RaycastHit[] { };
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
