using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KariEnemyStatus2 : MonoBehaviour
{
    public int x;
    public int y;

    public void PassPos(int x,int y) {
        this.x = x;
        this.y = y;
    }

    //3秒後にエネミーを消す
    public void Death() {
        Destroy(gameObject, 3.0f);
    }
}
