using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject card;
    [SerializeField] private Vector3 a;
    [SerializeField] private Vector3 b;
    [SerializeField] private Vector3 c;
    [SerializeField] private int quantity;

    private List<Vector3> _positions = new();
    private float i = 0f;

    private void SlerpMove(float index)
    {
        var t = index / quantity;
        Debug.Log(t);
        var first = Vector3.Slerp(a, b, t);
        var second = Vector3.Slerp(b, c, t);

        var go = Instantiate(card);
        go.transform.position = Vector3.Slerp(first, second, t);
        // card.transform.position = Vector3.Slerp(first, second, t);
    }
    
    private void LerpMove(float index)
    {
        var t = index / quantity;
        Debug.Log(t);
        var first = Vector3.Lerp(a, b, t);
        var second = Vector3.Lerp(b, c, t);
        
        var go = Instantiate(card);
        go.transform.position = Vector3.Slerp(first, second, t);
        // card.transform.position = Vector3.Lerp(first, second, t);
    }

    private void Update()
    {
        if (i > quantity) return;
        LerpMove(i);
        i += 1f;
    }
}
