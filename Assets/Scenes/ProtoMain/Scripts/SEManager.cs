using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{

    //再生を管理する効果音をセットする変数
    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audioSource;
    private AudioSource  sePlay;


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
            case "Select"://1　魔方陣時のカーソルの移動
                audioSource.PlayOneShot(audioClips[0],0.5f);
                break;
            case "MagicChange"://2　魔方陣の色の入れ替え
                audioSource.PlayOneShot(audioClips[1], 0.5f);
                break;
            case "MagicAreaSelect"://3　魔方を打つエリアを選択
                audioSource.PlayOneShot(audioClips[2], 0.5f);
                break;
            case "MagicCursorSelect"://4　魔法を打つ時のカーソル移動
                audioSource.PlayOneShot(audioClips[3], 0.5f);
                break;
            case "MagicShot"://5　魔法を打つ時
                audioSource.PlayOneShot(audioClips[4],0.3f);
                break;
            case "TurnChange"://6　ターンが変わる時
                audioSource.PlayOneShot(audioClips[5], 0.5f);
                break;
            case "EnemySpawn"://7　敵が出現した時
                audioSource.PlayOneShot(audioClips[6], 0.5f);
                break;
            case "EnemyDead"://8　敵を倒したとき
                audioSource.PlayOneShot(audioClips[7], 0.5f);
                break;
            case "Select2"://9魔方陣選択2
                audioSource.PlayOneShot(audioClips[8], 0.5f);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
