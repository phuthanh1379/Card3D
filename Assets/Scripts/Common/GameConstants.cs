using System;
using System.Collections.Generic;
using Board;
using Card_3D;
using DG.Tweening;
using UnityEngine;

namespace Common
{
    public static class GameConstants
    {
        public const float CardAnimDuration = 0.1f;
        public const float CardWidth = 0.001f;
    }

    public static class CardsDeal
    {
        public const float NoiseValue = 0.05f;
        public const float Duration = 0.2f;
        public const Ease EaseType = Ease.InSine;
    }

    /// <summary>
    /// Figures to use for PickUpCards animation
    /// </summary>
    public static class CardsPickUp
    {
        // Setting for 24 cards
        public const float XLim = 2f;
        public const float ZLim = 1.5f;
        public const float YRotate = 5f;
        
        // Setting for 13 cards
        // public const float XLim = 1.8f;
        // public const float ZLim = 1.3f;
        // public const float YRotate = 8f;
        
        // Setting for 5 cards
        // public const float XLim = 0.5f;
        // public const float ZLim = 0.3f;
        // public const float YRotate = 10f;

        public const float YStep = 0.001f;
        // public const float XStep = 0.1f;
        // public const float ZStep = 0.01f;
    }

    public static class CardsToHand
    {
        // 0.625f for a close-tight arrangement of cards
        // public const float XStep = 0.35f; // 5 -> 13 cards
        
        // Setting for 24 cards
        public const float XLim = 2f;
        public const float ZLim = 0.5f;
        public const float YRotate = 2f;
        
        public const float XStep = 0.25f; // 24 cards
        public const float YStep = 0.001f;
    }
    
    public enum Direction
    {
        South = 0,
        East = 1,
        North = 2,
        West = 3
    }

    [Serializable]
    public class Hand
    {
        public Direction direction;
        public BoardSlot slot;
        public List<Card3D> cards = new();

        public void AddCard(Card3D card, out float yPos)
        {
            slot.AddCard(card.transform, out var y);
            yPos = y;
            cards.Add(card);
        }
    }
}