using System;
using System.Collections.Generic;
using Card_3D;
using UnityEngine;

namespace Common
{
    public static class GameConstants
    {
        public const float CardAnimDuration = 0.1f;
        public const float CardWidth = 0.001f;
    }

    /// <summary>
    /// Figures to use for PickUpCards animation
    /// </summary>
    public static class CardsPickUp
    {
        public const float XLim = 0.5f;
        public const float ZLim = 0.3f;
        public const float XStep = 0.1f;
        public const float YStep = 0.001f;
        public const float ZStep = 0.01f;
        public const float YRotate = 10f;
    }

    public static class CardsToHand
    {
        // 0.625f for a close-tight arrangement of cards
        public const float XStep = 0.35f;
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
        public Transform holder;
        public List<Card3D> cards = new();
    }
}