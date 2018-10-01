using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public enum AudioType
{
    ENV = 1,
    OPER = 2,
    INTRO = 3
}

public class Audio : Singleton<Audio>
{
    public Sprite[] vols;

    AudioSource environment;
    AudioSource operation;
    AudioSource intro;

    SpriteRenderer sr;
    float timeSpan = 0.2f;

    Dictionary<string, AudioClip> audios = new Dictionary<string, AudioClip>();

    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);

        environment = transform.Find("Environment").GetComponent<AudioSource>();
        operation = transform.Find("Operation").GetComponent<AudioSource>();
        intro = transform.Find("Intro").GetComponent<AudioSource>();
        sr = transform.GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    public void SetVolume(int vol)
    {
        float volume = vol / 100f;
        environment.volume = volume;
        operation.volume = volume;
        intro.volume = volume;
    }

    public float Play(AudioType audioType, string name)
    {
        //Debug.Log("Play Audio:"+name);

        AudioClip clip = GetAudio(name);

        switch (audioType)
        {
            case AudioType.ENV:
                environment.clip = clip;
                environment.Play();
                break;
            case AudioType.OPER:
                operation.clip = clip;
                operation.Play();
                break;
            case AudioType.INTRO:
                StartCoroutine(PlayAudio(clip));
                break;
            default:
                break;
        }

        return clip.length;
    }

    IEnumerator PlayAudio(AudioClip clip)
    {
        intro.clip = clip;
        intro.Play();
        sr.enabled = true;

        int count = (int)(clip.length / timeSpan);
        for (int i = 0; i <= count; i++)
        {
            sr.sprite = vols[i % 3];
            yield return new WaitForSeconds(timeSpan);
        }
        sr.sprite = vols[0];
        sr.enabled = false;
    }

    AudioClip GetAudio(string name)
    {
        if (!audios.ContainsKey(name))
        {
            audios[name] = Resources.Load("Audios/" + name) as AudioClip;
        }

        return audios[name];
    }
}
