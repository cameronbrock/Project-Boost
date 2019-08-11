//using System.Collections;
//using System.Collections.Generic;
//using System;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        // Thrust
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector3.up);
        }

        // Rotation
        if ( Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) )
        {
            print("Rotating left.");
        }
        else if ( Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) )
        {
            print("Rotating right.");
        }
    }
}
