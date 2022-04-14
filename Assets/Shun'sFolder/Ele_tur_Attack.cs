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
    private GameObject tage;

    public void Set_Init(int _lev, GameObject _tage)
    {
        Level = _lev;
        tage = _tage;
        Stage_mass = GameObject.Find("MassRoot");
        Set_pos();
    }
    private void Start()
    {
        Invoke("tur_Attack", 2.0f);
        Invoke("tur_Attack", 4.0f);
        Invoke("tur_Attack", 6.0f);
        //Destroy(gameObject, 7.0f);
    }

    void Set_pos()
    {
        pos.x = int.Parse(tage.name) / 15;
        pos.y = int.Parse(tage.name) % 15;
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
                //if(i > Stage_mass.transform.childCount -1 || j > Stage_mass.transform.GetChild(i).childCount - 1) { continue; }
                //create_Ele_Efe(Stage_mass.transform.GetChild(i).GetChild(j).gameObject); 

                int targetNum = ((i * 15) + (j % 15));
                if (0 < targetNum && targetNum < Stage_mass.transform.childCount - 1)
                {
                    Ene_Damage(Stage_mass.transform.GetChild(targetNum).gameObject);
                    create_Ele_Efe(Stage_mass.transform.GetChild(targetNum).gameObject);
                }
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

    void Ene_Damage(GameObject _tage)
    {
        Vector2 magic_pos= new Vector2( int.Parse(_tage.name) / 15, int.Parse(_tage.name) % 15);

        GameObject enemies = GameObject.Find("Sponer");
        for(int i = 0; i < enemies.transform.childCount; i++)
        {
            Enemy ene = enemies.transform.GetChild(i).GetComponent<Enemy>();
            Vector2 ene_pos = new Vector2(ene.X, ene.Y);
            if(magic_pos == ene_pos)
            {
                ene.Damage(1);
            }
        }
    }
}
