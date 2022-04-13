using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagon_Electric : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Electric_Obj;

    [SerializeField, Range(1, 3)] int test_Level = 1;
    [SerializeField, Range(0, 10)] int cre_X = 0;
    [SerializeField, Range(0, 14)] int cre_Y = 0;


    
    [SerializeField] private GameObject Stage_mass;

    private int Lev = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject cre_pos = Stage_mass.transform.GetChild(cre_X).GetChild(cre_Y).gameObject;
            Create_turret(cre_pos, test_Level);
        }
    }

    public void Create_turret(GameObject tage, int Lev)
    {
        Vector3 cre_pos = tage.transform.position;
        //タレットの生成位置の補正
        cre_pos.z -= 2.0f;

        GameObject obj = Instantiate(Electric_Obj, cre_pos, Quaternion.identity);

        obj.GetComponent<Ele_tur_Attack>().Set_Init(Lev, Get_Map_Pos(tage),Stage_mass);
    }

    private Vector2 Get_Map_Pos(GameObject _tage)
    {
        Vector2 pos = new Vector2(0, 0);

        //for(int i = 0; i < _tage.transform.root.childCount; i++)
        //{
        //    if(_tage.name == _tage.transform.root.GetChild(i).name)
        //    {
        //        pos.x = i;
        //        break;
        //    }
        //}

        //for (int i = 0; i < _tage.transform.root.childCount; i++)
        //{
        //    if (_tage.name == _tage.transform.root.GetChild((int)pos.x).gameObject.name)
        //    {
        //        pos.y = i;
        //        break;
        //    }
        //}
        pos = new Vector2(cre_X, cre_Y);

        return pos;
    }
}
