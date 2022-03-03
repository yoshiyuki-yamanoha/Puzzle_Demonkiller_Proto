using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMagic : MonoBehaviour
{

    [SerializeField] private GameObject Magic;

    [SerializeField] private Transform[] Player;

    [SerializeField] string PlayerStr;       //敵の魔法陣の正解

    private int magicShootCnt;               // 魔法を撃った数

    List<GameObject> Enemy_List = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        setPlayerMagicStr();

        magicShootCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        setPlayerMagicStr();
        Ene_MC_Check();
    }

    void Ene_MC_Check()
    {
        if (Enemy_List.Count == 0) return;

        foreach (GameObject ene in Enemy_List)
        {
            if(ene != null)
            {
                string eneStr = ene.transform.GetChild(1).GetComponent<Ene_MagicCircle>().GetEneStr();
                if (eneStr == PlayerStr)
                {
                    //魔法を撃つ
                    //魔法を５回撃つごとに落ちてくる魔法に変える
                    if (magicShootCnt < 5)
                    {
                        this.gameObject.GetComponent<MagicFlyingToTheEnemy>().M_FireForward(ene);
                        magicShootCnt++;
                    }
                    else
                    {
                        this.gameObject.GetComponent<FallingMagic>().M_FireFall(ene);
                        magicShootCnt = 0;
                    }

                    //敵が倒れる処理
                    Enemy_List.Remove(ene);
                    Destroy(ene.transform.GetChild(1).gameObject);
                    //del.GetComponent<Demon>().Damage(100.0f);
                    break;
                }
            }
        }
    }

    public void CreateMagic()
    {
        Instantiate(Magic, transform.position, Quaternion.identity);
    }

    public void Enelist_Add(GameObject obj)
    {
        Enemy_List.Add(obj);
    }
    
    public void Enelist_Delete(GameObject obj)
    {
        //Enemy_List.(obj);
    }

    void setPlayerMagicStr()
    {
        PlayerStr = "";

        foreach (Transform o in Player)
        {
            if (o.childCount > 0)
                PlayerStr = PlayerStr + o.GetChild(0).gameObject.name;
        }
    }

    public string Get_Str()
    {
        return PlayerStr;
    }
}
