using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    #region fields
    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource battleMusic;
    private AudioSource sfxSource;
    private AudioSource uiSFXSource;
    [SerializeField] private AudioMixer mixer;

    public Coroutine activeSFXStitching;
    public Coroutine activeBackgroundMusic;
    private bool firstMusicSourceIsPlaying = true;
    
    public AudioSource MusicSource { get => musicSource; set => musicSource = value; }
    public AudioSource MusicSource2 { get => musicSource2; set => musicSource2 = value; }
    public AudioSource BattleMusic { get => battleMusic; set => battleMusic = value; }
    public AudioSource SFXSource { get => sfxSource; set => sfxSource = value; }
    public AudioSource UiSFXSource { get => uiSFXSource; set => uiSFXSource = value; }
    #endregion

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        MusicSource = gameObject.AddComponent<AudioSource>();
        MusicSource2 = gameObject.AddComponent<AudioSource>();
        BattleMusic = gameObject.AddComponent<AudioSource>();
        SFXSource = gameObject.AddComponent<AudioSource>();
        UiSFXSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[1];
        musicSource2.outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[1];
        BattleMusic.outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[1];
        SFXSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[2];
        UiSFXSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[3];
        
        MusicSource.loop = true;
        MusicSource2.loop = true;
        BattleMusic.loop = true;
    }

    public void PlayMusic(AudioClip clip) {

        AudioSource activeSource = (firstMusicSourceIsPlaying) ? MusicSource : MusicSource2;

        activeSource.clip = clip;
        activeSource.volume = 1;
        activeSource.Play();
    }
    public IEnumerator PlayMusicWithMultipleParts(List<AudioClipExtended> audioClips) {

        AudioSource activeSource = (firstMusicSourceIsPlaying) ? MusicSource : MusicSource2;
        if (activeBackgroundMusic != null)
        {
            StopCoroutine(activeBackgroundMusic);
            activeSource.Stop();
        }
        for (int i = 0; i < audioClips.Count; i++)
        {

            activeSource.clip = audioClips[i].audio;
            activeSource.volume = 1;
            activeSource.Play();
            if (!audioClips[i].looping && !audioClips[i].stopAfterPlay)
            {
                activeSource.loop = false;
                yield return new WaitForSecondsRealtime(audioClips[i].audio.length);
            }
            else if (audioClips[i].looping)
            {
                activeSource.loop = true;
                break;
            }
            else if (audioClips[i].stopAfterPlay) {

                activeSource.loop = false;
                yield return new WaitForSecondsRealtime(audioClips[i].audio.length);
                activeSource.Stop();
                break;
            }
        }

    }
    public IEnumerator PlayMusicWithMultipleParts(List<AudioClipExtended> audioClips, float volume)
    {

        AudioSource activeSource = (firstMusicSourceIsPlaying) ? MusicSource : MusicSource2;
        if (activeBackgroundMusic != null)
        {
            StopCoroutine(activeBackgroundMusic);
            activeSource.Stop();
        }
        for (int i = 0; i < audioClips.Count; i++)
        {

            activeSource.clip = audioClips[i].audio;
            activeSource.volume = volume;
            activeSource.Play();
            if (!audioClips[i].looping && !audioClips[i].stopAfterPlay)
            {
                activeSource.loop = false;
                yield return new WaitForSecondsRealtime(audioClips[i].audio.length);
            }
            else if (audioClips[i].looping)
            {
                activeSource.loop = true;
                break;
            }
            else if (audioClips[i].stopAfterPlay)
            {

                activeSource.loop = false;
                yield return new WaitForSecondsRealtime(audioClips[i].audio.length);
                activeSource.Stop();
                break;
            }
        }

    }
    public IEnumerator PlayAudioWithMultipleParts(List<AudioClipExtended> audioClips) {


        AudioSource activeSource = UiSFXSource;
        if (activeSFXStitching != null) {
            StopCoroutine(activeSFXStitching);
            activeSource.Stop();
        }

        for (int i = 0; i < audioClips.Count; i++)
        {
            activeSource.clip = audioClips[i].audio;
            activeSource.volume = 1;
            activeSource.Play();
            if (!audioClips[i].looping && !audioClips[i].stopAfterPlay)
            {
                activeSource.loop = false;
                yield return new WaitForSecondsRealtime(audioClips[i].audio.length);
            }
            else if (audioClips[i].looping)
            {
                activeSource.loop = true;
                break;
            }
            else if (audioClips[i].stopAfterPlay)
            {

                activeSource.loop = false;
                yield return new WaitForSecondsRealtime(audioClips[i].audio.length);
                activeSource.Stop();
                break;
            }
        }
        activeSFXStitching = null;
    }
    public void PlayMusicWithFade(AudioClip newClip, float transitionTime, float newVolume)
    {

        AudioSource activeSource = (firstMusicSourceIsPlaying) ? MusicSource : MusicSource2;
        activeSource.volume = newVolume;

        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }

    public void PlayMusicWithCrossFade(AudioClip newClip, float transitionTime, float newVolume)
    {

        AudioSource activeSource = (firstMusicSourceIsPlaying) ? MusicSource : MusicSource2;
        AudioSource newSource = (firstMusicSourceIsPlaying) ? MusicSource2 : MusicSource;

        activeSource.volume = newVolume;

        firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

        newSource.clip = newClip;

        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));
    }

    public void PlayBattleMusicWithCrossFade(AudioClip newClip, float transitionTime, float newVolume) {

        AudioSource activeSource = (firstMusicSourceIsPlaying) ? MusicSource : MusicSource2;

        BattleMusic.clip = newClip;
        BattleMusic.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, BattleMusic, transitionTime, newVolume));
    }
    public void StopBattleMusic() {

        BattleMusic.Stop();
    }
    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        float volumeMax = activeSource.volume;
        if (!activeSource.isPlaying)
            activeSource.Play();

        float t = 0.0f;

        for (t = 0; t < transitionTime; t += Time.deltaTime) {
            activeSource.volume = (volumeMax - (t / transitionTime));
            yield return null;
        }

        activeSource.Pause();

        activeSource.clip = newClip;

        activeSource.Play();

        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
           Mathf.Clamp(activeSource.volume, (t / transitionTime),volumeMax);
            yield return null;
        }
    }
    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime)
    {
        newSource.Play();
        float volumeMax = original.volume;

        float t = 0.0f;

        for (t = 0.0f; t <= transitionTime; t += Time.deltaTime) {
            original.volume = (volumeMax - (t / transitionTime));
            newSource.volume = 0 + (t / transitionTime);
            if (newSource.volume > volumeMax)
                newSource.volume = volumeMax;

            yield return null;
        }

        original.Pause();
    }
    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime, float maxVolume)
    {

        float t = 0.0f;

        for (t = 0.0f; t <= transitionTime; t += Time.deltaTime)
        {
            original.volume = (maxVolume - (t / transitionTime));
            newSource.volume = 0 + (t / transitionTime);
            if (newSource.volume > maxVolume)
                newSource.volume = maxVolume;

            yield return null;
        }

        original.Pause();
    }
    public void PlaySFX(AudioClip clip) {

        SFXSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        SFXSource.PlayOneShot(clip, volume);
    }
    public void PlaySFX(AudioClip clip, float volume, float delay)
    {
        StartCoroutine(PlaySFXDelay(clip, volume, delay));    
    }
    public IEnumerator PlaySFXDelay(AudioClip clip, float volume, float delay) {

        float timer = 0;
        while (timer < delay) {
            timer += Time.deltaTime;
            yield return null;
        }
        sfxSource.PlayOneShot(clip, volume);
    }
    public void PlaySFX(AudioClip clip, float volume, bool oneShot)
    {
        if (oneShot == false)
        {
            SFXSource.volume = volume;
            SFXSource.clip = clip;

            SFXSource.Play();
        }
        else PlaySFX(clip, volume);
    }
    public void PlayUISFX(AudioClip clip)
    {
        UiSFXSource.PlayOneShot(clip);
    }

    public void PlayUISFX(AudioClip clip, float volume)
    {
        UiSFXSource.PlayOneShot(clip, volume);
    }
    public void PlayUISFX(AudioClip clip, float volume, float delay)
    {
        StartCoroutine(PlayUISFXDelay(clip, volume, delay));
    }
    public IEnumerator PlayUISFXDelay(AudioClip clip, float volume, float delay)
    {

        float timer = 0;
        while (timer < delay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        UiSFXSource.PlayOneShot(clip, volume);
    }
    public void PlayUISFX(AudioClip clip, float volume, bool oneShot)
    {
        if (oneShot == false)
        {
            UiSFXSource.Stop();
            UiSFXSource.volume = volume;
            UiSFXSource.clip = clip;

            UiSFXSource.Play();
        }
        else PlaySFX(clip, volume);
    }
    public void StopMusic() {

        MusicSource.Pause();
        MusicSource2.Pause();
    }
}
