using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;
using System;

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

    SEManager sePlay = null;  //SE

    // Update is called once per frame
    void Start()
    {
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>(); //SE
        StartCoroutine(Ele_Attack());

    }

    public void Set_Ini(GameObject _target, int _level)
    {

        //最初の_targetはマス？のはず
        target = _target;
        magic_Level = _level;

        EnemyBase eb = target.GetComponent<EnemyBase>();
        
        tar_ene_pos.x = eb.X;
        tar_ene_pos.y = eb.Y;

        GameObject first_Target = target;
        if(first_Target != null)
        {
            targets.Add(first_Target);
            first_Target.GetComponent<EnemyBase>().Damage(4.0f);
        }

        //初撃用のエフェクトの場所の指定＋削除
        GameObject _efe = Instantiate(e_Ele, transform);
        LightningBoltScript lb = _efe.GetComponent<LightningBoltScript>();
        Vector3 pos = new Vector3(target.transform.position.x, target.transform.position.y + 50.0f, target.transform.position.z);
        lb.StartPosition = Correction_Pos(pos);
        lb.EndPosition = Correction_Pos(target.transform.position);
        Destroy(_efe, 0.6f);

        Destroy(this.gameObject, magic_Level * 0.7f);

    }

    IEnumerator Ele_Attack()
    {
        for(int i = 0; i < magic_Level; i++)
        {
            yield return new WaitForSeconds(i * 0.2f);
            Debug.Log(i + "回目のジャンぴ");
            Transmission_Attack(i);

        }
        Destroy(this.gameObject, 0.5f);
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
            EnemyBase eb = target.GetComponent<EnemyBase>();
            tar_ene_pos.x = eb.X;
            tar_ene_pos.y = eb.Y;
            eb.Damage(4.0f);

            //雷の五芒星のSEを鳴らす
            sePlay.Play("ThunderMagicStar");

            GameObject _efe = Instantiate(e_Ele, transform);
            LightningBoltScript lb = _efe.GetComponent<LightningBoltScript>();
            lb.StartPosition = Correction_Pos(targets[_num].transform.position);
            lb.EndPosition = Correction_Pos(targets[_num + 1].transform.position);

        }
        else
        {
            //次の敵を探しきれなくなったら終了
            Destroy(this.gameObject, 0.5f);
        }

    }

    [Serializable]
    struct eenneeSta {
        public int xx;
        public int yy;
        public GameObject obj;
    }

    //Enemyを返す
    GameObject SearchEnemy()
    {
        //Enemy全取得用
        EnemyBase enemyObjs = new EnemyBase();

        //今いる位置から(x差分+y差分)が2以下の敵を判定
        foreach (GameObject ene in enemyObjs.GetEnemyList())
        {
            if (ene != null)
            {
                if (ene.tag == "Enemy2") continue;

                EnemyBase eb = ene.GetComponent<EnemyBase>();

                int eneX = Math.Abs(eb.X - (int)tar_ene_pos.x);
                int eneY = Math.Abs(eb.Y - (int)tar_ene_pos.y);
                int total = eneX + eneY;

                //中心との距離が2以下かつ0(自分ではない)以外の敵
                if (total > 0 && total <= 2)
                {
                    //タグを変える
                    ene.tag = "Enemy2";

                    return ene;
                }
            }
        }

        //タグを変えた敵全員を戻す
        var tagChangedEnemies = GameObject.FindGameObjectsWithTag("Enemy2");
        foreach (var tce in tagChangedEnemies)
            tce.tag = "Enemy";

        Debug.Log("敵居なかったわ");
        return null;
    }

    //雷の位置を高くする
    Vector3 Correction_Pos(Vector3 _pos)
    {
        _pos.y += 1.0f;

        return _pos;
    }
}
