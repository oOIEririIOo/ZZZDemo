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
        [Header("��Ƶ����")]
        public AudioClip clip;

        [Header("��Ƶ����")]
        public AudioMixerGroup outputGroup;

        [Header("��Ƶ����")]
        [Range(0, 1)]
        public float volume;

        [Header("��Ƶ�Ƿ񿪾ֲ���")]
        public bool playOnAwake;

        [Header("�Ƿ񿪾ֲ���")]
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
            //TODO :Debug.Log($"{name}��Ƶ������");
            //Debug.Log($"{name}��Ƶ������");
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
            Debug.Log($"{name}��Ƶ������");
            return;
        }
        INSTANCE.audiosDic[name].Stop();
    }
}
