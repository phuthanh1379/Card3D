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
    private List<Card3D> _dealt = new();

    private void Start()
    {
        SpawnCards();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            ShowTrump(1);
        
        if (Input.GetKeyDown(KeyCode.Q))
            BackToDeck();
    }

    private void SpawnCards()
    {
        // Spawn the cards
        var rnd = new System.Random();
        for (var i = 0; i < quantity; i++)
        {
            SpawnCard(cardInfo[rnd.Next(cardInfo.Count)], i * GameConstants.CardWidth, i);
        }
    }
    
    private void SpawnCard(CardInfo info, float yPos, int index)
    {
        var newPos = transform.position + new Vector3(0f, 4f, 0f);
        var card = Instantiate(cardPrefab, newPos, Quaternion.identity);
        card.transform.SetParent(transform);

        card.Index = index;
        // card.DeckPosition = new Vector3(0f, yPos, 0f);
        // card.ShowTrumpPosition = showTrumpTarget.position + new Vector3(0f, yPos, 0f);
        
        card.RenderInfo(info, backMaterial, sideMaterial, transform);
     
        _cards.Add(card);
    }

    /// <summary>
    /// Make all cards go back to the deck
    /// </summary>
    private void BackToDeck()
    {
        if (_dealt.Count > 0)
        {
            _cards.AddRange(_dealt);
            _dealt.Clear();
        }

        if (_cards.Count <= 0) return;
        foreach (var card in _cards)
        {
            card.JumpRotateCard(transform, false);
        }
    }

    private void ShowTrump(int num)
    {
        if (_cards.Count <= 0) return;

        for (var i = 0; i < num; i++)
        {
            _cards[^ (i + 1)].JumpRotateCard(showTrumpTarget, false);
        }
    }
    
    public void DealCard(int step, Transform target)
    {
        if (_cards.Count <= 0) return;

        // Top card
        for (var i = 0; i < step; i++)
        {
            var card = _cards[^1];
            _dealt.Add(card);
            _cards.Remove(card);
            card.JumpRotateCard(target);
        }
    }

    public void MoveCards(Transform target)
    {
        if (_cards.Count <= 0)
        {
            if (_dealt.Count <= 0) return;
            foreach (var card in _dealt)
            {
                card.JumpRotateCard(target);
            }
        }
        else
        {
            foreach (var card in _cards)
            {
                card.JumpRotateCard(target);
            }
        }
    }

    public void PickUpCards(Transform target)
    {
        
    }
}
