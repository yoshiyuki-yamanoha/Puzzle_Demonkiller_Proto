//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// コアの定数
    /// </summary>
    public class core/*:MonoBehaviour*/
    {
        public const int max_hp = 150;
    }

    // 敵によってコアへのダメージは変わる
    /// <summary>
    /// enemy attack power
    /// </summary>
    public class EAP
    {
        //zahaコアのダメージ処理テストのため変更した-
        public const int knock = 100;//10だった場所
        public const int explosion = 50;
        public const int beam = 30;
    }

    /// <summary>
    /// バリケードの定数
    /// </summary>
    public class barri/*:MonoBehaviour*/
    {
        public const int max_hp = 1;
    }
}

public class CoreInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
