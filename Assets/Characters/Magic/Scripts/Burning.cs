using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burning : MonoBehaviour
{
    [SerializeField] private GameObject Burning_model;

    private float Damege = 2;


    private void Start()
    {
        Instantiate(Burning_model, transform.position, Quaternion.identity);
    }

    public void Burning_damege()
    {
        this.GetComponent<Enemy>().Damage(Damege);
    }
}
