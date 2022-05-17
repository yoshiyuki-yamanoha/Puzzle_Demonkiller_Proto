using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effectcallback : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Debug.Log("particle再生終了したよ。");
        this.gameObject.SetActive(false);
    }
}
