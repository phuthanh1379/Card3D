using System.Collections.Generic;
using Card_3D;
using DG.Tweening;
using Unity.Android.Types;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    [Header("Deck Info")]
    [SerializeField] private int quantity;
    [SerializeField] private Card3D cardPrefab;

    [Header("Spawn Info -- Will move to CardDatabase or something")]
    [SerializeField] private List<CardInfo> cardInfo = new();
    [SerializeField] private Material backMaterial;
    [SerializeField] private Material sideMaterial;

    [Header("Animation")] 
    [SerializeField] private Transform showTrumpTarget;

    private List<Card3D> _cards = new();
    private List<Card3D> _dealt = new();

    private void Start()
    {
        SpawnCards();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            ShowTrump(3);
        
        if (Input.GetKeyDown(KeyCode.Q))
            BackToDeck();
    }

    private void SpawnCards()
    {
        // Spawn the cards
        var rnd = new System.Random();
        for (var i = 0; i < quantity; i++)
        {
            // Spawn random
            SpawnCard(cardInfo[rnd.Next(cardInfo.Count)], i * GameConstants.CardWidth, i);
        }
    }
    
    private void SpawnCard(CardInfo info, float yPos, int index)
    {
        var newPos = transform.position + new Vector3(0f, 4f, 0f);
        var card = Instantiate(cardPrefab, newPos, Quaternion.identity);
        card.transform.SetParent(transform);

        card.Index = index;
        // card.DeckPosition = new Vector3(0f, yPos, 0f);
        // card.ShowTrumpPosition = showTrumpTarget.position + new Vector3(0f, yPos, 0f);
        
        card.RenderInfo(info, backMaterial, sideMaterial, transform);
     
        _cards.Add(card);
    }

    private void ReIndex()
    {
        for (var i = 0; i < _cards.Count; i++)
        {
            _cards[i].Index = i;
        }
    }

    /// <summary>
    /// Make all cards go back to the deck
    /// </summary>
    private void BackToDeck()
    {
        if (_dealt.Count > 0)
        {
            _cards.AddRange(_dealt);
            _dealt.Clear();
        }

        if (_cards.Count <= 0) return;
        ReIndex();
        foreach (var card in _cards)
        {
            card.JumpRotateCard(transform, false);
        }
    }

    private void ShowTrump(int num)
    {
        if (_cards.Count <= 0) return;

        for (var i = 0; i < num; i++)
        {
            _cards[^ (i + 1)].JumpRotateCard(showTrumpTarget, false);
        }
    }
    
    public void DealCard(int step, Transform target)
    {
        if (_cards.Count <= 0) return;

        for (var i = 0; i < step; i++)
        {
            // Top card 
            var card = _cards[^1];
            _dealt.Add(card);
            _cards.Remove(card);
            card.JumpRotateCard(target);
        }
    }

    public void MoveCards(Transform target)
    {
        if (_cards.Count <= 0)
        {
            if (_dealt.Count <= 0) return;
            foreach (var card in _dealt)
            {
                card.JumpRotateCard(target);
            }
        }
        else
        {
            foreach (var card in _cards)
            {
                card.JumpRotateCard(target);
            }
        }
    }

    public void PickUpCards(Transform target)
    {
        if (_cards.Count <= 0) return;

        // x 0.1, y 0.001, yRotate = 8
        var xLim = 0.5f;
        var zLim = 0.3f;
        var a = new Vector3(-xLim, 0f, -zLim);
        var b = new Vector3(0f, 0f, 0f);
        var c= new Vector3(xLim, 0f, -zLim);
        var xStep = 0.1f;
        var yStep = 0.001f;
        var zStep = 0.01f;
        var yRotate = 10f;

        if (_cards.Count % 2 != 0)
        {
            var m = _cards.Count / 2;
            var finalSeq = DOTween.Sequence();

            for (var i = 0; i < _cards.Count; i++)
            {
                _cards[i].transform.SetParent(target);

                var t = (float) (i + 1) / (_cards.Count + 1);
                Debug.Log(t);
                var first = Vector3.Lerp(a, b, t);
                var second = Vector3.Lerp(b, c, t);
                var final = Vector3.Slerp(first, second, t);

                var step = i - m;
                var pos = target.position + new Vector3(step * xStep, step * yStep, step * zStep);
                // var t1 = _cards[i].Move(new Vector3(target.position.x, yStep * i, target.position.z) + final);
                var t1 = _cards[i].LocalMove(final + new Vector3(0f, i * yStep, 0f));
                var t2 = _cards[i].Rotate(new Vector3(0f, step * yRotate, 0f));
                
                var seq = _cards[i].JumpRotateSequence(target);
                finalSeq
                    .Join(t1)
                    .Join(t2)
                    ;

                if (i == _cards.Count - 1)
                    seq.OnComplete(() => finalSeq.Play());

                seq.Play();
            }
        }
        else
        {
            var m1 = _cards.Count / 2 - 1;
            var m2 = _cards.Count / 2;

            var finalSeq = DOTween.Sequence();
            
            for (var i = 0; i < _cards.Count; i++)
            {
                _cards[i].transform.SetParent(target);

                var step = Mathf.Abs(i - m1) < Mathf.Abs(i - m2) ? (i - m1 - 0.5f) : (i - m2 + 0.5f);
                var pos = target.position + new Vector3(step * xStep, step * yStep, step * zStep);
                var t1 = _cards[i].Move(pos);
                var t2 = _cards[i].Rotate(new Vector3(0f, step * yRotate, 0f));

                var seq = _cards[i].JumpRotateSequence(target);
                finalSeq
                    .Join(t1)
                    .Join(t2)
                    ;

                if (i == _cards.Count - 1)
                    seq.OnComplete(() => finalSeq.Play());

                seq.Play();
            }
        }
    }

    public void MoveToHands(Transform target)
    {
        
    }
}
