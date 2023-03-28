using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TestTween : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> clips = new();
    private List<float> _durations = new();
    
    private const string MyString = "Some Tweeners have specific special options that will be available to you depending on the type of thing you're tweening. It's all automatic: if a Tweener has specific options you'll see a specific SetOptions methods present for that Tweener, otherwise you won't. It's magic!";
    private List<string> _strings = new();
    private string _string;

    private Sequence sequence;

    private void Awake()
    {
        for (var i = 0; i < 20; i++)
        {
            _durations.Add((float) i/100);
        }
        
        sequence = DOTween.Sequence();
        var rnd = new System.Random();
        
        for (var i = 0; i < MyString.Length; i++)
        {
            var temp = MyString[i];
            _string = string.Concat(_string, MyString[i]);
            _strings.Add(_string);

            var tween = DOTween
                .To(
                    () => text.text,
                    x =>
                    {
                        text.text = x;
                    },
                    _strings[i],
                    _durations[rnd.Next(_durations.Count)]
                )
                .OnPlay(() =>
                {
                    if (!char.IsWhiteSpace(temp))
                        PlayRandomClip();
                });
            
            sequence
                .Append(
                    tween
                        // .OnPlay(() => Debug.Log(tween.Duration()))
                );
        }

        sequence.Play();
    }

    private void PlayRandomClip()
    {
        var rnd = new System.Random();
        audioSource.PlayOneShot(clips[rnd.Next(clips.Count)]);
    }
}
