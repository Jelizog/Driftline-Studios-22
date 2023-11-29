using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class FirstDrift : MonoBehaviour
{
    public float acceleration = 5f;
    public float steering = 3f;
    public float driftFactor = 3f;
    public float distanceToRear = 1f; // Distance from center to rear axle

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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
        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
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

        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if (rb.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(rightAngleFromForward), Color.green);

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (driftForce * rb.GetRelativeVector(rightAngleFromForward.normalized));
        rb.AddForce(-relativeForce * driftFactor);

        if (Input.GetButton("Jump"))
        {
            rb.drag = 0.0f;
        }
        else
        {
            rb.drag = 1.0f;
        }
    }

    void RestartGame()
    {
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

