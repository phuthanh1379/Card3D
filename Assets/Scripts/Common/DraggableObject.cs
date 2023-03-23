using DG.Tweening;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoardController table;
    
    private float _startYPos;
    private bool _isSnappable = true;
    private bool _isTriggered = false;

    private Tween fallTween;

    private void Awake()
    {
        table = BoardController.Instance;
    }

    private void Start()
    {
        _startYPos = 0f;

        fallTween = transform.DOMoveY(0f, 0.5f);
        fallTween.Play();
    }

    private void OnMouseDrag()
    {
        _isSnappable = false;
        var newWorldPosition = new Vector3(table.currentMousePosition.x, _startYPos + 1, table.currentMousePosition.z);
        
        // var difference = newWorldPosition - transform.position;
        // var mult = 2;
        // rb.velocity = 10 * difference;
        // rb.rotation = Quaternion.Euler(new Vector3(rb.velocity.z * mult, 0f, -rb.velocity.x * mult));

        transform.DOMove(newWorldPosition, 0.5f).Play();
    }

    private void OnMouseUp()
    {
        _isSnappable = true;
        Debug.Log("Fall!");
        fallTween.Restart();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isSnappable) return;
        if (!other.CompareTag("TableSlot")) return;

        _isTriggered = true;
        fallTween.Pause();
        
        // rb.velocity = Vector3.zero;
        var sequence = DOTween.Sequence();
        var position = other.GetComponent<BoardSlot>().GetSnapPosition();
        
        Debug.Log($"x={position.x}, y={position.y}, z={position.z}");
        
        sequence
            .Append(transform.DOMove(position, 0.5f))
            .Play();
    }
}
