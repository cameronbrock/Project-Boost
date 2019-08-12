//using System.Collections;
//using System.Collections.Generic;
//using System;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rb;
    AudioSource audio;

    float thrustForce;
    float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        audio.loop = true;

        thrustForce = 1.0f;
        rotationSpeed = 1.0f;

        // Prevent the rocket from tipping over by restricting rotation about X and Y.
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);

        // Thrust
        if ( Input.GetKey(KeyCode.W) )
        {
            rb.AddRelativeForce(thrustForce * Vector3.up);

            if (!audio.isPlaying)
            {
                audio.Play();
            }

        }
        else
        {
            audio.Stop();
        }

        if ( Input.GetKey(KeyCode.S) )
        {
            rb.AddRelativeForce(thrustForce * Vector3.down);
        }

        // Rotation
        if ( Input.GetKey(KeyCode.A) )
        {
            transform.Rotate(rotationSpeed * Vector3.forward);
        }
        if ( Input.GetKey(KeyCode.D) )
        {
            transform.Rotate(rotationSpeed * Vector3.back);
        }

        // Allow the player to reset position.
        if ( Input.GetKey(KeyCode.R) )
        {
            transform.position = new Vector3(0, 0, 0);
        }


    }
}
