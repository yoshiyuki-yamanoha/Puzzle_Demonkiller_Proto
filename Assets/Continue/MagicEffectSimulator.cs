using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//魔法撃つ場所選択時のうっすー－－－－－－－い魔法のシミュレーション的なやつを実装しようとおもって作ったすくりぽうｔ
//不透明度を下げた魔法を予め用意しておくか、魔法打つまで不透明度を下げて、打つ瞬間に戻す処理。

public class MagicEffectSimulator : MonoBehaviour
{

    //魔法エフェクト Prefab
    [SerializeField] GameObject p_magicEffect;

    //シミュレーションするかのフラグ
    bool isSimulation;

    //シミュレーションループ用のタイム変数 (フレーム)
    float time = 2;

    private void Start()
    {
        //魔法の不透明度を下げる(？)
    }



    private void FixedUpdate()
    {
        //魔法のエフェクトがnullではないかつ
        if (p_magicEffect && isSimulation) {

            time -= Time.deltaTime;

            if (time <= 0) {
                time = 0;
                GameObject mag = Instantiate(p_magicEffect, transform.position, Quaternion.identity);
                Destroy(mag, 2.0f);
            }

        }
    }

    //魔法のエフェクトを入れる
    public void SetSimulationMagicEffect(GameObject magicEffect) {
        p_magicEffect = magicEffect;
    }

    //魔法のエフェクトのシミュレーションをするか
    public void ChangeSimilationFlg(bool flag) {
        isSimulation = flag;
    }
}
