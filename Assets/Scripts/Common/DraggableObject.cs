using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoardController table;
    
    private float _startYPos;
    private bool _isSnappable = true;

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
        // rb.isKinematic = false;
        _isSnappable = false;
        var newWorldPosition = new Vector3(table.currentMousePosition.x, _startYPos + 1, table.currentMousePosition.z);
        
        var difference = newWorldPosition - transform.position;

        var mult = 2;
        rb.velocity = 10 * difference;
        rb.rotation = Quaternion.Euler(new Vector3(rb.velocity.z * mult, 0f, -rb.velocity.x * mult));
    }

    private void OnMouseExit()
    {
        _isSnappable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (!_isSnappable) return;
        if (other.CompareTag("TableSlot"))
            transform.DOMove(other.transform.position, 0.5f).Play();
            // transform.position = other.transform.position;
    }
}
