using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//再生を管理する効果音をセットする変数

public class SEPlayer : MonoBehaviour
{
    //再生を管理する効果音たちをセットする変数
    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audioSource;

    void Start()
    {
        //コンポーネント取得
        audioSource = GetComponent<AudioSource>();
    }

    //効果音を再生する
    public void Play(string seName)
    {
        switch (seName)
        {
            case "DECIDE":
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "SWITCH":
                audioSource.PlayOneShot(audioClips[1]);
                break;
            case "SE2":
                audioSource.PlayOneShot(audioClips[2]);
                break;
                //:
                //:
                //以下効果音の数だけ記述
        }
    }
}
