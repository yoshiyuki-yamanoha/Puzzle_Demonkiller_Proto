//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace CoreBase
{
    /// <summary>
    /// コア
    /// </summary>
    public class Core_RequiredElement
    {
        public  GameObject obj;
        public int hp;
    }

    public class Core/*:Core_RequiredElement*/
    {
        public readonly int max_hp = 15;

        public GameObject obj;
        public int hp;
    }

    /// <summary>
    /// enemy attack power
    /// </summary>
    public class EAP
    {
        //zahaコアのダメージ処理テストのため変更した-
        public const int knock = 1;//10だった場所
        public const int explosion = 5;
        public const int beam = 3;
    }

    /// <summary>
    /// バリケード
    /// </summary>
    public class Barricade_Class/*:Core_RequiredElement*/
    {
        public readonly int max_hp = 1;

        public GameObject obj;
        public int hp;
    }

    //public class Core_Slider
    //{

    //}
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
