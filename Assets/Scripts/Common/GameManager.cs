using System;
using System.Collections;
using System.Collections.Generic;
using Card_3D;
using Common;
using Deck;
using DG.Tweening;
using UnityEditor;
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
        
        // TODO: Init deck
        deck.Init();
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

    #region Deal Cards
    
    private void ShowTrump(int num)
    {
        if (_cards.Count <= 0) return;

        for (var i = 0; i < num; i++)
        {
            _cards[^ (i + 1)].JumpRotateCard(showTrumpTarget, false);
        }
    }
    
    /// <summary>
    /// Deal cards from given steps
    /// </summary>
    private void DealSteps()
    {
        // S - E - N - W
        foreach (var step in steps)
        {
            for (var j = 0; j < board.Hands.Count; j++)
            {
                DealCard(step, board.Hands[j], (Direction) j);
            }
        }
    }

    private void DealCard(int step, Hand targetHand, Direction direction)
    {
        if (_cards.Count <= 0) return;

        for (var i = 0; i < step; i++)
        {
            // Top card 
            var card = _cards[^1];

            foreach (var hand in board.Hands)
            {
                if (hand.direction == direction)
                    hand.cards.Add(card);
            }
                
            _cards.Remove(card);
            card.JumpRotateCard(targetHand.holder, setParent: true);
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
                
                card.JumpRotateCard(hand.holder);
            }
        }
    }
    
    #endregion

    #region Pickup Cards

        /// <summary>
        /// Animation when first pick up cards (before going to player's hand) 
        /// </summary>
        /// <param name="target"></param>
        private void PickUpCards(Transform target)
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

                    mainSequence.Join(cards[i].JumpRotateSequence(target, setParent: true));
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

                    mainSequence.Join(cards[i].JumpRotateSequence(target, setParent: true));
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

        private void SlidingCards(Transform target)
        {
            var cards = _cards;
            
            var a = new Vector3(-CardsPickUp.XLim, 0f, -CardsPickUp.ZLim);
            var b = Vector3.zero;
            var c = new Vector3(CardsPickUp.XLim, 0f, -CardsPickUp.ZLim);

            var mainSequence = DOTween.Sequence();
            var finalSeq = DOTween.Sequence();
            
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
                var pos = final + new Vector3(0f, i * CardsPickUp.YStep, 0f);
                var rot = new Vector3(0f, step * CardsPickUp.YRotate, 0f);
                // var t1 = cards[i].LocalMove(final + new Vector3(0f, i * CardsPickUp.YStep, 0f));
                // var t2 = cards[i].Rotate(new Vector3(0f, step * CardsPickUp.YRotate, 0f));

                // mainSequence.Join(cards[i].JumpRotateSequence(target, setParent: true));
                // finalSeq
                //     .Join(t1)
                //     .Join(t2)
                //     ;

                var i1 = i;
                var move = deck.transform
                    .DOMove(pos, 0.1f)
                    .OnComplete(() =>
                    {
                        cards[i1].transform.SetParent(transform);
                    })
                    ;
                var rotate = deck.transform.DORotate(rot, 0.1f);
                mainSequence
                    .Append(move)
                    // .Join(rotate)
                    ;

                // var step = Mathf.Abs(i - m1) < Mathf.Abs(i - m2) ? (i - m1 - 0.5f) : (i - m2 + 0.5f);
                // var t1 = cards[i].LocalMove(final + new Vector3(0f, i * CardsPickUp.YStep, 0f));
                // var t2 = cards[i].Rotate(new Vector3(0f, step * CardsPickUp.YRotate, 0f));

                // mainSequence.Join(cards[i].JumpRotateSequence(target, setParent: true));
                // finalSeq
                //     .Join(t1)
                //     .Join(t2)
                //     ;
            }
            
            // finalSeq.OnComplete(() => MoveToHands(playerHand, cards));
            mainSequence
                // .Append(finalSeq)
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

        if (cards.Count % 2 != 0)
        {
            var m = cards.Count / 2;

            for (var i = 0; i < cards.Count; i++)
            {
                cards[i].Index = i;
                cards[i].transform.SetParent(target);

                var step = i - m;
                var pos = target.position + new Vector3(step * CardsToHand.XStep, step * CardsToHand.YStep, 0f);
                var t1 = cards[i].Move(pos);
                var t2 = cards[i].LocalRotate(Vector3.zero);

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
                cards[i].Index = i;
                cards[i].transform.SetParent(target);

                var step = Mathf.Abs(i - m1) < Mathf.Abs(i - m2) ? (i - m1 - 0.5f) : (i - m2 + 0.5f);
                var pos = target.position + new Vector3(step * CardsToHand.XStep, step * CardsToHand.YStep, 0f);
                var t1 = cards[i].Move(pos);
                var t2 = cards[i].LocalRotate(Vector3.zero);

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
            PickUpCards(playerIntro);
        
        // Deck
        if (Input.GetKeyDown(KeyCode.A))
            ShowTrump(3);

        if (Input.GetKeyDown(KeyCode.Q))
            BackToDeck();
        
        if (Input.GetKeyDown(KeyCode.Return))
            Shake();

        if (Input.GetKeyDown(KeyCode.D))
            SlidingCards(playerIntro);
    }

    private void Shake()
    {
        foreach (var card in _cards)
        {
            var sequence = DOTween.Sequence();
            var shakePos = card.ShakePosition();
            var shakeRot = card.ShakeRotation();
            sequence
                // .Append(shakePos)
                .Join(shakeRot)
                .Play();
        }
    }

    #endregion
}
