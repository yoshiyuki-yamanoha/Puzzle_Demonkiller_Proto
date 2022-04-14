using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearMgr : MonoBehaviour
{
    float coolTime;
    // Start is called before the first frame update
    void Start() {
        coolTime = 0.5f;
    }

    // Update is called once per frame
    void Update() {
        coolTime -= Time.deltaTime;
        if (Input.GetButtonDown("Fire2") && coolTime <= 0.0f) {
            coolTime = 0.5f;
            GameMgr.Instance.GotoTitleScene();
        }
        else if (coolTime <= 0.0f) {
            coolTime = 0.0f;
        }
    }
}
