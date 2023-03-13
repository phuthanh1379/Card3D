using System.Collections.Generic;
using UnityEngine;

public class CardSpawn : MonoBehaviour
{
    [SerializeField] private List<CardInfo> cardInfo = new();
    [SerializeField] private Card3D baseCard;
    [SerializeField] private Material backMaterial;
    [SerializeField] private Transform cardParent;
 
    private List<Card3D> _cards = new();
    
    private void Start()
    {
        foreach (var info in cardInfo)
        {
            SpawnCard(info);
        }
    }

    private void SpawnCard(CardInfo info)
    {
        var card = Instantiate(baseCard, transform);
        card.RenderInfo(info, backMaterial);
        _cards.Add(card);
    }
}
