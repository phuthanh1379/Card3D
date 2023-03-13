using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class CardTransition : ICardTransition
{
    private const Ease ease = Ease.OutQuad;
    private Sequence _sequence;
    private float _duration;
    private float _delay;
    private Transform _transform;
    private Transform _transformParent;
    public float Duration => _duration;

    public int Id { get; private set; }
    private int _transitionId;
    private static int TransitionCounter;
    private event EventHandler _startHandler;
    private StartData _startData;

    public CardTransition(int id)
    {
        Id = id;
        _transitionId = ++TransitionCounter;
    }

    public ICardTransition CreateSequence(Transform transform, Transform transformParent, int duration, int delay)
    {
        _duration = ToMillisecond(duration);
        _delay = ToMillisecond(delay);
        _transform = transform;
        _transformParent = transformParent;

        var sequenceDuration = _sequence?.Duration() ?? -1;
        if (_duration + _delay <= sequenceDuration)
        {
            UnityEngine.Debug.LogError($"Kill");
            DOTween.Kill(transform, true);
        }

        _sequence = DOTween.Sequence(_transform);
        _sequence.AppendCallback(OnStart);
        return this;
    }

    private void OnStart()
    {
        _transform.SetParent(_transformParent);
        _startHandler?.Invoke(this, _startData);
    }

    public ICardTransition Translate(Vector3 newPosition)
    {
        _sequence.Join(_transform.DOLocalMove(newPosition, _duration).SetEase(ease));
        return this;
    }

    public ICardTransition Scale(Vector3 newScale)
    {
        _sequence.Join(_transform.DOScale(newScale, _duration).SetEase(ease));
        return this;
    }

    public ICardTransition Rotate(Vector3 newRotate)
    {
        _sequence.Join(_transform.DOLocalRotate(newRotate, _duration).SetEase(ease));
        return this;
    }

    public ICardTransition Rotate(Vector3 newRotate, float duration)
    {
        _sequence.Join(_transform.DOLocalRotate(newRotate, duration).SetEase(ease));
        return this;
    }

    public ICardTransition Join(Tween tween)
    {
        _sequence.Join(tween.SetEase(ease));
        return this;
    }

    public ICardTransition Append(Tween tween)
    {
        _sequence.Append(tween.SetEase(ease));
        return this;
    }

    public ICardTransition Insert(float atPosition, Tween tween)
    {
        _sequence.Insert(atPosition, tween.SetEase(ease));
        return this;
    }

    public ICardTransition InsertCallback(float atPosition, TweenCallback callback)
    {
        _sequence.InsertCallback(atPosition, callback);
        return this;
    }

    public void Play()
    {
        _sequence.PrependInterval(_delay);
        _sequence.Play();
    }

    public ICardTransition OnStart(EventHandler startHandler)
    {
        _startHandler = startHandler;
        return this;
    }

    public ICardTransition OnStart(EventHandler startHandler, StartData startData)
    {
        _startHandler = startHandler;
        _startData = startData;
        return this;
    }

    private static float ToMillisecond(int value) => value / 1_000f;
}

public class StartData : EventArgs
{
    public Hashtable HashTable { get; }
    public StartData()
    {
        HashTable = new();
    }
}
