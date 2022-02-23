using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideEffect : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject,2.5f);
    }

}
