using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] private List<Transform> points = new();

    private void Start()
    {
        var sequence = DOTween.Sequence();
        foreach (var point in points)
        {
            sequence.Append(transform.DOMove(point.position, 1f));
        }

        sequence.Play();
    }
}
