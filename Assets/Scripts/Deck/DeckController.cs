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

    [Header("Animation")] 
    [SerializeField] private Transform showTrumpTarget;

    private List<Card3D> _cards = new();
    private const float CardWidth = 0.001f;

    private void Start()
    {
        SpawnCards();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            PlayCardsFx();
        
        if (Input.GetKeyDown(KeyCode.Q))
            PlayCardsFxRev();
    }

    private void SpawnCards()
    {
        // Spawn the cards
        var rnd = new System.Random();
        for (var i = 0; i < quantity; i++)
        {
            SpawnCard(cardInfo[rnd.Next(cardInfo.Count)], i * CardWidth, i);
        }
    }
    
    private void SpawnCard(CardInfo info, float yPos, int index)
    {
        // var newPos = transform.position + new Vector3(0f, yPos, 0f);
        var newPos = transform.position + new Vector3(0f, 4f, 0f);
        var card = Instantiate(cardPrefab, newPos, Quaternion.identity);
        card.transform.SetParent(transform);

        card.Index = index;
        card.DeckPosition = new Vector3(0f, yPos, 0f);
        card.ShowTrumpPosition = showTrumpTarget.position + new Vector3(0f, yPos, 0f);
        
        card.RenderInfo(info, backMaterial, sideMaterial);
     
        _cards.Add(card);
    }

    private void PlayCardsFx()
    {
        foreach (var card in _cards)
        {
            card.PlayEffect();
        }
    }
    
    private void PlayCardsFxRev()
    {
        foreach (var card in _cards)
        {
            card.PlayEffectReverse();
        }
    }
}
