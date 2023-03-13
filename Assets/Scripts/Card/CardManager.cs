using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [SerializeField] private Button dealCard;
    [SerializeField] private Button dealRestCard;
    [SerializeField] private Button moveToTable;
    [SerializeField] private Button showTrumpCard;

    [SerializeField] private CardItem cardItem;
    [SerializeField] private Transform tableParent;
    [SerializeField] private Transform showTrumpParent;
    [SerializeField] private List<Transform> handParents;
    [SerializeField] private Transform tempSouthHandParents;
    [SerializeField] private List<Transform> showParents;
    [SerializeField] private List<Transform> winParents;

    private readonly List<CardItem> _listCard = new();

    private readonly Dictionary<int, List<CardItem>> _playerCard = new();
    private readonly Dictionary<int, List<CardItem>> _playerWinCard = new();
    private readonly Dictionary<int, CardItem> _cardItemCollider = new();
    private readonly Vector3 _flipVector = new(0, 180, 0);

    private int _cardCounter;

    private void Awake()
    {
        Application.targetFrameRate = 120;
        Instance = this;
    }

    private void Start()
    {
        for (var i = 0; i < 4; i++)
        {
            _playerCard[i] = new();
            _playerWinCard[i] = new();
        }
        dealCard.onClick.AddListener(OnClickDealCard);
        dealRestCard.onClick.AddListener(OnClickDealRestCard);
        moveToTable.onClick.AddListener(OnClickMoveToTable);
        showTrumpCard.onClick.AddListener(OnShowTrumpCard);

        for (var i = 0; i < 4 * 8; i++)
        {
            var item = Instantiate(cardItem, tableParent, false);
            item.name = $"Card {i}";
            item.transform.localPosition = new(0, 0, -0.015f * i);
            item.transform.localRotation = Quaternion.Euler(0, 0, 0);
            item.ClickHandler += OnClickCard;
            item.Init(i);
            item.SetIndex(TableSpriteIndex + i);
            _cardItemCollider[item.GetColliderId()] = item;
            _listCard.Add(item);
        }
        _cardCounter = _listCard.Count - 1;
    }

    private void OnDestroy()
    {
        dealCard.onClick.RemoveListener(OnClickDealCard);
        dealRestCard.onClick.RemoveListener(OnClickDealRestCard);
        moveToTable.onClick.RemoveListener(OnClickMoveToTable);
        showTrumpCard.onClick.RemoveListener(OnShowTrumpCard);
    }

    private void OnClickCard(List<int> instanceIds)
    {
        var cardItem = GetLastestItem(instanceIds);
        var playerId = PlayerId.South;
        PlayCard(cardItem, playerId);
        SortSouthGroup();
        var list = new List<CardItem>
        {
            cardItem
        };
        StartCoroutine(FinishTrick(list));
    }

    private IEnumerator FinishTrick(List<CardItem> list)
    {
        for (var i = 1; i < 4; i++)
        {
            yield return new WaitForSeconds(0.5f);
            var item = _playerCard[i][0];
            list.Add(item);
            PlayCard(item, i);
        }

        yield return new WaitForSeconds(1f);
        var ran = new System.Random();
        var value = ran.Next(0, 4);
        for (var i = 0; i < list.Count; i++)
        {
            var startData = new StartData();
            var item = list[i];
            startData.HashTable["card"] = item;
            startData.HashTable["index"] = (WinSpriteIndex + i) * 10;
            item.CreateSequence(winParents[value], 500, 0).Translate(Vector3.zero).Scale(Vector3.one).Rotate(_flipVector).OnStart(OnStartGroupWinTrick, startData);
            item.FadeAll();
            item.Play();
        }
    }

    private void OnStartGroupWinTrick(object sender, EventArgs e)
    {
        if (e is not StartData args || args.HashTable is not Hashtable data)
        {
            return;
        }

        if (data["card"] is not CardItem card || data["index"] is not int index)
        {
            return;
        }

        card.SetIndex(index);
    }

    private void PlayCard(CardItem cardItem, int playerId)
    {
        cardItem.IsEnable = false;
        _playerCard[playerId].Remove(cardItem);
        cardItem.CreateSequence(showParents[playerId], 500, 0)
                    .Translate(Vector3.zero)
                    .Scale(Vector3.one)
                    .Rotate(_flipVector, 0);
        cardItem.FadeInCardFront();
        cardItem.Play();
    }

    private CardItem GetLastestItem(List<int> instanceIds)
    {
        var cards = new List<CardItem>();
        foreach (var instanceId in instanceIds)
        {
            cards.Add(_cardItemCollider[instanceId]);
        }

        cards.Sort((a, b) => b.Index - a.Index);

        return cards.Count > 0 ? cards[0] : default;
    }

    private void OnShowTrumpCard()
    {
        if (_cardCounter < 0 || _cardCounter >= _listCard.Count)
        {
            return;
        }

        var item = _listCard[_cardCounter];
        item.ShowTrump(showTrumpParent);
        item.Play();
    }

    private void OnClickDealCard()
    {
        DealCard(3, new() { 3, 2 });
    }

    private void OnClickDealRestCard()
    {
        DealCard(2, new() { 3 });
    }

    private void OnClickMoveToTable()
    {
        _cardCounter = _listCard.Count - 1;

        foreach (var item in _playerCard)
        {
            item.Value.Clear();
        }

        foreach (var item in _playerWinCard)
        {
            item.Value.Clear();
        }

        for (var i = 0; i < _listCard.Count; i++)
        {
            var item = _listCard[i];
            item.SetIndex(TableSpriteIndex + i);
            item.CreateSequence(tableParent, 500, 0)
            .Translate(new(0, 0, -0.015f * i))
            .Scale(Vector3.one)
            .Rotate(Vector3.zero, 0);
            item.FadeOutShadow(0f);
            item.FadeInCardBack(0f);
            item.Play();
        }
    }

    private const int TableSpriteIndex = 0;
    private const int HandSpriteIndex = 200;
    private const int WinSpriteIndex = 100;

    private void OnStartGroupCard(object sender, EventArgs e)
    {
        if (e is not StartData args || args.HashTable is not Hashtable data)
        {
            return;
        }

        if (data["card"] is not CardItem card || data["index"] is not int index)
        {
            return;
        }

        card.SetIndex(index);
    }

    private void DealCard(int startIndex, List<int> steps)
    {
        var delay = 0;
        var southCard = _playerCard[PlayerId.South];
        for (var i = 0; i < steps.Count; i++)
        {
            var stepCounter = 0;
            for (var j = 0; j < 4;)
            {
                if (_cardCounter < 0 || _cardCounter >= _listCard.Count)
                {
                    return;
                }

                var item = _listCard[_cardCounter];
                _cardCounter--;

                if (startIndex == PlayerId.South)
                {
                    var itemSequence = item.CreateSequence(tempSouthHandParents, 500, delay);
                    itemSequence.Translate(new(0, 0f, 0f)).Scale(Vector3.one);
                    item.Play();
                    item.IsEnable = true;

                    const float spacePerCard = 1f;
                    southCard.Add(item);
                    var startPosition = -(southCard.Count * spacePerCard / 2);
                    for (var k = 0; k < southCard.Count; k++)
                    {
                        var startData = new StartData();
                        startData.HashTable["card"] = southCard[k];
                        startData.HashTable["index"] = (HandSpriteIndex + k) * 10;

                        southCard[k].CreateSequence(handParents[startIndex], 500, delay + 1000)
                        .Translate(new(startPosition, 0f, 0f))
                        .Scale(Vector3.one)
                        .Rotate(_flipVector, 0)
                        .OnStart(OnStartGroupCard, startData);

                        southCard[k].FadeInCardFront(0);
                        southCard[k].FadeOutCardBack(0);
                        southCard[k].Play();
                        startPosition += spacePerCard;
                    }
                }
                else
                {
                    var handParent = handParents[startIndex];
                    var itemSequence = item.CreateSequence(handParent, 500, delay);
                    itemSequence.Translate(Vector3.zero).Scale(Vector3.one);
                    item.FadeAll();
                    item.Play();
                    if (startIndex == PlayerId.West)
                    {
                        _playerCard[PlayerId.West].Add(item);
                    }
                    else if (startIndex == PlayerId.North)
                    {
                        _playerCard[PlayerId.North].Add(item);
                    }
                    else if (startIndex == PlayerId.East)
                    {
                        _playerCard[PlayerId.East].Add(item);
                    }
                }
                delay += 100;
                stepCounter++;
                if (stepCounter == steps[i])
                {
                    stepCounter = 0;
                    startIndex = (startIndex + 1) % handParents.Count;
                    j++;
                }
            }
        }
    }

    private void SortSouthGroup()
    {
        const float spacePerCard = 1f;
        var handParent = handParents[0];
        var southCard = _playerCard[PlayerId.South];
        var startPosition = -(southCard.Count * spacePerCard / 2);
        for (var i = 0; i < southCard.Count; i++)
        {
            southCard[i].CreateSequence(handParent, 500, 0).Translate(new(startPosition, 0f, 0f)).Scale(Vector3.one).Rotate(_flipVector).Play();
            startPosition += spacePerCard;
        }
    }
}

public struct PlayerId
{
    public const int South = 0;
    public const int West = 1;
    public const int North = 2;
    public const int East = 3;
}