using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleRotate : MonoBehaviour
{
    [SerializeField] float rotatePow = 90f;   // 回転する角度
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale = new Vector3(2f, 2f, 2f); // スケールが勝手に大きくなるバグがあるので一旦応急処理
    }

    // 小さい魔法陣を回す処理
    public void RotateCycleL()
    {
        transform.Rotate(new Vector3(0f, 0f, rotatePow));

    }
    public void RotateCycleR()
    {
        transform.Rotate(new Vector3(0f, 0f, -rotatePow));

    }
}
