using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).name = i.ToString();
            //Debug.Log(i + "|わぁ : " + transform.GetChild(i).childCount);
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                transform.GetChild(i).GetChild(j).name = j.ToString();
            }
        }
    }
}
