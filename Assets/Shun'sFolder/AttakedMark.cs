using UnityEngine;

public class AttakedMark : MonoBehaviour
{
    //一度雷に当たった判定を持たせるよう
    void Start() {
        Destroy(this, 2f);
    }
}
