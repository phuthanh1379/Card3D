using UnityEngine;

public class TestSpawnCard : MonoBehaviour
{
    [SerializeField] private GameObject card;
    
    private bool _isContinuousDynamic = false;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            SpawnCard();
    }
    
    private void SpawnCard()
    {
        Instantiate(card, transform.position, Quaternion.identity);
    
        card.GetComponent<Rigidbody>().collisionDetectionMode = 
            !_isContinuousDynamic ? CollisionDetectionMode.Continuous : CollisionDetectionMode.ContinuousDynamic;
            
        _isContinuousDynamic = !_isContinuousDynamic;
    }
}
