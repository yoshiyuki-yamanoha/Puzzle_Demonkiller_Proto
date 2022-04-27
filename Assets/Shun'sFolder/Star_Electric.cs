using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class Star_Electric : MonoBehaviour
{
    //雷のエフェクト
    [SerializeField] private GameObject e_Ele;

    private GameObject target;
    private int magic_Level;

    //今選択している敵のマス上のXY
    private Vector2 tar_ene_pos;

    //撃った敵を取得２回目の防止用
    List<GameObject> targets = new List<GameObject>();

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(Ele_Attack());
    }

    public void Set_Ini(GameObject _target, int _level)
    {
        //最初の_targetはマス？のはず
        target = _target;
        magic_Level = _level;
            
        tar_ene_pos.x = int.Parse(target.name) % 20;
        tar_ene_pos.y = int.Parse(target.name) / 20;

        GameObject first_Target = Map_mass_EneSer(tar_ene_pos);
        if(first_Target != null)
        {
            targets.Add(first_Target);
            first_Target.GetComponent<EnemyBase>().Damage(4.0f);
        }

    }

    IEnumerator Ele_Attack()
    {
        for(int i = 0; i < magic_Level; i++)
        {
            yield return new WaitForSeconds(i * 0.5f);
            Transmission_Attack(i);

        }
    }

    GameObject Map_mass_EneSer(Vector2 pos)
    {
        //Enemy全取得用
        EnemyBase enemyObjs = new EnemyBase();

        foreach (GameObject ene in enemyObjs.GetEnemyList())
        {
            EnemyBase eb = ene.GetComponent<EnemyBase>();
            if (pos.x == eb.X && pos.y == eb.Y)
            {
                return ene;
            }
        }

        return null;

    }

    void Transmission_Attack(int _num)
    {

        if((target = SearchEnemy()) != null)
        {
            targets.Add(target);
            tar_ene_pos.x = target.GetComponent<EnemyBase>().X;
            tar_ene_pos.y = target.GetComponent<EnemyBase>().Y;

            GameObject _efe = Instantiate(e_Ele, transform);
            LightningBoltScript lb = _efe.GetComponent<LightningBoltScript>();
            lb.StartPosition = targets[_num].transform.position;
            lb.EndPosition = targets[_num + 1].transform.position;

        }
        else
        {
            //次の敵を探しきれなくなったら終了
            Destroy(this.gameObject, 0.5f);
        }

    }

    //Enemyを返す
    GameObject SearchEnemy()
    {
        //Enemy全取得用
        EnemyBase enemyObjs = new EnemyBase();

        //まず周りの9マスを探索
        for(int x = (int)tar_ene_pos.x; x - 1 < (int)tar_ene_pos.x + 1; x++)
        {
            for (int y = (int)tar_ene_pos.y - 1; y < (int)tar_ene_pos.y + 1; y++)
            {
                foreach(GameObject ene in enemyObjs.GetEnemyList())
                {
                    EnemyBase eb = ene.GetComponent<EnemyBase>();
                    if (x == eb.X && y == eb.Y)
                    {
                        foreach (GameObject tar in targets)
                        {
                            if(tar == ene)
                            {
                                return null;
                            }
                        }
                        //前にダメージを与えていないEnemyを返す
                        return ene;
                    }
                }
            }
        }

        Vector2[] correction_Value = new Vector2[4]{ new Vector2(0, 2), new Vector2(2, 0), new Vector2(0, -2), new Vector2(-2, 0) };

        for(int i = 0; i < correction_Value.Length; i++)
        {
            Vector2 tar_pos = new Vector2(tar_ene_pos.x + correction_Value[i].x, tar_ene_pos.y + correction_Value[i].y);
            foreach (GameObject ene in enemyObjs.GetEnemyList())
            {
                EnemyBase eb = ene.GetComponent<EnemyBase>();
                if (tar_pos.x == eb.X && tar_pos.y == eb.Y)
                {
                    return ene;
                }
            }
        }
        return null;
    }
}
