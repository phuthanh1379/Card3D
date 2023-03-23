using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    private Vector3 snapPosition;

    private const float CardWidth = 0.001f;

    private void Awake()
    {
        snapPosition = transform.position;
    }

    public Vector3 GetSnapPosition()
    {
        var result = snapPosition;
        snapPosition += new Vector3(0f, CardWidth, 0f);
        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
