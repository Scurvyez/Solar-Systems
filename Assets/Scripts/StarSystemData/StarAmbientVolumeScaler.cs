using Saving;
using UnityEngine;
using Utils;

public class StarAmbientVolumeScaler : MonoBehaviour
{
    public float scaleFactor;
    private AudioSource audioSource;
    private float originalMaxDistance;
    private float screenSpace;

    private double StarRadius;
    private float RadiusAsSolarRadii;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalMaxDistance = audioSource.maxDistance;
        screenSpace = CalculateScreenSpace();

        StarRadius = SaveManager.Instance.ActiveSave.starRadius;
        RadiusAsSolarRadii = (float)StarRadius / ConstantsUtil.SOL_RADII_METERS;
        scaleFactor = (RadiusAsSolarRadii) / 10;
    }

    private void Update()
    {
        //audioSource.maxDistance = originalMaxDistance + (scaleFactor * (transform.localScale.magnitude + screenSpace));
        //audioSource.maxDistance = audioSource.transform.localScale.magnitude + scaleFactor;
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
        if (Camera.main == null) return 0;
        Vector3 min = Camera.main.WorldToViewportPoint(center - Vector3.one * radius);
        Vector3 max = Camera.main.WorldToViewportPoint(center + Vector3.one * radius);
        float screenSpace = (max.x - min.x) * (max.y - min.y);
        return screenSpace;
    }
}
