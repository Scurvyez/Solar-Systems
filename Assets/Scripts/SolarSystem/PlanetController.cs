using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public SaveManager SaveManager;
    public GameObject starPrefab;

    public float orbitalPeriodFactor;
    public float rotationPeriodFactor;

    private void Start()
    {
        // instantiate the rocky planet prefab and set its properties
        transform.localScale = Vector3.one * 2f;
        transform.localPosition = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(0, 0, Random.Range(100f, 1000f));

        GetComponent<Renderer>().material.color = Color.red;

        //orbitalPeriodFactor = planet.OrbitalPeriod;
        //rotationPeriodFactor = planet.RotationPeriod;

        // add a Trail Renderer to the planet
        TrailRenderer tR = GetComponent<TrailRenderer>();
        tR.startWidth = 1.0f;
        tR.endWidth = 0.0f;
        tR.time = 7.0f;
        tR.material = new Material(Shader.Find("Sprites/Default"));
        tR.startColor = Color.white;
        tR.endColor = Color.clear;
    }

    private void Update()
    {
        // Rotate the planet around its own axis
        transform.Rotate(Vector3.up, Time.deltaTime * 10f);

        // Rotate the planet around the star
        transform.RotateAround(starPrefab.transform.position, Vector3.up, Time.deltaTime * 20f);
    }
}
