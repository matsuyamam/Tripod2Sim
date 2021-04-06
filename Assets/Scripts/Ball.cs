using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider t)
    {
        Debug.Log("OnTriggerEnter:" + t.gameObject.name);
        Rigidbody rb = t.gameObject.GetComponent<Rigidbody>();
        if (rb)
        { 
            rb.isKinematic = false;
        }
    }
}
