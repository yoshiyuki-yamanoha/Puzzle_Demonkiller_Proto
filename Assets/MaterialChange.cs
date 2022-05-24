using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    [SerializeField] AnimEvent anim_event;
    public Material change_material;
    Material material;
    Renderer my_renderer;
    // Start is called before the first frame update
    void Start()
    {
        GetMaterial();
        //SetMaterial(change_material);
    }

    void GetMaterial()
    {
        material = transform.gameObject.GetComponent<Renderer>().material;
    }

    public void SetMaterial(Material material)
    {
        my_renderer = transform.gameObject.GetComponent<Renderer>();
        my_renderer.material = material;
    }

    public void IceBreakMaterial()
    {
        //float des = my_renderer.material.GetFloat("_Destruction");//materialの値を取得

        //des += Time.deltaTime;//分解materialの値を生成

        //if (des >= 1f) { 
        //    des = 0f;
        SetMaterial(material);//materialを戻す。
        anim_event.Ice_star();
        anim_event.FlameBreakIceOff(); //氷を壊すフラグをオフ
        //}

        //my_renderer.material.SetFloat("_Destruction", des);//分解materialの値を更新
    }

    private void FixedUpdate()
    {
        if (anim_event.GetFlameIcebreak())
        {
            IceBreakMaterial();
        }
    }
}
