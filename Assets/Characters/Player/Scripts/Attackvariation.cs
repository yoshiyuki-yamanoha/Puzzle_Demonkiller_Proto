using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attackvariation : MonoBehaviour
{
    PointControl pointcon;

    public Text orbtext_R, orbtext_G, orbtext_B, orbtext_Y, orbtext_L;

    private Color orbColorCheck,testRed,testBule,testYellow,testGreen,testLightbule;
    private int countR, countB, countY, countG, countL, num_one;
    private float testTimeCount;
    public bool activeflg_orb;

    [SerializeField] private Magic mag;

    // Start is called before the first frame update
    void Start()
    {
        pointcon = GameObject.Find("Pointer").GetComponent<PointControl>();
        activeflg_orb = false;
        attackvar_erase();
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        countR = pointcon.red;
        countG = pointcon.green;
        countB = pointcon.blue;
        countY = pointcon.yellow;
        countL = pointcon.light_blue;

        orbtext_R.text = "赤: " + countR;
        orbtext_G.text = "緑: " + countG;
        orbtext_B.text = "青: " + countB;
        orbtext_Y.text = "黄: " + countY;
        orbtext_L.text = "水: " + countL;

        num_one = Mathf.Max(countR, countG, countB, countY, countL);

        //Debug.Log(num_one);

        //Debug.Log("赤:" + countR);
        //Debug.Log("緑:" + countG);
        //Debug.Log("青:" + countB);
        //Debug.Log("黄:" + countY);
        //Debug.Log("水:" + countL);


        //if (activeflg_orb)
        //{
        //    attackvar();
        //}

    }

    public void attackvar()
    {

        if (num_one > 0)
        {
            if (num_one != countR) orbtext_R.enabled = false;
            else { orbtext_R.enabled = true; mag.SetJouhou(0); mag.StartCoroutine(mag.DoubleCombo(num_one)); }
            if (num_one != countG) { orbtext_G.enabled = false; }
            else { orbtext_G.enabled = true; mag.SetJouhou(4); }
                if (num_one != countB) orbtext_B.enabled = false;
            else { orbtext_B.enabled = true; mag.SetJouhou(1); }
            if (num_one != countY) orbtext_Y.enabled = false;
            else { orbtext_Y.enabled = true; mag.SetJouhou(2); }
            if (num_one != countL) orbtext_L.enabled = false;
            else { orbtext_L.enabled = true; mag.SetJouhou(3); }

            activeflg_orb = true;
        }
        
    }

    public void attackvar_erase()
    {
        orbtext_R.enabled = false;
        orbtext_G.enabled = false;
        orbtext_B.enabled = false;
        orbtext_Y.enabled = false;
        orbtext_L.enabled = false;

        activeflg_orb = false;
    }

}
