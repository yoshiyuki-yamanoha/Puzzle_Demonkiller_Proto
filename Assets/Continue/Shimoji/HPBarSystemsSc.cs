using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarSystemsSc : MonoBehaviour
{
    [SerializeField] Transform startPosObj;
    [SerializeField] GameObject parts;
    [SerializeField] CoreBase.Core _core;

    GameObject[] partsList;

    //前フレームのコアのHP
    int oldCoreHp;

    void Start() {
        for (int i = 0; i < _core.max_hp; i++)
        {
            partsList[i] = Instantiate(parts, startPosObj.position, Quaternion.identity);
        }
    }

    void FixedUpdate() {
        int max = _core.max_hp;
        int hp = _core.hp;

        if (oldCoreHp != hp) {

            for (int i = 0; i < max; i++) {
                Image img = partsList[i].transform.GetChild(1).GetComponent<Image>();
                if (i < hp) img.enabled = true;
                else img.enabled = false;
            }

            
        }

        oldCoreHp = hp;
    }
}
