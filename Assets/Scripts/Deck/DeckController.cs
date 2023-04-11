using System;
using System.Collections.Generic;
using Card_3D;
using Common;
using DG.Tweening;
using Common;
using UnityEngine;

namespace Deck
{
    public class DeckController : MonoBehaviour
    {
        [Header("Deck Info")] 
        [SerializeField] private Card3D cardPrefab;
        private int quantity;

        [Header("Spawn Info -- Will move to CardDatabase or something")] 
        [SerializeField] private List<CardInfo> cardInfo = new();

        [SerializeField] private Material backMaterial;
        [SerializeField] private Material sideMaterial;

        [Header("Animation")] 
        [SerializeField] private Transform showTrumpTarget;

        private List<Card3D> _cards = new();

        public void Init(int n)
        {
            quantity = n;
        }
        
        public void SpawnCards()
        {
            // Spawn the cards
            var rnd = new System.Random();
            for (var i = 0; i < quantity; i++)
            {
                // Spawn random
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

        private void ReIndex()
        {
            for (var i = 0; i < _cards.Count; i++)
            {
                _cards[i].Index = i;
            }
        }

        /// <summary>
        /// Make all cards go back to the deck
        /// </summary>
        public void BackToDeck()
        {
            // TODO: Move to GameManager
            // foreach (var hand in hands)
            // {
            //     if (hand.cards.Count > 0)
            //     {
            //         _cards.AddRange(hand.cards);
            //         hand.cards.Clear();
            //     }
            // }

            if (_cards.Count <= 0) return;
            ReIndex();
            foreach (var card in _cards)
            {
                card.transform.SetParent(transform);
                card.JumpRotateCard(transform, false);
            }
        }

        public void ShowTrump(int num)
        {
            if (_cards.Count <= 0) return;

            for (var i = 0; i < num; i++)
            {
                _cards[^ (i + 1)].JumpRotateCard(showTrumpTarget, false);
            }
        }

        public void DealCard(int step, Hand hand)
        {
            if (_cards.Count <= 0) return;

            for (var i = 0; i < step; i++)
            {
                // Top card 
                var card = _cards[^1];

                // TODO: Move to GameManager
                // foreach (var hand in hands)
                // {
                //     if (hand.direction == direction)
                //         hand.cards.Add(card);
                // }
                
                _cards.Remove(card);
                card.JumpRotateCard(hand.holder);
            }
        }

        public void MoveCards(Transform target)
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
            
            foreach (var card in _cards)
            {
                card.JumpRotateCard(target);
            }
        }

        /// <summary>
        /// Animation when first pick up cards (before going to player's hand) 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="hand"></param>
        public void PickUpCards(Transform target, Transform hand)
        {
            // if (_cards.Count <= 0)

            var a = new Vector3(-CardsPickUp.XLim, 0f, -CardsPickUp.ZLim);
            var b = Vector3.zero;
            var c = new Vector3(CardsPickUp.XLim, 0f, -CardsPickUp.ZLim);

            if (_cards.Count % 2 != 0)
            {
                var m = _cards.Count / 2;
                var finalSeq = DOTween.Sequence();

                for (var i = 0; i < _cards.Count; i++)
                {
                    _cards[i].transform.SetParent(target);

                    // Lerp to make curves
                    var t = (float)(i + 1) / (_cards.Count + 1);
                    var first = Vector3.Lerp(a, b, t);
                    var second = Vector3.Lerp(b, c, t);
                    var final = Vector3.Slerp(first, second, t);

                    var step = i - m;
                    var t1 = _cards[i].LocalMove(final + new Vector3(0f, i * CardsPickUp.YStep, 0f));
                    var t2 = _cards[i].Rotate(new Vector3(0f, step * CardsPickUp.YRotate, 0f));

                    var seq = _cards[i].JumpRotateSequence(target);
                    finalSeq
                        .Join(t1)
                        .Join(t2)
                        ;

                    if (i == _cards.Count - 1)
                        seq.OnComplete(() =>
                        {
                            finalSeq
                                .OnComplete(() => MoveToHands(hand))
                                .Play();
                        });

                    seq.Play();
                }
            }
            else
            {
                var m1 = _cards.Count / 2 - 1;
                var m2 = _cards.Count / 2;

                var finalSeq = DOTween.Sequence();

                for (var i = 0; i < _cards.Count; i++)
                {
                    _cards[i].transform.SetParent(target);

                    // Lerp to make curves
                    var t = (float)(i + 1) / (_cards.Count + 1);
                    var first = Vector3.Lerp(a, b, t);
                    var second = Vector3.Lerp(b, c, t);
                    var final = Vector3.Slerp(first, second, t);

                    var step = Mathf.Abs(i - m1) < Mathf.Abs(i - m2) ? (i - m1 - 0.5f) : (i - m2 + 0.5f);
                    var t1 = _cards[i].LocalMove(final + new Vector3(0f, i * CardsPickUp.YStep, 0f));
                    var t2 = _cards[i].Rotate(new Vector3(0f, step * CardsPickUp.YRotate, 0f));

                    var seq = _cards[i].JumpRotateSequence(target);
                    finalSeq
                        .Join(t1)
                        .Join(t2)
                        ;

                    if (i == _cards.Count - 1)
                        seq.OnComplete(() =>
                        {
                            finalSeq
                                .OnComplete(() => MoveToHands(hand))
                                .Play();
                        });

                    seq.Play();
                }
            }
        }

        /// <summary>
        /// Move the picked up cards to player's hand
        /// </summary>
        /// <param name="target"></param>
        private void MoveToHands(Transform target)
        {
            var finalSeq = DOTween.Sequence();

            if (_cards.Count % 2 != 0)
            {
                var m = _cards.Count / 2;

                for (var i = 0; i < _cards.Count; i++)
                {
                    _cards[i].transform.SetParent(target);

                    var step = i - m;
                    var pos = target.position + new Vector3(step * CardsToHand.XStep, step * CardsToHand.YStep, 0f);
                    var t1 = _cards[i].Move(pos);
                    var t2 = _cards[i].LocalRotate(Vector3.zero);

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

                for (var i = 0; i < _cards.Count; i++)
                {
                    _cards[i].transform.SetParent(target);

                    var step = Mathf.Abs(i - m1) < Mathf.Abs(i - m2) ? (i - m1 - 0.5f) : (i - m2 + 0.5f);
                    var pos = target.position + new Vector3(step * CardsToHand.XStep, step * CardsToHand.YStep, 0f);
                    var t1 = _cards[i].Move(pos);
                    var t2 = _cards[i].LocalRotate(Vector3.zero);

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
    }
}