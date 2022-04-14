using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class Ele_tur_Attack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private int Level = 0;
    [SerializeField]private Vector2 pos = new Vector2(0, 0);
    [SerializeField]private GameObject lightning_Efe;
    [SerializeField]private GameObject Stage_mass;

    private int efe_Max = 3;

    public void Set_Init(int _lev,Vector2 _pos, GameObject _Stage)
    {
        Level = _lev;
        pos = _pos;
        Stage_mass = _Stage;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void Start()
    {
        Invoke("tur_Attack", 2.0f);
        Invoke("tur_Attack", 4.0f);
        Invoke("tur_Attack", 6.0f);
        Destroy(gameObject, 7.0f);
    }

    private void tur_Attack()
    {
        Vector2 Range_min = new Vector2((int)pos.x - Level, (int)pos.y - Level);
        Vector2 Range_Max = new Vector2((int)pos.x + Level, (int)pos.y + Level);

        for(int i = (int)Range_min.x; i <= (int)Range_Max.x; i++)
        {
            for(int j = (int)Range_min.y; j <= (int)Range_Max.y; j++)
            {
                //マスに雷落としたいなぁ

                if(i < 0 || j < 0) { continue; }
                if(i > Stage_mass.transform.childCount -1 || j > Stage_mass.transform.GetChild(i).childCount - 1) { continue; }
                create_Ele_Efe(Stage_mass.transform.GetChild(i).GetChild(j).gameObject);
            }
        }
    }

    private void create_Ele_Efe(GameObject _tage)
    {
        for(int i = 0; i < efe_Max; i++)
        {
            GameObject Efe = Instantiate(lightning_Efe, _tage.transform.position, Quaternion.identity);

            Vector3 start_pos = transform.position;
            start_pos.y = 2.0f;
            start_pos.z += 1.8f;
            Efe.GetComponent<LightningBoltScript>().StartPosition = start_pos;
            Efe.GetComponent<LightningBoltScript>().StartObject = null;


            Efe.GetComponent<LightningBoltScript>().EndObject = _tage;
            Destroy(Efe, 1.0f);
        }
    }
}
