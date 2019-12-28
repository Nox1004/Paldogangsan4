using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오디오 소스를 호출해주는 매니저 스크립트 
/// </summary>
public class SoundManager : Singleton<SoundManager> {

    public void PlayClip(AudioClip clip, bool isLoop = false, float volume = 1.0f)
    {
        GameObject go = new GameObject("Audio");
        AudioSource audioSource = go.AddComponent<AudioSource>();

        go.transform.SetParent(this.gameObject.transform);
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        audioSource.volume = volume;
        audioSource.spatialBlend = 0.0f;

        StartCoroutine(RemoveSourceWhenDone(audioSource));
    }
    
    // <summary>
    // 오디오소스가 종료되면 AudioSource 컴포넌트를 가지고 있는 오브젝트를 제거해준다.
    // </summary>
    private IEnumerator RemoveSourceWhenDone(AudioSource audioSource)
    {
        audioSource.Play();

        while (audioSource.isPlaying)
        {
            yield return null;
        }

        Destroy(audioSource.gameObject);
    }
}
