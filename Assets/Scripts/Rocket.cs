
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{

    Rigidbody rb;
    AudioSource audio;

    // Rocket specifications.
    [SerializeField] float thrustForce;
    [SerializeField] float rotationSpeed;

    [SerializeField] float levelLoadDelay = 2.0f;

    // Audio clips.
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip successSound;

    // Particle effects.
    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    // Activate debug commands.
    [SerializeField] bool levelControlOn;
    [SerializeField] bool disableCollisions;

    float fuelRemaining;
    [SerializeField] float fuelDrainingSpeed;
    public Text fuelGauge;

    enum State
    {
        ALIVE, DYING, TRANSCENDING
    }

    [SerializeField] State state = State.ALIVE;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();

        audio.loop = true;
        rb.drag = 0.375f;

        thrustForce = 100.0f;
        rotationSpeed = 75.0f;

        fuelRemaining = 100.0f;

    }

    // Update is called once per frame
    void Update()
    {
        fuelGauge.text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex + 1) + "\nFUEL: " + ((int) fuelRemaining) + " %";
        ProcessInput();
    }
    
    // Reset the current level.
    private void SameLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Load the next scene/level.
    private void NextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (currentLevel < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(currentLevel + 1);
        }
    }

    private void PrevLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (currentLevel > 0)
        {
            SceneManager.LoadScene(currentLevel - 1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If we are not alive, ignore collisions.
        if (state != State.ALIVE)
        {
            return;
        }

        // Process collision.
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Goal":
                state = State.TRANSCENDING;
                audio.Stop();
                audio.PlayOneShot(successSound);
                successParticles.Play();
                Invoke("NextLevel", levelLoadDelay);
                break;
            case "Fuel":
                break;
            default:
                if (!disableCollisions)
                {
                    state = State.DYING;
                    audio.Stop();
                    audio.PlayOneShot(deathSound);
                    deathParticles.Play();
                    Invoke("SameLevel", levelLoadDelay);
                }
                break;
        }
    }

    // Thrust
    private void ProcessThrustInput()
    {
        
        // Forward thrust
        
        if (Input.GetKey(KeyCode.W) && (fuelRemaining > Mathf.Epsilon))
        {
            rb.AddRelativeForce(thrustForce * Time.deltaTime * Vector3.up);

            // Drain remaining fuel.
            fuelRemaining -= fuelDrainingSpeed * Time.deltaTime;
            if (fuelRemaining <= Time.deltaTime)
            {
                fuelRemaining = 0.0f;
            }
            if (!audio.isPlaying)
            {
                audio.PlayOneShot(mainEngine);
            }
            engineParticles.Play();
        }
        
        else
        {
            audio.Stop();
            engineParticles.Stop();
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

        // Freeze the rocket in the Z-direction to keep the game 2D.
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        
        // If the player is dead, freeze controls.
        if (state == State.ALIVE)
        {
            //Thrust
            ProcessThrustInput();

            // Rotation
            ProcessRotationInput();
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
        }
        
        // Allow the player to reset position.
        if (Input.GetKey(KeyCode.R))
        {
            SameLevel();
        }
        // Allow the player to go to next level via L-key.
        else if (Input.GetKeyDown(KeyCode.L) && levelControlOn)
        {
            NextLevel();
        }
        // Allow the player to go to previous level via K-key.
        else if (Input.GetKeyDown(KeyCode.K) && levelControlOn)
        {
            PrevLevel();
        }

    }
}
