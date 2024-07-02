using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity = -9.8f;
    private Rigidbody[] _rigidbodies;

    private void Start()
    {
        _rigidbodies = FindObjectsOfType<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 gravityDirection = new Vector3(0, gravity, 0);

        foreach (Rigidbody rb in _rigidbodies)
        {
            rb.AddForce(gravityDirection * rb.mass);
        }
    }
}
