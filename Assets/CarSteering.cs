using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class CarSteering : MonoBehaviour
{
    public float acceleration = 30f;
    public float steering = 2f;
    public float driftFactor = 0.95f;
    public float distanceToRear = 1.5f; // Distance from center to rear axle

    public float maxSpeed = 50f; // Define the maximum speed of the car
    public float maxSteeringAtLowSpeed = 3f; // Maximum steering effect at low speed
    public float minSteeringAtHighSpeed = 0.5f; // Minimum steering effect at high speed
    public float carDrag = 0.5f; // Drag of the car
    /*public float handbrakeStrength = 1000f; // Handbrake Strength*/

    private Rigidbody2D rb;

    /*private bool handbrakeActive = false;// Define Handbrake */

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        Vector3 moveVelocity = moveDirection * acceleration * Time.deltaTime;

        rb.velocity = moveVelocity;

        /*// Check for handbrake input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            handbrakeActive = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            handbrakeActive = false;
        }*/

        // Check if the 'R' key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void FixedUpdate()
    {
        float h = -Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calculate the rear position of the car
        Vector2 rearPosition = rb.position - (Vector2)(transform.up * distanceToRear);

        // Apply force at the rear position for acceleration
        Vector2 force = transform.up * (v * acceleration);
        rb.AddForceAtPosition(force, rearPosition);

        // Steering
        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up)); // Check the direction of movement
        if (direction >= 0.0f)
        {
            rb.rotation += h * steering * (rb.velocity.magnitude / 5.0f);
            rb.angularVelocity = 0;
        }
        else
        {
            rb.rotation -= h * steering * (rb.velocity.magnitude / 5.0f);
            rb.angularVelocity = 0;
        }

        float currentSpeedFactor = rb.velocity.magnitude / maxSpeed; // Assuming maxSpeed is defined
        float steeringEffect = Mathf.Lerp(maxSteeringAtLowSpeed, minSteeringAtHighSpeed, currentSpeedFactor);
        rb.angularVelocity = h * steeringEffect;

        // Apply drag
        rb.drag = carDrag; // carDrag is a defined variable for drag

        /*if (handbrakeActive)
        {
            ApplyHandbrake();
        }*/
    }

    /*private void ApplyHandbrake()
    {
        // Apply a backward force on the car's rear axle
        Vector2 handbrakeForce = -transform.forward * handbrakeStrength;
        rb.AddForceAtPosition(handbrakeForce, transform.position - transform.forward * 2, ForceMode2D.Impulse);
    }*/

    void RestartGame()
    {
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
