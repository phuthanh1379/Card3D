using System;
using System.Collections;
using System.Collections.Generic;
using Card_3D;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    [Header("Deck Info")]
    [SerializeField] private int quantity;
    [SerializeField] private Card3D cardPrefab;

    [Header("Spawn Info -- Will move to CardDatabase or something")]
    [SerializeField] private List<CardInfo> cardInfo = new();
    [SerializeField] private Material backMaterial;
    [SerializeField] private Material sideMaterial;
    
    private List<Card3D> _cards = new();
    private const float CardWidth = 0.001f;

    private void Start()
    {
        SpawnCards();
    }

    private void SpawnCards()
    {
        // Spawn the cards
        var rnd = new System.Random();
        for (var i = 0; i < quantity; i++)
        {
            SpawnCard(cardInfo[rnd.Next(cardInfo.Count)], i * CardWidth);
        }
    }
    
    private void SpawnCard(CardInfo info, float yPos)
    {
        var card = Instantiate(cardPrefab, new Vector3(0f, yPos, 0f), Quaternion.identity);
        card.transform.SetParent(transform);
        card.RenderInfo(info, backMaterial, sideMaterial);
     
        _cards.Add(card);
    }
}
