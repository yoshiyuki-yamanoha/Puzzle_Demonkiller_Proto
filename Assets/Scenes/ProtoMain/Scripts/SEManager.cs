using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{

    //再生を管理する効果音をセットする変数
    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントの取得
        audioSource = GetComponent<AudioSource>();
        
    }

    //効果音を再生する
    public void Play(string seName)
    {
        switch (seName)
        {
            case "Select"://1
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "MagicChange"://2
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "MagicAreaSelect"://3
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "MagicCursorSelect"://4
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "MagicShot"://5
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "TurnChange"://6
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "EnemySpawn"://7
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "EnemyDead"://8
                audioSource.PlayOneShot(audioClips[0]);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
