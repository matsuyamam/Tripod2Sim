using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject arm;
    bool done;

    // Start is called before the first frame update
    void Start()
    {
        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            if (transform.localEulerAngles.x > 350.0f || transform.localEulerAngles.x < 10.0f)
            {
                arm.GetComponent<Rigidbody>().isKinematic = false;
                done = true;
            }
        }
    }
}
