﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb_anim : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("Death");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
