using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAmbientVolumeScaler : MonoBehaviour
{
    public float scaleFactor;
    private AudioSource audioSource;
    private float originalMaxDistance;
    private float screenSpace;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalMaxDistance = audioSource.maxDistance;
        screenSpace = CalculateScreenSpace();
    }

    private void Update()
    {
        audioSource.maxDistance = originalMaxDistance + (scaleFactor * (transform.localScale.magnitude + screenSpace));
    }

    /// <summary>
    /// 1. Calculates the size of the sphere's bounding sphere in world space using the spheres' radius.
    /// 2. Uses the WorldToViewportPoint method to convert the center and corner points of the bounding sphere to viewport space (screen space).
    /// 3. size of the bounding sphere in screen space is then calculated by finding the difference between the maximum and minimum x and y coordinates.
    /// </summary>
    private float CalculateScreenSpace()
    {
        // Get the renderer component
        SphereCollider sphereCollider = GetComponent<SphereCollider>();

        // Calculate the size of the sphere's bounding sphere in screen space
        Vector3 center = sphereCollider.bounds.center;
        float radius = sphereCollider.radius;
        Vector3 min = Camera.main.WorldToViewportPoint(center - Vector3.one * radius);
        Vector3 max = Camera.main.WorldToViewportPoint(center + Vector3.one * radius);
        float screenSpace = (max.x - min.x) * (max.y - min.y);

        return screenSpace;
    }
}
