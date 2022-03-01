using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNumText : MonoBehaviour
{
    [Header("敵の数")]
    public int Enemy_Count;
    [Header("テキスト")]
    public Text Enemy_num_text;

    // Start is called before the first frame update
    void Start()
    {
        Enemy_num_text = this.gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy_Count > 0)
        {
            Enemy_num_text.text = "敵の数: " + Enemy_Count.ToString("0");
        }
        else
        {
            Enemy_num_text.text = "クリア ";
        }

    }

    public void Enemy_Num()
    {
        Enemy_Count -= 1;
    }
}
