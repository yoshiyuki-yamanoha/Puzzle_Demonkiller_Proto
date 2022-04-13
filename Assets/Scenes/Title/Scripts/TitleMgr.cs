using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMgr : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if(Input.GetButtonDown("Fire1")) {
            GameMgr.Instance.GotoSelectScene();
        }
    }
}
