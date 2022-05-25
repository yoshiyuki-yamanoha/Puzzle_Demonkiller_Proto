﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour
{
    //再生を管理する効果音をセットする変数
    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audioSource;
    private AudioSource bgmPlay;

    float soundAttenuation;
    public bool isFadeOutFlag;

    // Start is called before the first frame update
    void Awake()
    {
        //コンポーネントの取得
        //audioSource = GetComponent<AudioSource>();
        audioSource = this.gameObject.transform.GetComponent<AudioSource>();

        isFadeOutFlag = false;
    }

    void FixedUpdate()
    {
        if (isFadeOutFlag == true)
            LowerTheVolume();
    }

    //効果音を再生する
    public void Play(string bgmName)
    {
        switch (bgmName)
        {

            case "TITLEBGM"://1　タイトルBGM
                
                audioSource.clip = audioClips[0];
                audioSource.loop = true;
                audioSource.volume = 0.1f;////もともと0.05
                audioSource.Play();
                break;

            case "PLAYBGM"://2　プレイBGM
 
                audioSource.clip = audioClips[1];
                audioSource.loop = true;
                audioSource.volume = 0.06f; //音量の調整 //もともと0.02
                audioSource.Play();
                break;
            case "ENDBGM"://3　エンドBGM
                audioSource.clip = audioClips[2];
                audioSource.loop = true;
                audioSource.volume = 0.06f; //0.03
                audioSource.Play();
                break;

            case "SELECTBGM":
                audioSource.clip = audioClips[3];
                audioSource.loop = true;
                audioSource.volume = 0.05f;//0.05
                audioSource.Play();
                break;
            case "CLEARBGM":
                audioSource.clip = audioClips[4];
                audioSource.volume = 0.16f;//0.08
                audioSource.Play();
                break;
            case "GAMEOVERBGM":
                audioSource.clip = audioClips[5];
                audioSource.loop = true;
                audioSource.volume = 0.2f;//0.16
                audioSource.Play();
                break;
            case "BOSSBGM":
                audioSource.clip = audioClips[6];
                audioSource.loop = true;
                audioSource.volume = 0.6f;
                audioSource.Play();
                break;
        }
    }

    public float GetBGMVolume()
    {
        return audioSource.volume;
    }

    /// <summary>
    /// audioSourceのvolumeの値を減らします
    /// </summary>
    /// <param name="attenuation">一度に減らす値</param>
    /// <returns>volumeの値を減らせたら真を、減らせなければ偽を返します</returns>
    public bool LowerTheVolume()
    {
        
        audioSource.volume -= soundAttenuation * Time.deltaTime;

        if (audioSource.volume > 0)
        {
            return true;
        }
        else
        {
            audioSource.volume = 0;
            isFadeOutFlag = false;

            return false;
        }
    }

    //public void SetSoundAttenuation(float attenuation)
    //{
    //    soundAttenuation = attenuation;
    //}

    public void StartSoundFadeOut(float attenuation)
    {
        soundAttenuation = attenuation;
        isFadeOutFlag = true;
    }
}
