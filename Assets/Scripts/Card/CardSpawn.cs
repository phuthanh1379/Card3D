using System;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawn : MonoBehaviour
{
    [SerializeField] private List<CardInfo> cardInfo = new();
    [SerializeField] private Card3D baseCard;
    [SerializeField] private Material backMaterial;
    [SerializeField] private Material sideMaterial;
    [SerializeField] private Transform cardParent;
 
    private List<Card3D> _cards = new();
    
    private void Start()
    {
        foreach (var info in cardInfo)
        {
            SpawnCard(info);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            SpawnRandomCard();
    }

    private void SpawnCard(CardInfo info)
    {
        var card = Instantiate(baseCard, cardParent);
        card.RenderInfo(info, backMaterial, sideMaterial);
        _cards.Add(card);
    }

    private void SpawnRandomCard()
    {
        var rnd = new System.Random();
        SpawnCard(cardInfo[rnd.Next(cardInfo.Count)]);
    }
}
