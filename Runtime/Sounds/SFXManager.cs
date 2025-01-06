using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHG.Utilities.Patterns;

namespace CHG.Utilities.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXManager : SingletonMonobehaviour<SFXManager>
    {
        AudioSource audioSource;

        [SerializeField, Range(0, 1)]
        float volume = 1f;

        //[SerializeField]

        private void Awake() {
            audioSource = GetComponent<AudioSource>();
            
            if(audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.volume = volume;
        }

        public void Play(AudioClip clip, float volume = 1)
        {
            audioSource.PlayOneShot(clip, volume);
        }        
        public void SetVolume(float newVolume)
        {
            volume = newVolume;
            audioSource.volume = volume;
        }
    }
}
