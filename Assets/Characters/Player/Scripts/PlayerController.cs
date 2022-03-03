using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject particleObject;

    private Vector3 cameraDefaultPosition = new Vector3(0,15,-12);
    private Vector3 lastMousePosition;
    private Vector3 newAngle = new Vector3(0, 0, 0);

    public float y_rotate, x_rotate, y_reverce, x_reverce;

    public ClearCheck cc;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.transform.position = cameraDefaultPosition;
        newAngle = this.transform.localEulerAngles;
        lastMousePosition = Input.mousePosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        newAngle.y += Input.GetAxis("Horizontal") * y_rotate * x_reverce;
        //newAngle.y += (Input.mousePosition.x - lastMousePosition.x) * y_rotate * x_reverce;
        newAngle.x -= Input.GetAxis("Vertical") * x_rotate * y_reverce;
        //newAngle.x -= (Input.mousePosition.y - lastMousePosition.y) * x_rotate * y_reverce;
        mainCamera.gameObject.transform.localEulerAngles = newAngle;
        lastMousePosition = Input.mousePosition;
        //if (cc.magicPoint > 0)
        //{
            if (Input.GetButtonDown("Fire1"))
            {
                PlayerAttack();
                cc.SubMP();
            }
        //}
    }

    public void PlayerAttack()
    {
        
        Instantiate(particleObject, mainCamera.transform.position, transform.rotation);

    }
}
