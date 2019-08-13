
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rb;
    AudioSource audio;

    int currentLevel;

    [SerializeField] float thrustForce;
    [SerializeField] float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();

        audio.loop = true;
        rb.drag = 0.375f;

        thrustForce = 100.0f;
        rotationSpeed = 50.0f;

        currentLevel = SceneManager.GetActiveScene().buildIndex;

        // Prevent the rocket from tipping over by restricting rotation about X and Y.
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Goal":
                currentLevel++;
                print("GOAL - Current level: " + currentLevel);
                SceneManager.LoadScene(currentLevel);
                break;
            case "Fuel":
                break;
            default:
                print("DEAD - Current level: " + currentLevel);
                SceneManager.LoadScene(currentLevel);
                break;
        }
    }

    // Thrust
    private void ProcessThrustInput()
    {
        // Forward thrust
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(thrustForce * Time.deltaTime * Vector3.up);

            if (!audio.isPlaying)
            {
                audio.Play();
            }

        }
        else
        {
            audio.Stop();
        }

        // Backwards thrust
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeForce(thrustForce * Time.deltaTime * Vector3.down);
        }
    }

    // Rotation
    private void ProcessRotationInput()
    {

        rb.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.back);
        }

        rb.freezeRotation = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

    }

    private void ProcessInput()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        //Thrust
        ProcessThrustInput();

        // Rotation
        ProcessRotationInput();

        // Allow the player to reset position.
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(currentLevel);
        }

    }
}
