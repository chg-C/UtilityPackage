using System.Collections;
using UnityEngine;
using CHG.Utilities.Patterns;

namespace CHG.Utilities.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class BGMManager : SingletonMonobehaviour<BGMManager>
    {   
        AudioSource audioSource;
        [SerializeField, Range(0, 1)]
        float volume = 1f;
        [SerializeField]
        float fadeDuration = 1f;
        private void Awake() {
            audioSource = GetComponent<AudioSource>();

            if(audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        
        /// <summary>
        /// 배경음악 클립을 플레이
        /// </summary>
        /// <param name="clip">재생할 배경음악 AudioClip</param>
        /// <param name="immediate">배경음악이 바뀔 때 Fade 효과를 적용할지 여부</param>
        /// <param name="loop">배경음악이 루프될지 여부</param>
        /// <returns></returns>
        public AudioClip PlayMusic(AudioClip clip, bool immediate = false, bool loop = true)
        {
            AudioClip rtn = audioSource.clip;

            audioSource.loop = loop;
            if(audioSource.isPlaying && !immediate)
            {
                StartCoroutine(PlayMusicCoroutine(clip));
            }
            else
            {
                audioSource.clip = clip;
                audioSource.Play();
            }

            return rtn;
        }

        private IEnumerator PlayMusicCoroutine(AudioClip clip)
        {
            if (audioSource.isPlaying)
            {
                yield return StartCoroutine(FadeOut());
            }

            audioSource.clip = clip;
            audioSource.Play();
            yield return StartCoroutine(FadeIn());
        }
        private IEnumerator FadeOut()
        {
            float startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume; // 원래 볼륨으로 복원
        }
        private IEnumerator FadeIn()
        {
            audioSource.volume = 0; // 볼륨을 0으로 설정
            float targetVolume = volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = targetVolume; // 최종 볼륨 설정
        }
        /// <summary>
        /// 현재 재생중인 배경음악 중단
        /// </summary>
        /// <param name="immediate">Fade 효과를 적용할지 여부</param>
        public void StopMusic(bool immediate = true)
        {
            if(!audioSource.isPlaying)
                return;

            if(immediate)
                audioSource.Stop();
            else
                StartCoroutine("FadeOut");
        }
        /// <summary>
        /// 배경음악 볼륨 설정
        /// </summary>
        /// <param name="newVolume">새로운 볼륨(0~1)</param>
        public void SetVolume(float newVolume)
        {
            volume = newVolume;
            audioSource.volume = volume;
        }
    }
}