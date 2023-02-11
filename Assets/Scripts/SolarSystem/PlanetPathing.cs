using UnityEngine;

public class PlanetPathing : MonoBehaviour
{
    public float speed = 25f;

    private void Update()
    {
        transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
    }
}
