using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    //再生を管理する効果音をセットする変数
    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audioSource;
    private AudioSource bgmPlay;


    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントの取得
        audioSource = GetComponent<AudioSource>();

    }

    //効果音を再生する
    public void Play(string bgmName)
    {
        switch (bgmName)
        {
            case "TITLEBGM"://1　タイトルBGM
                audioSource.PlayOneShot(audioClips[0], 0.2f);
                break;
            case "PLAYBGM"://2　プレイBGM
                audioSource.PlayOneShot(audioClips[1], 0.5f);
                break;
            case "ENDBGM"://3　エンドBGM
                audioSource.PlayOneShot(audioClips[2], 0.5f);
                break;
        }
    }
}
