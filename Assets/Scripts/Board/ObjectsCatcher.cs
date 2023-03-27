using System;
using System.Collections.Generic;
using UnityEngine;
using Card_3D;

namespace Board
{
    public class ObjectsCatcher : MonoBehaviour
    {
        private List<Card3D> cards = new();

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Card"))
                CleanCard(collision.gameObject);
        }

        private void CleanCard(GameObject card)
        {
            cards.Add(card.GetComponent<Card3D>());
            card.SetActive(false);
            card.transform.SetParent(transform);
        }
    }
}