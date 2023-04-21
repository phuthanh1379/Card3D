using System;
using System.Collections;
using System.Collections.Generic;
using Board;
using Card_3D;
using Common;
using Deck;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardManager board;
    [SerializeField] private DeckController deck;
    
    [Header("Spawn Info -- Will move to CardDatabase or something")]
    [SerializeField] private Card3D cardPrefab;
    [SerializeField] private List<CardInfo> cardInfo = new();
    [SerializeField] private Material backMaterial;
    [SerializeField] private Material sideMaterial;
    
    [Header("Target")]
    [SerializeField] private Transform showTrumpTarget;
    [SerializeField] private Transform playerIntro;
    [SerializeField] private Transform playerHand;

    [Header("Settings")] 
    [SerializeField] private int numberOfCards;
    [SerializeField] private List<int> steps = new();
    
    // Private
    private List<Card3D> _cards = new();

    private void Init()
    {
        // TODO: Init board
        board.Init();
        
        // TODO: Init deck
        
    }

    #region Spawn

    private void SpawnCards()
    {
        // Spawn the cards
        var rnd = new System.Random();
        for (var i = 0; i < numberOfCards; i++)
        {
            // Spawn random
            SpawnCard(cardInfo[rnd.Next(cardInfo.Count)], i * GameConstants.CardWidth, i);
        }
    }

    private void SpawnCard(CardInfo info, float yPos, int index)
    {
        var newPos = deck.Pos + new Vector3(0f, 4f, 0f);
        var card = Instantiate(cardPrefab, newPos, Quaternion.identity);
        card.transform.SetParent(deck.transform);

        card.Index = index;
        card.RenderInfo(info, backMaterial, sideMaterial, deck.transform);

        _cards.Add(card);
    }

    #endregion

    #region Back to Deck

    /// <summary>
    /// Make all cards go back to the deck
    /// </summary>
    private void BackToDeck()
    {
        // TODO: Move to GameManager
        foreach (var hand in board.Hands)
        {
            if (hand.cards.Count > 0)
            {
                _cards.AddRange(hand.cards);
                hand.cards.Clear();
            }
        }

        if (_cards.Count <= 0) return;
        ReIndex();
        for (var i = 0; i < _cards.Count; i++)
        {
            _cards[i].Index = i;
            _cards[i].JumpRotateCard(deck.transform, false, true);
        }
    }

    private void ReIndex()
    {
        for (var i = 0; i < _cards.Count; i++)
        {
            _cards[i].Index = i;
        }
    }

    #endregion

    #region Shuffle
    
    private void Shuffle()
    {
        if (_cards is not { Count: > 0 }) return;
        
        // Cut the deck to 2 equal size
        var half = new List<Card3D>();
        var temp = new List<Card3D>();
        temp.AddRange(_cards);

        var x = showTrumpTarget.position.x;
        var d = 0.15f;

        var cut = DOTween.Sequence();
        Debug.Log($"h={_cards.Count / 2}, c={_cards.Count}");

        for (var i = 0; i < _cards.Count / 2; i++)
        {
            cut.Append(_cards[i].transform.DOMoveX(x, d));
            
            half.Add(_cards[i]);
            _cards.Remove(_cards[i]);
        }

        Debug.Log($"h={half.Count}, c={_cards.Count}");
        
        // Intertwine the cards from both halves
        var seq = DOTween.Sequence();
        
        for (var i = 0; i < temp.Count; i++)
        {
            if (i % 2 == 0)
            {
                if (half.Count > 0)
                {
                    seq.Append(half[0].transform.DOMoveX(x / 2, d));
                    temp.Add(half[0]);
                    half.Remove(half[0]);
                }
                else
                {
                    if (_cards.Count <= 0) break;
                    seq.Append(_cards[0].transform.DOMoveX(x / 2, d));
                    temp.Add(_cards[0]);
                    _cards.Remove(_cards[0]);
                }
                
                Debug.Log($"h={half.Count}, c={_cards.Count}");
            }
            else
            {
                if (_cards.Count > 0)
                {
                    seq.Append(_cards[0].transform.DOMoveX(x / 2, d));
                    temp.Add(_cards[0]);
                    _cards.Remove(_cards[0]);
                }
                else
                {
                    if (half.Count <= 0) break;
                    seq.Append(half[0].transform.DOMoveX(x / 2, d));
                    temp.Add(half[0]);
                    half.Remove(half[0]);
                }
                
                Debug.Log($"h={half.Count}, c={_cards.Count}");
            }
        }

        cut.Append(seq).Play();
    }

    #endregion

    #region Deal Cards
    
    private void ShowTrump(int num)
    {
        if (_cards.Count <= num) return;
        for (var i = 0; i < num; i++)
        {
            _cards[^(i + 1)].JumpRotateCard(showTrumpTarget, false);
        }
    }
    
    /// <summary>
    /// Deal cards from given steps
    /// </summary>
    private void DealSteps()
    {
        var mainSeq = DOTween.Sequence();
        var finalSeq = DOTween.Sequence();
        
        // S - E - N - W
        foreach (var step in steps)
        {
            for (var j = 0; j < board.Hands.Count; j++)
            {
                DealCard(step, board.Hands[j], (Direction) j, ref mainSeq, ref finalSeq);
            }
        }

        mainSeq
            .Append(finalSeq)
            .Play();
    }

    private void DealCard(int step, Hand targetHand, Direction direction, ref Sequence mainSeq, ref Sequence finalSeq)
    {
        if (_cards.Count <= 0) return;

        for (var i = 0; i < step; i++)
        {
            // Top card 
            var card = _cards[^1];
            var hand = board.GetHand(direction);
            var yPos = 0f;
            hand?.AddCard(card, out yPos);
            _cards.Remove(card);

            var sequence = DOTween.Sequence();

            var pos = targetHand.slot.Position + new Vector3(0f, yPos, 0f);
            var rot = targetHand.slot.Rotation;
            var x = new System.Random().Next(2);
            // var y = new System.Random().Next(2);
            var z = new System.Random().Next(2);
            var posNoise = pos + new Vector3(x, yPos, z) * CardsDeal.NoiseValue;
            var rePosTween = card.Move(pos);

            sequence
                .Append(card.Jump(posNoise, CardsDeal.Duration))
                .Join(card.Rotate(rot, CardsDeal.Duration))
                .SetEase(CardsDeal.EaseType)
                ;

            mainSeq.Append(sequence);
            finalSeq.Join(rePosTween);
        }
    }

    #endregion

    #region Move Cards

    private void MoveCards(Direction direction)
    {
        foreach (var hand in board.Hands)
        {
            if (hand.direction != direction) continue;
            foreach (var card in _cards)
            {
                // if (_cards.Count <= 0)
                // {
                //     foreach (var hand in hands)
                //     {
                //         if (hand.cards.Count > 0)
                //         {
                //             foreach (var card in hand.cards)
                //                 card.JumpRotateCard(target);
                //         }
                //     }
                // }
                // else
                // {
                //     foreach (var card in _cards)
                //     {
                //         card.JumpRotateCard(target);
                //     }
                // }
                
                card.JumpRotateCard(hand.slot.transform);
            }
        }
    }
    
    #endregion

    #region Pickup Cards

    /// <summary>
    /// Animation when first pick up cards (before going to player's hand) 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="targetHand"></param>
    private void PickUpCards(Transform target, Transform targetHand)
    {
        // Check if player has cards
        // var hand = board.GetHand(Direction.South);
        // if (hand == null) return;
        // if (hand.cards.Count <= 0) return;
        
        // var cards = hand.cards;
        var cards = _cards;
        
        var a = new Vector3(-CardsPickUp.XLim, 0f, -CardsPickUp.ZLim);
        var b = Vector3.zero;
        var c = new Vector3(CardsPickUp.XLim, 0f, -CardsPickUp.ZLim);

        var mainSequence = DOTween.Sequence();
        var finalSeq = DOTween.Sequence();

        if (cards.Count % 2 != 0)
        {
            var m = cards.Count / 2;

            for (var i = 0; i < cards.Count; i++)
            {
                cards[i].Index = i;

                // Lerp to make curves
                var t = (float)(i + 1) / (cards.Count + 1);
                var first = Vector3.Lerp(a, b, t);
                var second = Vector3.Lerp(b, c, t);
                var final = Vector3.Lerp(first, second, t);

                var step = i - m;
                var t1 = cards[i].LocalMove(final + new Vector3(0f, i * CardsPickUp.YStep, 0f));
                var t2 = cards[i].Rotate(new Vector3(0f, step * CardsPickUp.YRotate, 0f));

                mainSequence.Join(cards[i].JumpRotateSequence(target, out var rePosTween, useNoise: false, setParent: true));
                finalSeq
                    .Join(t1)
                    .Join(t2)
                    ;
            }
        }
        else
        {
            var m1 = cards.Count / 2 - 1;
            var m2 = cards.Count / 2;

            for (var i = 0; i < cards.Count; i++)
            {
                cards[i].Index = i;

                // Lerp to make curves
                var t = (float)(i + 1) / (cards.Count + 1);
                var first = Vector3.Lerp(a, b, t);
                var second = Vector3.Lerp(b, c, t);
                var final = Vector3.Lerp(first, second, t);

                var step = Mathf.Abs(i - m1) < Mathf.Abs(i - m2) ? (i - m1 - 0.5f) : (i - m2 + 0.5f);
                var t1 = cards[i].LocalMove(final + new Vector3(0f, i * CardsPickUp.YStep, 0f));
                var t2 = cards[i].Rotate(new Vector3(0f, step * CardsPickUp.YRotate, 0f));

                mainSequence.Join(cards[i].JumpRotateSequence(target, out var rePosTween, useNoise: false, setParent: true));
                finalSeq
                    .Join(t1)
                    .Join(t2)
                    ;
            }
        }

        finalSeq.OnComplete(() => MoveToHands(playerHand, cards));
        mainSequence
            .Append(finalSeq)
            .Play();
    }

    #endregion

    #region Move to Hands

    /// <summary>
    /// Move the picked up cards to player's hand
    /// </summary>
    /// <param name="target"></param>
    /// <param name="cards"></param>
    private void MoveToHands(Transform target, List<Card3D> cards)
    {
        var finalSeq = DOTween.Sequence();
        
        var a = new Vector3(-CardsToHand.XLim, 0f, -CardsToHand.ZLim);
        var b = Vector3.zero;
        var c = new Vector3(CardsToHand.XLim, 0f, -CardsToHand.ZLim);

        if (cards.Count % 2 != 0)
        {
            var m = cards.Count / 2;

            for (var i = 0; i < cards.Count; i++)
            {
                // Lerp to make curves
                var t = (float)(i + 1) / (cards.Count + 1);
                var first = Vector3.Lerp(a, b, t);
                var second = Vector3.Lerp(b, c, t);
                var final = Vector3.Lerp(first, second, t);
                
                cards[i].Index = i;
                cards[i].transform.SetParent(target);

                var step = i - m;
                // var pos = target.position + new Vector3(step * CardsToHand.XStep, step * CardsToHand.YStep, 0f);
                var pos = final + new Vector3(0f, i * CardsToHand.YStep, 0f);
                var t1 = cards[i].LocalMove(pos);
                var t2 = cards[i].LocalRotate(new Vector3(0f, step * CardsToHand.YRotate, 0f));

                finalSeq
                    .Join(t1)
                    .Join(t2)
                    ;
            }
        }
        else
        {
            var m1 = _cards.Count / 2 - 1;
            var m2 = _cards.Count / 2;

            for (var i = 0; i < cards.Count; i++)
            {
                // Lerp to make curves
                var t = (float)(i + 1) / (cards.Count + 1);
                var first = Vector3.Lerp(a, b, t);
                var second = Vector3.Lerp(b, c, t);
                var final = Vector3.Lerp(first, second, t);
                
                cards[i].Index = i;
                cards[i].transform.SetParent(target);

                var step = Mathf.Abs(i - m1) < Mathf.Abs(i - m2) ? (i - m1 - 0.5f) : (i - m2 + 0.5f);
                // var pos = target.position + new Vector3(step * CardsToHand.XStep, step * CardsToHand.YStep, 0f);
                var pos = final + new Vector3(0f, i * CardsToHand.YStep, 0f);
                var t1 = cards[i].LocalMove(pos);
                var t2 = cards[i].LocalRotate(new Vector3(0f, step * CardsToHand.YRotate, 0f));

                finalSeq
                    .Join(t1)
                    .Join(t2)
                    ;
            }
        }

        finalSeq
            .SetDelay(0.5f)
            .Play();
    }

    #endregion

    #region Unity

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        SpawnCards();
    }

    private void Update()
    {
        // Board
        if (Input.GetKeyDown(KeyCode.Alpha1))
            MoveCards(Direction.South);
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            MoveCards(Direction.East);
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            MoveCards(Direction.North);
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
            MoveCards(Direction.West);
        
        if (Input.GetKeyDown(KeyCode.Space))
            DealSteps();
        
        if (Input.GetKeyDown(KeyCode.E))
            PickUpCards(playerIntro, playerHand);
        
        // Deck
        if (Input.GetKeyDown(KeyCode.X))
            Shuffle();
        
        if (Input.GetKeyDown(KeyCode.D))
            MoveToHands(playerHand, _cards);
            
        if (Input.GetKeyDown(KeyCode.A))
            ShowTrump(1);

        if (Input.GetKeyDown(KeyCode.Q))
            BackToDeck();
    }

    #endregion
}
