using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Deck;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardManager board;
    [SerializeField] private DeckController deck;

    [Header("Settings")] 
    [SerializeField] private int numberOfCards;

    private void Init()
    {
        // TODO: Init board
        
        // Init deck
        deck.Init(numberOfCards);
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        deck.SpawnCards();
    }

    private void Update()
    {
        // Board
        if (Input.GetKeyDown(KeyCode.Alpha1))
            board.MoveCards(Direction.South);
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            board.MoveCards(Direction.East);
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            board.MoveCards(Direction.North);
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
            board.MoveCards(Direction.West);
        
        if (Input.GetKeyDown(KeyCode.Space))
            board.DealSteps();
        
        if (Input.GetKeyDown(KeyCode.E))
            board.PickupCards();
        
        // Deck
        if (Input.GetKeyDown(KeyCode.A))
            deck.ShowTrump(3);

        if (Input.GetKeyDown(KeyCode.Q))
            deck.BackToDeck();
    }
}
