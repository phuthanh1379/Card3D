using System;
using System.Collections.Generic;
using UnityEngine;

namespace Card_3D
{
    public class CardSpawn : MonoBehaviour
    {
        [SerializeField] private int quantity;
        [SerializeField] private Vector3 startPosition;
        
        [SerializeField] private List<CardInfo> cardInfo = new();
        [SerializeField] private Card3D baseCard;
        [SerializeField] private Material backMaterial;
        [SerializeField] private Material sideMaterial;
        [SerializeField] private Transform cardParent;
     
        private List<Card3D> _cards = new();
        private List<float> cardYPos = new();
        private bool _isContinuousDynamic = false;
        
        private void Start()
        {
            // Init();
        }
    
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                SpawnRandomCard();
        }
    
        private void Init()
        {
            for (var i = 0; i < quantity; i++)
            {
                SpawnRandomCard();
            }
            
            //// Spawn all cards from settings
            // foreach (var info in cardInfo)
            // {
            //     SpawnCard(info);
            // }
        }
    
        private void SpawnCard(CardInfo info)
        {
            var card = Instantiate(baseCard, startPosition, Quaternion.identity);
            card.transform.SetParent(cardParent);
            card.RenderInfo(info, backMaterial, sideMaterial);
    
            card.GetComponent<Rigidbody>().collisionDetectionMode = 
                !_isContinuousDynamic ? CollisionDetectionMode.Continuous : CollisionDetectionMode.ContinuousDynamic;
            
            _cards.Add(card);
            _isContinuousDynamic = !_isContinuousDynamic;
        }
    
        private void SpawnRandomCard()
        {
            var rnd = new System.Random();
            SpawnCard(cardInfo[rnd.Next(cardInfo.Count)]);
        }
    }
}