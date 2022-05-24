using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreBase;

public class ManageBarricade : TrunManager
{
    [SerializeField] Vector2Int map_pos;
    TrunManager turnMGR;
    [SerializeField]private int myNumber;
    static public List<GameObject> bariicades = new List<GameObject>();
    Barricade_Class barri = new Barricade_Class();
    [SerializeField] GameObject barriBreakEffect = null;

    SEManager sePlay = null;//SE
    MapMass map_mass;
    void Start()
    {
        barri.obj= this.gameObject;
        barri.hp = barri.max_hp;
        //bariicades = new List<GameObject>();
        turnMGR = GameObject.Find("TrunManager").gameObject.GetComponent<TrunManager>();
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>();//SE
        map_mass = GameObject.Find("MapInstance").GetComponent<MapMass>();
    }

    public Vector2Int GetPos()
    {
        return map_pos;
    }

    public void SetMapPos(Vector2Int map_pos)
    {
        this.map_pos = map_pos;
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
            List<GameObject> remove_obj = new List<GameObject>();
            foreach(GameObject o in bariicades)
            {
                if(o != null)//nullチェック
                if(o.GetComponent<ManageBarricade>().myNumber == this.myNumber)
                {
                    remove_obj.Add(o);
                }
            }

            foreach(GameObject o in remove_obj)
            {
                if (o.GetComponent<ManageBarricade>() != null) //nullチェック
                {
                    // バリケードが壊れる際のエフェクトを生成
                    GeneratesBarricadesBreakedEffect(o);
                    bariicades.Remove(o);
                    map_mass.Map[o.GetComponent<ManageBarricade>().GetPos().y, o.GetComponent<ManageBarricade>().GetPos().x] = 0;
                    Destroy(o.gameObject);
                }
            }

            // バリケードが壊れる際のエフェクトを生成
            GeneratesBarricadesBreakedEffect(this.gameObject);
            Destroy(this.gameObject);
            sePlay.Play("DestroyBarricade");
        }
    }

    // バリケードが壊れる際のエフェクトを生成
    private void GeneratesBarricadesBreakedEffect(GameObject obj)
    {
        Instantiate(barriBreakEffect, new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z), Quaternion.identity);
    }
}
