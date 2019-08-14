using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;

    [Range(0, 1)]
    [SerializeField]
    float movementFactor; // 0 for not moved, 1 for fully moved.

    Vector3 startingPosition;


    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        // Set a movement factor.
        if (period <= Mathf.Epsilon) return;
        float cycles = Time.time / period;

        const float tau = 2 * Mathf.PI;
        float rawSinWave = Mathf.Sin(cycles * tau);

        //print(rawSinWave);

        movementFactor = (rawSinWave / 2f) + 0.5f;

        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPosition + offset;
    }
}
