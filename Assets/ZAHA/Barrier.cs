using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    float hp;

    public float Hp { get => hp; set => hp = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool HPCheck(float hp)
    {
        if(Hp < 0)
        {
            Hp = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}
