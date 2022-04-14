using System.Collections;
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


    // Start is called before the first frame update
    void Awake()
    {
        //コンポーネントの取得
        //audioSource = GetComponent<AudioSource>();
        audioSource = this.gameObject.transform.GetComponent<AudioSource>();

    }

    //効果音を再生する
    public void Play(string bgmName)
    {
        switch (bgmName)
        {
            case "TITLEBGM"://1　タイトルBGM
                
                audioSource.clip = audioClips[0];
                audioSource.loop = true;
                audioSource.Play();
                break;
            case "PLAYBGM"://2　プレイBGM
 
                audioSource.clip = audioClips[1];
                audioSource.loop = true;
                audioSource.volume = 0.6f;
                audioSource.Play();
                break;
            case "ENDBGM"://3　エンドBGM
                audioSource.clip = audioClips[2];
                audioSource.loop = true;
                audioSource.volume = 0.6f;
                audioSource.Play();
                break;
        }
    }
}
