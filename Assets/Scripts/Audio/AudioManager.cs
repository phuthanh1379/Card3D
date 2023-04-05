using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioSfx> sfx = new();
        public static AudioManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void PlaySfxOneShot(string sfxName)
        {
            foreach (var s in sfx)
            {
                if (!s.name.Equals(sfxName)) continue;
                audioSource.PlayOneShot(s.clip);
                break;
            }
        }
    }

    [Serializable]
    public class AudioSfx
    {
        public string name;
        public AudioClip clip;
    }
}