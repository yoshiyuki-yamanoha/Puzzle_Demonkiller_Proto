using UnityEngine;

public class EnemySpawn : MonoBehaviour
{


    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField, Range(0, 180)] float spawnInterval;
    float spawnTime = 0;

    [SerializeField] private int maxSpawnedEnemyNum;

    private void Start()
    {
        spawnTime = spawnInterval;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //敵上限までspawnIntervalの間隔で湧く
        {
            var enemies = GameObject.FindObjectsOfType<EnemyMove>();
            if (enemies.Length < maxSpawnedEnemyNum) {

                spawnTime--;

                if (spawnTime <= 0) {
                    SpawnEnemy();
                    spawnTime = spawnInterval;
                }

            }
        }
    }

    public void SpawnEnemy() {

        //スポーンブロック指定
        int num = Random.Range(0, spawnPoints.Length);

        //指定したブロックの半径ｎメートルでランダム座標
        Vector3 randomAddPos = new Vector3 {
            x = Random.Range(-5, 5),
            y = 0,
            z = Random.Range(-2, 2)
        };

        Vector3 pos = spawnPoints[num].transform.position + randomAddPos; 

        //敵生成
        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }
}
