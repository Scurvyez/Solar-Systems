using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private Vector3 initialLookRotation;
    [SerializeField] private Vector2 sensitivity;
    [Tooltip("The rotation acceleration, in degrees / second")]
    [SerializeField] private Vector2 acceleration;
    [Tooltip("The period to wait until resetting the input value. Set this as low as possible, without encountering stuttering")]
    [SerializeField] private float inputLagPeriod;

    private Vector2 rotation; // The current rotation, in degrees
    private Vector2 velocity; // The current rotation velocity, in degrees
    private Vector2 lastInputEvent; // The last received non-zero input value
    private float inputLagTimer; // The time since the last received non-zero input value
    private float yVelocity;

    private bool isPaused = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        initialLookRotation = new Vector3(0, 35, 0);
        rotation = initialLookRotation;
    }

    private Vector2 GetInput()
    {
        // Add to the lag timer
        inputLagTimer += Time.deltaTime;

        // Get the input vector. This can be changed to work with the new input system or even touch controls
        Vector2 input = new(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        if ((Mathf.Approximately(0, input.x) && Mathf.Approximately(0, input.y)) == false || inputLagTimer >= inputLagPeriod)
        {
            lastInputEvent = input;
            inputLagTimer = 0;
        }

        return lastInputEvent;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isPaused = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isPaused = false;
            }
        }

        if (!isPaused)
        {
            // Get the input vector. This can be changed to work with the new input system or even touch controls
            Vector2 input = GetInput();

            // The wanted velocity is the current input scaled by the sensitivity
            // This is also the maximum velocity
            Vector2 wantedVelocity = input * sensitivity;

            // Calculate new rotation
            velocity = new(
                Mathf.MoveTowards(velocity.x, wantedVelocity.x, acceleration.x * Time.deltaTime),
                Mathf.MoveTowards(velocity.y, wantedVelocity.y, acceleration.y * Time.deltaTime));

            rotation += velocity * Time.deltaTime;

            // Convert the rotation to euler angles
            transform.localEulerAngles = new(rotation.y, rotation.x, 0);

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            transform.position += 2.5f * vertical * transform.forward;
            transform.position += 2.5f * horizontal * transform.right;

            // Check for "q" key press
            if (Input.GetKey(KeyCode.Q))
            {
                yVelocity = Mathf.MoveTowards(yVelocity, 1, acceleration.y * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                yVelocity = Mathf.MoveTowards(yVelocity, -1, acceleration.y * Time.deltaTime);
            }
            else
            {
                yVelocity = Mathf.MoveTowards(yVelocity, 0, acceleration.y * Time.deltaTime);
            }

            transform.position += 1.5f * yVelocity * transform.up;
        }
    }
}
