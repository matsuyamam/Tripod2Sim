using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera targetCamera;
    public GameObject baseObject;
    public GameObject targetObject;
    public float ZoomSensitivity;
    public float PitchSensitivity;

    float Yaw;
    float rotX;
    float length;

    // Start is called before the first frame update
    void Start()
    {
        Yaw = 180f;
        rotX = -40f;
        length = 0.85f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.PageDown))
        {
            length += ZoomSensitivity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.PageUp))
        {
            length -= ZoomSensitivity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rotX += PitchSensitivity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rotX -= PitchSensitivity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Yaw += PitchSensitivity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Yaw -= PitchSensitivity * Time.deltaTime;
        }

        Quaternion q = Quaternion.Euler(rotX, Yaw, 0f);
        targetCamera.transform.position = q * new Vector3(0f, 0f, length) + baseObject.transform.position;
        targetCamera.transform.LookAt(targetObject.transform);
    }
}
