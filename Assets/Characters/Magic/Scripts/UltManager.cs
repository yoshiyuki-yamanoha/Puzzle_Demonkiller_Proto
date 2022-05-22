using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltManager : MonoBehaviour
{
    public Slider ultslider;

    // Start is called before the first frame update
    void Start()
    {
        ultslider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButtonDown("Ult")) && (ultslider.value >= 10))
        {
            ultslider.value -= 10;
            Debug.Log("必殺技の予定だよ");
        }
        //Debug.Log(ultslider.value);
    }

    public void ultChage()
    {
        ultslider.value += 10;
    }
}
