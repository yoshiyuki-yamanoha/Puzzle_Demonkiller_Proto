using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ただの案山子を量産する魔法シーン用のスポナー
//敵が全員倒されたらもう一度湧かせるだけ
public class MagicOnlySceneSpawner : MonoBehaviour
{
    //一度に湧く敵の数
    [SerializeField, Range(0, 15)] 
    int oT_SpawnalbeEnemyNum;

    //ただの案山子プレファブ
    [SerializeField]
    GameObject enemyPrefab;

    //mapmassスクリプト
    [SerializeField]
    MapMass s_MapMass;

    struct SpawnedEnemyMass {
        public int x;
        public int y;
    }

    GameObject[] masses;

    // Start is called before the first frame update
    void Start()
    {
        masses = new GameObject[s_MapMass.GetAllMassObjects().Length];
        masses = s_MapMass.GetAllMassObjects();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length <= 0) {

            //敵を一気に湧かせる
            for (int i = 0; i < oT_SpawnalbeEnemyNum; i++) {
                //座標を指定
                var pos = RandomMassSelect();

                //指定の座標に敵を沸かせる
                GeneEnemy(pos.x, pos.y);
            }
        }
    }

    void GeneEnemy(int x, int y) {

        var massObj = s_MapMass.GetGameObjectOfSpecifiedMass(x, y);

        var buff = Instantiate(enemyPrefab, massObj.transform.position, Quaternion.identity);

        buff.GetComponent<KariEnemyStatus2>().PassPos(x, y);

    }

    //ランダムな敵が居ない&何もないマスを返す
    Vector2Int RandomMassSelect() {

        Vector2Int ret = new Vector2Int();
        bool isThrough = false;
        do
        {
            ret.x = Random.Range(0, 19);
            ret.y = Random.Range(0, 19);
            if (s_MapMass.Map[ret.y, ret.x] == (int)MapMass.Mapinfo.NONE)
            {
                s_MapMass.Map[ret.y, ret.x] = (int)MapMass.Mapinfo.Enemy;
                isThrough = true;
            }

        } while (!isThrough);

        return ret;
    }


}
