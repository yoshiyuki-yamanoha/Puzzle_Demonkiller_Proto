using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarSystemsSc : MonoBehaviour
{
    [SerializeField] ManageCoreState s_ManageCoreState;

    Image[] partsList = new Image[15];
    int nowHP = 0;

    //前フレームのコアのHP
    int oldCoreHp;

    //インターバル
    [SerializeField] float interval = 0.1f;
    float currentCount = 0;

    //音
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip charge;
    [SerializeField] AudioSource source;

    void FixedUpdate() {

        if (!s_ManageCoreState) {
            s_ManageCoreState = GameObject.Find("Core").GetComponent<ManageCoreState>();

            for (int i = 0; i < s_ManageCoreState.core.max_hp; i++)
            {
                partsList[i] = transform.GetChild(i).GetChild(1).GetComponent<Image>();
                partsList[i].enabled = false;
            }

            source.PlayOneShot(charge);
        }

        int hp = s_ManageCoreState.core.hp;

        if(hp != oldCoreHp) currentCount = interval;

        if (s_ManageCoreState) {
            if (interval != 0)
            {
                currentCount -= Time.deltaTime;
                if (currentCount <= 0)
                {
                    HPProccess();
                }
            }
        }


        oldCoreHp = hp;
    }

    void HPProccess() {
        int max = s_ManageCoreState.core.max_hp;
        int hp = s_ManageCoreState.core.hp;

        if (hp != nowHP)
        {
            if (nowHP > hp) nowHP--;
            if (nowHP < hp) nowHP++;

            for (int i = 0; i < max; i++)
            {
                if (i < nowHP) partsList[i].enabled = true;
                else partsList[i].enabled = false;
            }

            //source.PlayOneShot(clip);
            currentCount = interval;

            //if (nowHP == hp) source.PlayOneShot(clip);
        }

        

        
    }
}
