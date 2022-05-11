﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{

    //再生を管理する効果音をセットする変数
    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audioSource;
    private AudioSource sePlay;


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
            case "Select"://1　魔方陣時の決定音
                audioSource.PlayOneShot(audioClips[0],2.0f);
                break;

            case "MagicChange"://2　魔法を入れ替えた時の音 //廃止予定
                audioSource.PlayOneShot(audioClips[1], 0.5f);
                break;
            case "MagicAreaSelect"://3　魔法陣の色の入れ替え
                audioSource.PlayOneShot(audioClips[2], 0.1f);
                break;
            case "MagicCursorSelect"://4　魔法を打つ時のカーソル移動
                audioSource.PlayOneShot(audioClips[3], 1.0f);
                break;
            case "MagicShot"://5　魔法を打つ時
                audioSource.PlayOneShot(audioClips[4],0.3f);
                break;
            case "TurnChange"://6　ターンが変わる時
                audioSource.PlayOneShot(audioClips[5],1.0f);
                break;
            case "EnemySpawn"://7　敵が出現した時
                audioSource.PlayOneShot(audioClips[6], 2.0f);
                break;
            case "EnemyDead"://8　敵を倒したとき
                audioSource.PlayOneShot(audioClips[7], 0.5f);
                break;
            case "Select3"://9魔方陣時のカーソル移動
                audioSource.PlayOneShot(audioClips[8], 1.0f);
                break;
            case "matchSE"://10魔方陣を組んだとき(パズルをクリアしたとき)
                audioSource.PlayOneShot(audioClips[9], 0.4f);
                break;
            case "EnemyAtack"://11敵攻撃
                audioSource.PlayOneShot(audioClips[10], 2.0f);
                break;
            case "DestroyBarricade"://12バリケード破壊
                audioSource.PlayOneShot(audioClips[11], 1.3f);
                break;
            case "GoblinSpawn"://13　ゴブリン出現
                audioSource.PlayOneShot(audioClips[12], 1.3f);
                break;
            case "GoblinDeath"://13ゴブリン死亡
                audioSource.PlayOneShot(audioClips[13], 1.3f);
                break;
            case "DemonDeath"://14デモン死亡
                audioSource.PlayOneShot(audioClips[14], 1.3f);
                break;
            case "BombSpawn"://15ボム兵出現
                audioSource.PlayOneShot(audioClips[15], 1.3f);
                break;
            case "BombDeath"://16ボム兵死亡
                audioSource.PlayOneShot(audioClips[16], 1.3f);
                break;
            case "BombEnemExplosion"://17ボム兵爆発
                audioSource.PlayOneShot(audioClips[17], 1.3f);
                break;
            case "FlameSpawn"://18炎の剣のモンスター出現
                audioSource.PlayOneShot(audioClips[18], 1.3f);
                break;
            case "FlameAttack":
                audioSource.PlayOneShot(audioClips[19], 1.3f);
                break;
            case "FlameDeath"://18炎の剣のモンスター死亡
                audioSource.PlayOneShot(audioClips[20], 1.3f);
                break;
            case "FireMagicStar":
                audioSource.PlayOneShot(audioClips[21], 1.0f);
                break;
            case "IceMagicStar":
                audioSource.PlayOneShot(audioClips[22], 1.3f);
                break;
            case "IceMagicPenta":
                audioSource.PlayOneShot(audioClips[23], 1.3f);
                break;
            case"ThunderMagicFire":
                audioSource.PlayOneShot(audioClips[24], 2.3f);
                break;
            case "ThunderMagicStar":
                audioSource.PlayOneShot(audioClips[25], 1.3f);
                break;



        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
