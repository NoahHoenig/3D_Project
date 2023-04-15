using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FirstPerson : MonoBehaviour
{
    public Transform orientation;

    private float y_rotation = 0f;
    private float x_rotation = 0f;

    public float sens_x;
    public float sens_y;
    // Start is called before the first frame update
    void Start()
    {
   
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        y_rotation -= Input.GetAxis("Mouse Y") * Time.deltaTime * sens_x;
        x_rotation += Input.GetAxis("Mouse X") * Time.deltaTime *sens_y;
        y_rotation = Mathf.Clamp(y_rotation, -90f, 90f);
        
        orientation.rotation = Quaternion.Euler(0, x_rotation, 0);
        transform.rotation = Quaternion.Euler(y_rotation, x_rotation, 0);
        
    }
}
