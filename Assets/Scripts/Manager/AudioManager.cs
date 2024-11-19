using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingleMonoBase<AudioManager>
{
    
    [System.Serializable]
    public class Sound
    {
        [Header("音频剪辑")]
        public AudioClip clip;

        [Header("音频分组")]
        public AudioMixerGroup outputGroup;

        [Header("音频音量")]
        [Range(0, 1)]
        public float volume;

        [Header("音频是否开局播放")]
        public bool playOnAwake;

        [Header("是否开局播放")]
        public bool loop;

    }

    public List<Sound> sounds;
    private Dictionary<string, AudioSource> audiosDic;

    private protected override void Awake()
    {
        base.Awake();

        audiosDic = new Dictionary<string, AudioSource>();
    }
    private void Start()
    {
        foreach(var sound in sounds)
        {
            GameObject obj = new GameObject(sound.clip.name);
            obj.transform.SetParent(transform);

            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.playOnAwake = sound.playOnAwake;
            source.loop = sound.loop;
            source.volume = sound.volume;
            source.outputAudioMixerGroup = sound.outputGroup;

            if (sound.playOnAwake)
                source.Play();

            audiosDic.Add(sound.clip.name, source);

        }
    }

    public void PlayAudio(string name,bool isWait = false)
    {
        if( ! INSTANCE.audiosDic.ContainsKey(name))
        {
            //TODO :Debug.Log($"{name}音频不存在");
            //Debug.Log($"{name}音频不存在");
            return;
        }
        if (isWait)
        {
            if (!INSTANCE.audiosDic[name].isPlaying)
                INSTANCE.audiosDic[name].PlayOneShot(INSTANCE.audiosDic[name].clip);
        }
        else INSTANCE.audiosDic[name].PlayOneShot(INSTANCE.audiosDic[name].clip);
    }

    public void StopAudio(string name)
    {
        if (!INSTANCE.audiosDic.ContainsKey(name))
        {
            Debug.Log($"{name}音频不存在");
            return;
        }
        INSTANCE.audiosDic[name].Stop();
    }
}
