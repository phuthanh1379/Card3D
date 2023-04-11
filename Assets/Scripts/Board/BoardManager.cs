using System;
using System.Collections.Generic;
using Common;
using UnityEngine;
using Deck;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private DeckController deck;

    [Header("Hands")] 
    [SerializeField] private List<Hand> hands = new();
    [SerializeField] private Transform playerHand;
    [SerializeField] private Transform playerIntro;

    [Header("Deal Settings")] 
    [SerializeField] private List<int> steps = new();
    
    public void DealSteps()
    {
        // S - E - N - W
        for (var i = 0; i < steps.Count; i++)
        {
            var direction = (Direction) i;
            
            foreach (var hand in hands)
            {
                DealCard(steps[i], hand);
                // deck.DealCard(step, hand, intro: playerIntro, hand: playerHand);
            }
        }
    }

    private void DealCard(int step, Hand hand)
    {
        deck.DealCard(step, hand);
    }

    public void MoveCards(Direction direction)
    {
        foreach (var hand in hands)
            if (hand.direction == direction)
                deck.MoveCards(hand.holder);
    }

    public void PickupCards()
    {
        deck.PickUpCards(playerIntro, playerHand);
    }
}
