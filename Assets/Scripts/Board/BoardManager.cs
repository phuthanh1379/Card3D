using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private DeckController deck;

    [Header("Hands")] 
    [SerializeField] private List<Transform> hands = new();
    [SerializeField] private Transform playerHand;
    [SerializeField] private Transform playerIntro;

    [Header("Deal Settings")] 
    [SerializeField] private List<int> steps = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            MoveCards(hands[0]);
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            MoveCards(hands[1]);
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            MoveCards(hands[2]);
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
            MoveCards(hands[3]);
        
        if (Input.GetKeyDown(KeyCode.Space))
            DealSteps();
        
        if (Input.GetKeyDown(KeyCode.D))
            MoveToHands(playerHand);
        
        if (Input.GetKeyDown(KeyCode.E))
            PickupCards(playerIntro);
    }

    private void DealSteps()
    {
        foreach (var step in steps)
        {
            foreach (var hand in hands)
            {
                DealCard(step, hand);
            }
        }
    }

    private void DealCard(int step, Transform target)
    {
        deck.DealCard(step, target);
    }

    private void MoveCards(Transform target)
    {
        deck.MoveCards(target);
    }

    private void PickupCards(Transform target)
    {
        deck.PickUpCards(target);
    }
    
    private void MoveToHands(Transform target)
    {
        deck.MoveToHands(target);
    }
}
