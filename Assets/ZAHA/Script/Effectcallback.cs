using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effectcallback : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        this.gameObject.SetActive(false);
    }
}
