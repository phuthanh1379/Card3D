using DG.Tweening;
using System;
using UnityEngine;

public interface ICardTransition
{
    float Duration { get; }
    ICardTransition CreateSequence(Transform transform, Transform transformParent, int duration, int delay);
    ICardTransition Translate(Vector3 newPosition);
    ICardTransition Scale(Vector3 newScale);
    ICardTransition Rotate(Vector3 newRotate);
    ICardTransition Rotate(Vector3 newRotate, float duration);
    ICardTransition Join(Tween tween);
    ICardTransition Append(Tween tween);
    ICardTransition Insert(float atPosition, Tween tween);
    ICardTransition InsertCallback(float atPosition, TweenCallback callback);
    ICardTransition OnStart(EventHandler startHandler);
    ICardTransition OnStart(EventHandler startHandler, StartData startData);
    void Play();
}