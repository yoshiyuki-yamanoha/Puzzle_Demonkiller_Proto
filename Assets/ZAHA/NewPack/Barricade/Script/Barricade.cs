using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    [SerializeField] Material material_color = default;
    Color init_color = default;//初期カラー
    float hp = 0;
    [SerializeField] float maxhp = 0;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxhp;
        init_color = material_color.color;//初期カラー保存
    }

    //barricadematerialカラー変更
    public void ColorChange()
    {
        float damageColor = hp / maxhp;
        material_color.color = new Color(damageColor, damageColor, damageColor, 1);
    }

    //barricade耐久値
    public void Damage(float attack)
    {
        hp -= attack;
        if (hp <= 0)
        {
            BarricadeDestroy();
        }
    }

    //barricade消す。
    public void BarricadeDestroy()
    {
        Destroy(this.gameObject);//自分自身を消す。
    }
}
