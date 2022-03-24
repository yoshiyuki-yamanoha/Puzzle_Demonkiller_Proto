using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem particle = null;
    [SerializeField] GameObject particle_obj = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Play()
    {
        particle.Play();
    }
}
