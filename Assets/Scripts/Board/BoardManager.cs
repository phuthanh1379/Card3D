using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private DeckController deck;

    [Header("Hands")] 
    [SerializeField] private List<Transform> hands = new();

    [Header("Deal Settings")] 
    [SerializeField] private List<int> steps = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            DealCard(hands[0]);
    }

    private void DealCard(Transform target)
    {
        deck.DealCard(target);
    }
}
