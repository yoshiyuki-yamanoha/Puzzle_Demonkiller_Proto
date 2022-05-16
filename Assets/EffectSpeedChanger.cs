using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpeedChanger : MonoBehaviour
{
    [SerializeField] ParticleSystem[] effects;

    public void SpeedChanger(float pow)
    {
        for (int i = 0; i < effects.Length; i++) {
            var spd = effects[i].main;
            spd.simulationSpeed *= pow;
        }
    }

    public void SizeChanger(float scale)
    {
        transform.localScale *= scale;
    }
}
