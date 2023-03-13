using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoardController table;

    private float _startYPos;

    private void Awake()
    {
        table = BoardController.Instance;
    }

    private void Start()
    {
        _startYPos = 0f;
    }

    private void OnMouseDrag()
    {
        var newWorldPosition = new Vector3(table.currentMousePosition.x, _startYPos + 1, table.currentMousePosition.z);
        
        var difference = newWorldPosition - transform.position;

        var mult = 2;
        rb.velocity = 10 * difference;
        rb.rotation = Quaternion.Euler(new Vector3(rb.velocity.z * mult, 0f, -rb.velocity.x * mult));
    }
}
