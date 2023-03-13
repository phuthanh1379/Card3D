using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer frontSprite;
    [SerializeField] private SpriteRenderer backSprite;
    [SerializeField] private SpriteRenderer shadowSprite;
    [SerializeField] private Collider itemCollider;

    public event Action<List<int>> ClickHandler;
    private ICardTransition _cardTransition;
    public int Index { get; private set; }

    private bool _isEnable;
    public bool IsEnable
    {
        get => _isEnable;
        set
        {
            _isEnable = value;
            itemCollider.enabled = value;
        }
    }

    private void Awake()
    {
        ClearShadow();
    }

    public void ClearShadow()
    {
        shadowSprite.color = Color.clear;
    }

    public int GetColliderId()
    {
        return itemCollider.GetInstanceID();
    }
    private int _id;
    public void Init(int id)
    {
        _id = id;
    }

    public void SetIndex(int index)
    {
        Index = index;
        shadowSprite.sortingOrder = index;
        frontSprite.sortingOrder = index + 1;
        backSprite.sortingOrder = index + 1;
    }

    public ICardTransition CreateSequence(Transform transformParent, int duration, int delay)
    {
        _cardTransition = new CardTransition(_id);

        return _cardTransition.CreateSequence(transform, transformParent, duration, delay);
    }

    public ICardTransition FadeOutShadow()
    {
        return FadeOutShadow(_cardTransition.Duration);
    }

    public ICardTransition FadeOutShadow(float duration)
    {
        return _cardTransition.Join(frontSprite.DOFade(0, duration)).Join(shadowSprite.DOFade(0, duration));
    }

    public ICardTransition FadeAll()
    {
        var duration = _cardTransition.Duration;
        return _cardTransition.Join(shadowSprite.DOFade(0, duration))
                .Join(frontSprite.DOFade(0, duration))
                .Join(backSprite.DOFade(0, duration));
    }

    public CardItem ShowTrump(Transform parentTransform)
    {
        CreateSequence(parentTransform, 500, 0).Translate(Vector3.zero).Scale(Vector3.one).Rotate(new(0, 180, 0));
        FadeInCardFront(0);
        return this;
    }

    public ICardTransition FadeInCardFront()
    {
        var duration = _cardTransition.Duration;
        return FadeInCardFront(duration);
    }

    public ICardTransition FadeInCardFront(float duration)
    {
        return _cardTransition.Join(frontSprite.DOFade(1, duration)).Join(shadowSprite.DOFade(0.4f, duration));
    }

    public ICardTransition FadeOutCardFront()
    {
        var duration = _cardTransition.Duration;
        return FadeOutCardFront(duration);
    }

    public ICardTransition FadeOutCardFront(float duration)
    {
        return _cardTransition.Join(frontSprite.DOFade(0, duration)).Join(shadowSprite.DOFade(0, duration));
    }

    public ICardTransition FadeInCardBack(float duration)
    {
        return _cardTransition.Join(backSprite.DOFade(1, duration));
    }

    public ICardTransition FadeOutCardBack()
    {
        return _cardTransition.Join(backSprite.DOFade(0, _cardTransition.Duration));
    }

    public ICardTransition FadeOutCardBack(float duration)
    {
        return _cardTransition.Join(backSprite.DOFade(0, duration));
    }

    public void Play()
    {
        _cardTransition.Play();
    }

    private void OnMouseDown()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var result = Physics.RaycastAll(cameraRay);
        var instanceIds = new List<int>();
        foreach (var item in result)
        {
            instanceIds.Add(item.colliderInstanceID);
        }

        ClickHandler?.Invoke(instanceIds);
    }
}