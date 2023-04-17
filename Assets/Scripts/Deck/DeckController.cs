using System;
using System.Collections.Generic;
using Card_3D;
using Common;
using DG.Tweening;
using Common;
using UnityEngine;

namespace Deck
{
    /// <summary>
    /// The holder of all cards
    /// </summary>
    public class DeckController : MonoBehaviour
    {
        public Transform startPosition;
        public Vector3 Pos => transform.position;

        public void Init()
        {
            transform.position = startPosition.position;
            transform.rotation = startPosition.rotation;
        }
    }
}