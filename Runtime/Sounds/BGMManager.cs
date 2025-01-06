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
        public void StopMusic(bool immediate = true)
        {
            if(!audioSource.isPlaying)
                return;

            if(immediate)
                audioSource.Stop();
            else
                StartCoroutine("FadeOut");
        }

        public void SetVolume(float newVolume)
        {
            volume = newVolume;
            audioSource.volume = volume;
        }
    }
}