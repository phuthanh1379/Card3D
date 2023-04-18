using Common;
using UnityEngine;

namespace Board
{
    public class BoardSlot : MonoBehaviour
    {
        public int Count { get; private set; }

        public void Init()
        {
            Count = 0;
        }
        
        public void AddCard(Transform card, out float yPos)
        {
            card.SetParent(transform);
            Count++;
            yPos = Count * GameConstants.CardWidth;
        }

        public void RemoveCard()
        {
            Count--;
        }
    }
}
