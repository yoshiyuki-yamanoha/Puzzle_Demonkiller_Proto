using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star_Fire : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject Magic_Obj;
    private GameObject Stage_mass;
    private int level = 0;
    private GameObject tage;
    private float damege_num = 2.0f;

    private Vector2Int tage_pos;
    SEManager sePlay = null;  //SE

    public void set_Init(int _Level, GameObject _tage)
    {
        level = _Level;
        tage = _tage;
        tage_pos = Set_pos(tage);
        Stage_mass = GameObject.Find("MassRoot");
        sePlay = GameObject.Find("Audio").GetComponent<SEManager>(); //SE

        Create_Chain_Explosion();
    }

    public void Create_Chain_Explosion()
    {
        for(int i = 1; i <= level; i++) //レベル分の魔法を放つ
        {
            //炎の五芒星の音を鳴らしますt
            sePlay.Play("FireMagicStar");

            for (int j = 0; j < Mathf.Pow((i * 2) + 1,2.0f) ; j++)
            {
                //攻撃範囲の座標分
                Vector2Int at_pos = new Vector2Int(j % ((i * 2) + 1), j / ((i * 2) + 1));

                //範囲の外側のマスだけ攻撃
                if (at_pos.y == 0 || at_pos.y == (i * 2) || at_pos.x == 0 || at_pos.x == (i * 2))
                {
                    //ターゲットの座標＋範囲
                    int targetNum = tage_pos.x + (tage_pos.y * 20) + at_pos.x - i + ((at_pos.y - i)  * 20);

                    Vector2Int pos = Vector2Int.zero;

                    pos.x = targetNum % 20;
                    pos.y = targetNum / 20;

                    StartCoroutine(Create_Magic(Stage_mass.transform.GetChild(targetNum).gameObject, i));
                }
                else
                {
                    continue;
                }
            }
        }

        Destroy(this.gameObject, level * 0.5f);
    }

    IEnumerator Create_Magic(GameObject _tage,int num)
    {
        float delay = (num) * 0.3f;
        yield return new WaitForSeconds(delay);
        //sePlay.Play("BombDeath");

        GameObject Magic = Instantiate(Magic_Obj, _tage.transform.position, transform.rotation, transform);
        //Destroy(Magic, 1.5f);
    }

    private void ene_Damege(GameObject ene)
    {
        EnemyBase e = ene.GetComponent<EnemyBase>();
        if (e != null)
        {
            e.Damage(damege_num);
        }
    }
    Vector2Int Set_pos(GameObject _tage)
    {
        Vector2Int pos = Vector2Int.zero;

        pos.x = int.Parse(_tage.name) % 20;
        pos.y = int.Parse(_tage.name) / 20;

        return pos;
    }
}
