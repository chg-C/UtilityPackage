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
        /// <summary>
        /// 효과음 재생하기 
        /// </summary>
        /// <param name="clip">재생할 효과음 AudioClip</param>
        /// <param name="volume">효과음의 볼륨</param>
        public void Play(AudioClip clip, float volume = 1)
        {
            audioSource.PlayOneShot(clip, volume);
        }
        /// <summary>
        /// 효과음 볼륨 설정
        /// </summary>
        /// <param name="newVolume">새로운 볼륨(0~1)</param>
        public void SetVolume(float newVolume)
        {
            volume = newVolume;
            audioSource.volume = volume;
        }
    }
}
