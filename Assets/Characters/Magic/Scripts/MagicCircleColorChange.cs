using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCircleColorChange : MonoBehaviour
{
    Image magicCicle;
    // Start is called before the first frame update
    void Start()
    {
        magicCicle = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform transform- 
    }

    public void CircleColorChange(Color c)
    {
        magicCicle.color = c;
    }
}
