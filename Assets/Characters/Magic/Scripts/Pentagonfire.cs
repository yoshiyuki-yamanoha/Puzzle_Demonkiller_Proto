using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagonfire : MonoBehaviour
{
    [SerializeField] GameObject fireWall;

    private float level;

    private bool EnemyInvasion;//適当変数
    private bool F_magicStartUp;
    public void P_Fire(GameObject tage)
    {
        Instantiate(fireWall, tage.transform.position, Quaternion.identity);

        F_magicStartUp = true;
    }
}
