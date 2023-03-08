using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGen : MonoBehaviour
{
    public GameObject planetPrefab;
    public GameObject starPrefab;
    
    public void Start()
    {
        int numPlanets = Random.Range(1, 11); // generate a random number of planets between 1 and 10

        for (int i = 0; i < numPlanets; i++)
        {
            // create a new rocky planet object
            RockyPlanet planet = new RockyPlanet("Rocky Planet " + (i + 1), Random.Range(100, 200), Random.Range(20, 30),
                Random.Range(0, 180), Random.Range(250, 350), true, false, Random.Range(0.5f, 1.5f),
                Random.Range(5f, 10f), Random.Range(8f, 12f), Random.Range(0.2f, 0.3f), Random.Range(10, 20),
                Random.Range(0.1f, 0.2f), new List<Moon>());

            // instantiate the rocky planet prefab and set its properties
            GameObject planetObj = Instantiate(planetPrefab);
            planetObj.name = planet.Name;
            planetObj.transform.localScale = Vector3.one * planet.Radius * 2f;
            planetObj.transform.localPosition = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(0, 0, Random.Range(20f, 100f));

            planetObj.GetComponent<Renderer>().material.color = Color.grey;

            // add the planet to the list of planets orbiting the star object
        }
    }

    public void Update()
    {
        transform.Rotate(Vector3.up, 0.5f);
    }
}
