using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour
{
    public GameObject Base;
    public GameObject Parts;
    public GameObject Controll;
    public GameObject Prize;

    GameObject parent;
    GameObject control;
    GameObject[] childs;
    float rot;

    bool start;

    // Start is called before the first frame update
    void Start()
    {
        control = Instantiate(Controll, new Vector3(0.0f, 0.0f, -0.33f), Quaternion.identity);
        control.transform.Find("Cylinder").localRotation = Quaternion.Euler(new Vector3(10.0f, 0.0f, 0.0f));

        parent = Instantiate(Base, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        CameraControl camera = gameObject.GetComponent<CameraControl>();
        camera.baseObject = parent;
        camera.targetObject = parent;

        childs = new GameObject[15];

        for (int i=0; i<15; i++)
        {
            float radius = 0.205f;
            float rotation = 2.0f * Mathf.PI * i / 15.0f;
            float x = radius * Mathf.Sin(rotation);
            float z = radius * Mathf.Cos(rotation);
        
            childs[i] = Instantiate(Parts, new Vector3(x, 0.0f, -z), Quaternion.Euler(0.0f, Mathf.Rad2Deg * -rotation, 0.0f)/*, parent.transform*/);
            childs[i].transform.Find("Cube2").SetParent(parent.transform);

            HingeJoint[] joints = childs[i].GetComponentsInChildren<HingeJoint>();
            foreach(var j in joints)
            {
                Rigidbody[] bodys = parent.GetComponentsInChildren<Rigidbody>();
                j.connectedBody = bodys[0];
            }
        }

        for (int i = 0; i < 11; i++)
        {
            float radius = 0.160f;
            float rotation = 2.0f * Mathf.PI * i / 11.0f;
            float x = radius * Mathf.Sin(rotation);
            float z = radius * Mathf.Cos(rotation);

            Instantiate(Prize, new Vector3(x, 0.001f, -z), Quaternion.Euler(0.0f, Mathf.Rad2Deg * -rotation, 0.0f)/*, parent.transform*/);
        }
        for (int i = 0; i < 10; i++)
        {
            float radius = 0.150f;
            float rotation = 2.0f * Mathf.PI * i / 10.0f;
            float x = radius * Mathf.Sin(rotation);
            float z = radius * Mathf.Cos(rotation);

            Instantiate(Prize, new Vector3(x, 0.091f, -z), Quaternion.Euler(0.0f, Mathf.Rad2Deg * -rotation, 0.0f)/*, parent.transform*/);
        }
        for (int i = 0; i < 9; i++)
        {
            float radius = 0.140f;
            float rotation = 2.0f * Mathf.PI * i / 9.0f;
            float x = radius * Mathf.Sin(rotation);
            float z = radius * Mathf.Cos(rotation);

            Instantiate(Prize, new Vector3(x, 0.181f, -z), Quaternion.Euler(0.0f, Mathf.Rad2Deg * -rotation, 0.0f)/*, parent.transform*/);
        }

        start = false;
        rot = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!start)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                control.transform.Find("Cylinder").GetComponent<Rigidbody>().isKinematic = false;
                start = true;
            }
        }

        rot += 5.0f * Time.deltaTime;
        parent.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, -rot, 0));

        for (int i = 0; i < 15; i++)
        {
            Rigidbody[] bodys = childs[i].GetComponentsInChildren<Rigidbody>();
            if(bodys[0].isKinematic)
            {
                float radius = 0.205f;
                float rotation = Mathf.Deg2Rad * rot + 2.0f * Mathf.PI * i / 15.0f;
                float x = radius * Mathf.Sin(rotation);
                float z = radius * Mathf.Cos(rotation);
                bodys[0].MovePosition(new Vector3(x, 0.0f, -z));
                bodys[0].MoveRotation(Quaternion.Euler(0.0f, Mathf.Rad2Deg * -rotation, 0.0f));
            }
            if (bodys[1].isKinematic)
            {
                float radius = 0.31f;
                float rotation = Mathf.Deg2Rad * rot + 2.0f * Mathf.PI * i / 15.0f;
                float x = radius * Mathf.Sin(rotation);
                float z = radius * Mathf.Cos(rotation);
                bodys[1].MovePosition(new Vector3(x, 0.0f, -z));
                bodys[1].MoveRotation(Quaternion.Euler(-55.0f, Mathf.Rad2Deg * -rotation, 0.0f));
            }
        }
    }
}
