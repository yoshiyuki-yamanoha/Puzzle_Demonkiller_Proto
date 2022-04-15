using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MAGIC
{
    STAR_FIRE = 0,
    STAR_ICE,
    STAR_ELECTRIC,
    PENTAGON_FIRE,
    PENTAGON_ICE,
    PENTAGON_ELECTRIC,
}

public class Shoot_The_at_Enemy_Magic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] magic = new GameObject[6];

    public Vector3[] move_Pos = new Vector3[2];

    private int magic_Lv;
    private int magic_Type;

    public GameObject taget;

    private float SPEED = 0.7f;
    private float disPer;

    GameObject[] targets;

    delegate void Event(GameObject _magic);
    private struct magic_Func
    {
        public Event execution;
    }
    magic_Func[] magic_f = new magic_Func[6];

    // Update is called once per frame

    private void Initialized()
    {
        magic_f[(int)MAGIC.STAR_FIRE].execution = Magic_Star_Fire;
        magic_f[(int)MAGIC.STAR_ICE].execution = Magic_Star_Ice;
        magic_f[(int)MAGIC.STAR_ELECTRIC].execution = Magic_Star_Electric;
        magic_f[(int)MAGIC.PENTAGON_FIRE].execution = Magic_Pentagon_Fire;
        magic_f[(int)MAGIC.PENTAGON_ICE].execution = Magic_Pentagon_Ice;
        magic_f[(int)MAGIC.PENTAGON_ELECTRIC].execution = Magic_Pentagon_Electric;
    }
    void FixedUpdate()
    {
        Magic_Move();
    }

    public void Set_Ini(GameObject _tage, int _Lv, int _type, GameObject[] _targets = null)
    {
        magic_Lv = _Lv;
        magic_Type = _type;

        taget = _tage;
        targets = _targets;
        Target_Change();

        move_Pos[0] = transform.position;
        move_Pos[1] = taget.transform.position;

        disPer = 0.0f;
        Initialized();
    }

    private void Magic_Move()
    {
        disPer += Time.deltaTime / SPEED;
        if (disPer < 1.0f)
        {
            transform.position = Vector3.Lerp(move_Pos[0], move_Pos[1], disPer);
        }
        else
        {
            magic_Impact();
            Destroy(gameObject);
        }
    }

    //タイプごとの魔法を発動
    private void magic_Impact()
    {
        GameObject cre_Magic = Instantiate(magic[magic_Type], transform.position, Quaternion.identity);
        magic_f[magic_Type].execution(cre_Magic);
    }

    
    private void Magic_Star_Fire(GameObject _magic)
    {
        if (magic_Type == (int)MAGIC.STAR_FIRE)
        {
            GameObject fire;

            fire = Instantiate(magic[(int)MAGIC.STAR_FIRE], transform.position,Quaternion.identity);
            // レベルに応じてスケールの変更

            Destroy(fire, 1.0f);
        }
    }

    private void Magic_Star_Ice(GameObject _magic)
    {
        if (magic_Type == (int)MAGIC.STAR_ICE)
        {
            _magic.GetComponent<Star_Ice>().Create_IceBergs(taget, magic_Lv);
            Destroy(gameObject);
        }
    }
    private void Magic_Star_Electric(GameObject _magic)
    {
        if (magic_Type == (int)MAGIC.STAR_ELECTRIC)
        {
            _magic.GetComponent<Ele_tur_Attack>().Set_Init(magic_Lv, taget);
        }
    }

    private void Magic_Pentagon_Fire(GameObject _magic)
    {
        if (magic_Type == (int)MAGIC.PENTAGON_FIRE)
        {
            foreach(GameObject tar in targets)
            {

            }
        }
    }

    private void Magic_Pentagon_Ice(GameObject _magic)
    {
        if (magic_Type == (int)MAGIC.PENTAGON_ICE)
        {

        }
    }
    private void Magic_Pentagon_Electric(GameObject _magic)
    {
        if (magic_Type == (int)MAGIC.PENTAGON_ELECTRIC)
        {
            _magic.GetComponent<Ele_tur_Attack>().Set_Init(magic_Lv, taget);
        }
    }

    private void Target_Change()
    {
        if (magic_Type == (int)MAGIC.STAR_ICE)
        {
            int num = (int.Parse(taget.name) / 15) * 15 + 14;
            taget = taget.transform.parent.GetChild(num).gameObject;
        }
    }
}
