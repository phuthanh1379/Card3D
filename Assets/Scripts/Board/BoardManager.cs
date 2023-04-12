using System;
using System.Collections.Generic;
using Common;
using UnityEngine;
using Deck;

public class BoardManager : MonoBehaviour
{
    [Header("Hands")] 
    [SerializeField] private List<Hand> hands = new();

    public List<Hand> Hands => hands;

    public Hand GetHand(Direction direction)
    {
        foreach (var hand in hands)
        {
            if (hand.direction == direction)
                return hand;
        }

        return null;
    }
}
