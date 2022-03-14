using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTuyoiMagic : MonoBehaviour
{

    [SerializeField] private Magic m;

    public void ShotMagic() {
        m.SpeedDown(5);
    }
}
