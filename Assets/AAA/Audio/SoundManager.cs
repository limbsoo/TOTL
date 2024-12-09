using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundCatecory
{
    Master,
    BGM,
    Effect,
}

public enum SoundType
{
    Hit,
    GetItem,
    ArriveGoal,
    GameOver,
    StageStart,
    Main
}




public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //if (instance == null) { instance = this; }
        //else Destroy(gameObject);
    }

    [SerializeField] AudioMixer _audioMixer;
    Dictionary<string, AudioClip> _audioClipDictionary;
    [SerializeField]  AudioClip[] _preLoadAudioClip;
    List<SoundClip> _loopSounds;

    Action SoundManagerStarted;

    void Start()
    {
        _audioClipDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in _preLoadAudioClip) { _audioClipDictionary.Add(clip.name, clip); }
        _loopSounds = new List<SoundClip>();

        if (SoundManagerStarted != null) { SoundManagerStarted.Invoke(); }
    }

    AudioClip GetClip(string clipName)
    {
        AudioClip clip = _audioClipDictionary[clipName];

        if (clip == null) { Debug.LogError(clipName + "�� �������� �ʽ��ϴ�."); }

        return clip;
    }


    public void StopAllSound()
    {
        if (_loopSounds == null) return;

        //foreach (SoundClip audioPlayer in _loopSounds)
        //{
        //    //_loopSounds.Remove(audioPlayer);
        //    Destroy(audioPlayer.gameObject);
        //    //return;
        //}

        _loopSounds.Clear();
    }


    public void StopLoopSound(string clipName)
    {
        foreach (SoundClip audioPlayer in _loopSounds)
        {
            if (audioPlayer.ClipName == clipName)
            {
                _loopSounds.Remove(audioPlayer);
                Destroy(audioPlayer.gameObject);
                return;
            }
        }

        Debug.LogWarning(clipName + "�� ã�� �� �����ϴ�.");
    }

    public void Play(string clipName, SoundCatecory soundCatecory, bool isLoop, float delay = 0f)
    {
        if(_loopSounds == null)
        {
            SoundManagerStarted = () =>
            {
                GameObject go = new GameObject(clipName);
                SoundClip sound = go.AddComponent<SoundClip>();

                if (isLoop) { _loopSounds.Add(sound); }

                sound.InitSound(GetClip(clipName));
                sound.Play(_audioMixer.FindMatchingGroups(soundCatecory.ToString())[0], delay, isLoop);
            };
        }

        else
        {
            GameObject go = new GameObject(clipName);
            SoundClip sound = go.AddComponent<SoundClip>();

            if (isLoop) { _loopSounds.Add(sound); }

            sound.InitSound(GetClip(clipName));
            sound.Play(_audioMixer.FindMatchingGroups(soundCatecory.ToString())[0], delay, isLoop);
        }


    }



    //���� �ε�� �� �ɼ� �Ŵ��������� ��� ���� �ҷ��� ����� �ɼ��� ũ��� �ʱ�ȭ��Ű�� �Լ�.
    public void InitVolumes(float bgm, float effect)
    {
        SetVolume(SoundCatecory.BGM, bgm);
        SetVolume(SoundCatecory.Effect, effect);
    }

    //�ɼ��� ������ �� �Ҹ��� �ҷ��� �����ϴ� �Լ�
    public void SetVolume(SoundCatecory soundCatecory, float value)
    {
        _audioMixer.SetFloat(soundCatecory.ToString(), value);
    }

}


[RequireComponent(typeof(AudioSource))]
public class SoundClip : MonoBehaviour
{
    private AudioSource _AudioSource;
    public string ClipName
    {
        get { return _AudioSource.clip.name; }
    }

    public void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioMixerGroup audioMixer, float delay, bool isLoop)
    {
        _AudioSource.outputAudioMixerGroup = audioMixer;
        _AudioSource.loop = isLoop;
        _AudioSource.Play();

        if (!isLoop) { StartCoroutine(COR_DestroyWhenFinish(_AudioSource.clip.length)); }
    }

    public void InitSound(AudioClip clip)
    {
        _AudioSource.clip = clip;
    }


    private IEnumerator COR_DestroyWhenFinish(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        Destroy(gameObject);
    }
}