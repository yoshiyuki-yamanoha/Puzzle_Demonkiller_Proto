using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreBase;

public class ManageBarricade : TrunManager
{
    TrunManager turnMGR;
    [SerializeField]private int myNumber;
    static public List<GameObject> bariicades = new List<GameObject>();
    Barricade_Class barri = new Barricade_Class();
    
    void Start()
    {
        barri.obj= this.gameObject;
        barri.hp = barri.max_hp;
        //bariicades = new List<GameObject>();
        turnMGR = GameObject.Find("TrunManager").gameObject.GetComponent<TrunManager>();
    }


    void FixedUpdate()
    {

        TrunPhase currentPhase = turnMGR.GetTrunPhase();

        if (currentPhase == TrunPhase.Enemy)
        {
            CheckHP();
        }

    }

    public void SetMyNumber(int num)
    {
        myNumber = num;
        bariicades.Add(this.gameObject);
    }

    public void ReceiveDamage(int enemy_attack)
    {
        //barri.hp -= EAP.knock;
        barri.hp -= enemy_attack;
        CheckHP();

    }

    public void CheckHP()
    {
        // ｈｐが零になっていなるか確認
        if (barri.hp<= 0)
        {
            foreach(GameObject o in bariicades)
            {
                if(o.GetComponent<ManageBarricade>().myNumber == this.myNumber)
                {
                    Destroy(o.gameObject);
                }
            }
            Destroy(this.gameObject);
        }
    }
}
