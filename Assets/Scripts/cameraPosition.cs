using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPosition : MonoBehaviour
{
    public Transform camera_Pos;

    // Update is called once per frame
    void Update()
    {
        transform.position = camera_Pos.position;
    }
}
