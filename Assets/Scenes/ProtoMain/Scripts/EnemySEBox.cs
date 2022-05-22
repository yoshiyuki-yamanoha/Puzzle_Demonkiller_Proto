using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySEBox : MonoBehaviour
{
    SEManager sePlay;
    EnemyBase EnemyTyp;
    
    int typ;
    

    // Start is called before the first frame update
    void Start()
    {
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>();

        if (typ==0)//ゴブリンの場合
        {
            sePlay.Play("GoblinDeath");
        }
        else if (typ == 1) //デモンの場合
        {
            sePlay.Play("DemonDeath");
        }
        else if (typ==2)  //ボム兵の場合
        {
            sePlay.Play("BombDeath");
        }
        else if (typ == 3)  //炎の剣の場合
        {
            sePlay.Play("FlameDeath");
        }
 
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnemyTyp(int typ) {
       
        this.typ = typ; 
    }
}
