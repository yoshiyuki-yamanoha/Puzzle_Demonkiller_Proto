using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagonfire : MonoBehaviour
{
    [SerializeField] GameObject fireWall;

    private float level;

    private bool EnemyInvasion;//適当変数
    private bool F_magicStartUp;
    void Start()
    {
        
    }

    void Update()
    {
        CheckEnemy();

        //if ((!F_magicStartUp) && (EnemyInvasion))
        //{
        //    //炎の壁の処理
        //    //P_Fire();
        //}
    }

    public void CheckEnemy()
    {
        //if(えねみーの座標が魔法陣と一緒なら){
        //EnemyInvasion = true;
        //}
        //if(えねみーの座標が炎の壁と一緒なら){
        //this.gameObject.AddComponent<Burning>();
        //}
    }
    public void P_Fire(GameObject tage)
    {
        Instantiate(fireWall, tage.transform.position, Quaternion.identity);

        F_magicStartUp = true;
    }
}
