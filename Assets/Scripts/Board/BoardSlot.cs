using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public Vector3 snapPosition;

    private void Awake()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
