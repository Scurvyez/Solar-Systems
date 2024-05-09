using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity = -9.8f;

    private void FixedUpdate()
    {
        Vector3 gravityDirection = new Vector3(0, gravity, 0);
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.AddForce(gravityDirection * rb.mass);
        }
    }
}
