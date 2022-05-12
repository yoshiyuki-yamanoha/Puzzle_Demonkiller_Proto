using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star_Ice : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject Ice;
    [SerializeField]private GameObject Ice_efe;
    private GameObject Stage_mass;

    private float upSpeed = 10.0f;
    private float Max_y = 0.8f;

    SEManager sePlay = null;  //SE

    [SerializeField] private float breakSpeed = 0.2f;

    [SerializeField]private List<GameObject> Ice_objs = new List<GameObject>();
    void Start()
    {
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>(); //SE
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        child_Move();
    }

    public void Create_IceBergs(GameObject taget, int _lev)
    {
        int num = (int.Parse(taget.name) % 20);
        Debug.Log(int.Parse(taget.name));

        Stage_mass = GameObject.Find("MassRoot");
        Ice_objs.Clear();
        for (int i = 0; i < 20; i++)
        {
           if(i%2==0) StartCoroutine(IceSE(i));//SE
            for (int j = 0; j < _lev; j++)
            {
                StartCoroutine(cre_ice(num + j, i));
            }
        }
        //for (int j = 0; j < _lev; j++)
        //{
        //    Ene_Damage(num + j);
        //}
    }

    IEnumerator cre_ice(int num,int child)
    {
        float delay = (child) * 0.1f;
        yield return new WaitForSeconds(delay);

        int ObjNum = (380 + (num * 2)) - ((num) + (child * 20));
        if (0 <= ObjNum || ObjNum > Stage_mass.transform.childCount - 1)
        {
            Vector3 crePos = new Vector3(Stage_mass.transform.GetChild(ObjNum).position.x,
                                           Stage_mass.transform.GetChild(ObjNum).position.y - 2.0f,
                                              Stage_mass.transform.GetChild(ObjNum).position.z);


            GameObject IceChild = Instantiate(Ice, crePos, Quaternion.identity, this.transform);
            GameObject Iceefe = Instantiate(Ice_efe, Stage_mass.transform.GetChild(ObjNum).position, Quaternion.identity);

            StartCoroutine(Ene_Damage(ObjNum % 20, ObjNum / 20));

            Ice_objs.Add(IceChild);

            IceChild.transform.localScale = new Vector3(2.5f, 1.0f, 2.5f);
            Iceefe.transform.localScale = new Vector3(1f, 1.5f, 1f);

            Destroy(IceChild, 4.0f);
            Destroy(Iceefe, 1.0f);
        }
    }

    private void child_Move()
    {
        for(int i=0; i < Ice_objs.Count; i++)
        {
            if(Ice_objs[i] != null)
            {
                Vector3 pos = Ice_objs[i].transform.localPosition;
                if (Max_y > pos.y)
                {
                    pos.y += upSpeed * Time.deltaTime;

                    Ice_objs[i].transform.localPosition = pos;
                }
                else
                {
                    Material material = Ice_objs[i].GetComponent<Renderer>().material;
                    if (material.HasProperty("_Destruction"))
                    {
                        float Des = material.GetFloat("_Destruction");
                        Des += breakSpeed * Time.deltaTime;
                        if(Des > 1.0f) { Des = 1.0f; }

                        material.SetFloat("_Destruction", Des);
                        Debug.Log(Des);
                    }
                }
            }
            else
            {
                Ice_objs.Remove(Ice_objs[i]);
            }
        }
    }

    IEnumerator Ene_Damage(int _x , int _y)
    {
        yield return new WaitForSeconds(0.2f);
        EnemyBase eb = new EnemyBase();

        List<GameObject> enemies = eb.GetEnemyList();

        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyBase ene = enemies[i].GetComponent<EnemyBase>();
            
            if (ene != null)
            {
                if (_x == ene.X && _y == ene.Y)
                {
                    ene.Damage(2);
                }
            }

        }
    }

    IEnumerator IceSE(int num)
    {
        float delay = (num) * 0.1f;
        yield return new WaitForSeconds(delay);

        //氷の五芒星のSEを流す
        sePlay.Play("IceMagicStar");


    }

}
