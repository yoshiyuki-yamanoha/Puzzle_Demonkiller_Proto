using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagonfire : MonoBehaviour
{
    [SerializeField] GameObject fireWall;

    public void P_Fire(GameObject tage)
    {
        Instantiate(fireWall, tage.transform.position, Quaternion.identity);

    }
}
