using Common;
using UnityEngine;

namespace Board
{
    public class BoardSlot : MonoBehaviour
    {
        public int Count { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }

        public void Init()
        {
            Count = 0;
            Position = transform.position;
            Rotation = transform.rotation.eulerAngles;
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
